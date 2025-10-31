using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CosmosDBapp.Model;

public class Contact
{
    [JsonPropertyName("name")]
    [Required]
    [StringLength(15, ErrorMessage = "Name skal være mindst 2 i længde.",MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;
        
    [JsonPropertyName("email")]
    [Required]
    [StringLength(30, ErrorMessage = "Email skal være mindst 8 i længde.",MinimumLength = 8)]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("phone")]
    [Required]
    [StringLength(11, ErrorMessage = "Tlf nr skal bestå af mindst 8 tal.",MinimumLength = 8)]

    public string Phone { get; set; } = string.Empty;
}