using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Enum;

namespace Application.Dtos
{
    public class UpdateOperatorDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }
        public string Phone { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "El email no es válido")]
        public string Email { get; set; }
        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Role Position { get; set; }
    }
}
