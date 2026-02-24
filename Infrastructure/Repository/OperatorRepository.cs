using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Infrastructure.Repository
{
    public class OperatorRepository : BaseRepository<Operator>, IOperatorRepository
    {
     
        public OperatorRepository(MuniDbContext _context) : base(_context)
        {

        }

        public void AddOperator(Operator Operator)
        {
            _muniDbContext.Operators.Add(Operator);
            _muniDbContext.SaveChanges();
        }

        // public List<Operator> GetOperators()
        // {
        //     return _muniDbContext.Operators.ToList();
        // }

        public Operator? GetOperatorByDni(int dni)
        {
            return _muniDbContext.Operators.FirstOrDefault(g => g.DNI == dni);
        }

        public Operator UpdateOperator(Operator Operator)
        {
            _muniDbContext.Update(Operator);
            _muniDbContext.SaveChanges();
            return Operator;
        }
        public void DeleteOperator(Operator Operator)
        {
            _muniDbContext.Remove(Operator);
            _muniDbContext.SaveChanges();
        }
        public Operator? GetUserByNLegajoAndPassword(int NLegajo, string Password)
        {
            return _muniDbContext.Operators.FirstOrDefault(p => p.NLegajo == NLegajo && p.Password == Password);
        }
    }
}
