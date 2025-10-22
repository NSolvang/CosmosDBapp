using CosmosDBapp.Service;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(s =>
{
    var cosmosClient = new CosmosClient(
        builder.Configuration["CosmosDb:Account"],
        builder.Configuration["CosmosDb:Key"]);

    return new SupportService(
        cosmosClient,
        builder.Configuration["CosmosDb:DatabaseName"],
        builder.Configuration["CosmosDb:ContainerName"]);
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