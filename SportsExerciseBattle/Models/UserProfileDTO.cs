using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Models
{
    public class UserProfileDTO
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("Bio")]
        public string Bio { get; set; } = string.Empty;
        [JsonPropertyName("Image")]
        public string Image { get; set; } = string.Empty;
        public override string ToString()
        {
            return $"Name: {Name}, Bio: {Bio}, Image: {Image}";
        }
    }
}
