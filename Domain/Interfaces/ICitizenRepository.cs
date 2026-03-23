using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ICitizenRepository : IBaseRepository<Citizen>
    {
        void AddCitizen(Citizen citizen);
        Citizen? GetCitizenByDni(int dni);
        Citizen UpdateCitizen(Citizen citizen);
        void DeleteCitizen(Citizen citizen);

    }
}
