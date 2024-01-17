using MaximApp.Areas.ViewModels;
using MaximApp.Context;
using MaximApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MaximApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    [AutoValidateAntiforgeryToken]
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;

        public SettingController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            List<Setting> settings = _context.settings.ToList();
            return View(settings);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {
            Setting setting = _context.settings.Where(x => x.Id == id).SingleOrDefault();
            return View(setting);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Setting setting)
        {
            Setting settings = _context.settings.FirstOrDefault(x => x.Id == id);
            settings.Key = setting.Key;
            settings.Value = setting.Value;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            Setting setting = _context.settings.FirstOrDefault(x => x.Id == id);
            _context.settings.Remove(setting);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
    }
}
