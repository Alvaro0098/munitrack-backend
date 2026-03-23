using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repository
{
    public class AreaRepository: BaseRepository<Area>, IAreaRepository
    {
        public AreaRepository(MuniDbContext _context) : base(_context)
        {

        }

        public void AddArea(Area Area)
        {
            _muniDbContext.Areas.Add(Area);
            _muniDbContext.SaveChanges();
        }


        public Area? GetAreaById(int id)
        {
            return _muniDbContext.Areas.FirstOrDefault(g => g.Id == id);
        }

        public Area? GetAreaByName(string name)
        {
            return _muniDbContext.Areas.FirstOrDefault(g => g.Name == name);
        }

        public Area UpdateArea(Area Area)
        {
            _muniDbContext.Update(Area);
            _muniDbContext.SaveChanges();
            return Area;
        }
        public void DeleteArea(Area Area)
        {
            _muniDbContext.Remove(Area);
            _muniDbContext.SaveChanges();
        }
    }
}
