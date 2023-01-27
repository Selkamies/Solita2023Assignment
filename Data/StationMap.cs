using CsvHelper.Configuration;
using Solita2023Assignment.Models;

namespace Solita2023Assignment.Data
{
    /// <summary>
    /// Maps the properties of Station Model class to the columns of .csv file.
    /// </summary>
    public class StationMap : ClassMap<Station>
    {
        public StationMap()
        {
            Map(m => m.PublicStationID).Name("ID");
            Map(m => m.NameFI).Name("Nimi");
            Map(m => m.NameSV).Name("Namn");
            Map(m => m.AddressFI).Name("Osoite");
            Map(m => m.AddressSV).Name("Adress");
            Map(m => m.CityFI).Name("Kaupunki");
            Map(m => m.CitySV).Name("Stad");
            Map(m => m.Operator).Name("Operaattor");
            Map(m => m.Capacity).Name("Kapasiteet");
            Map(m => m.XCoordinate).Name("x");
            Map(m => m.YCoordinate).Name("y");
        }
    }
}
