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
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
        public string? Password { get; set; }
        [RegularExpression(@"^\d{10}$", ErrorMessage = "El teléfono debe tener exactamente 10 dígitos")]
        public string? Phone { get; set; }
        [Required]
        [StringLength(256)]
        [EmailAddress(ErrorMessage = "El email no es válido")]
        public string Email { get; set; }
        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Role Position { get; set; }
    }
}
