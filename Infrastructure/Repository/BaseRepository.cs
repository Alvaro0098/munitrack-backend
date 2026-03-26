using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Infrastructure.Repository
{
   public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        public readonly MuniDbContext _muniDbContext;
        public BaseRepository(MuniDbContext muniDbContext)
        {
            _muniDbContext = muniDbContext;
        }

        public virtual List<T> Get()
        {
            return _muniDbContext.Set<T>().ToList();
        }
    }
}
