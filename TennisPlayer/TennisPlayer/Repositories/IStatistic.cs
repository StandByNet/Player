using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TennisPlayer.Repositories
{
    public interface IStatistic
    {
        Task<string> GetCountryMoreRation();
        Task<double> GetImc();
        Task<double> GetMediane();
    }
}
