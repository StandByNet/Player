using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TennisPlayer.Models;
using TennisPlayer.Repositories;

namespace TennisPlayer.Controllers
{
    [Route("Player")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        public readonly IPlayerBestRepository playerBestRepository;

        public PlayerController(IPlayerBestRepository _playerBestRepository)
        {
            playerBestRepository = _playerBestRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IOrderedEnumerable<Player>>> GetPlayers()
        {
            try
            {
                var playerLst = await playerBestRepository.GetAllPlayersOrderByBestScor();
                if (playerLst == null)
                    return NotFound();
                return Ok(playerLst);
            }
            catch (Exception ex)
            {
                return NotFound("Anomalie: " + ex.Message);
            }
        }
    }
}
