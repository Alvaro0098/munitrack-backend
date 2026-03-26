using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class CreateCitizenDto
    {
        [Required]
        [Range(1000000, 99999999, ErrorMessage = "El DNI debe tener entre 7 y 8 dígitos")]
        public int DNI { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(100)]
        public string LastName { get; set; }
        [Required]
        [StringLength(200)]
        public string Adress { get; set; }
        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "El teléfono debe tener exactamente 10 dígitos")]
        public string Phone { get; set; }
        [Required]
        [StringLength(256)]
        public string Email { get; set; }
  

    }
}
