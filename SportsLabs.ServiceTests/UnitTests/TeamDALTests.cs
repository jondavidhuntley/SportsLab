using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SportsLabs.Domain.Entities;
using SportsLabs.Services.DAL.Dapper;
using System.Linq;
using System.Threading.Tasks;

namespace SportsLabs.ServiceTests.UnitTests
{
    [TestFixture]
    public class TeamDALTests
    {
        private string _dbConnection = string.Empty;

        [SetUp]
        public void Initialise()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("TestSettings.json")
                .Build();           

            _dbConnection = config.GetConnectionString("TeamDatabase");
        }

        [Test]
        public async Task Select_Teams_Test()
        {
            var mockLogger = new Mock<ILogger<TeamDAL>>();
            var dalService = new TeamDAL(_dbConnection, mockLogger.Object);

            var results = dalService.GetAllTeamsAsync().GetAwaiter().GetResult();
            var teams = results.ToList();

            // Assertions - Check we got some results
            Assert.NotNull(results);
            Assert.True(teams.Count > 0);
        }

        [TestCase(2610, ExpectedResult = "Olympiacos")]
        [TestCase(7889, ExpectedResult = "Liverpool")]
        [TestCase(50004, ExpectedResult = "Grasshoppers")]
        public async Task<string> TestTeamDetailFetch(int teamId)
        {
            var mockLogger = new Mock<ILogger<TeamDAL>>();
            var dalService = new TeamDAL(_dbConnection, mockLogger.Object);

            var team = await dalService.GetTeamDetailsAsync(teamId);

            return team.Name;
        }

        [Test]
        public async Task Select_Team_Countries_Test()
        {
            var mockLogger = new Mock<ILogger<TeamDAL>>();
            var dalService = new TeamDAL(_dbConnection, mockLogger.Object);

            var results = dalService.GetTeamCountriesAsync().GetAwaiter().GetResult();
            var countries = results.ToList();

            // Assertions - Check we got some results
            Assert.NotNull(results);
            Assert.True(countries.Count > 0);
        }

        [Test]
        public async Task Update_Team_Test()
        {
            var mockLogger = new Mock<ILogger<TeamDAL>>();
            var dalService = new TeamDAL(_dbConnection, mockLogger.Object);

            var newTeamName = "Manchester United";

            var testTeam = new FootballTeam
            {
                Id = 7889,
                Name = newTeamName,
                Country = "England",
                Eliminated = true
            };

            // Fetch an existing Team & store name
            var currentTeam = await dalService.GetTeamDetailsAsync(testTeam.Id);
            var originalName = currentTeam.Name;

            // Update Team
            var affectedRows = dalService.UpdateTeamAsync(testTeam).GetAwaiter().GetResult();

            // Get Updated Team
            var updatedTeam = await dalService.GetTeamDetailsAsync(testTeam.Id);

            if (affectedRows > 0)
            {
                testTeam.Name = originalName;

                // Reset Team Name to Original
                var resetRows = dalService.UpdateTeamAsync(testTeam).GetAwaiter().GetResult();
            }

            // Assertions - Check we updated a single row
            Assert.AreEqual(affectedRows, 1);
            Assert.AreEqual(updatedTeam.Name, newTeamName);
        }
    }
}