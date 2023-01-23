using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Solita2023Assignment.Data;
using Solita2023Assignment.Models;

namespace Solita2023Assignment.Pages.Stations
{
    public class EditModel : PageModel
    {
        private readonly Solita2023Assignment.Data.Solita2023AssignmentContext _context;

        public EditModel(Solita2023Assignment.Data.Solita2023AssignmentContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Station Station { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Station == null)
            {
                return NotFound();
            }

            var station =  await _context.Station.FirstOrDefaultAsync(m => m.ID == id);
            if (station == null)
            {
                return NotFound();
            }
            Station = station;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Station).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StationExists(Station.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool StationExists(int id)
        {
          return (_context.Station?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
