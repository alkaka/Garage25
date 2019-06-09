using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Garage25.Models;

namespace Garage25.Controllers
{
    public enum VSearchTerm
    {
        Name
    }

    public class VehicleTypesController : Controller
    {
        private readonly Garage25Context _context;

        public VehicleTypesController(Garage25Context context)
        {
            _context = context;
        }

        // GET: VehicleTypes
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.VehicleType
        //                        .OrderBy(v => v.Name)
        //                        .ToListAsync());
        //}

        // GET: VehicleTypes
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["NameSortOrder"] = string.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";

            IQueryable<VehicleType> result = _context.VehicleType;

            switch (sortOrder)
            {
                case "Name_desc":
                    result = result.OrderByDescending(v => v.Name);
                    TempData["message"] = "Sorting \'Name\' descending";
                    break;
                default:
                    result = result.OrderBy(m => m.Name);
                    if (TempData["message"] == null)
                        TempData["message"] = "Sorting \'Name\' ascending";
                    else if (!TempData["message"].ToString().Contains("Vehicle Type"))
                        TempData["message"] = "Sorting \'Name\' ascending";
                    break;
            }

            return View(nameof(Index), await result.ToListAsync());
        }

        // GET: VehicleTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleType = await _context.VehicleType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicleType == null)
            {
                return NotFound();
            }

            return View(vehicleType);
        }

        // GET: VehicleTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VehicleTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] VehicleType vehicleType)
        {
            if (await _context.VehicleType.AnyAsync(v => v.Name == vehicleType.Name))
            {
                ModelState.AddModelError("Name", $"\'{vehicleType.Name}\' already exists!");
            }

            if (ModelState.IsValid)
            {
                _context.Add(vehicleType);
                await _context.SaveChangesAsync();

                TempData["Message"] = $"Vehicle Type \'{vehicleType.Name}\' is created";

                return RedirectToAction(nameof(Index));
            }
            return View(vehicleType);
        }

        // GET: VehicleTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleType = await _context.VehicleType.FindAsync(id);
            if (vehicleType == null)
            {
                return NotFound();
            }

            CreateSelectList();

            return View(vehicleType);
        }

        private void CreateSelectList()
        {
            List<VehicleType> vehicleTypes = new List<VehicleType> {
                new VehicleType { Name = "Airplane" },
                new VehicleType { Name = "Bicycle" },
                new VehicleType { Name = "Boat" },
                new VehicleType { Name = "Bus" },
                new VehicleType { Name = "Car" },
                new VehicleType { Name = "Lorry" },
                new VehicleType { Name = "Moped" },
                new VehicleType { Name = "Motorcycle" },
                new VehicleType { Name = "Train" },
                new VehicleType { Name = "Truck" }
            };
            ViewData["Name"] = new SelectList(vehicleTypes, "Name", "Name");
        }

        // POST: VehicleTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] VehicleType vehicleType)
        {
            if (id != vehicleType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicleType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleTypeExists(vehicleType.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vehicleType);
        }

        // GET: VehicleTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleType = await _context.VehicleType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicleType == null)
            {
                return NotFound();
            }

            return View(vehicleType);
        }

        // POST: VehicleTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicleType = await _context.VehicleType.FindAsync(id);
            _context.VehicleType.Remove(vehicleType);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"Vehicle Type \'{vehicleType.Name}\' is deleted";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Filter(string search, string reset)
        {
            IQueryable<VehicleType> result = _context.VehicleType;

            if (string.IsNullOrWhiteSpace(reset))
            {
                if (!string.IsNullOrWhiteSpace(search))
                {
                    ViewData["Search"] = search;

                    result = result.Where(m => m.Name.Contains(search, StringComparison.CurrentCultureIgnoreCase));  
                }
            }

            return View(nameof(Index), await result.OrderBy(m => m.Name).ToListAsync());
        }

        private bool VehicleTypeExists(int id)
        {
            return _context.VehicleType.Any(e => e.Id == id);
        }
    }
}
