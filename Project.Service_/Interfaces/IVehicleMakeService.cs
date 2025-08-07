using Project.Service_.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Service_.Interfaces
{
    public interface IVehicleMakeService
    {
        Task<IEnumerable<VehicleMakeDto>> GetAllAsync(string search = null, string sortOrder = null, int page = 1, int pageSize = 10);
        Task<VehicleMakeDto> GetByIdAsync(int id);
        Task CreateAsync(VehicleMakeDto make);
        Task UpdateAsync(VehicleMakeDto make);
        Task DeleteAsync(int id);
    }
}