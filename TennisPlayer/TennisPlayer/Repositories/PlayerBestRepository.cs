using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TennisPlayer.Models;

namespace TennisPlayer.Repositories
{
    public class PlayerBestRepository : IPlayerBestRepository
    {
        private readonly IJsonFileReader jsonFileReader;
        private readonly string urlDto = @"Data\headtohead.json";

        public PlayerBestRepository(IJsonFileReader _jsonFileReader)
        {
            jsonFileReader = _jsonFileReader;
        }

        public async Task<IOrderedEnumerable<Player>> GetAllPlayersOrderByBestScor()
        {
            var bestPlayerOrderByPoint = await jsonFileReader.ReadAsync(urlDto);
            return bestPlayerOrderByPoint?.OrderByDescending(x => x?.Data?.Points);
        }
    }
}
