# TODO list for Solita2023Assignment

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

Station dtails: 
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