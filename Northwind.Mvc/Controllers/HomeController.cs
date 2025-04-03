using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.EntityModels;
using Northwind.Mvc.Models;
using System.Diagnostics;

namespace Northwind.Mvc.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NorthwindDatabaseContext db;

        public HomeController(ILogger<HomeController> logger,
            NorthwindDatabaseContext injectedContext)
        {
            _logger = logger;
            db = injectedContext;
        }

        public IActionResult Index()
        {
            HomeIndexViewModel model = new(

                VisitorCount: Random.Shared.Next(1, 1001),
                Categories: db.Categories.ToList(),
                Products: db.Products.ToList()

            );

            return View(model);
        }
        public IActionResult ProductDetail(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest("You must pass a product ID in the route, " +
                    "for example, /Home/ProductDetail/21");
            }
            Product? model = db.Products
                .SingleOrDefault(p => p.ProductId == id);
            if (model is null)
            {
                return NotFound($"ProductId{id} not found");
            }

            return View(model);
        }

        public IActionResult CategoryDetail(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest("You must pass a category ID in the route, " +
                    "for example, /Home/CategoryDetail/1");
            }

            Category? model = db.Categories
                .Include(c => c.Products)
                .SingleOrDefault(c => c.CategoryId == id);

            if (model is null)
            {
                return NotFound($"CategoryId{id} not found");
            }

            return View(model);
        }

        public IActionResult ModelBinding()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ModelBinding(Thing thing)
        {
            HomeModelBindingViewModel model = new(
                Thing: thing, HasErrors: !ModelState.IsValid,
                ValidationErrors: ModelState.Values
                .SelectMany(state => state.Errors)
                .Select(error => error.ErrorMessage)
            );
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}