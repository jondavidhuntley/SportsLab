using System.Collections.Generic;
using System.Threading.Tasks;
using SportsLabs.Domain.Entities;

namespace SportsLabs.Services.DAL
{
    /// <summary>
    /// Team DAL Interface
    /// </summary>
    public interface ITeamDAL
    {
        /// <summary>
        /// Get All Football Teams
        /// </summary>
        /// <returns>IEnumerable FootballTeam</returns>
        Task<IEnumerable<FootballTeam>> GetAllTeamsAsync();

        /// <summary>
        /// Get Team Details
        /// </summary>
        /// <param name="teamId">Team Id</param>
        /// <returns>Football Team</returns>
        Task<FootballTeam> GetTeamDetailsAsync(int teamId);

        /// <summary>
        /// Get Distinct Countries
        /// </summary>
        /// <returns>List of string</returns>
        Task<IEnumerable<string>> GetTeamCountriesAsync();

        /// <summary>
        /// Update Existing Team
        /// </summary>
        /// <param name="team">Football Team</param>
        /// <returns>Affected Rows</returns>
        Task<int> UpdateTeamAsync(FootballTeam team);
    }
}