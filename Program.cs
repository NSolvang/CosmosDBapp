using CosmosDBapp.Service;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

// Tilføj User Secrets (hvis vi er i Development)
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Opret CosmosClient og SupportService
builder.Services.AddSingleton(s =>
{
    var cosmosSection = builder.Configuration.GetSection("CosmosDb");

    string account = cosmosSection["Account"];                   // fra appsettings.json
    string key = cosmosSection["Key"];                             // fra User Secrets
    string databaseName = cosmosSection["DatabaseName"];           // fra appsettings.json
    string containerName = cosmosSection["ContainerName"];         // fra appsettings.json

    var cosmosClient = new CosmosClient(account, key);

    return new SupportService(cosmosClient, databaseName, containerName);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseRouting();
app.UseHttpsRedirection();

app.MapControllers();
app.MapBlazorHub();                  
app.MapFallbackToPage("/_Host");    

app.Run();