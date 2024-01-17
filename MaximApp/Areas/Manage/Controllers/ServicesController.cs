using MaximApp.Areas.ViewModels;
using MaximApp.Context;
using MaximApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaximApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    [AutoValidateAntiforgeryToken]
    public class ServicesController : Controller
    {
        private readonly AppDbContext _context;

        public ServicesController(AppDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            List<Services> services = _context.services.ToList();
            return View(services);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateServicesVM servicesvm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Services services = new Services()
            {
                Name = servicesvm.Name,
                Description = servicesvm.Description,
                Icon = servicesvm.Icon,
            };
            await _context.AddAsync(services);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Update (int id)
        {
            var services = _context.services.Where(x => x.Id == id).SingleOrDefault();
            UpdateServicesVM vm = new UpdateServicesVM()
            {
                Name = services.Name,
                Description = services.Description,
                Icon = services.Icon,
            };
            return View (vm);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Update (int id,UpdateServicesVM servicesvm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Services service = _context.services.FirstOrDefault(x => x.Id == id);
            service.Name = servicesvm.Name;
            service.Description = servicesvm.Description;
            service.Icon = servicesvm.Icon;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)       
        { 
            Services services = _context.services.FirstOrDefault(x=>x.Id == id);
            _context.services.Remove(services);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }


    }
}
