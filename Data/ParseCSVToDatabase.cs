using CsvHelper;
using Solita2023Assignment.Models;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Solita2023Assignment.Data
{
    /// <summary>
    /// Open .csv files with station and journey data, parse them and insert valid rows
    /// to database.
    /// </summary>
    public class ParseCSVToDatabase
    {
        private string CSVFilePath = Environment.CurrentDirectory + "/Data/";

        // TODO: Save the file names in appsettings.json instead?
        private const string StationsCSVFileName = "Helsingin_ja_Espoon_kaupunkipyB6rA4asemat_avoin.csv";
        private const string JourneysCSV05FileName = "2021-05.csv";
        private const string JourneysCSV06FileName = "2021-06.csv";
        private const string JourneysCSV07FileName = "2021-07.csv";

        /// <summary>
        /// Dictionary of Station public id keys paired with station database id values.
        /// </summary>
        private Dictionary<int, int>? StationIDs;



        /// <summary>
        /// Parse the .csv file with citybike stations to Station model and insert them to the database.
        /// </summary>
        public void ParseStations()
        {
            // Read all station public ids and database ids to a dictionary.
            using (Solita2023AssignmentContext context = new Solita2023AssignmentContext())
            {
                // Only bother reading from the .csv file and inserting Stations to the database
                // if the Station database table is empty.
                if (!context.Station.Any())
                {
                    System.Diagnostics.Debug.Print("\nStarting to parse Stations.\n");

                    StationCSVToDatabase(fileName: StationsCSVFileName, dbContext: context);                    
                }
            }
        }

        /// <summary>
        /// Read though the .csv file with citybike stations and insert them to the database.
        /// </summary>
        /// <param name="fileName">File name of the .csv file with Stations.</param>
        /// <param name="dbContext">Database context for Entity Framework to use when INSERTing Stations to database.</param>
        public void StationCSVToDatabase(string fileName, Solita2023AssignmentContext dbContext)
        {
            string filePath = CSVFilePath + fileName;

            System.Diagnostics.Debug.Print($"\nFile path set to {filePath}.\n");

            if (!File.Exists(filePath))
            {
                System.Diagnostics.Debug.Print($"File at {filePath} not found.\n");
                return;
            }

            StreamReader streamReader = new StreamReader(path: filePath);
            CsvReader csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

            csvReader.Read();
            csvReader.ReadHeader();

            csvReader.Context.RegisterClassMap<StationMap>();

            while (csvReader.Read())
            {
                Station? station = csvReader.GetRecord<Station>();

                if (station != null && ValidateRow(station))
                {
                    dbContext.Add(station);
                }
            }

            System.Diagnostics.Debug.Print($"Inserting the stations in {fileName} to database.\n");
            dbContext.SaveChanges();
        }



        /// <summary>
        /// Parse the .csv files with journeys between citybike stations to Journey model
        /// and insert them to database.
        /// </summary>
        public void ParseJourneys()
        {
            using (Solita2023AssignmentContext context = new Solita2023AssignmentContext())
            {
                // Only bother reading and inserting Journeys if the Journey table is empty.
                if (!context.Journey.Any()) 
                {
                    System.Diagnostics.Debug.Print("\nStarting to parse Journeys.\n");

                    // Create a dictionary of Station public ids as key and their database id as value.
                    // Journeys store the public ids of departure and arrival stations, but we want to change
                    // those to database primary key ids.
                    this.StationIDs = context.Station.Select(s => new { s.PublicStationID, s.ID }).ToDictionary(s => s.PublicStationID, s => s.ID);

                    JourneyCSVToDatabase(JourneysCSV05FileName, context);
                    JourneyCSVToDatabase(JourneysCSV06FileName, context);
                    JourneyCSVToDatabase(JourneysCSV07FileName, context);
                }
            }
        }

        public void JourneyCSVToDatabase(string fileName, Solita2023AssignmentContext dbContext)
        {
            if (this.StationIDs == null)
            {
                System.Diagnostics.Debug.Print($"ERROR: JourneyCSVToDatabase.StationIDs is null when trying to add Journeys to a database. ");
                return;
            }

            // Successfully parsed journeys.
            int successCounter = 0;
            int distanceOrDurationFailedCounter = 0;
            int validateFailedCounter = 0;
            // The public station id of departure or arrival station id failed to match any station.
            int idFailedCounter = 0;
            int nullCounter = 0;
            int totalRows;
            int totalFails;

            string filePath = CSVFilePath + fileName;

            System.Diagnostics.Debug.Print($"\nFile path set to {filePath}.\n");

            if (!File.Exists(filePath))
            {
                System.Diagnostics.Debug.Print($"File at {filePath} not found.\n");
                return;
            }

            StreamReader streamReader = new StreamReader(path: filePath);
            CsvReader csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

            csvReader.Read();
            csvReader.ReadHeader();

            csvReader.Context.RegisterClassMap<JourneyMap>();

            while (csvReader.Read())
            {
                Journey? journey = csvReader.GetRecord<Journey>();

                if (journey == null)
                {
                    nullCounter++;
                    continue;
                }

                if (journey.DistanceMeters < 10 || journey.DurationSeconds < 10)
                {
                    distanceOrDurationFailedCounter++;
                    continue;
                }

                if (!ValidateRow(journey))
                {
                    validateFailedCounter++;
                    continue;
                }

                // Check if there is a match for the public departure and arrival station ids in the dictionary
                // that matches them with the stations' database ids.
                if (this.StationIDs.TryGetValue(journey.DepartureStationID, out int foundDepartureStationDBID)
                    && this.StationIDs.TryGetValue(journey.ArrivalStationID, out int foundArrivalStationDBID))
                {
                    // Change the public ids to database ids.
                    journey.DepartureStationID = foundDepartureStationDBID;
                    journey.ArrivalStationID = foundArrivalStationDBID;
                }

                else
                {
                    idFailedCounter++;
                    continue;
                }

                // Add the row to a pending list that will be inserted to database later.
                dbContext.Add(journey);

                successCounter++;

                // Every 100 000 successful rows insert them to the database and print success/fail information.
                // This takes about ~9 minutes and uses ~3GB of memory.
                // Doing this every 10 000 rows doubles the time to ~20 minutes, and uses ~2GB of memory.
                // Doing this in one batch after every file (~1 000 000 rows) takes ~8 minutes and uses ~6GB of memory.
                if (successCounter % 100000 == 0)
                {
                    totalRows = successCounter + nullCounter + distanceOrDurationFailedCounter + validateFailedCounter + idFailedCounter;
                    totalFails = totalRows - successCounter;

                    System.Diagnostics.Debug.Print($"Successes: {successCounter}/{totalRows}.");
                    System.Diagnostics.Debug.Print($"Fails: {totalFails}/{totalRows}.");
                    System.Diagnostics.Debug.Print($"Null fails: {nullCounter}.");
                    System.Diagnostics.Debug.Print($"Distance or duration fails: {distanceOrDurationFailedCounter}.");
                    System.Diagnostics.Debug.Print($"Validation fails: {validateFailedCounter}.");
                    System.Diagnostics.Debug.Print($"Id fails: {idFailedCounter}.\n");

                    System.Diagnostics.Debug.Print($"Inserting 100 000 journeys to database...\n");

                    // Applies the changes to the database.
                    dbContext.SaveChanges();
                }
            } // while (csvReader.Read())

            totalRows = successCounter + nullCounter + distanceOrDurationFailedCounter + validateFailedCounter + idFailedCounter;
            totalFails = totalRows - successCounter;

            // TODO: Move the parsing counters to an object, and pass it to a method that prints this.
            System.Diagnostics.Debug.Print($"\n----- FILE: {fileName} FINISHED! -----");
            System.Diagnostics.Debug.Print($"Successes: {successCounter}/{totalRows}.");
            System.Diagnostics.Debug.Print($"Fails: {totalFails}/{totalRows}.");
            System.Diagnostics.Debug.Print($"Null fails: {nullCounter}.");
            System.Diagnostics.Debug.Print($"Distance or duration fails: {distanceOrDurationFailedCounter}.");
            System.Diagnostics.Debug.Print($"Validation fails: {validateFailedCounter}.");
            System.Diagnostics.Debug.Print($"Id fails: {idFailedCounter}.");
            System.Diagnostics.Debug.Print($"----- FILE: {fileName} FINISHED! -----\n");

            System.Diagnostics.Debug.Print($"Inserting remaining the journeys in {fileName} to database.\n");
            dbContext.SaveChanges();
        } // JourneyCSVToDatabase()



        /// <summary>
        /// Validate the Station or Journey row that csvReader read from .csv file.
        /// TODO: Change name to RowIsValid?
        /// </summary>
        /// <param name="modelRow">Entity Framework model object with row data.</param>
        /// <returns>True if the row is valid, false if not.</returns>
        public bool ValidateRow(object modelRow)
        {
            List<ValidationResult> validationErrors = new List<ValidationResult>();

            if (!Validator.TryValidateObject(instance: modelRow,
                                             validationContext: new ValidationContext(modelRow),
                                             validationResults: validationErrors,
                                             validateAllProperties: true))
            {
                return false;
            }

            else
            {
                return true;
            }
        } // ValidateRow()
    }
}
