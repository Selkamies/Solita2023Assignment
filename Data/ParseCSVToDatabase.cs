using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Elfie.Model.Tree;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using Microsoft.EntityFrameworkCore;
using Solita2023Assignment.Models;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;
using System.Globalization;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace Solita2023Assignment.Data
{
    /// <summary>
    /// Open .csv files with station and journey data, parse them and insert valid rows
    /// to database.
    /// </summary>
    public class ParseCSVToDatabase
    {
        private string CSVFolderPath = Environment.CurrentDirectory + "/CSV/";
        // TODO: Save the file names in appsettings.json instead, in ConnectionStrings?
        private const string StationsCSVFileName = "Helsingin_ja_Espoon_kaupunkipyB6rA4asemat_avoin.csv";
        private const string JourneysCSV05FileName = "2021-05.csv";
        private const string JourneysCSV06FileName = "2021-06.csv";
        private const string JourneysCSV07FileName = "2021-07.csv";

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
            string filePath = CSVFolderPath + fileName;

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
                // TODO: Doesn't work?
                if (!context.Journey.Any())

                    System.Diagnostics.Debug.Print("Starting to parse Stations.");
                {
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

            int successCounter = 0;
            int distanceOrDurationFailedCounter = 0;
            int validateFailedCounter = 0;
            int idFailedCounter = 0;
            int nullCounter = 0;
            int totalRows;
            int totalFails;

            string filePath = CSVFolderPath + fileName;

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
                    if (journey.DistanceMeters >= 10 || journey.DurationSeconds >= 10)
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

                            if (successCounter % 10000 == 0)
                            {
                                totalRows = successCounter + nullCounter + distanceOrDurationFailedCounter + validateFailedCounter + idFailedCounter;
                                totalFails = totalRows - successCounter;

                                System.Diagnostics.Debug.Print($"Successes: {successCounter}/{totalRows}.");
                                System.Diagnostics.Debug.Print($"Fails: {totalFails}/{totalRows}.");
                                System.Diagnostics.Debug.Print($"Null fails: {nullCounter}.");
                                System.Diagnostics.Debug.Print($"Distance or duration fails: {distanceOrDurationFailedCounter}.");
                                System.Diagnostics.Debug.Print($"Validation fails: {validateFailedCounter}.");
                                System.Diagnostics.Debug.Print($"Id fails: {idFailedCounter}.");
                            }
                        }

                        else
                        {
                            validateFailedCounter++;
                            continue;
                        }
                    }

                    else
                    {
                        distanceOrDurationFailedCounter++;
                    }
                }

                else
                {
                    nullCounter++;
                }
            } // while (csvReader.Read())

            totalRows = successCounter + nullCounter + distanceOrDurationFailedCounter + validateFailedCounter + idFailedCounter;
            totalFails = totalRows - successCounter;

            System.Diagnostics.Debug.Print($"\n----- FILE: {fileName} FINISHED! -----");
            System.Diagnostics.Debug.Print($"Successes: {successCounter}/{totalRows}.");
            System.Diagnostics.Debug.Print($"Fails: {totalFails}/{totalRows}.");
            System.Diagnostics.Debug.Print($"Null fails: {nullCounter}.");
            System.Diagnostics.Debug.Print($"Distance or duration fails: {distanceOrDurationFailedCounter}.");
            System.Diagnostics.Debug.Print($"Validation fails: {validateFailedCounter}.");
            System.Diagnostics.Debug.Print($"Id fails: {idFailedCounter}.");
            System.Diagnostics.Debug.Print($"----- FILE: {fileName} FINISHED! -----\n");

            // TODO: Add stuff to database in smaller batches? Every 10k or 100k rows?
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
            // TODO: Create the list once and just clear it here.
            List<ValidationResult> validationErrors = new List<ValidationResult>();
            if (!Validator.TryValidateObject(instance: modelRow,
                                             validationContext: new ValidationContext(modelRow),
                                             validationResults: validationErrors,
                                             validateAllProperties: true))
            {
                System.Diagnostics.Debug.Print("Row failed to validate: ");

                foreach (ValidationResult error in validationErrors)
                {
                    System.Diagnostics.Debug.Print(error.ErrorMessage);
                }

                return false;
            }

            else
            {
                return true;
            }
        } // ValidateRow()







        /*public bool ParseRow(Station modelRow)
        {
            return ValidateRow(modelRow);
        }

        public bool ParseRow(Journey modelRow)
        {
            // Don't bother adding journeys with distances less than 10 meters or duration less than 10 seconds.
            if (modelRow.DistanceMeters < 10 || modelRow.DurationSeconds < 10)
            {
                return false;
            }

            return ValidateRow(modelRow);
        }*/







        // TODO: Any way to generalize this so that we would only need separate methods for 
        //       parsing a single row of either station or journey?
        /*private void ReadStationRows(CsvReader reader)
        {
            int validRowCounter = 0;
            int invalidRowCounter = 0;

            while (reader.Read())
            {
                // Load a csv row to a Station model class using previously registered StationMap class mapping.
                Station? row = reader.GetRecord<Station>();

                if (row != null)
                {
                    // TODO: Can we make this more generic, so that we can use this for both journeys and stations?
                    //       Pass a type or an enum corresponding to type or something.
                    if (ParseStationRow(stationRow: row))
                    {
                        // TODO: Row is valid, insert it into the database.
                        validRowCounter++;
                    }

                    else
                    {
                        // TODO: Row is not valid, increase a counter or something.
                        invalidRowCounter++;
                    }
                }
            } // while (reader.Read())

            System.Diagnostics.Debug.Print($"\nValid rows: {validRowCounter}, invalid rows: {invalidRowCounter}.\n");
        }

        private void ReadJourneyRows(CsvReader reader)
        {
            while (reader.Read())
            {
                // Load a csv row to a Station model class using previously registered StationMap class mapping.
                Journey? row = reader.GetRecord<Journey>();

                if (row != null)
                {
                    // TODO: Can we make this more generic, so that we can use this for both journeys and stations?
                    //       Pass a type or an enum corresponding to type or something.
                    if (ParseJourneyRow(journeyRow: row))
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

        /// <summary>
        /// Parse station rows by validating the data read to Station model object.
        /// </summary>
        /// <param name="stationRow">The Station model object containing data read from the .csv row.</param>
        /// <returns>True if the row data vas validated, false if not.</returns>
        private bool ParseStationRow(Station stationRow)
        {
            List<ValidationResult> validationErrors = new List<ValidationResult>();
            if (!Validator.TryValidateObject(instance: stationRow,
                                             validationContext: new ValidationContext(stationRow),
                                             validationResults: validationErrors,
                                             validateAllProperties: true))
            {
                foreach (ValidationResult error in validationErrors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                return false;
            }

            else
            {
                return true;
            }
        } // ParseStationRow()

        private bool ParseJourneyRow(Journey journeyRow)
        {
            List<ValidationResult> validationErrors = new List<ValidationResult>();
            if (!Validator.TryValidateObject(instance: journeyRow,
                                             validationContext: new ValidationContext(journeyRow),
                                             validationResults: validationErrors,
                                             validateAllProperties: true))
            {
                foreach (ValidationResult error in validationErrors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                return false;
            }

            else
            {
                return true;
            }
        } // ParseJourneyRow()*/
    }
}
