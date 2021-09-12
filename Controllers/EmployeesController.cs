using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeCrud.Models;
using PagedList.Mvc;
using PagedList;
using Microsoft.OData.Edm;

namespace EmployeeCrud.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly EmployeeContext _context;

        public EmployeesController(EmployeeContext context)
        {
            _context = context;
        }
       

        public async Task<IActionResult> Index(int pg=1)
        {
            List<Employee> employees = _context.Employees.ToList();

            //const int pageSize = 10;

            //if (pg < 1)
            //    pg = 1;

            //var recsCount = employees.Count();

            //var pager = new Pager(recsCount, pg, pageSize);

            //int recSkip = (pg - 1) * pageSize;

            //var data = employees.Skip(recSkip).Take(pager.PageSize).ToList();

            //this.ViewBag.pager = pager;

            //return View(data);
            return View(await _context.Employees.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Index(string SortOrder,string SearchString)
        {
            ViewData["NameSortparam"] = string.IsNullOrEmpty(SortOrder) ? "nameDesc" : "";
            ViewData["DateSortParm"] = SortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = SearchString;
            var employees = from s in _context.Employees
                            select s;
            if (!string.IsNullOrEmpty(SearchString))
            {
                employees = employees.Where(s => 
                                   s.FullName.Contains(SearchString) ||
                                   s.Address.Contains(SearchString));

            }
        
            switch (SortOrder)
            {
                case "nameDesc":
                    employees = employees.OrderByDescending(s => s.FullName);
                    break;
                case "Date":
                    employees = employees.OrderBy(s => s.Brirhday);
                    break;
                case "date_desc":
                    employees = employees.OrderByDescending(s => s.Brirhday);
                    break;
                default:
                    employees = employees.OrderBy(s => s.FullName);
                    break;
            }
                  
                   return View(await employees.AsNoTracking().ToListAsync());

        }


        // GET: Employees/Create
        public IActionResult AddOrEdit(int id=0)
        {
            if (id == 0)
                return View(new Employee());
            else
                return View(_context.Employees.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("EmpoyeeID,FullName,Address,Brirhday")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (employee.EmpoyeeID == 0)
                    _context.Add(employee);
                else
                    _context.Update(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var employee = await _context.Employees.FindAsync(id);
        //    if (employee == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(employee);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("EmpoyeeID,FullName,Address,Brirhday")] Employee employee)
        //{
        //    if (id != employee.EmpoyeeID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(employee);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!EmployeeExists(employee.EmpoyeeID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(employee);
        //}

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var empoyee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(empoyee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }


    }
}
