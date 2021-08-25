using ExactScore.Data;
using ExactScore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ExactScore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Fixtures
                .Include(p => p.HomeTeam)
                .Include(p => p.AwayTeam)
                .Include(p => p.Round)
                .Where(p => !p.Round.Closed)
                .OrderBy(p => p.Round.OrderNumber).ThenBy(p => p.Date)
                .Take(4);
            return View(await applicationDbContext.ToListAsync());
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
