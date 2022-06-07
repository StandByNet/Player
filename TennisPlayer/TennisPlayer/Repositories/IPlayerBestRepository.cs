using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TennisPlayer.Models;

namespace TennisPlayer.Repositories
{
    public interface IPlayerBestRepository
    {
        Task<IOrderedEnumerable<Player>> GetAllPlayersOrderByBestScor();
    }
}
