using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Solita2023Assignment.Models;

namespace Solita2023Assignment.Data
{
    /// <summary>
    /// Maps the properties of Journey Model class to the columns of .csv file.
    /// </summary>
    public class JourneyMap : ClassMap<Journey>
    {
        public JourneyMap()
        {
            Map(m => m.DepartureTime).Name("Departure");
            Map(m => m.ArrivalTime).Name("Return");
            // TODO: The .csv files have public station id's, we have to find relevant database id.
            Map(m => m.DepartureStationID).Name("Departure station id");
            Map(m => m.ArrivalStationID).Name("Return station id");
            // TODO: There are a bunch of rows with distance of xxx6.67 meters.
            Map(m => m.DistanceMeters).Name("Covered distance (m)").TypeConverter<FloatDistanceConverter>();
            Map(m => m.DurationSeconds).Name("Duration (sec.)");
        }
    }

    public class FloatDistanceConverter : Int32Converter
    {
        public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            else
            {
                return base.ConvertFromString(text.Split('.')[0], row, memberMapData);
            }
        }
    }

    /*public class FloatDistanceConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return text.Equals(int.Parse(text.Split(".")[0]));
            //return text.Equals("Yes", StringComparison.OrdinalIgnoreCase);
        }
    }*/
}
