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
    public class ParkedVehiclesController : Controller
    {
        private readonly Garage25Context _context;

        public ParkedVehiclesController(Garage25Context context)
        {
            _context = context;
        }

        // GET: ParkedVehicles
        public async Task<IActionResult> Index2()
        {
            var result = CreateParkedVehicleViewModels();

            return View(await result.OrderBy(p => p.RegNum).ToListAsync());
        }

        // GET: ParkedVehicles
        public async Task<IActionResult> Index()
        {
            return View(await _context.ParkedVehicle
                                    .OrderBy(p => p.RegNum)
                                    .ToListAsync());
        }

        // GET: ParkedVehicles/Details/5
        public async Task<IActionResult> 
            Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkedVehicle = await _context.ParkedVehicle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parkedVehicle == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .FirstOrDefaultAsync(m => m.Id == parkedVehicle.MemberId);
            if (member == null)
            {
                return NotFound();
            }
            ViewData["membername"] = member.UserName;

            var vehicletype = await _context.VehicleType
                .FirstOrDefaultAsync(m => m.Id == parkedVehicle.VehicleTypeId);
            if (vehicletype == null)
            {
                return NotFound();
            }
            ViewData["vehicletypename"] = vehicletype.Name;

            return View(parkedVehicle);
        }

        // GET: ParkedVehicles/Create
        public IActionResult Create2()
        {
            // Add some bogus data
            var vehicle = new Bogus.DataSets.Vehicle();

            // Select random color
            string[] colors = { "Black",
                                "Blue",
                                "Green",
                                "Grey",
                                "Lila",
                                "Magenta",
                                "Purple",
                                "Red",
                                "White",
                                "Yellow" };
            Random random = new Random();

            var createPVViewModel = new CreatePVViewModel
            {
                RegNum = vehicle.Vin().Substring(0, 6),
                Color = colors[random.Next(0, 9)],
                UserName = ""
            };

            ViewData["UserName"] = new SelectList(_context.Member
                                                   .OrderBy(m => m.UserName), "UserName", "UserName");

            ViewData["TypeName"] = new SelectList(_context.VehicleType
                                                    .OrderBy(v => v.Name), "Name", "Name");

            return View(createPVViewModel);
        }

        // POST: ParkedVehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create2([Bind("RegNum,Color,UserName,TypeName")] CreatePVViewModel createPVViewModel)
        {
            if (await _context.ParkedVehicle.AnyAsync(p => p.RegNum == createPVViewModel.RegNum))
            {
                ModelState.AddModelError("RegNum", $"\'{createPVViewModel.RegNum}\' already exists!");
            }

            if (!await _context.Member.AnyAsync(m => m.UserName == createPVViewModel.UserName))
            {
                ModelState.AddModelError("UserName", $"\'{createPVViewModel.UserName}\' do not exists!");
            }

            if (!await _context.VehicleType.AnyAsync(v => v.Name == createPVViewModel.TypeName))
            {
                ModelState.AddModelError("TypeName", $"\'{createPVViewModel.TypeName}\' do not exists!");
            }

            if (ModelState.IsValid)
            {
                Member member = await _context.Member
                                .FirstOrDefaultAsync(m => m.UserName == createPVViewModel.UserName);
                if (member == null)
                {
                    return NotFound();
                }

                VehicleType vehicleType = await _context.VehicleType
                                .FirstOrDefaultAsync(v => v.Name == createPVViewModel.TypeName);
                if (vehicleType == null)
                {
                    return NotFound();
                }

                DateTime now = DateTime.Now;
                ParkedVehicle parkedVehicle = new ParkedVehicle
                {
                    RegNum = createPVViewModel.RegNum,
                    Color = createPVViewModel.Color,
                    CheckInTime = now,
                    MemberId = member.Id,
                    Member = member,
                    VehicleTypeId = vehicleType.Id,
                    VehicleType = vehicleType
                };

                _context.Add(parkedVehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["UserName"] = new SelectList(_context.Member
                                                   .OrderBy(m => m.UserName), "UserName", "UserName");

            ViewData["TypeName"] = new SelectList(_context.VehicleType
                                                   .OrderBy(v => v.Name), "Name", "Name");

            return View(createPVViewModel);
        }

        // GET: ParkedVehicles/Create
        public IActionResult Create()
        {
            // Add some bogus data
            DateTime now = DateTime.Now;
            var vehicle = new Bogus.DataSets.Vehicle();
            var parkedVehicle = new ParkedVehicle
            {
                RegNum = vehicle.Vin().Substring(0, 6),
                CheckInTime = now
            };
            return View(parkedVehicle);
        }

        // POST: ParkedVehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RegNum,Color,Member.UserName")] ParkedVehicle parkedVehicle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(parkedVehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(parkedVehicle);
        }

        // GET: ParkedVehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkedVehicle = await _context.ParkedVehicle.FindAsync(id);
            if (parkedVehicle == null)
            {
                return NotFound();
            }
            return View(parkedVehicle);
        }

        // POST: ParkedVehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RegNum,Color")] ParkedVehicle parkedVehicle)
        {
            if (id != parkedVehicle.Id)
            {
                return NotFound();
            }

            if (await _context.ParkedVehicle.AnyAsync(p => p.RegNum == parkedVehicle.RegNum))
            {
                ModelState.AddModelError("RegNum", $"\'{parkedVehicle.RegNum}\' already exists");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parkedVehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParkedVehicleExists(parkedVehicle.Id))
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
            return View(parkedVehicle);
        }

        // GET: ParkedVehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkedVehicle = await _context.ParkedVehicle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parkedVehicle == null)
            {
                return NotFound();
            }

            return View(parkedVehicle);
        }

        // POST: ParkedVehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parkedVehicle = await _context.ParkedVehicle.FindAsync(id);
            _context.ParkedVehicle.Remove(parkedVehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParkedVehicleExists(int id)
        {
            return _context.ParkedVehicle.Any(e => e.Id == id);
        }

        private void CreateSelectList()
        {
            //List<VehicleType> vehicleTypes = new List<VehicleType> {
            //    new VehicleType { Name = "Airplane" },
            //    new VehicleType { Name = "Bicycle" },
            //    new VehicleType { Name = "Boat" },
            //    new VehicleType { Name = "Bus" },
            //    new VehicleType { Name = "Car" },
            //    new VehicleType { Name = "Lorry" },
            //    new VehicleType { Name = "Moped" },
            //    new VehicleType { Name = "Motorcycle" },
            //    new VehicleType { Name = "Train" },
            //    new VehicleType { Name = "Truck" }
            //};

            ViewData["Name"] = new SelectList(_context.VehicleType, "Name", "Name");
        }

        private IQueryable<ParkedVehicleViewModel> CreateParkedVehicleViewModels()
        {
            return _context.ParkedVehicle
                     .Select( p => new ParkedVehicleViewModel
                     {
                         Id = p.Id,
                         RegNum = p.RegNum,
                         ParkingTime = DateTime.Now - p.CheckInTime,
                         Owner = _context.Member.Any(m => m.Id == p.MemberId) ? 
                                     _context.Member.First(m => m.Id == p.MemberId).UserName : "",
                         VehicleType = _context.VehicleType.Any(v => v.Id == p.VehicleTypeId) ?
                                            _context.VehicleType.First(v => v.Id == p.VehicleTypeId).Name : ""
                     }); 
        }
    }
}
