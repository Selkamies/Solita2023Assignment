﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solita2023Assignment.Models
{
    /// <summary>
    /// A single journey between two citybike stations.
    /// TODO: More annotations?
    /// </summary>
    public class Journey
    {
        /// <summary>
        /// Database id number of the journey.
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// Date and time of the departure (start of the journey).
        /// </summary>
        [Required, Display(Name = "Departure time")]
        public DateTime DepartureTime { get; set; }
        /// <summary>
        /// Date and time of the return (end of the journey).
        /// </summary>
        [Required, Display(Name = "Arrival time")]
        public DateTime ArrivalTime { get; set; }

        // The station id's are added to the database, but we want to display either only
        // the name of the stations or both name and public id (instead of database id) of the stations.
        // TODO: "!" at the end of default suppresses null warnings. What is the best way to do this,
        //       using Station? causes warnings in pages and not using it causes warnings here?
        /// <summary>
        /// Database id (not public id) of the departure station. Gets saved to database.
        /// </summary>
        [Required, ForeignKey("DepartureStation"), Display(Name = "Departure station")]
        public int DepartureStationID { get; set; }
        /// <summary>
        /// Reference to the departure station model, not saved to database. 
        /// Used to get the name and public id of the station.
        /// </summary>
        public Station DepartureStation { get; set; } = default!;
        /// <summary>
        /// Database id (not public id) of the departure station.
        /// </summary>
        [Required, ForeignKey("ArrivalStation"), Display(Name = "Arrival station")]
        public int ArrivalStationID { get; set; }
        /// <summary>
        /// Reference to the return station model, not saved to database. 
        /// Used to get the name and public id of the station.
        /// </summary>
        public Station ArrivalStation { get; set; } = default!;

        // TODO: The database expects the distance in meters in create Journey page,
        //       but we display it as kilometers in the Journey table column.
        //       This might be confusing for users? Add a toggle to display either?
        /// <summary>
        /// Distance of the journey in meters.
        /// </summary>
        [Required, Display(Name = "Distance (m)")]
        public int DistanceMeters { get; set; }
        /// <summary>
        /// Duration of the journey in seconds.
        /// </summary>
        [Required, Display(Name = "Duration")]
        public int DurationSeconds { get; set; }



        /// <summary>
        /// Returns the distance in kilometers, rounded to three decimal places.
        /// </summary>
        public float DistanceKilometers
        {
            get
            {
                // NOTE: The 1000 (meters) must be a float to get float out of the calculation.
                return float.Round(this.DistanceMeters / 1000f, 3);
            }
        }

        /// <summary>
        /// Returns the journey duration in "2m, 32s format. Don't display minutes or seconds if they are 0.
        /// </summary>
        public string DurationMinutesAndSeconds
        {
            get
            {
                // Gets the number of minutes in the duration, and then the remainder in seconds.
                // NOTE: TimeSpan was also an option, but would have needed manual fiddling anyway,
                //       if the journey was longer than an hour.
                (int minutes, int seconds) = Math.DivRem(this.DurationSeconds, 60);

                // Display only minutes if seconds are 0.
                if (minutes != 0 && seconds == 0)
                {
                    return $"{minutes}m";
                }

                // Display only seconds if minutes are 0.
                else if (minutes == 0 && seconds != 0)
                {
                    return $"{seconds}s";
                }

                // Display both minutes and seconds if neither are 0.
                else
                {
                    return $"{minutes}m, {seconds}s";
                }
            }
        } // public string DurationInMinutesAndSeconds
    } // public class Journey
}
