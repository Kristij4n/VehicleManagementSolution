using Project.MVC_.ViewModels;
using Project.Service_.DTOs;
using Project.Service_.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System;

public class VehicleMakeController : Controller
{
    private readonly IVehicleMakeService _vehicleMakeService;
    private readonly IMapper _mapper;

    public VehicleMakeController(IVehicleMakeService vehicleMakeService, IMapper mapper)
    {
        _vehicleMakeService = vehicleMakeService;
        _mapper = mapper;
    }

    public async Task<ActionResult> Index(string search, string sortOrder, int page = 1, int pageSize = 10)
    {
        var makesDto = await _vehicleMakeService.GetAllAsync(search, sortOrder, page, pageSize);
        var viewModels = _mapper.Map<List<VehicleMakeViewModel>>(makesDto);

        ViewBag.CurrentSort = sortOrder;
        ViewBag.CurrentFilter = search;
        ViewBag.Page = page;
        ViewBag.PageSize = pageSize;

        return View(viewModels);
    }

    public async Task<ActionResult> Details(int? id)
    {
        if (id == null)
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        var makeDto = await _vehicleMakeService.GetByIdAsync(id.Value);
        if (makeDto == null)
            return HttpNotFound();

        var viewModel = _mapper.Map<VehicleMakeViewModel>(makeDto);
        return View(viewModel);
    }

    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(VehicleMakeViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        var dto = _mapper.Map<VehicleMakeDto>(viewModel);
        await _vehicleMakeService.CreateAsync(dto);
        TempData["Success"] = "Vehicle Make created successfully!";
        return RedirectToAction("Index");
    }

    public async Task<ActionResult> Edit(int? id)
    {
        if (id == null)
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        var makeDto = await _vehicleMakeService.GetByIdAsync(id.Value);
        if (makeDto == null)
            return HttpNotFound();

        var viewModel = _mapper.Map<VehicleMakeViewModel>(makeDto);
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(VehicleMakeViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        var dto = _mapper.Map<VehicleMakeDto>(viewModel);
        await _vehicleMakeService.UpdateAsync(dto);
        TempData["Success"] = "Vehicle Make updated successfully!";
        return RedirectToAction("Index");
    }

    public async Task<ActionResult> Delete(int? id)
    {
        if (id == null)
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        var makeDto = await _vehicleMakeService.GetByIdAsync(id.Value);
        if (makeDto == null)
            return HttpNotFound();

        var viewModel = _mapper.Map<VehicleMakeViewModel>(makeDto);
        return View(viewModel);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
        var makeDto = await _vehicleMakeService.GetByIdAsync(id);
        if (makeDto == null)
            return HttpNotFound();

        try
        {
            await _vehicleMakeService.DeleteAsync(id);
            TempData["Success"] = "Vehicle Make deleted.";
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction("Index");
    }
}