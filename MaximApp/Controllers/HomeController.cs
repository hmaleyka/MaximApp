
using MaximApp.Context;
using MaximApp.Models;
using MaximApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MaximApp.Controllers
{
    public class HomeController : Controller
    {
       private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
               services=_context.services.ToList(),
            };
            return View(homeVM);
        }

        
    }
}