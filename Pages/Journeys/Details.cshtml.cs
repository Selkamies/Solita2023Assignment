using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Solita2023Assignment.Models;

namespace Solita2023Assignment.Pages.Journeys
{
    public class DetailsModel : PageModel
    {
        private readonly Solita2023Assignment.Data.Solita2023AssignmentContext _context;

        public DetailsModel(Solita2023Assignment.Data.Solita2023AssignmentContext context)
        {
            _context = context;
        }



        public Journey Journey { get; set; } = default!; 



        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Journey == null)
            {
                return NotFound();
            }

            var journey = await _context.Journey.FirstOrDefaultAsync(m => m.ID == id);

            if (journey == null)
            {
                return NotFound();
            }
            else 
            {
                // TODO: I don't thing we would need to do this manually? What is wrong?
                // TODO: Only select the name, we don't need the whole station data?
                journey.DepartureStation = _context.Station.Single(s => s.ID == journey.DepartureStationID);
                journey.ArrivalStation = _context.Station.Single(s => s.ID == journey.ArrivalStationID);

                Journey = journey;
            }

            return Page();
        }
    }
}
