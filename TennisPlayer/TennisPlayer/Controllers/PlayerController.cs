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
        public readonly IPlayerByIdRepository playerByIdRepository;
        public readonly IStatistic statistic;


        public PlayerController(IPlayerBestRepository _playerBestRepository, IPlayerByIdRepository _playerByIdRepository, IStatistic _statistic)
        {
            playerBestRepository = _playerBestRepository;
            playerByIdRepository = _playerByIdRepository;
            statistic = _statistic;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayerById(int id)
        {
            try
            {
                Player player = await playerByIdRepository.GetThePlayerById(id);
                if (player == null)
                    return NotFound();
                return Ok(player);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("/Statistic")]
        public async Task<ActionResult<string>> Get()
        {
            try
            {
                //get Country have max win
                var contry = await statistic.GetCountryMoreRation();
                //get IMC Moy
                var imcMoy = await statistic.GetImc();
                //get Mediane
                var medianeTailPlayer = await statistic.GetMediane();
                return Ok("contry = " + contry + " , Imc = " + imcMoy + " , medianeTailPlayer : " + medianeTailPlayer);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
