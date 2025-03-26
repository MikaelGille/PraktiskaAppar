using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Northwind.EntityModels;

namespace Northwind.Razor.Employees.MyFeature.Pages;

public class EmployeesModel(NorthwindDatabaseContext context) : PageModel
{
    private NorthwindDatabaseContext _context = context;

    public IList<Employee>? Employees { get; set; }

    public async Task OnGetAsync()
    {
        ViewData["Title"] = "Northwind App - Employees";

        Employees = await _context.Employees
                                  .OrderBy(e => e.LastName)
                                  .ThenBy(e => e.FirstName)
                                  .ToListAsync();
    }
}