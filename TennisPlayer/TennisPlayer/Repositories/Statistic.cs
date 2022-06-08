using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TennisPlayer.Repositories
{
    public class Statistic : IStatistic
    {
        private readonly IJsonFileReader jsonFileReader;
        private readonly IConfiguration Configuration;

        public Statistic(IJsonFileReader _jsonFileReader, IConfiguration configuration)
        {
            jsonFileReader = _jsonFileReader;
            Configuration = configuration;
        }
        public async Task<string> GetCountryMoreRation()
        {
            var allPlayers = await jsonFileReader.ReadAsync(GetUrlData());
            int countLstGangePart = allPlayers.Max(c => c.Data.Last.Count(c => c == 1));
            string countryFound = null;
            foreach (var playerItem in allPlayers)
            {
                if (playerItem.Data.Last.Count(c => c == 1) == countLstGangePart)
                    countryFound = playerItem.Country.Code;
            }
            return countryFound;
        }

        public async Task<double> GetImc()
        {
            var allPlayers = await jsonFileReader.ReadAsync(GetUrlData());
            return allPlayers.Average(c => (c.Data.Weight * 0.001) / (c.Data.Height * 0.01));
        }

        public async Task<double> GetMediane()
        {
            var allPlayers = await jsonFileReader.ReadAsync(GetUrlData());
            int countAllPlayer = allPlayers.Count();
            int medianeTailPlayer = 0;
            int indexMedian = countAllPlayer / 2;
            var medianTails = allPlayers.OrderBy(c => c.Data.Height);
            if (countAllPlayer % 2 != 0)
            {
                medianeTailPlayer = medianTails.ElementAtOrDefault(indexMedian + 1).Data.Height;
            }
            else
            {
                int medianeTailPlayer1 = medianTails.ElementAtOrDefault(indexMedian - 1).Data.Height;
                int medianeTailPlayer2 = medianTails.ElementAtOrDefault(indexMedian).Data.Height;
                medianeTailPlayer = (medianeTailPlayer1 + medianeTailPlayer2) / 2;
            }
            return medianeTailPlayer;
        }
        public string GetUrlData()
        { 
            return Configuration["UrlDto"];
        }

    }
}
