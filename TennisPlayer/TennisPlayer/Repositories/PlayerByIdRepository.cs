using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TennisPlayer.Models;

namespace TennisPlayer.Repositories
{
    public class PlayerByIdRepository : IPlayerByIdRepository
    {
        private readonly IJsonFileReader jsonFileReader;
        private readonly IConfiguration Configuration;

        public PlayerByIdRepository(IJsonFileReader _jsonFileReader, IConfiguration configuration)
        {
            jsonFileReader = _jsonFileReader;
            Configuration = configuration;
        }

        public async Task<Player> GetThePlayerById(int id)
        {
            string urlDto = Configuration["UrlDto"];
            var allPlayers = await jsonFileReader.ReadAsync(urlDto);
            return allPlayers.Where(x => x.Id == id).FirstOrDefault();
        }
    }
}
