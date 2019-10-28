using System.Collections.Generic;
using System.Threading.Tasks;
using SportsLabs.DataServices.Entities;
using SportsLabs.Domain.Entities;

namespace SportsLabs.Services
{
    public interface ITeamDataService
    {
        /// <summary>
        /// Get All Football Teams
        /// </summary>
        /// <returns>FootballTeam List</returns>
        Task<List<FootballTeam>> GetAllTeamsAsync();

        /// <summary>
        /// Get Football Team Detail
        /// </summary>
        /// <param name="teamId">Team Id</param>
        /// <returns>FootballTeam</returns>
        Task<FootballTeam> GetFootballTeamAsync(int teamId);

        /// <summary>
        /// Get Country List
        /// </summary>
        /// <returns>List of Country Strings</returns>
        Task<List<string>> GetCountryList();

        /// <summary>
        /// Update Football Team
        /// </summary>
        /// <param name="team">Team</param>
        /// <returns>Command Response</returns>
        Task<CommandResponse> UpdateTeamAsync(FootballTeam team);
    }
}