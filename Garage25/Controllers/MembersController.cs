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
    public enum SearchTerm
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
        public async Task<IActionResult> Index2()
        {
            IQueryable<SearchMViewModel> result = CreateSearchMViewModels();

            return View(nameof(Index2), await result
                                            .OrderBy(m => m.UserName)
                                            .ToListAsync());
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            await UpdateParkedVehicles();

            return View(await _context.Member
                            .OrderBy(m => m.UserName)
                            .ToListAsync());
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

                    switch ((SearchTerm)int.Parse(searchterm))
                    {
                        case SearchTerm.UserName:
                            result = result.Where(m => m.UserName.Contains(search));
                            break;
                        case SearchTerm.Email:
                            result = result.Where(m => m.Email.Contains(search));
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
                detailsMViewModel.ParkedVehicles = await parkedVehicles.ToListAsync<ParkedVehicle>();
            }
            
            return View(detailsMViewModel);
        }


        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
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

            await CheckUnique(member);

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
                return RedirectToAction(nameof(Index));
            }
            return View(member);
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
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(int id)
        {
            return _context.Member.Any(e => e.Id == id);
        }

        private IQueryable<SearchMViewModel> CreateSearchMViewModels()
        {
            return _context.Member
                     .Select(m => new SearchMViewModel
                     {
                         Id = m.Id,
                         UserName = m.UserName,
                         Email = m.Email,
                         ParkedVehicles = m.ParkedVehicles
                     });
        }
    }
}
