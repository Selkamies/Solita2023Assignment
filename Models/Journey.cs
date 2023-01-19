using System.ComponentModel.DataAnnotations;

namespace Solita2023Assignment.Models
{
    /// <summary>
    /// A single journey between two citybike stations.
    /// TODO: More annotations?
    /// TODO: The .csv files use "return station", but would "end station" or "arrival station" be more clear? 
    ///       We don't have to use the .csv names.
    /// </summary>
    public class Journey
    {
        /// <summary>
        /// Database id number of the journey.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Date and time of the departure (start of the journey).
        /// </summary>
        [Display(Name = "Departure time")]
        public DateTime DepartureTime { get; set; }
        /// <summary>
        /// Date and time of the return (end of the journey).
        /// </summary>
        [Display(Name = "Departure time")]
        public DateTime ReturnTime { get; set; }

        // The station id's are added to the database, but we want to display either only
        // the name of the stations or both name and public id (instead of database id) of the stations.
        // TODO: "!" at the end of default suppresses null warnings. What is the best way to do this,
        //       using Station? causes warnings in pages and not using it causes warnings here?
        /// <summary>
        /// Database id (not public id) of the departure station. Gets saved to database.
        /// </summary>
        [Display(Name = "Departure station")]
        public int DepartureStationId { get; set; }
        /// <summary>
        /// Reference to the departure station model, not saved to database. 
        /// Used to get the name and public id of the station.
        /// </summary>
        public Station DepartureStation { get; set; } = default!;
        /// <summary>
        /// Database id (not public id) of the departure station.
        /// </summary>
        [Display(Name = "Departure station")]
        public int ReturnStationId { get; set; }
        /// <summary>
        /// Reference to the return station model, not saved to database. 
        /// Used to get the name and public id of the station.
        /// </summary>
        public Station ReturnStation { get; set; } = default!;

        /// <summary>
        /// Distance of the journey in meters.
        /// </summary>
        [Display(Name = "Distance (m)")]
        public int DistanceMeters { get; set; }
        /// <summary>
        /// Duration of the journey in seconds.
        /// </summary>
        [Display(Name = "Duration (s)")]
        public int DurationSeconds { get; set; }
    }
}
