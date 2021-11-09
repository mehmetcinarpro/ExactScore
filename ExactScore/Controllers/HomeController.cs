using ExactScore.Data;
using ExactScore.Data.Repositories;
using ExactScore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ExactScore.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStandingsRepository _standingsRepository;
        private readonly IPredictionRepository _predictionRepository;

        public HomeController(ApplicationDbContext context,
            IStandingsRepository standingsRepository,
            IPredictionRepository predictionRepository
            )
        {
            _context = context;
            _standingsRepository = standingsRepository;
            _predictionRepository = predictionRepository;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var otherUsersIds = _context.Users.Where(u => u.Id != userId).Select(u => u.Id).ToList();
            var othersPredictions = new List<PredictionViewModel>();
            foreach (var id in otherUsersIds)
            {
                othersPredictions.AddRange(await _predictionRepository.GetInProgressPredictions(id));
            }

            return View(new HomeViewModel
            {
                MissingPredictions = await _predictionRepository.GetMissingPredictions(userId),
                InProgressPredictions = await _predictionRepository.GetInProgressPredictions(userId),
                InProgressOthersPredictions = othersPredictions,
                Standings = await _standingsRepository.GetStandings(),
                PlayerOfRound = await _predictionRepository.GetPlayerOfRound()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStandings(int id)
        {
            await _standingsRepository.RefreshStandings();
            return RedirectToAction(nameof(Index));
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
