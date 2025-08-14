using AutoMapper;
using Project.MVC_.ViewModels;
using Project.Service_.DTOs;
using Project.Service_.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Project.MVC_.Controllers
{
    public class VehicleModelController : Controller
    {
        private readonly IVehicleModelService _vehicleModelService;
        private readonly IVehicleMakeService _vehicleMakeService;
        private readonly IMapper _mapper;

        public VehicleModelController(IVehicleModelService vehicleModelService, IVehicleMakeService vehicleMakeService, IMapper mapper)
        {
            _vehicleModelService = vehicleModelService;
            _vehicleMakeService = vehicleMakeService;
            _mapper = mapper;
        }

        public async Task<ActionResult> Index(string search, string sortOrder, int page = 1, int pageSize = 10, int? makeId = null)
        {
            var makes = await _vehicleMakeService.GetAllAsync();
            ViewBag.Makes = new SelectList(makes.Items, "Id", "Name", makeId);

            var pagedResult = await _vehicleModelService.GetAllAsync(search, sortOrder, page, pageSize, makeId);
            var viewModels = _mapper.Map<IEnumerable<VehicleModelViewModel>>(pagedResult.Items);

            ViewBag.CurrentSort = sortOrder;
            ViewBag.CurrentFilter = search;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = pagedResult.TotalCount;

            return View(viewModels);
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var dto = await _vehicleModelService.GetByIdAsync(id.Value);
            if (dto == null)
                return HttpNotFound();

            var viewModel = _mapper.Map<VehicleModelViewModel>(dto);
            return View(viewModel);
        }

        public async Task<ActionResult> Create()
        {
            await PopulateMakeDropdown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(VehicleModelViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateMakeDropdown(viewModel.MakeId);
                return View(viewModel);
            }

            var dto = _mapper.Map<VehicleModelDto>(viewModel);
            var success = await _vehicleModelService.AddAsync(dto);
            if (success)
                TempData["Success"] = "Vehicle Model created successfully!";
            else
                TempData["Error"] = "Failed to create Vehicle Model.";
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var dto = await _vehicleModelService.GetByIdAsync(id.Value);
            if (dto == null)
                return HttpNotFound();

            var viewModel = _mapper.Map<VehicleModelViewModel>(dto);
            await PopulateMakeDropdown(viewModel.MakeId);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(VehicleModelViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateMakeDropdown(viewModel.MakeId);
                return View(viewModel);
            }

            var dto = _mapper.Map<VehicleModelDto>(viewModel);
            var success = await _vehicleModelService.UpdateAsync(dto);
            if (success)
                TempData["Success"] = "Vehicle Model updated successfully!";
            else
                TempData["Error"] = "Failed to update Vehicle Model.";
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var dto = await _vehicleModelService.GetByIdAsync(id.Value);
            if (dto == null)
                return HttpNotFound();

            var viewModel = _mapper.Map<VehicleModelViewModel>(dto);
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var dto = await _vehicleModelService.GetByIdAsync(id);
            if (dto == null)
                return HttpNotFound();

            var success = await _vehicleModelService.DeleteAsync(id);
            if (success)
                TempData["Success"] = "Vehicle Model deleted.";
            else
                TempData["Error"] = "Failed to delete Vehicle Model.";

            // Redirect to VehicleModel list!
            return RedirectToAction("Index");
        }

        private async Task PopulateMakeDropdown(int? selectedMakeId = null)
        {
            var makes = await _vehicleMakeService.GetAllAsync();
            ViewBag.Makes = new SelectList(makes.Items, "Id", "Name", selectedMakeId);
        }
    } //updated VehicleModel for create new model

} //update controllers to use new PagedResult and pass TotalCount to views