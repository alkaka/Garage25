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
    public enum MSearchTerm
    {
        UserName,
        Email,
        TimeStamp
    }
    public class MembersController : Controller
    {
        private readonly Garage25Context _context;

        public MembersController(Garage25Context context)
        {
            _context = context;
        }

        // GET: Members
        //public async Task<IActionResult> Index2()
        //{
        //    IQueryable<SearchMViewModel> result = CreateSearchMViewModels();

        //    return View(nameof(Index2), await result
        //                                    .OrderBy(m => m.UserName)
        //                                    .ToListAsync());
        //}

        //// GET: Members
        //public async Task<IActionResult> Index()
        //{
        //    await UpdateParkedVehicles();

        //    return View(await _context.Member
        //                    .OrderBy(m => m.UserName)
        //                    .ToListAsync());
        //}

        public async Task<IActionResult> Index(string sortOrder)
        {
            await UpdateParkedVehicles();

            ViewData["UserNameSortOrder"] = string.IsNullOrEmpty(sortOrder) ? "UserName_desc" : "";
            ViewData["EmailSortOrder"] = sortOrder == "Email" ? "Email_desc" : "Email";
            ViewData["NumParkedVehiclesSortOrder"] = sortOrder == "NumParkedVehicles" ? "NumParkedVehicles_desc" : "NumParkedVehicles";

            IQueryable<Member> result = _context.Member;

            switch (sortOrder)
            {
                case "UserName_desc":
                    result = result.OrderByDescending(m => m.UserName);
                    TempData["message"] = "Sorting \'User Name\' descending";
                    break;
                case "Email":
                    result = result.OrderBy(m => m.Email);
                    TempData["message"] = "Sorting \'Email\' ascending";
                    break;
                case "Email_desc":
                    result = result.OrderByDescending(m => m.Email);
                    TempData["message"] = "Sorting \'Email\' descending";
                    break;
                case "NumParkedVehicles":
                    result = result.OrderBy(m => m.ParkedVehicles.Count);
                    TempData["message"] = "Sorting \'Number of Parked Vehicles\' ascending";
                    break;
                case "NumParkedVehicles_desc":
                    result = result.OrderByDescending(m => m.ParkedVehicles.Count);
                    TempData["message"] = "Sorting \'Number of Parked Vehicles\' descending";
                    break;
                default:
                    result = result.OrderBy(m => m.UserName);
                    if (TempData["message"] == null)
                        TempData["message"] = "Sorting \'User Name\' ascending";
                    else if (!TempData["message"].ToString().Contains("Member"))
                        TempData["message"] = "Sorting \'User Name\' ascending";
                    break;
            }

            return View(nameof(Index), await result.ToListAsync());
        }

        private async Task UpdateParkedVehicles()
        {
            foreach (var member in _context.Member)
            {
                var parkedVehicles = _context.ParkedVehicle
                                        .Where(p => p.MemberId == member.Id);
                if (parkedVehicles != null)
                    member.ParkedVehicles = await parkedVehicles.ToListAsync();
            }
        }

        public async Task<IActionResult> Filter(string search, string reset, string searchterm)
        {
            IQueryable<Member> result = _context.Member;

            if (string.IsNullOrWhiteSpace(reset) && !string.IsNullOrWhiteSpace(searchterm))
            {
                if (!string.IsNullOrWhiteSpace(search))
                {
                    ViewData["Search"] = search;
                    ViewData["Select"] = searchterm;

                    search = search.ToUpper();

                    switch ((MSearchTerm)int.Parse(searchterm))
                    {
                        case MSearchTerm.UserName:
                            // result = result.Where(m => m.UserName.Contains(search)); // Seems to work with case
                            result = result.Where(m => m.UserName.Contains(search, StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case MSearchTerm.Email:
                            // result = result.Where(m => m.Email.Contains(search)); // Seems to work with case
                            result = result.Where(m => m.Email.Contains(search, StringComparison.CurrentCultureIgnoreCase));
                            break;
                        default:
                            break;
                    }
                }
            }

            await UpdateParkedVehicles();

            return View(nameof(Index), await result
                                            .OrderBy(m => m.UserName)
                                            .ToListAsync());
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details2(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }


            //var parkedVehicles = _context.ParkedVehicle
            //    .Where(p => p.MemberId == member.Id);
            

            //if (parkedVehicles == null)
            //{
            //    ViewData["numparkedvehicles"] = "0";
            //}
            //else
            //{
            //    int numParkedVehicles = parkedVehicles.Count();

            //    ViewData["numparkedvehicles"] = numParkedVehicles.ToString();
            //}

            var detailsMViewModel = new DetailsMViewModel
            {
                Id = member.Id,
                UserName = member.UserName,
                Email = member.Email,
                ParkedVehicles = member.ParkedVehicles
            };

            var parkedVehicles = _context.ParkedVehicle
               .Where(p => p.MemberId == member.Id);

            if (parkedVehicles != null)
            {
                detailsMViewModel.ParkedVehicles = await parkedVehicles.OrderBy(p => p.RegNum).ToListAsync<ParkedVehicle>();
            }
            
            return View(detailsMViewModel);
        }


        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id, string sortOrder)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            //var parkedVehicles = _context.ParkedVehicle
            //    .Where(p => p.MemberId == member.Id);


            //if (parkedVehicles == null)
            //{
            //    ViewData["numparkedvehicles"] = "0";
            //}
            //else
            //{
            //    int numParkedVehicles = parkedVehicles.Count();

            //    ViewData["numparkedvehicles"] = numParkedVehicles.ToString();
            //}

            var parkedVehicles = _context.ParkedVehicle
               .Where(p => p.MemberId == member.Id);

            if (parkedVehicles != null)
            {
                ViewData["RegNumSortOrder"] = string.IsNullOrEmpty(sortOrder) ? "RegNum_desc" : "";
                ViewData["ColorSortOrder"] = sortOrder == "Color" ? "Color_desc" : "Color";
                ViewData["CheckInTimeSortOrder"] = sortOrder == "CheckInTime" ? "CheckInTime_desc" : "CheckInTime";

                switch (sortOrder)
                {
                    case "RegNum_desc":
                        parkedVehicles = parkedVehicles.OrderByDescending(p => p.RegNum);
                        TempData["message"] = "Sorting \'Registration Number\' descending";
                        break;
                    case "Color":
                        parkedVehicles = parkedVehicles.OrderBy(p => p.Color);
                        TempData["message"] = "Sorting \'Color\' ascending";
                        break;
                    case "Color_desc":
                        parkedVehicles = parkedVehicles.OrderByDescending(p => p.Color);
                        TempData["message"] = "Sorting \'Color\' descending";
                        break;
                    case "CheckInTime":
                        parkedVehicles = parkedVehicles.OrderBy(p => p.CheckInTime);
                        TempData["message"] = "Sorting \'CheckInTime\' ascending";
                        break;
                    case "CheckInTime_desc":
                        parkedVehicles = parkedVehicles.OrderByDescending(p => p.CheckInTime);
                        TempData["message"] = "Sorting \'CheckInTime\' descending";
                        break;
                    default:
                        parkedVehicles = parkedVehicles.OrderBy(p => p.RegNum);
                        if (TempData["message"] == null)
                            TempData["message"] = "Sorting \'Registration Number\' ascending";
                        else if (!TempData["message"].ToString().Contains("Member"))
                            TempData["message"] = "Sorting \'Registration Number\' ascending";
                        break;
                }

                member.ParkedVehicles = await parkedVehicles.ToListAsync<ParkedVehicle>();
            }

            return View(member);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Email")] Member member)
        {
            await CheckUnique(member);

            if (ModelState.IsValid)
            {
                _context.Add(member);
                await _context.SaveChangesAsync();

                TempData["Message"] = $"Member \'{member.UserName}\' is registered";

                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Email")] Member member)
        {
            if (id != member.Id)
            {
                return NotFound();
            }

            // Check UserName
            if (_context.Member.AsNoTracking().First(m => m.Id == member.Id).UserName != member.UserName &&
                 _context.Member.AsNoTracking().Any(m => m.UserName == member.UserName))
            {
                ModelState.AddModelError("UserName", $"\'{member.UserName}\' already exists!");
            }

            // Check Email
            if (_context.Member.AsNoTracking().First(m => m.Id == member.Id).Email != member.Email &&
                 _context.Member.AsNoTracking().Any(m => m.Email == member.Email))
            {
                ModelState.AddModelError("Email", $"\'{member.Email}\' already exists!");
            }

            //// Check UserName
            //if (await UserNameChangedAsync(member) && await UserNameExistsAsync(member))
            //{
            //    ModelState.AddModelError("UserName", $"\'{member.UserName}\' already exists!");
            //}

            //// Check Email
            //if (await EmailChangedAsync(member) && await EmailExistsAsync(member))
            //{
            //    ModelState.AddModelError("Email", $"\'{member.Email}\' already exists!");
            //}

            //if (await UserNameChangedAsync(member) || await EmailChangedAsync(member))
            //{
            //    await CheckUnique(member);
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                     await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                TempData["Message"] = $"Member \'{member.UserName}\' is updated";

                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        private async Task<bool> EmailExistsAsync(Member member)
        {
            return await _context.Member.AnyAsync(m => m.Email == member.Email);
        }

        private async Task<bool> UserNameExistsAsync(Member member)
        {
            return await _context.Member.AnyAsync(m => m.UserName == member.UserName);
        }

        private async Task<bool> EmailChangedAsync(Member member)
        {
            return (await _context.Member.FirstAsync(m => m.Id == member.Id)).Email != member.Email;
        }

        private async Task<bool> UserNameChangedAsync(Member member)
        {
            return (await _context.Member.FirstAsync(m => m.Id == member.Id)).UserName != member.UserName;
        }

        private async Task CheckUnique(Member member)
        {
            if (await _context.Member.AnyAsync(m => m.UserName == member.UserName))
            {
                ModelState.AddModelError("UserName", $"\'{member.UserName}\' already exists!");
            }

            if (await _context.Member.AnyAsync(m => m.Email == member.Email))
            {
                ModelState.AddModelError("Email", $"\'{member.Email}\' already exists!");
            }
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Member.FindAsync(id);
            _context.Member.Remove(member);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"Member \'{member.UserName}\' is removed";

            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(int id)
        {
            return _context.Member.Any(e => e.Id == id);
        }
    }
}
