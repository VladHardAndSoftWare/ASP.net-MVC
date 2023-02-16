using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASPMVC.Models;

namespace ASPMVC.Controllers
{
    public class EmpoyeesController : Controller
    {
        private readonly TestDbContext _context;

        public EmpoyeesController(TestDbContext context)
        {
            _context = context;
        }

        // GET: Empoyees
        public async Task<IActionResult> Index()
        {
            var testDbContext = _context.Empoyees.Include(e => e.Department);
            return View(await testDbContext.ToListAsync());
        }

        // GET: Empoyees/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Empoyees == null)
            {
                return NotFound();
            }

            var empoyee = await _context.Empoyees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empoyee == null)
            {
                return NotFound();
            }

            return View(empoyee);
        }

        // GET: Empoyees/Create
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id");
            return View();
        }

        // POST: Empoyees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,SurName,Patronymic,DateOfBirth,DocSeries,DocNumber,Position,DepartmentId")] Empoyee empoyee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(empoyee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", empoyee.DepartmentId);
            return View(empoyee);
        }

        // GET: Empoyees/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Empoyees == null)
            {
                return NotFound();
            }

            var empoyee = await _context.Empoyees.FindAsync(id);
            if (empoyee == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", empoyee.DepartmentId);
            return View(empoyee);
        }

        // POST: Empoyees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,FirstName,SurName,Patronymic,DateOfBirth,DocSeries,DocNumber,Position,DepartmentId")] Empoyee empoyee)
        {
            if (id != empoyee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empoyee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpoyeeExists(empoyee.Id))
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
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", empoyee.DepartmentId);
            return View(empoyee);
        }

        // GET: Empoyees/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Empoyees == null)
            {
                return NotFound();
            }

            var empoyee = await _context.Empoyees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empoyee == null)
            {
                return NotFound();
            }

            return View(empoyee);
        }

        // POST: Empoyees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Empoyees == null)
            {
                return Problem("Entity set 'TestDbContext.Empoyees'  is null.");
            }
            var empoyee = await _context.Empoyees.FindAsync(id);
            if (empoyee != null)
            {
                _context.Empoyees.Remove(empoyee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpoyeeExists(decimal id)
        {
            return (_context.Empoyees?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> EmpoyeesDepartmentView(Guid departmentId)
        {

            var departments = new SelectList(_context.Departments, "Id", "Name");
            ViewData["DepartmentId"] = departments;

            var employees = _context.Empoyees.Where(e => e.DepartmentId == departmentId);

            return View(await employees.ToListAsync());
        }



    }
}
