using Microsoft.AspNetCore.Mvc.Rendering;
using Solita2023Assignment.Models;

namespace Solita2023Assignment.ViewModels
{
    /// <summary>
    /// TODO: Actually use this.
    /// TODO: Use the list of stations to display their name and public id 
    /// instead of a list of database station ids.
    /// </summary>
    public class JourneyViewModel
    {
        /// <summary>
        /// Date and time of the departure (start of the journey).
        /// </summary>
        public DateTime DepartureTime { get; set; }
        /// <summary>
        /// Date and time of the return (end of the journey).
        /// </summary>
        public DateTime ReturnTime { get; set; }

        /// <summary>
        /// Database id (not public id) of the departure station. Gets saved to database.
        /// </summary>
        public int DepartureStationId { get; set; }
        /// <summary>
        /// Database id (not public id) of the departure station. Gets saved to database.
        /// </summary>
        public int ReturnStationId { get; set; }
        /// <summary>
        /// List of stations to select for the station ids.
        /// </summary>
        public IEnumerable<SelectListItem> Stations { get; set; } = default!;

        /// <summary>
        /// Distance of the journey in meters.
        /// </summary>
        public int DistanceMeters { get; set; }
        /// <summary>
        /// Duration of the journey in seconds.
        /// </summary>
        public int DurationSeconds { get; set; }
    }
}
