using System.ComponentModel.DataAnnotations;

namespace Solita2023Assignment.Models
{
    // TODO: Add more annotations, such as Required, maxlength.
    // TODO: StationId should be unique. 
    // TODO: Localization/translation. 
    // TODO: Is FId needed?
    // TODO: Can be null in .csv: City names, operator.

    /// <summary>
    /// A citybike station in Helsinki/Espoo area.
    /// </summary>
    public class Station
    {
        /// <summary>
        /// Database id number of the station.
        /// </summary>
        public int Id { get; set; }

        // No idea what FId is, seems to count up from 1, so the same as database id number? Not needed?
        // public int FId { get; set; }

        /// <summary>
        /// Public id number of the station. Id in the .csv file.
        /// </summary>
        [Display(Name = "Station Id")]
        public int PublicStationId { get; set; }

        /// <summary>
        /// Finnish name of the station.
        /// </summary>
        [Display(Name = "Nimi")]
        public string NameFI { get; set; } = string.Empty;
        /// <summary>
        /// Swedish name of the station.
        /// </summary>
        [Display(Name = "Namn")]
        public string NameSV { get; set; } = string.Empty;

        /// <summary>
        /// Finnish address of the station.
        /// </summary>
        [Display(Name = "Osoite")]
        public string AddressFI { get; set; } = string.Empty;
        /// <summary>
        /// Swedish address of the station.
        /// </summary>
        [Display(Name = "Adress")]
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
        [Display(Name = "X (Longtitude)")]
        public float XCoordinate { get; set; }
        /// <summary>
        /// Map Y-coordinate (Latitude) of the station.
        /// </summary>
        [Display(Name = "Y (Latitude)")]
        public float YCoordinate { get; set; }
    }
}
