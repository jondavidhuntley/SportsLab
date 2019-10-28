using Microsoft.Extensions.Logging;
using SportsLabs.DataServices.Entities;
using SportsLabs.Domain.Entities;
using SportsLabs.Services.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsLabs.Services
{
    public class TeamDataService : ITeamDataService
    {
        private readonly ITeamDAL _teamDAL;        
        private readonly ILogger<TeamDataService> _logger;

        /// <summary>
        /// Creates new instance of Team Data Service
        /// </summary>
        /// <param name="teamDAL">Team DAL</param>        
        /// <param name="logger">EVent Logger</param>
        public TeamDataService(ITeamDAL teamDAL, ILogger<TeamDataService> logger)
        {
            _teamDAL = teamDAL;            
            _logger = logger;
        }

        /// <summary>
        /// Get All Football Teams
        /// </summary>
        /// <returns>FootballTeam List</returns>
        public async Task<List<FootballTeam>> GetAllTeamsAsync()
        {
            List<FootballTeam> teams = null;
            _logger.LogInformation("Fetching All Football Teams");
            
            var data = await _teamDAL.GetAllTeamsAsync();

            if (data != null)
            {
                teams = data.ToList();
                _logger.LogInformation($"Found & Recovered: {teams.Count} teams");
            }
            else
            {
                _logger.LogWarning("Unable to find any teams");
            }

            return teams;
        }

        /// <summary>
        /// Get Football Team Detail
        /// </summary>
        /// <param name="teamId">Team Id</param>
        /// <returns>FootballTeam</returns>
        public async Task<FootballTeam> GetFootballTeamAsync(int teamId)
        {
            var data = await _teamDAL.GetTeamDetailsAsync(teamId);

            if (data != null)
            {
                _logger.LogInformation($"Found Football Team: {data.Name}");
            }

            return data;
        }

        /// <summary>
        /// Get Country List
        /// </summary>
        /// <returns>List of Country Strings</returns>
        public async Task<List<string>> GetCountryList()
        {
            List<string> countries = null;

            var data = await _teamDAL.GetTeamCountriesAsync();

            if (data != null)
            {
                countries = data.ToList();
                _logger.LogInformation($"Found Football Team {countries.Count} Countries");
            }

            return countries;
        }

        /// <summary>
        /// Update Football Team
        /// </summary>
        /// <param name="team">Team</param>
        /// <returns>Command Response</returns>
        public async Task<CommandResponse> UpdateTeamAsync(FootballTeam team)
        {
            var response = new CommandResponse
            {
                Action = Enums.CommandAction.Edit,
                RecordId = team.Id                
            };

            try
            {
                int updatedRows = await _teamDAL.UpdateTeamAsync(team);

                if (updatedRows > 0)
                {
                    response.Success = true;
                    _logger.LogInformation($"Successfully Updated Football Team ID:{team.Id}");
                }
            }
            catch(Exception ex)
            {
                _logger.LogCritical($"Unable to Update Team: {team.Name} - Exception:{ex.Message}-{ex.StackTrace}");
                response.FaultMessage = ex.Message;
            }

            return response;
        }
    }
}