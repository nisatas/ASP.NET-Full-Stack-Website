using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BeygirMuhendisi.Web.Data;
using System.Linq;

namespace BeygirMuhendisi.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly UygulamaDbContext _context;

        public AdminController(UygulamaDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var tahminler = _context.Tahminler
                .OrderByDescending(t => t.Tarih)
                .ToList();
            return View(tahminler);
        }
    }
}