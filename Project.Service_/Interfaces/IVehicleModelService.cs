using Project.Service_.DTOs;
using System.Threading.Tasks;

namespace Project.Service_.Interfaces
{
    public interface IVehicleModelService
    {
        Task<PagedResult<VehicleModelDto>> GetAllAsync(string search, string sortOrder, int page, int pageSize, int? makeId);
        Task<VehicleModelDto> GetByIdAsync(int id);
        Task<bool> AddAsync(VehicleModelDto model);
        Task<bool> UpdateAsync(VehicleModelDto model);
        Task<bool> DeleteAsync(int id);
    }
} //paging result returns total count