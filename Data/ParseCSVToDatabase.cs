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
                    System.Diagnostics.Debug.Print("Starting to parse Stations.");

                    StationCSVToDatabase(fileName: StationsCSVFileName, dbContext: context);

                    // Read the database ids (generated during the last step when calling SaveChanges())
                    // and public ids of the stations, and save them to a dictionary. We will need to 
                    // find the database id of a station using it's public id when inserting Journeys.
                    // TODO: Just keep the Station models in memory, there aren't that many of them?
                    
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

            System.Diagnostics.Debug.Print($"File path set to {filePath}.");

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
                    System.Diagnostics.Debug.Print("Starting to parse Journeys.");

                    // Create a dictionary of Station public ids as key and their database id as value.
                    // Journeys store the public ids of departure and arrival stations, but we want to change
                    // those to database primary key ids.
                    this.StationIDs = context.Station.Select(s => new { s.PublicStationID, s.ID }).ToDictionary(s => s.PublicStationID, s => s.ID);

                    JourneyCSVToDatabase(JourneysCSV05FileName, context);
                    JourneyCSVToDatabase(JourneysCSV06FileName, context);
                    JourneyCSVToDatabase(JourneysCSV07FileName, context);
                }
            }

            // TODO: Change the DepartureStationID and ArrivalStationID of each station to database station ids.
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

            System.Diagnostics.Debug.Print($"File path set to {filePath}.");

            StreamReader streamReader = new StreamReader(path: filePath);
            CsvReader csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

            csvReader.Read();
            csvReader.ReadHeader();

            csvReader.Context.RegisterClassMap<JourneyMap>();

            while (csvReader.Read())
            {
                Journey? journey = csvReader.GetRecord<Journey>();

                if (journey != null )
                {
                    // Don't bother adding journeys with distances less than 10 meters or duration less than 10 seconds.
                    if (journey.DistanceMeters >= 10 && journey.DurationSeconds >= 10)
                    {
                        if (ValidateRow(journey))
                        {
                            // Link existing station to the journey's DepatureStation and ArrivalStation properties.
                            // The journey loaded from .csv file have stored the public ids for these stations, but this
                            // allows us to change those to use database ids.
                            if(this.StationIDs.TryGetValue(journey.DepartureStationID, out int foundDepartureStationDBID)
                               && this.StationIDs.TryGetValue(journey.ArrivalStationID, out int foundArrivalStationDBID))
                            {
                                journey.DepartureStationID = foundDepartureStationDBID;
                                journey.ArrivalStationID = foundArrivalStationDBID;
                            }

                            else
                            {
                                idFailedCounter++;
                                continue;
                            }

                            dbContext.Add(journey);

                            successCounter++;

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

                                System.Diagnostics.Debug.Print($"Inserting to database...\n");
                                dbContext.SaveChanges();
                            }
                        }

                        else
                        {
                            validateFailedCounter++;
                            continue;
                        }
                    } // if (journey.DistanceMeters >= 10 && journey.DurationSeconds >= 10)

                    else
                    {
                        distanceOrDurationFailedCounter++;
                        continue;
                    }
                } // if (journey != null)

                else
                {
                    nullCounter++;
                    continue;
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

            // TODO: Add stuff to database in smaller batches? Every 10k or 100k rows?
            System.Diagnostics.Debug.Print($"Inserting the journeys in {fileName} to database.\n");
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
            // TODO: Create the list once and just clear it here?
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
