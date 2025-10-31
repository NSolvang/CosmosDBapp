using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CosmosDBapp.Model
{
    public class SupportMessage : IValidatableObject
    {
        [Required]
        [JsonPropertyName("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [JsonPropertyName("contact")]
        [Required]
        public Contact Contact { get; set; } = new Contact();
        
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;
        
        [JsonPropertyName("dateTime")]
        public DateTime DateTime { get; set; } = DateTime.UtcNow;

        
        //Den ville ikke lade mig bruge [ValidateComplexType] til min nested Contact object,
        //så fandt den her løsning i stedet.
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            
            if (Contact != null)
            {
                var context = new ValidationContext(Contact);
                Validator.TryValidateObject(Contact, context, results, true);
            }
            
            return results;
        }
    }
}