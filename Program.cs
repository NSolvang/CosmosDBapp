using CosmosDBapp.Service;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

// Tilføj User Secrets, hvis i Development
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Hent Cosmos DB settings fra konfigurationen
var cosmosSection = builder.Configuration.GetSection("CosmosDb");
string? connectionString = cosmosSection["ConnectionString"];
string? account = cosmosSection["Account"];
string? key = cosmosSection["Key"];
string? databaseName = cosmosSection["DatabaseName"];
string? containerName = cosmosSection["ContainerName"];

// Registrer SupportService med CosmosClient
builder.Services.AddSingleton(s =>
{
    CosmosClient cosmosClient;

    if (!string.IsNullOrEmpty(connectionString))
    {
        // Brug connection string, hvis den findes
        cosmosClient = new CosmosClient(connectionString);
    }
    else if (!string.IsNullOrEmpty(account) && !string.IsNullOrEmpty(key))
    {
        // Brug account + key, hvis connection string ikke findes
        cosmosClient = new CosmosClient(account, key);
    }
    else
    {
        throw new Exception("Cosmos DB konfiguration mangler. Tjek appsettings.json eller secrets.json.");
    }

    return new SupportService(cosmosClient, databaseName!, containerName!);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();