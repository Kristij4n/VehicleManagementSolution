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

        public async Task<IEnumerable<VehicleModelDto>> GetAllAsync(string search, string sortOrder, int page, int pageSize, int? makeId)
        {
            var query = _context.VehicleModels
                .Include(vm => vm.Make)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(vm => vm.Name.Contains(search) || vm.Abrv.Contains(search));

            if (makeId.HasValue)
                query = query.Where(vm => vm.MakeId == makeId);

            if (!string.IsNullOrEmpty(sortOrder) && sortOrder.ToLower() == "name_desc")
                query = query.OrderByDescending(vm => vm.Name);
            else
                query = query.OrderBy(vm => vm.Name);

            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            var models = await query.ToListAsync();

            // Ensure MakeName is always filled in DTO
            var dtos = models.Select(model => new VehicleModelDto
            {
                Id = model.Id,
                Name = model.Name,
                Abrv = model.Abrv,
                MakeId = model.MakeId,
                MakeName = model.Make != null ? model.Make.Name : null
            });

            return dtos;
        }

        public async Task<VehicleModelDto> GetByIdAsync(int id)
        {
            var model = await _context.VehicleModels
                .Include(vm => vm.Make)
                .FirstOrDefaultAsync(vm => vm.Id == id);

            if (model == null)
                return null;

            return new VehicleModelDto
            {
                Id = model.Id,
                Name = model.Name,
                Abrv = model.Abrv,
                MakeId = model.MakeId,
                MakeName = model.Make != null ? model.Make.Name : null
            };
        }

        public async Task AddAsync(VehicleModelDto modelDto)
        {
            var model = _mapper.Map<VehicleModel>(modelDto);
            _context.VehicleModels.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(VehicleModelDto modelDto)
        {
            var model = await _context.VehicleModels.FindAsync(modelDto.Id);
            if (model != null)
            {
                model.Name = modelDto.Name;
                model.Abrv = modelDto.Abrv;
                model.MakeId = modelDto.MakeId;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var model = await _context.VehicleModels.FindAsync(id);
            if (model != null)
            {
                _context.VehicleModels.Remove(model);
                await _context.SaveChangesAsync();
            }
        }
    }
}