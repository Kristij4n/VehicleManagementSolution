using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Project.Service_.Data;
using Project.Service_.DTOs;
using Project.Service_.Interfaces;
using Project.Service_.Models;
using System;

namespace Project.Service_.Services
{
    public class VehicleMakeService : IVehicleMakeService
    {
        private readonly VehicleDbContext _context;
        private readonly IMapper _mapper;

        public VehicleMakeService(VehicleDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VehicleMakeDto>> GetAllAsync(string search = null, string sortOrder = null, int page = 1, int pageSize = 10)
        {
            var query = _context.VehicleMakes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(m => m.Name.Contains(search) || m.Abrv.Contains(search));

            switch (sortOrder)
            {
                case "name_desc":
                    query = query.OrderByDescending(m => m.Name);
                    break;
                case "abrv":
                    query = query.OrderBy(m => m.Abrv);
                    break;
                case "abrv_desc":
                    query = query.OrderByDescending(m => m.Abrv);
                    break;
                default:
                    query = query.OrderBy(m => m.Name);
                    break;
            }

            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            var makes = await query.ToListAsync();
            return _mapper.Map<IEnumerable<VehicleMakeDto>>(makes);
        }

        public async Task<VehicleMakeDto> GetByIdAsync(int id)
        {
            var make = await _context.VehicleMakes.FindAsync(id);
            return _mapper.Map<VehicleMakeDto>(make);
        }

        public async Task CreateAsync(VehicleMakeDto makeDto)
        {
            var make = _mapper.Map<VehicleMake>(makeDto);
            _context.VehicleMakes.Add(make);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(VehicleMakeDto makeDto)
        {
            var existing = await _context.VehicleMakes.FindAsync(makeDto.Id);
            if (existing != null)
            {
                _mapper.Map(makeDto, existing);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var make = await _context.VehicleMakes.Include(m => m.VehicleModels).FirstOrDefaultAsync(m => m.Id == id);
            if (make != null)
            {
                if (make.VehicleModels != null && make.VehicleModels.Any())
                    throw new InvalidOperationException("Cannot delete Make with existing Models.");
                _context.VehicleMakes.Remove(make);
                await _context.SaveChangesAsync();
            }
        }
    }
}