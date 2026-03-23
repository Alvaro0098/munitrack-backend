using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IIncidenceService
    {
        void CreateIncidence(CreateIncidenceDTO Dto, int operatorId);
        List<Incidence> GetIncidences();
        List<Incidence> GetDeletedIncidences();
        Incidence UpdateIncidence(int id, UpdateIncidenceDTO Dto);
        bool DeleteIncidence(int id);
        bool RestoreIncidence(int id);
    }
}
