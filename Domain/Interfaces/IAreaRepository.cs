using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAreaRepository
    {
        void AddArea(Area Area);
        Area? GetAreaById(int id);
        Area? GetAreaByName(string name);
        Area UpdateArea(Area Area);
        void DeleteArea(Area Area);

    }
}
