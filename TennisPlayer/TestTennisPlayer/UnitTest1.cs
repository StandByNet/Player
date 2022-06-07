using Assert = Xunit.Assert;
using TennisPlayer.Controllers;
using TennisPlayer.Models;
using TennisPlayer.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using System.Collections.Generic;

namespace TestTennisPlayer
{
    public class UnitTest1
    {
        private readonly Mock<IPlayerBestRepository> playerBestRepository = new Mock<IPlayerBestRepository>();
        private readonly Mock<IPlayerByIdRepository> playerByIdRepository = new Mock<IPlayerByIdRepository>();
        private readonly Mock<IStatistic> statistic = new Mock<IStatistic>();
        private readonly Mock<IJsonFileReader> jsonFileReader = new Mock<IJsonFileReader>();
        private readonly Random rand = new Random();

        #region Test List players  order by of The best
        [Fact]
        public async Task GetPlayers_ListNull_return_NoFound()
        {
            //arrange
            jsonFileReader.Setup(js => js.ReadAsync(It.IsAny<Guid>().ToString()))
                .ReturnsAsync(GetPlayers());
            PlayerBestRepository playerBestRepository1 = new PlayerBestRepository(jsonFileReader.Object);
            playerBestRepository.Setup(repo => repo.GetAllPlayersOrderByBestScor())
               .ReturnsAsync((IOrderedEnumerable<Player>)null);

            var controller = new PlayerController(playerBestRepository.Object, playerByIdRepository.Object, statistic.Object);
            //act
            var result = await controller.GetPlayers();
            //assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetPlayers_ListNoNull_return_OkSecces()
        {
            //arrange
            var lstPlayers = GetPlayers();
            var playersExpected = GetOrderPlayers(lstPlayers);
            jsonFileReader.Setup(js => js.ReadAsync(It.IsAny<Guid>().ToString()))
                .ReturnsAsync(lstPlayers);
            PlayerBestRepository playerBestRepository1 = new PlayerBestRepository(jsonFileReader.Object);
            playerBestRepository.Setup(repo => repo.GetAllPlayersOrderByBestScor())
               .ReturnsAsync(playersExpected);

            var controller = new PlayerController(playerBestRepository.Object, playerByIdRepository.Object, statistic.Object);
            //act
            var result = await controller.GetPlayers();
            //assert
            ((OkObjectResult)result.Result).Value.Should().BeEquivalentTo(playersExpected, option => option.ComparingByMembers<IOrderedEnumerable<Player>>());
        }

        private IOrderedEnumerable<Player> GetOrderPlayers(List<Player> playersForOrder)
        {
            return playersForOrder.OrderByDescending(c => c?.Data?.Points);
        }
        #endregion

        #region Unit Test <Get Player by ID>
        [Fact]
        public async Task GetThePlayerById_WithidNonExsistPlayer_returnNotFound()
        {
            int id = rand.Next();
            //arrange
            playerByIdRepository.Setup(repo => repo.GetThePlayerById(id))
                .ReturnsAsync((Player)null);
            var controller = new PlayerController(playerBestRepository.Object, playerByIdRepository.Object, statistic.Object);
            //act
            var result = await controller.GetPlayerById(17);
            //assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetThePlayerById_WithIdExsistPlayer_ReturnSuccess()
        {
            Player playerExpected = CreatPlayer();
            var lstPlyersIn = GetPlayers();
            jsonFileReader.Setup(repo => repo.ReadAsync(It.IsAny<Guid>().ToString()))
              .ReturnsAsync(lstPlyersIn);
            PlayerByIdRepository playerByIdRepository1 = new PlayerByIdRepository(jsonFileReader.Object);
            playerByIdRepository.Setup(repo => repo.GetThePlayerById(It.IsAny<int>()))
                .ReturnsAsync(playerExpected);
            var controller = new PlayerController(playerBestRepository.Object, playerByIdRepository.Object, statistic.Object);
            //act
            var result = await controller.GetPlayerById(17);
            //assert
            ((OkObjectResult)result.Result).Value.Should().BeEquivalentTo(playerExpected, option => option.ComparingByMembers<Player>());
            // Assert.IsType<Player>(((OkObjectResult)result.Result).Value);
        }

        private Player CreatPlayer()
        {
            int[] last = new int[5] { 1, 1, 1, 1, 1 };
            Country country = new Country()
            {
                Code = "USA",
                Picture = Guid.NewGuid().ToString()
            };
            Data data = new Data()
            {
                Age = rand.Next(),
                Points = rand.Next(),
                Weight = 1000,//rand.Next(),
                Height =100,// rand.Next(),
                Last = last,
                Rank = rand.Next()
            };
            return new Player()
            {
                Id = rand.Next(),
                Firstname = Guid.NewGuid().ToString(),
                Lastname = Guid.NewGuid().ToString(),
                Shortname = Guid.NewGuid().ToString(),
                Sex = Guid.NewGuid().ToString(),
                Country = country,
                Picture = Guid.NewGuid().ToString(),
                Data = data,
            };
        }

        private List<Player> GetPlayers()
        {
            List<Player> k = new List<Player>();
            for (int i = 0; i < 2; i++)
            {
                k.Add(CreatPlayer());
            }
            return k;
        }
        #endregion

        #region unit Test Stat
        [Fact]
        public async Task Get_WithIdExsistPlayer_ReturnSuccess()
        {
            Player playerExpected = CreatPlayer();
            var lstPlyersIn = GetPlayers();
            jsonFileReader.Setup(repo => repo.ReadAsync(It.IsAny<Guid>().ToString()))
              .ReturnsAsync(lstPlyersIn);
            playerByIdRepository.Setup(repo => repo.GetThePlayerById(It.IsAny<int>()))
                .ReturnsAsync(playerExpected);
            statistic.Setup(repo => repo.GetCountryMoreRation())
                .ReturnsAsync("USA");
            statistic.Setup(repo => repo.GetImc())
                .ReturnsAsync(1);
            statistic.Setup(repo => repo.GetMediane())
                .ReturnsAsync(100);
            //act
            var controller = new PlayerController(playerBestRepository.Object, playerByIdRepository.Object, statistic.Object);
            var result = await controller.Get();

            //assert
            ((OkObjectResult)result.Result).Value.Should().BeEquivalentTo("contry = USA , Imc = 1 , medianeTailPlayer : 100");

        }
        #endregion
    }
}
