using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SportsLabs.DataServices.Entities;
using SportsLabs.Domain.Entities;
using SportsLabs.Services;
using SportsLabs.Services.DAL;
using SportsLabs.Services.DAL.Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsLabs.ServiceTests.UnitTests
{
    [TestFixture]
    public class DataServiceTests
    {        
        private static IServiceProvider _serviceProvider;

        [SetUp]
        public void Initialise()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("TestSettings.json")
                .Build();

            IServiceCollection services = new ServiceCollection()
                .AddLogging();

            // Register DAL
            services.AddSingleton<ITeamDAL>((svc) =>
            {
                var dbConnection = config.GetConnectionString("TeamDatabase");
                var logger = svc.GetService<ILogger<TeamDAL>>();

                return new TeamDAL(dbConnection, logger);
            });

            // Register Data Service
            services.AddSingleton<ITeamDataService, TeamDataService>();

            // Create Service Provider
            _serviceProvider = services.BuildServiceProvider();
        }

        [Test]
        public async Task ResolveDataService_Select_Teams_Test()
        {
            var dataService = _serviceProvider.GetService<ITeamDataService>();

            var teams = await dataService.GetAllTeamsAsync();

            Assert.NotNull(teams);
            Assert.True(teams.Count > 0);
        }

        [Test]
        public async Task SelectAllTeams_Test_DAL_Method_Called()
        {
            var mockDAL = new Mock<ITeamDAL>();
            var mockLogger = new Mock<ILogger<TeamDataService>>();

            // Setup Mock DAL
            IEnumerable<FootballTeam> teamsList = new List<FootballTeam>
            {
                new FootballTeam { Id = 1234, Name = "Liverpool", Country = "England", Eliminated = false },
                new FootballTeam { Id = 1234, Name = "Nottingham Forest", Country = "England", Eliminated = true },
                new FootballTeam { Id = 1234, Name = "Derby County", Country = "England", Eliminated = false }
            };

            mockDAL.Setup(d => d.GetAllTeamsAsync()).Returns(Task.FromResult(teamsList));

            // Create Data Service
            var dataService = new TeamDataService(mockDAL.Object, mockLogger.Object);

            // Call Data Service
            var teams = await dataService.GetAllTeamsAsync();

            // Check DAL Get All Teams was Called Once
            mockDAL.Verify(d => d.GetAllTeamsAsync(), Times.Once);

            Assert.NotNull(teams);
            Assert.AreEqual(teams.Count, 3);
        }

        [Test]
        public async Task SelectSingleTeam_Test_DAL_Method_Called()
        {
            var mockDAL = new Mock<ITeamDAL>();
            var mockLogger = new Mock<ILogger<TeamDataService>>();

            var teamId = 1234;
            var expectedTeam = new FootballTeam { Id = teamId, Name = "Liverpool", Country = "England", Eliminated = false };
           
            mockDAL.Setup(d => d.GetTeamDetailsAsync(teamId)).Returns(Task.FromResult(expectedTeam));

            // Create Data Service
            var dataService = new TeamDataService(mockDAL.Object, mockLogger.Object);

            // Call Data Service
            var team = await dataService.GetFootballTeamAsync(teamId);

            // Check DAL Get Team Detail was Called Once
            mockDAL.Verify(d => d.GetTeamDetailsAsync(teamId), Times.Once);

            Assert.NotNull(team);
            Assert.AreEqual(team.Name, expectedTeam.Name);
        }

        [Test]
        public async Task UpdateTeam_Test_Verification()
        {
            var mockDAL = new Mock<ITeamDAL>();
            var mockLogger = new Mock<ILogger<TeamDataService>>();

            var teamId = 1234;
            var team = new FootballTeam { Id = teamId, Name = "Liverpool", Country = "England", Eliminated = false };

            var expectedResponse = new CommandResponse
            {
                RecordId = teamId,
                Action = Services.Enums.CommandAction.Edit,
                Success = true
            };

            // Setup DAL to return 1 Affected Row after Update
            mockDAL.Setup(d => d.UpdateTeamAsync(team)).Returns(Task.FromResult(1));

            // Create Data Service
            var dataService = new TeamDataService(mockDAL.Object, mockLogger.Object);

            // Call Update
            var response = await dataService.UpdateTeamAsync(team);

            // Check DAL Update Method was Called
            mockDAL.Verify(d => d.UpdateTeamAsync(team), Times.Once);

            Assert.AreEqual(expectedResponse.RecordId, response.RecordId);
            Assert.True(expectedResponse.Success);
        }
    }
 }