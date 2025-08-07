using Project.Service_.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Service_.Interfaces
{
    public interface IVehicleModelService
    {
        Task<IEnumerable<VehicleModelDto>> GetAllAsync(string search, string sortOrder, int page, int pageSize, int? makeId);
        Task<VehicleModelDto> GetByIdAsync(int id);
        Task AddAsync(VehicleModelDto model);
        Task UpdateAsync(VehicleModelDto model);
        Task DeleteAsync(int id);
    }
}
