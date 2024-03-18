using Akalaat.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.BLL.Interfaces
{
    public interface IDistrictRepository:IGenericRepository<District>
    {
        Task<IReadOnlyList<District>> GetAllDistrictsByCityId(int CityId);
    }
}
