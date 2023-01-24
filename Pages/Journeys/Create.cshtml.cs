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
            //this.CreateStationList();
        }

        /// <summary>
        /// This is used to get the list of stations in the cshtml.
        /// </summary>
        //public List<SelectListItem>? Stations { get; set; }

        /*private bool CreateStationList()
        {
            this.Stations = _context.Station.Select(station => new SelectListItem
            {
                Text = station.NameFI,
                Value = station.ID.ToString()
            }).ToList();

            if (this.Stations.Count > 0)
            {
                return true;
            }

            else
            {
                return false;
            }
        }*/

        public IActionResult OnGet()
        {
            // These disappear when "create" button is pressed (and page is reloaded) and adding a new journey fails.
            ViewData["ArrivalStationID"] = new SelectList(_context.Station, "ID", "NameFI");
            ViewData["DepartureStationID"] = new SelectList(_context.Station, "ID", "NameFI");

            /*ViewData["ArrivalStationID"] = this.Stations;
            ViewData["DepartureStationID"] = this.Stations*/

            return Page();
        }

        [BindProperty]
        public Journey Journey { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Journey == null || Journey == null)
            {
                /*if (!ModelState.IsValid)
                {
                    // TODO: We get here, and get error that station must be selected. The create page then 
                    //       reloads, and the station dropdowns are empty.
                    System.Diagnostics.Debug.Print("ModelState is not valid.");
                    // These are correct id's, but don't validate.
                    System.Diagnostics.Debug.Print($"Selected Departure Station ID: {Journey.DepartureStationID}");
                    System.Diagnostics.Debug.Print($"Selected Arrival Station ID: {Journey.ArrivalStationID}");
                }

                if (_context.Journey == null)
                {
                    System.Diagnostics.Debug.Print("_context.Journey is null.");
                }

                if (Journey == null)
                {
                    System.Diagnostics.Debug.Print("Journey is null.");
                }*/

                return Page();
            }

            _context.Journey.Add(Journey);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
