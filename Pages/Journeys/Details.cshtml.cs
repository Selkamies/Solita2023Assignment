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
    }
}
