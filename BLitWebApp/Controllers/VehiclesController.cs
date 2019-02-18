using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BLitWebApp.Data;
using BLitWebApp.Models;

namespace BLitWebApp.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly VehicleContext _context;

        public VehiclesController(VehicleContext context)
        {
            _context = context;
        }

        // GET: Vehicles
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["VINSort"] = String.IsNullOrEmpty(sortOrder) ? "VIN_desc" : "";
            ViewData["CarModelSort"] = sortOrder == "CarModel_desc" ? "CarModel_asc" : "CarModel_desc";
            ViewData["CarClassSort"] = sortOrder == "CarClass_desc" ? "CarClass_asc" : "CarClass_desc";
            ViewData["ManufacturerSort"] = sortOrder == "Manufacturer_desc" ? "Manufacturer_asc" : "Manufacturer_desc";

            var vehicleContext = _context.Vehicles.Include(v => v.CarClass)
                .Include(v => v.CarModel)
                    .ThenInclude(v => v.Manufacturer)
                .Include(v => v.CarClass)
                .AsNoTracking();

            var vehicles = from v in vehicleContext
                           select v;

            switch (sortOrder)
            {
                case "VIN_desc":
                    vehicles = vehicles.OrderByDescending(v => v.VIN);
                    break;
                case "CarModel_asc":
                    vehicles = vehicles.OrderBy(v => v.CarModel.Name);
                    break;
                case "CarModel_desc":
                    vehicles = vehicles.OrderByDescending(v => v.CarModel.Name);
                    break;
                case "CarClass_asc":
                    vehicles = vehicles.OrderByDescending(v => v.CarClass.Name);
                    break;
                case "CarClass_desc":
                    vehicles = vehicles.OrderByDescending(v => v.CarClass.Name);
                    break;
                case "Manufacturer_asc":
                    vehicles = vehicles.OrderBy(v => v.CarModel.Manufacturer.Name);
                    break;
                case "Manufacturer_desc":
                    vehicles = vehicles.OrderByDescending(v => v.CarModel.Manufacturer.Name);
                    break;
                default:
                    vehicles = vehicles.OrderBy(v => v.VIN);
                    break;
            }
            return View(await vehicles.AsNoTracking().ToListAsync());
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Create Context variable with data from all models
            var vehicle = await _context.Vehicles
                .Include(v => v.CarModel)
                    .ThenInclude(v => v.Manufacturer)
                .Include(v => v.CarClass)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            ViewData["CarClassID"] = new SelectList(_context.CarClasses, "ID", "Name");
            ViewData["CarModelID"] = new SelectList(_context.CarModels, "ID", "Name");
            return View();
        }

        // GET: Vehicles/CreateCarModel
        public IActionResult CreateCarModel()
        {
            ViewData["ManufacturerID"] = new SelectList(_context.Manufacturers, "ID", "Name");
            return View();
        }

        // GET: Vehicles/CreateCarClass
        public IActionResult CreateCarClass()
        {
            return View();
        }

        // GET: Vehicles/CreateManufacturer
        public IActionResult CreateManufacturer()
        {
            return View();
        }

        // POST: Vehicles/CreateManufacturer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateManufacturer([Bind("Name")] Manufacturer manufacturer)
        {
            var manufacturerList = _context.Manufacturers.ToList();

            foreach (Manufacturer c in manufacturerList)
            {
                if (c.Name == manufacturer.Name)
                {
                    ModelState.AddModelError("Name", "Name must be unique");
                    return View();
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(manufacturer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Changes could not be saved, please try again.");
                }
            }

            //Return view with error warnings
            return View(manufacturer);
        }

        // POST: Vehicles/CreateCarModel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCarClass([Bind("Name")] CarClass carClass)
        {
            var carClassList = _context.CarClasses.ToList();

            foreach(CarClass c in carClassList)
            {
                if (c.Name == carClass.Name)
                {
                    ModelState.AddModelError("Name", "Name must be unique");
                    return View();
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(carClass);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Changes could not be saved, please try again.");
                }
            }

            //Return view with error warnings
            return View(carClass);
        }

        // POST: Vehicles/CreateCarModel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCarModel([Bind("Name,ManufacturerID")] CarModel carModel)
        {
            //Check if name is unique
            var carModelList = _context.CarModels.ToList();
            foreach (CarModel c in carModelList)
            {
                if (c.Name == carModel.Name)
                {
                    ModelState.AddModelError("Name", "Name must be unique");
                    ViewData["ManufacturerID"] = new SelectList(_context.Manufacturers, "ID", "Name");
                    return View();
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(carModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Changes could not be saved, please try again.");
                }
            }

            //Return view with error warnings
            ViewData["ManufacturerID"] = new SelectList(_context.Manufacturers, "ID", "Name");
            return View(carModel);
        }

        // POST: Vehicles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VIN,CarModelID,CarClassID")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(vehicle);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Changes could not be saved, please try again.");
                }
            }

            //Return view with error warnings
            ViewData["CarClassID"] = new SelectList(_context.CarClasses, "ID", "Name", vehicle.CarClassID);
            ViewData["CarModelID"] = new SelectList(_context.CarModels, "ID", "Name", vehicle.CarModelID);
            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            ViewData["CarClassID"] = new SelectList(_context.CarClasses, "ID", "Name", vehicle.CarClassID);
            ViewData["CarModelID"] = new SelectList(_context.CarModels, "ID", "Name", vehicle.CarModelID);
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,VIN,CarModelID,CarClassID")] Vehicle vehicle)
        {
            if (id != vehicle.ID)
            {
                return NotFound();
            }
            var vehicleToUpdate = await _context.Vehicles.SingleOrDefaultAsync(v => v.ID == id);
            if (await TryUpdateModelAsync<Vehicle>(
                vehicleToUpdate,
                "",
                v => v.VIN, v => v.CarModelID, v => v.CarClassID))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError("", "Unable to update changes.");
                }
                
            }
            ViewData["CarClassID"] = new SelectList(_context.CarClasses, "ID", "Name", vehicle.CarClassID);
            ViewData["CarModelID"] = new SelectList(_context.CarModels, "ID", "Name", vehicle.CarModelID);
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles
                .Include(v => v.CarClass)
                .Include(v => v.CarModel)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicles.Any(e => e.ID == id);
        }
    }
}
