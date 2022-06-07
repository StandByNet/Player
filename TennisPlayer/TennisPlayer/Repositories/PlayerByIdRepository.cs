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
        private readonly string urlDto = @"Data\headtohead.json";

        public PlayerByIdRepository(IJsonFileReader _jsonFileReader)
        {
            jsonFileReader = _jsonFileReader;
        }

        public async Task<Player> GetThePlayerById(int id)
        {
            var allPlayers = await jsonFileReader.ReadAsync(urlDto);
            return allPlayers.Where(x => x.Id == id).FirstOrDefault();
        }
    }
}
