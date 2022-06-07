using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TennisPlayer.Models;

namespace TennisPlayer.Repositories
{
    public class JsonFileReader : IJsonFileReader
    {
        public async Task<List<Player>> ReadAsync(string filePath)
        {
            Headtohead headtohead = new Headtohead();
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string json = reader.ReadToEnd();
                    dynamic files = JsonConvert.DeserializeObject(json);
                    dynamic PlayersList = files.ToObject<Headtohead>();
                    headtohead = (Headtohead)PlayersList;
                }
                await Task.Delay(3000);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return headtohead?.Players;
        }
    }
}

