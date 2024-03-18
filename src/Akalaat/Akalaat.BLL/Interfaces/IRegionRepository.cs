using Akalaat.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.BLL.Interfaces
{
    public interface IRegionRepository:IGenericRepository<Region>
    {
        Task<IReadOnlyList<Region>> GetAllRegionsByDistrictId(int DistrictId);
    }
}
