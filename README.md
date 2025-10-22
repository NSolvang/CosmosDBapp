Lavet af: Nikolaj Solvang

Formål: Forbinde azure cosmosdb til en blazor web app, hvor vi kan vise/oprette data. 

1. Forudsætninger: Installer følgende for at gå videre.
.NET 8 SDK
Visual Studio / Rider / VS Code
Azure CLI

2. Opret Cosmos DB-konto
Du kan enten bruge en eksisterende Azure-konto eller oprette en ny til testformål:

az login
az group create --name ibas-test-rg --location westeurope
az cosmosdb create \
  --name ibas-test-account \
  --resource-group ibas-test-rg \
  --kind GlobalDocumentDB \
  --locations regionName=westeurope failoverPriority=0 isZoneRedundant=False

  3. Opret database og container
Applikationen forventer følgende struktur (kan ændres i appsettings.json):

az cosmosdb sql database create \
  --account-name ibas-test-account \
  --resource-group ibas-test-rg \
  --name IBasSupportDB

az cosmosdb sql container create \
  --account-name ibas-test-account \
  --resource-group ibas-test-rg \
  --database-name IBasSupportDB \
  --name ibassupport \
  --partition-key-path "/category"


  4. Konfigurer appsettings.json

I Blazor-projektet, opdater appsettings.json med din egen Cosmos DB URL og key:

"CosmosDb": {
  "Account": "https://ibas-test-account.documents.azure.com:443/",
  "Key": "<DIN_NØGLE_HER>",
  "DatabaseName": "IBasSupportDB",
  "ContainerName": "ibassupport"
}

5. Kør applikationen

Fra projektmappen i terminalen:
dotnet restore
dotnet build
dotnet run

Derefter kan du gå til den localhost som blazor giver dig. Nu kan du bruge applikationen. 

6.

Status: Kravene til opgaven er opfyldt.

Mangler: Man kan vel altid tilføje flere ting, men man kunne fx tilføje noget filtrering, søgefunktion, slet-knap osv. 

Næste trin: Tilføj login, så kun specifikke brugere kan se/ændre deres egne beskeder.

