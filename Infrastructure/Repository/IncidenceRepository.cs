using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class IncidenceRepository : BaseRepository<Incidence>, IIncidenceRepository
    {
        public IncidenceRepository(MuniDbContext _context) : base(_context)
        {

        }

        public void AddIncidence(Incidence Incidence)
        {
            _muniDbContext.Incidences.Add(Incidence);
            _muniDbContext.SaveChanges();
        }

        public List<Incidence> GetAllIncidences()
        {
            return _muniDbContext.Incidences
            .Include(i => i.Area)
            .Include(i => i.Operator)
            .Where(i => i.Deleted == 0) 
            .ToList();
        }

        public Incidence? GetIncidenceById(int id)
        {
            return _muniDbContext.Incidences.FirstOrDefault(p => p.Id == id);
        }

        public Incidence UpdateIncidence(Incidence incidence)
        {
            _muniDbContext.Update(incidence);
            _muniDbContext.SaveChanges();
            return incidence;
        }
        public void DeleteIncidence(Incidence incidence)
        {
            _muniDbContext.Remove(incidence);
            _muniDbContext.SaveChanges();
        }
    }
}
