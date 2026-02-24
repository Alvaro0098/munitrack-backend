using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IIncidenceRepository
    {
        void AddIncidence(Incidence Incidence);
        List<Incidence> GetAllIncidences();
        Incidence? GetIncidenceById(int id);
        Incidence UpdateIncidence(Incidence incidence);
        void DeleteIncidence(Incidence incidence);
    }
}
