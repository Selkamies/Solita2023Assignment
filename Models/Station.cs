using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using static System.Collections.Specialized.BitVector32;
using System.Text;

namespace Solita2023Assignment.Models
{
    // TODO: Add more annotations, such as Required, maxlength.
    // TODO: StationId should be unique. 
    // TODO: Localization/translation. 
    // TODO: Is FId needed?
    // TODO: Can be null in .csv: City names, operator.

    // TODO: Total number of journeys starting from station.
    // TODO: Total number of journeys ending at station.
    // TODO: Map location.
    // TODO: The average distance of a journey starting from the station
    // TODO: The average distance of a journey ending at the station
    // TODO: Top 5 most popular return stations for journeys starting from the station
    // TODO: Top 5 most popular departure stations for journeys ending at the station
    // TODO: Filter the above per month.

    /// <summary>
    /// A citybike station in Helsinki/Espoo area.
    /// TODO: Max length for strings?
    /// </summary>
    public class Station
    {
        /// <summary>
        /// Database id number of the station.
        /// </summary>
        [Key]
        public int ID { get; set; }

        // No idea what FId is, seems to count up from 1, so the same as database id number? Not needed?
        // public int FId { get; set; }

        /// <summary>
        /// Public id number of the station. Id in the .csv file.
        /// </summary>
        [Required, Display(Name = "Station Id")]
        public int PublicStationID { get; set; }

        /// <summary>
        /// Finnish name of the station.
        /// </summary>
        [Required, Display(Name = "Nimi")]
        public string NameFI { get; set; } = string.Empty;
        /// <summary>
        /// Swedish name of the station.
        /// </summary>
        [Required, Display(Name = "Namn")]
        public string NameSV { get; set; } = string.Empty;

        /// <summary>
        /// Finnish address of the station.
        /// </summary>
        [Required, Display(Name = "Osoite")]
        public string AddressFI { get; set; } = string.Empty;
        /// <summary>
        /// Swedish address of the station.
        /// </summary>
        [Required, Display(Name = "Adress")]
        public string AddressSV { get; set; } = string.Empty;

        /// <summary>
        /// Finnish name of the city where the station is in.
        /// </summary>
        [Display(Name = "Kaupunki")]
        public string CityFI { get; set; } = string.Empty;
        /// <summary>
        /// Swedish name of the city where the station is in.
        /// </summary>
        [Display(Name = "Stad")]
        public string CitySV { get; set; } = string.Empty;

        /// <summary>
        /// Name of the company that operates the station.
        /// </summary>
        public string Operator { get; set; } = string.Empty;
        /// <summary>
        /// Maximum capacity for citybikes at the station.
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Map X-coordinate (Longitude) of the station.
        /// </summary>
        [Required, Display(Name = "X (Longtitude)")]
        public float XCoordinate { get; set; }
        /// <summary>
        /// Map Y-coordinate (Latitude) of the station.
        /// </summary>
        [Required, Display(Name = "Y (Latitude)")]
        public float YCoordinate { get; set; }
    }
}
