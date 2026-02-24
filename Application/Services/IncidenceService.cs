using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces;

namespace Application.Services
{
    public class IncidenceService : IIncidenceService
    {
        public readonly IIncidenceRepository _incidenceRepository;
        public IncidenceService (IIncidenceRepository incidenceRepository)
        {
            _incidenceRepository = incidenceRepository;
        }

        public void CreateIncidence(CreateIncidenceDTO Dto, int operatorId)
        {
            Incidence newIncidence = new Incidence()
            {
                Date = Dto.Date,
                IncidenceType = Dto.IncidenceType,
                Description = Dto.Description,
                State = Dto.State,
                AreaId = Dto.AreaId,
                OperatorId = operatorId
            };
            _incidenceRepository.AddIncidence(newIncidence);
        }

        public List<Incidence> GetIncidences()
        {
            return _incidenceRepository.GetAllIncidences();
        }

        public Incidence UpdateIncidence(int id, UpdateIncidenceDTO Dto)
        {
            var updatedIncidence = _incidenceRepository.GetIncidenceById(id);
            if (updatedIncidence is null)
                throw new Exception("Incidencia no encontrada");

            updatedIncidence.State = Dto.State;
            updatedIncidence.Description = Dto.Description;

            _incidenceRepository.UpdateIncidence(updatedIncidence);
            return updatedIncidence;
        }

        public bool DeleteIncidence(int id)
        {
            var incidenceDelete = _incidenceRepository.GetIncidenceById(id);
            if (incidenceDelete is null)
                return false;

            incidenceDelete.Deleted = 1;
            _incidenceRepository.UpdateIncidence(incidenceDelete);
            return true;
        }
    }
}
