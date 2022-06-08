using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration Configuration;

        public PlayerBestRepository(IJsonFileReader _jsonFileReader, IConfiguration configuration)
        {
            jsonFileReader = _jsonFileReader;
            Configuration = configuration;
        }

        public async Task<IOrderedEnumerable<Player>> GetAllPlayersOrderByBestScor()
        {
            string urlDto = Configuration["UrlDto"];
            var bestPlayerOrderByPoint = await jsonFileReader.ReadAsync(urlDto);
            return bestPlayerOrderByPoint?.OrderByDescending(x => x?.Data?.Points);
        }
    }
}
