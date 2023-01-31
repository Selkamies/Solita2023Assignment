using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Microsoft.IdentityModel.Tokens;
using Solita2023Assignment.Models;

namespace Solita2023Assignment.Data
{
    /// <summary>
    /// Maps the properties of Journey Model class to the columns of .csv file for CsvHelper.
    /// </summary>
    public class JourneyMap : ClassMap<Journey>
    {
        public JourneyMap()
        {
            Map(m => m.DepartureTime).Name("Departure");
            Map(m => m.ArrivalTime).Name("Return");
            // The .csv files have public station id's, we have to find relevant database id.
            Map(m => m.DepartureStationID).Name("Departure station id");
            Map(m => m.ArrivalStationID).Name("Return station id");
            // There are a bunch of rows with distance of xxx6.67 meters.
            Map(m => m.DistanceMeters).Name("Covered distance (m)").TypeConverter<FloatDistanceConverter>();
            Map(m => m.DurationSeconds).Name("Duration (sec.)");
        }
    }

    /// <summary>
    /// Custom converter for distances, they should be ints, but some are empty and 
    /// some are floats that end with 6.67.
    /// </summary>
    public class FloatDistanceConverter : Int32Converter
    {
        public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrEmpty(text) || text == "0")
            {
                return null;
            }

            else
            {
                /*string splitText = text.Split('.')[0];

                if (splitText == "0")
                {
                    return null;
                }*/

                return base.ConvertFromString(text.Split('.')[0], row, memberMapData);
            }
        }
    }
}
