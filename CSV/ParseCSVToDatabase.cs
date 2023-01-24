using CsvHelper;
using CsvHelper.Configuration;
using Solita2023Assignment.Models;
using System.Globalization;

namespace Solita2023Assignment.CSV
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
            Map(m => m.XCoordinate).Name("X");
            Map(m => m.YCoordinate).Name("Y");
        }
    }

    /// <summary>
    /// Maps the properties of Journey Model class to the columns of .csv file.
    /// </summary>
    public class JourneyMap : ClassMap<Journey>
    {
        public JourneyMap() 
        {
            Map(m => m.DepartureTime).Name("Departure");
            Map(m => m.ArrivalTime).Name("Return");
            Map(m => m.DepartureStation).Name("Departure station id");
            Map(m => m.ArrivalStation).Name("Return station id");
            Map(m => m.DistanceMeters).Name("Covered distance (m)");
            Map(m => m.DurationSeconds).Name("Duration (sec.)");
        }
    }

    enum ModelType
    {
        JOURNEY,
        STATION
    }

    /// <summary>
    /// Open .csv files with station and journey data, parse them and insert valid rows
    /// to database.
    /// TODO: Open .csv file with stations.
    /// TODO: Loop through the file and insert valid rows to database.
    /// TODO: Open .csv files with journeys.
    /// TODO: Loop through the file and ignore journeys that are shorter than 10 meters or lasted less than 10 seconds.
    /// </summary>
    public class ParseCSVToDatabase
    {
        private string CSVFolderPath = Environment.CurrentDirectory + "/CSV";
        // TODO: Save the file names in appsettings.json instead, in ConnectionStrings?
        private const string StationsCSVFileName = "Helsingin_ja_Espoon_kaupunkipyB6rA4asemat_avoin.csv";
        private const string JourneysCSV05FileName = "2021-05.csv";
        private const string JourneysCSV06FileName = "2021-06.csv";
        private const string JourneysCSV07FileName = "2021-07.csv";

        private void LoopThroughCSVRows(string fileName)
        {
            ModelType fileModelType;

            // If the first letter of the file name doesn't start with 2, it's the station file.
            if (fileName[0] != 2)   { fileModelType = ModelType.STATION; }
            else                    { fileModelType = ModelType.JOURNEY; }

            string filePath = this.CSVFolderPath + fileName;
            StreamReader streamReader = new StreamReader(path: filePath);
            // TODO: Culture ok?
            CsvReader csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

            csvReader.Read();
            csvReader.ReadHeader();

            // Read through the .csv file one row at a time (the files are large).
            if (fileModelType == ModelType.STATION)
            {
                // TODO: Can context be set here, or does it need to be set earlier?
                csvReader.Context.RegisterClassMap<StationMap>();
                this.ReadStationRows(reader: csvReader);
            }

            else
            {
                csvReader.Context.RegisterClassMap<JourneyMap>();
                this.ReadJourneyRows(reader: csvReader);
            }
        }

        // TODO: Any way to generalize this so that we would only need separate methods for 
        //       parsing a single row of either station or journey?
        private void ReadStationRows(CsvHelper.CsvReader reader)
        {
            while (reader.Read())
            {
                // Load a csv row to a Station model class using previously registered StationMap class mapping.
                Station? row = reader.GetRecord<Station>();

                if (row != null)
                {
                    // TODO: Can we make this more generic, so that we can use this for both journeys and stations?
                    //       Pass a type or an enum corresponding to type or something.
                    if (this.ParseStationRow(stationRow: row))
                    {
                        // TODO: Row is valid, insert it into the database.
                    }

                    else
                    {
                        // TODO: Row is not valid, increase a counter or something.
                    }
                }
            }
        }

        private void ReadJourneyRows(CsvHelper.CsvReader reader)
        {
            while (reader.Read())
            {
                // Load a csv row to a Station model class using previously registered StationMap class mapping.
                Journey? row = reader.GetRecord<Journey>();

                if (row != null)
                {
                    // TODO: Can we make this more generic, so that we can use this for both journeys and stations?
                    //       Pass a type or an enum corresponding to type or something.
                    if (this.ParseJourneyRow(journeyRow: row))
                    {
                        // TODO: Row is valid, insert it into the database.
                    }

                    else
                    {
                        // TODO: Row is not valid, increase a counter or something.
                    }
                }
            }
        }

        private bool ParseStationRow(Station stationRow)
        {
            return false;
        }

        private bool ParseJourneyRow(Journey journeyRow)
        {
            return false;
        }
    }
}
