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

        /// <summary>
        /// Sobrescribe Get() para devolver solo operadores no eliminados (soft delete)
        /// </summary>
        public override List<Operator> Get()
        {
            return _muniDbContext.Operators.Where(o => o.Deleted == 0).ToList();
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
            // Buscar solo operadores ACTIVOS (Deleted == 0)
            // Permite reutilizar DNIs de operadores que fueron borrados
            return _muniDbContext.Operators.FirstOrDefault(g => g.DNI == dni && g.Deleted == 0);
        }

        public Operator? GetOperatorByDniIncludingDeleted(int dni)
        {
            // Buscar en TODOS los operadores (incluyendo eliminados)
            // Usado solo para validación de duplicado en CreateOperator
            // Evita que alguien cree operador con DNI que alguna vez existió
            return _muniDbContext.Operators.FirstOrDefault(g => g.DNI == dni);
        }

        public Operator? GetOperatorByNLegajo(int nLegajo)
        {
            // Buscar solo operadores ACTIVOS (Deleted == 0)
            // Permite reutilizar NLegajo de operadores que fueron borrados
            return _muniDbContext.Operators.FirstOrDefault(o => o.NLegajo == nLegajo && o.Deleted == 0);
        }

        public Operator? GetOperatorByNLegajoIncludingDeleted(int nLegajo)
        {
            // Buscar en TODOS los operadores (incluyendo eliminados)
            // Usado solo para validación de duplicado en CreateOperator
            // Evita que alguien cree operador con NLegajo que alguna vez existió
            return _muniDbContext.Operators.FirstOrDefault(o => o.NLegajo == nLegajo);
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
            // Solo devolver operadores NO eliminados (soft-delete)
            return _muniDbContext.Operators.FirstOrDefault(p => p.NLegajo == NLegajo && p.Password == Password && p.Deleted == 0);
        }
    }
}
