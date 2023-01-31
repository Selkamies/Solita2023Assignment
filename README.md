# Solita2023Assignment

Pre-assignment for Solita Dev Academy Finland 2023. 

### Dependencies and requirements

* Microsoft Visual Studio
* ASP.NET Core
* .NET 7.0
* [CsvHelper](https://joshclose.github.io/CsvHelper/)

Dependencies
![Dependencies image missing](https://github.com/Selkamies/Solita2023Assignment/blob/master/Images/Dependencies.png?raw=true)

NuGet packages
![NuGet packages image missing](https://github.com/Selkamies/Solita2023Assignment/blob/master/Images/NuGetPackages.png?raw=true)

### Instructions

1. Download the four .csv files from the links in [assignment description](https://github.com/solita/dev-academy-2023-exercise) 
  or from links below and place them in a folder named "Data" at the root of the project. 
  Citybike journey data owned by City Bike Finland, citybike station data license [here](https://www.avoindata.fi/data/en/dataset/hsl-n-kaupunkipyoraasemat/resource/a23eef3a-cc40-4608-8aa2-c730d17e8902).

* https://dev.hsl.fi/citybikes/od-trips-2021/2021-05.csv
* https://dev.hsl.fi/citybikes/od-trips-2021/2021-06.csv
* https://dev.hsl.fi/citybikes/od-trips-2021/2021-07.csv
* https://opendata.arcgis.com/datasets/726277c507ef4914b0aec3cbcfcbfafc_0.csv

2. Open project with Visual Studio (2022?)

3. Compile and run project. On first excecution, the data from .csv files is imported to an SQLite database. On my computer this takes about 9 minutes.



# Recommended / Additional features TODO.

### CSV file -> Database importing and parsing
- [x] Import data from the CSV files to a database or in-memory storage.
- [x] Validate data before importing.
- [x] Don't import journeys that lasted for less than ten seconds.
- [x] Don't import journeys that covered distances shorter than 10 meters.
- [ ] Move .csv file names from ParseCSVToDatabase class to appsettings.json and read them from there, like with connection string.
- [ ] Lots of error handling when opening .csv files. Currently the file names and what Models are read from which file are hardcoded.
- [ ] Column ordering of database tables when the database is created at startup and not by migration.
- [ ] Instead of just checking if database exists at start, also check if it has any valid data in it.
- [ ] Tests.
- [ ] Support for future files containing journey data, don't use hardcoded names.

### Journeys
Journey index:
- [x] List journeys. If you don't implement pagination, use some hard-coded limit for the list length because showing several million rows would make any browser choke.
- [x] For each journey show departure and return stations, covered distance in kilometers and duration in minutes.
- [ ] Pagination.
- [ ] Ordering per column.
- [ ] Searching.
- [ ] Filtering.

Journey create/edit:
- [ ] When creating a new journey, don't allow the user to input the duration, but instead calculate it from departure and arrival times.
- [ ] Show the calculated duration to the user when departure or arrival time is changed. Requires Javascript.

### Stations
Station index:
- [x] List all the stations.
- [ ] Pagination.
- [ ] Searching.

Station details: 
- [x] Station name.
- [x] Station address.
- [ ] Total number of journeys starting from the station.
- [ ] Total number of journeys ending at the station.
- [ ] Station location on the map
- [ ] The average distance of a journey starting from the station
- [ ] The average distance of a journey ending at the station
- [ ] Top 5 most popular return stations for journeys starting from the station
- [ ] Top 5 most popular departure stations for journeys ending at the station
- [ ] Ability to filter all the calculations per month