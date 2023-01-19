using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Solita2023Assignment.Data;
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
            // Creates a dropdown list from stations, selection saves the database id of station,
            // but displays the Finnish name of the station.
            // TODO: Display both the name and public id of station?
            //SelectList stationList = new SelectList(_context.Station, "Id", "NameFI");
            SelectList stationList = new SelectList(items: _context.Station, 
                                                    dataValueField: "Id", 
                                                    dataTextField: "NameFI");
            ViewData["DepartureStationId"] = stationList;
            ViewData["ReturnStationId"] = stationList;

            return Page();
        }

        [BindProperty]
        public Journey Journey { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
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
