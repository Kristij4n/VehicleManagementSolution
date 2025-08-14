using Project.Service_.DTOs;
using System.Threading.Tasks;

namespace Project.Service_.Interfaces
{
    public interface IVehicleMakeService
    {
        Task<PagedResult<VehicleMakeDto>> GetAllAsync(string search = null, string sortOrder = null, int page = 1, int pageSize = 10);
        Task<VehicleMakeDto> GetByIdAsync(int id);
        Task<bool> CreateAsync(VehicleMakeDto make);
        Task<bool> UpdateAsync(VehicleMakeDto make);
        Task<bool> DeleteAsync(int id);
    }
} //paging result returns total count