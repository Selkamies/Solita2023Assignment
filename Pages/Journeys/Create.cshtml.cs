using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Solita2023Assignment.Models;

namespace Solita2023Assignment.Pages.Journeys
{
    public class CreateModel : PageModel
    {
        private readonly Solita2023Assignment.Data.Solita2023AssignmentContext _context;

        public CreateModel(Solita2023Assignment.Data.Solita2023AssignmentContext context)
        {
            _context = context;
        }



        public IActionResult OnGet()
        {
            // Create a list of stations for selection dropdown. Saved value of chosen station
            // is the station's database id, and we display the station's name.
            SelectList stations = new SelectList(_context.Station, "ID", "NameFI");

            ViewData["ArrivalStations"] = stations;
            ViewData["DepartureStations"] = stations;

            return Page();
        }



        [BindProperty]
        public Journey Journey { get; set; } = default!;
        


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            // TODO: Calculate duration from departure and arrival times.
            // TODO: Display the save the duration to the ModelState so that it validates.
            // TODO: Display the calculated duration to the user.

            if (!ModelState.IsValid || _context.Journey == null || Journey == null)
            {
                return Page();
            }

            _context.Journey.Add(Journey);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
