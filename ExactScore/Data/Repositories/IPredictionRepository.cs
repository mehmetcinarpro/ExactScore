using ExactScore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExactScore.Data.Repositories
{
    public interface IPredictionRepository
    {
        Task<IEnumerable<PredictionViewModel>> GetMissingPredictions(string userId);
        Task<IEnumerable<PredictionViewModel>> GetInProgressPredictions(string userId);
        Task<PlayerOfRoundViewModel> GetPlayerOfRound();
    }
}
