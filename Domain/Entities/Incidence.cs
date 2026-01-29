using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enum; // <--- Importante para IncidenceType e IncidenceState

namespace Domain.Entities
{
    public class Incidence
    {
        [Key] // Ahora sí lo va a reconocer
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "timestamp with time zone")] // Para que Postgres no proteste
        public DateTime Date { get; set; } = DateTime.UtcNow;       
        
        public IncidenceType IncidenceType { get; set; } 
        
        public string Description { get; set; }
        
        public IncidenceState State { get; set; } = IncidenceState.Started;
        
        public int AreaId { get; set; }
        public Area Area { get; set; }
        
        public int Deleted { get; set; } = 0;
    }
}