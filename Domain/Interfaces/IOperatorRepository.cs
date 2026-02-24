using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IOperatorRepository: IBaseRepository<Operator>
    {
        void AddOperator(Operator Operator);
        Operator? GetOperatorByDni(int NLegajo);
        Operator UpdateOperator(Operator Operator);
        void DeleteOperator(Operator Operator);
        Operator? GetUserByNLegajoAndPassword(int NLegajo, string Password);
    }
}
