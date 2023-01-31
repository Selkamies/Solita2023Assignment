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
    public class IndexModel : PageModel
    {
        private readonly Solita2023Assignment.Data.Solita2023AssignmentContext _context;

        public IndexModel(Solita2023Assignment.Data.Solita2023AssignmentContext context)
        {
            _context = context;
        }

        public IList<Journey> Journey { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Journey != null)
            {
                // TODO: Add pagination.
                Journey = await _context.Journey
                    .Include(j => j.ArrivalStation)
                    .Include(j => j.DepartureStation).Take(100).ToListAsync();
            }
        }
    }
}
