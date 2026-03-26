using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class AreaService : IAreaService
    {
        private readonly IAreaRepository _areaRepository;
        private readonly IIncidenceRepository _incidenceRepository;

        public AreaService(IAreaRepository areaRepository, IIncidenceRepository incidenceRepository)
        {
            _areaRepository = areaRepository;
            _incidenceRepository = incidenceRepository;
        }
        public List<Area> GetAreas()
        {
            return _areaRepository.Get();
        }
        public void CreateArea(CreateAndUpdateAreaDTO Dto)
        {
            var existingArea = _areaRepository.GetAreaByName(Dto.Name);
            if (existingArea != null)
                throw new Exception($"Ya existe ése nombre del Area");

            Area newArea = new Area()
            {
                Name = Dto.Name,
                Description = Dto.Description,
            };
            _areaRepository.AddArea(newArea);
        }
        public Area? GetAreaByName(string name)
        {
            return _areaRepository.GetAreaByName(name);
        }


        public bool DeleteArea(int id)
        {
            var areaDelete = _areaRepository.GetAreaById(id);
            if (areaDelete is null)
                return false;

            areaDelete.Deleted = 1;
            _areaRepository.UpdateArea(areaDelete);
            return true;
        }

        public Area? GetAreaById(int id)
        {
            return _areaRepository.GetAreaById(id);
        }

        public Area UpdateArea(int id, CreateAndUpdateAreaDTO Dto)
        {
            var areaEntity = _areaRepository.GetAreaById(id);
            if (areaEntity is null)
                throw new Exception("Area no encontrado");

            areaEntity.Name = Dto.Name;
            areaEntity.Description = Dto.Description;

            _areaRepository.UpdateArea(areaEntity);
            return areaEntity;
        }

    }
}
