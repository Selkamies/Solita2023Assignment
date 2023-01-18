using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Solita2023Assignment.Data;
using Solita2023Assignment.Models;

namespace Solita2023Assignment.Pages.Journeys
{
    public class DeleteModel : PageModel
    {
        private readonly Solita2023Assignment.Data.Solita2023AssignmentContext _context;

        public DeleteModel(Solita2023Assignment.Data.Solita2023AssignmentContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Journey Journey { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Journey == null)
            {
                return NotFound();
            }

            var journey = await _context.Journey.FirstOrDefaultAsync(m => m.Id == id);

            if (journey == null)
            {
                return NotFound();
            }
            else 
            {
                Journey = journey;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Journey == null)
            {
                return NotFound();
            }
            var journey = await _context.Journey.FindAsync(id);

            if (journey != null)
            {
                Journey = journey;
                _context.Journey.Remove(Journey);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
