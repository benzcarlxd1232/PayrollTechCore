using IT15_LabExam.Data;
using IT15_LabExam.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IT15_LabExam.Controllers
{
    public class PayrollsController : Controller
    {
        private readonly AppDbContext _context;

        public PayrollsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Payrolls — shows all payroll records
        public async Task<IActionResult> Index(int? employeeId)
        {
            var query = _context.Payrolls.Include(p => p.Employee).AsQueryable();

            if (employeeId.HasValue)
            {
                query = query.Where(p => p.EmployeeId == employeeId.Value);
                var emp = await _context.Employees.FindAsync(employeeId.Value);
                ViewBag.FilterEmployee = emp;
            }

            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "EmployeeId", "FullName");
            ViewBag.SelectedEmployeeId = employeeId;

            return View(await query.OrderByDescending(p => p.Date).ToListAsync());
        }

        // GET: Payrolls/Create
        public async Task<IActionResult> Create(int? employeeId)
        {
            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "EmployeeId", "FullName", employeeId);
            var model = new Payroll { Date = DateTime.Today };
            if (employeeId.HasValue) model.EmployeeId = employeeId.Value;
            return View(model);
        }

        // POST: Payrolls/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,Date,DaysWorked,Deduction")] Payroll payroll)
        {
            // Remove computed fields from validation
            ModelState.Remove("GrossPay");
            ModelState.Remove("NetPay");
            ModelState.Remove("Employee");

            if (ModelState.IsValid)
            {
                var employee = await _context.Employees.FindAsync(payroll.EmployeeId);
                if (employee == null)
                {
                    ModelState.AddModelError("EmployeeId", "Selected employee not found.");
                }
                else
                {
                    payroll.GrossPay = payroll.DaysWorked * employee.DailyRate;
                    if (payroll.Deduction > payroll.GrossPay)
                    {
                        ModelState.AddModelError("Deduction", "Deduction cannot exceed Gross Pay.");
                    }
                    else
                    {
                        payroll.NetPay = payroll.GrossPay - payroll.Deduction;
                        _context.Add(payroll);
                        await _context.SaveChangesAsync();
                        TempData["Success"] = "Payroll record added successfully.";
                        return RedirectToAction(nameof(Index), new { employeeId = payroll.EmployeeId });
                    }
                }
            }

            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "EmployeeId", "FullName", payroll.EmployeeId);
            return View(payroll);
        }

        // GET: Payrolls/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var payroll = await _context.Payrolls.Include(p => p.Employee).FirstOrDefaultAsync(p => p.PayrollId == id);
            if (payroll == null) return NotFound();
            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "EmployeeId", "FullName", payroll.EmployeeId);
            return View(payroll);
        }

        // POST: Payrolls/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PayrollId,EmployeeId,Date,DaysWorked,Deduction")] Payroll payroll)
        {
            if (id != payroll.PayrollId) return NotFound();

            ModelState.Remove("GrossPay");
            ModelState.Remove("NetPay");
            ModelState.Remove("Employee");

            if (ModelState.IsValid)
            {
                var employee = await _context.Employees.FindAsync(payroll.EmployeeId);
                if (employee == null)
                {
                    ModelState.AddModelError("EmployeeId", "Selected employee not found.");
                }
                else
                {
                    payroll.GrossPay = payroll.DaysWorked * employee.DailyRate;
                    if (payroll.Deduction > payroll.GrossPay)
                    {
                        ModelState.AddModelError("Deduction", "Deduction cannot exceed Gross Pay.");
                    }
                    else
                    {
                        payroll.NetPay = payroll.GrossPay - payroll.Deduction;
                        try
                        {
                            _context.Update(payroll);
                            await _context.SaveChangesAsync();
                            TempData["Success"] = "Payroll record updated successfully.";
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!_context.Payrolls.Any(p => p.PayrollId == payroll.PayrollId))
                                return NotFound();
                            throw;
                        }
                        return RedirectToAction(nameof(Index), new { employeeId = payroll.EmployeeId });
                    }
                }
            }

            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "EmployeeId", "FullName", payroll.EmployeeId);
            return View(payroll);
        }

        // GET: Payrolls/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var payroll = await _context.Payrolls.Include(p => p.Employee).FirstOrDefaultAsync(p => p.PayrollId == id);
            if (payroll == null) return NotFound();
            return View(payroll);
        }

        // POST: Payrolls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payroll = await _context.Payrolls.FindAsync(id);
            int? empId = payroll?.EmployeeId;
            if (payroll != null)
            {
                _context.Payrolls.Remove(payroll);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Payroll record deleted successfully.";
            }
            return RedirectToAction(nameof(Index), new { employeeId = empId });
        }
    }
}
