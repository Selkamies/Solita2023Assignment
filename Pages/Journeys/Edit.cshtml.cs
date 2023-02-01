using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Solita2023Assignment.Models;

namespace Solita2023Assignment.Pages.Journeys
{
    public class EditModel : PageModel
    {
        private readonly Solita2023Assignment.Data.Solita2023AssignmentContext _context;

        public EditModel(Solita2023Assignment.Data.Solita2023AssignmentContext context)
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

            var journey =  await _context.Journey.FirstOrDefaultAsync(m => m.ID == id);

            if (journey == null)
            {
                return NotFound();
            }

            Journey = journey;

            SelectList stationList = new SelectList(_context.Station, "ID", "NameFI");
            ViewData["ArrivalStationID"] = stationList;
            ViewData["DepartureStationID"] = stationList;

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

            _context.Attach(Journey).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JourneyExists(Journey.ID))
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

        private bool JourneyExists(int id)
        {
          return (_context.Journey?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
