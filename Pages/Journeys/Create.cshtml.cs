using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Solita2023Assignment.Models;
using System.Globalization;

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
            SelectList stations = new SelectList(_context.Station, "ID", "NameFI");
            ViewData["ArrivalStationID"] = stations;
            ViewData["DepartureStationID"] = stations;
            /*ViewData["ArrivalStationID"] = new SelectList(_context.Station, "ID", "NameFI");
            ViewData["DepartureStationID"] = new SelectList(_context.Station, "ID", "NameFI");*/

            return Page();
        }



        [BindProperty]
        public Journey Journey { get; set; } = default!;
        


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            // TODO: Calculate duration from departure and arrival times.
            /*int durationSeconds = (int)this.Journey.ArrivalTime.Subtract(this.Journey.DepartureTime).TotalSeconds;
            System.Diagnostics.Debug.Print($"Duration in seconds: {durationSeconds}.");
            // this.Journey.DurationSeconds = durationSeconds;

            // Disable validation for duration field, since it will be calculated.
            ModelStateEntry? valueToClean = ModelState["DurationSeconds"];
            if (valueToClean != null && durationSeconds > 10) 
            { 
                valueToClean.Errors.Clear(); 
            }

            ModelState.SetModelValue("DurationSeconds", new ValueProviderResult(durationSeconds.ToString(), CultureInfo.InvariantCulture));
            */

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
