using Solita2023Assignment.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddRazorPages();
var mvcBuilder = builder.Services.AddRazorPages();

// Use runtime compilation when developing, so we don't have to constantly rebuild when making small changes.
if (builder.Environment.IsDevelopment())
{
    mvcBuilder.AddRazorRuntimeCompilation();
}

//builder.Services.AddDbContext<Solita2023AssignmentContext>(options =>
//    options.UseSqlite(builder.Configuration.GetConnectionString("Solita2023AssignmentContext") ?? throw new InvalidOperationException("Connection string 'Solita2023AssignmentContext' not found.")));



// Register the database context we use.
builder.Services.AddDbContext<Solita2023AssignmentContext>();

// Create the database if it doesn't exist.
using (Solita2023AssignmentContext context = new Solita2023AssignmentContext())
{
    System.Diagnostics.Debug.Print("Database not found, creating a new one.");

    // Create the folder for the database.
    string databaseFolderString = Environment.CurrentDirectory + "/Database/";
    Directory.CreateDirectory(databaseFolderString);

    context.Database.EnsureCreated();
}

ParseCSVToDatabase parser = new ParseCSVToDatabase();
parser.ParseStations();
parser.ParseJourneys();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//ParseCSVToDatabase myCSVParser = new ParseCSVToDatabase();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
