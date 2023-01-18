using System.ComponentModel.DataAnnotations;

namespace Solita2023Assignment.Models
{
    // TODO: Add more annotations.

    /// <summary>
    /// A single journey between two citybike stations.
    /// </summary>
    public class Journey
    {
        /// <summary>
        /// Database id number of the journey.
        /// </summary>
        public int Id { get; set; }

        // TODO: Do we need the datatype annotation?
        //[DataType(DataType.DateTime)]
        public DateTime DepartureTime { get; set; }
        public DateTime ReturnTime { get; set; }

        [Required]
        public Station? DepartureStation { get; set; }
        [Required]
        public Station? ReturnStation { get; set; }

        public int DistanceMeters { get; set; }
        public int DurationSeconds { get; set; }
    }
}
