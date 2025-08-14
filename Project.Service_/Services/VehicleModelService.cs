using AutoMapper;
using Project.Service_.Data;
using Project.Service_.DTOs;
using Project.Service_.Interfaces;
using Project.Service_.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Service_.Services
{
    public class VehicleModelService : IVehicleModelService
    {
        private readonly VehicleDbContext _context;
        private readonly IMapper _mapper;

        public VehicleModelService(VehicleDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedResult<VehicleModelDto>> GetAllAsync(string search, string sortOrder, int page, int pageSize, int? makeId)
        {
            var query = _context.VehicleModels
                .Include(vm => vm.Make)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(vm => vm.Name.ToLower().Contains(searchLower) || vm.Abrv.ToLower().Contains(searchLower));
            }

            if (makeId.HasValue)
                query = query.Where(vm => vm.MakeId == makeId);

            int totalCount = await query.CountAsync();

            if (!string.IsNullOrEmpty(sortOrder) && sortOrder.ToLower() == "name_desc")
                query = query.OrderByDescending(vm => vm.Name);
            else
                query = query.OrderBy(vm => vm.Name);

            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            var models = await query.ToListAsync();

            //replaced manual mapping
            
            var dtos = _mapper.Map<IEnumerable<VehicleModelDto>>(models);

            return new PagedResult<VehicleModelDto>
            {
                Items = dtos,
                TotalCount = totalCount
            };
        }

        public async Task<VehicleModelDto> GetByIdAsync(int id)
        {
            var model = await _context.VehicleModels
                .Include(vm => vm.Make)
                .FirstOrDefaultAsync(vm => vm.Id == id);

            if (model == null)
                return null;
                        
            //replaced manual mapping

            return _mapper.Map<VehicleModelDto>(model);
        }

        public async Task<bool> AddAsync(VehicleModelDto modelDto)
        {
            var model = _mapper.Map<VehicleModel>(modelDto);
            _context.VehicleModels.Add(model);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(VehicleModelDto modelDto)
        {
            var model = await _context.VehicleModels.FindAsync(modelDto.Id);
            if (model != null)
            {
                _mapper.Map(modelDto, model);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var model = await _context.VehicleModels.FindAsync(id);
            if (model != null)
            {
                _context.VehicleModels.Remove(model);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }
    }
} //improved filtering - case Insensitive filtering