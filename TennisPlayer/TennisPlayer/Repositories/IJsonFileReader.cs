using System.Collections.Generic;
using System.Threading.Tasks;
using TennisPlayer.Models;

namespace TennisPlayer.Repositories
{
    public interface IJsonFileReader
    {
        public Task<List<Player>> ReadAsync(string filePath);
    }
}