using Dapper;
using Microsoft.Extensions.Logging;
using SportsLabs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SportsLabs.Services.DAL.Dapper
{
    /// <summary>
    /// Handles Football Team Data Processing with DB
    /// </summary>
    public class TeamDAL : ITeamDAL
    {
        private readonly string _sqlConnection;
        private readonly ILogger<TeamDAL> _logger;

        /// <summary>
        /// Creates new instance of TeamDAL
        /// </summary>
        /// <param name="sqlConnection">SQL Server Database Connection</param>
        /// <param name="logger">Event Logger</param>
        public TeamDAL(string sqlConnection, ILogger<TeamDAL> logger)
        {
            _sqlConnection = sqlConnection;
            _logger = logger;
        }

        /// <summary>
        /// Get All Football Teams
        /// </summary>
        /// <returns>IEnumerable FootballTeam</returns>
        public async Task<IEnumerable<FootballTeam>> GetAllTeamsAsync()
        {
            IEnumerable<FootballTeam> data = null;
            _logger.LogInformation("Starting Team Data Fetch");
            try
            {
                if (string.IsNullOrEmpty(_sqlConnection))
                {
                    throw new Exception("No SQL Server Connection String");
                }

                var query = "SELECT ID, Name, Country, Eliminated FROM Team ORDER BY Name ASC";

                using (var sqlConnection = new SqlConnection(_sqlConnection))
                {
                    await sqlConnection.OpenAsync();                    

                    var results = await sqlConnection.QueryMultipleAsync(query, null, null, 30, CommandType.Text);
                    data = await results.ReadAsync<FootballTeam>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            _logger.LogInformation("Completed Data Fetch");
            return data;
        }

        /// <summary>
        /// Get Distinct Countries
        /// </summary>
        /// <returns>List of string</returns>
        public async Task<IEnumerable<string>> GetTeamCountriesAsync()
        {
            IEnumerable<string> data = null;
            _logger.LogInformation("Starting Team Country Data Fetch");
            try
            {
                if (string.IsNullOrEmpty(_sqlConnection))
                {
                    throw new Exception("No SQL Server Connection String");
                }

                var query = "SELECT DISTINCT Country FROM Team ORDER BY Country ASC";

                using (var sqlConnection = new SqlConnection(_sqlConnection))
                {
                    await sqlConnection.OpenAsync();                    
                    
                    var results = await sqlConnection.QueryMultipleAsync(query, null, null, 30, CommandType.Text);
                    data = await results.ReadAsync<string>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            _logger.LogInformation("Completed Data Fetch");
            return data;
        }

        /// <summary>
        /// Get Team Details
        /// </summary>
        /// <param name="teamId">Team Id</param>
        /// <returns>Football Team</returns>
        public async Task<FootballTeam> GetTeamDetailsAsync(int teamId)
        {
            FootballTeam data = null;

            try
            {
                var query = $"SELECT ID, Name, Country, Eliminated FROM Team WHERE ID={teamId}";

                using (var sqlConnection = new SqlConnection(_sqlConnection))
                {
                    await sqlConnection.OpenAsync();
                    
                    // var dynParams = new DynamicParameters();                    
                    // dynParams.Add("@TeamId", teamId, DbType.Int32);
                    // data = await sqlConnection.QuerySingleOrDefaultAsync<FootballTeam>("usp_Team_GetDetail", dynParams, commandType: CommandType.StoredProcedure);
                    data = await sqlConnection.QuerySingleOrDefaultAsync<FootballTeam>(query, null, null, 30, CommandType.Text);

                    _logger.LogInformation("Completed Data Fetch");
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
            }

            return data;
        }

        /// <summary>
        /// Update Existing Team
        /// </summary>
        /// <param name="team">Football Team</param>
        /// <returns>Affected Rows</returns>
        public async Task<int> UpdateTeamAsync(FootballTeam team)
        {
            var affectedRows = 0;            
            
            var query = $"UPDATE Team SET Name='{team.Name}', Country='{team.Country}', Eliminated='{team.Eliminated}' WHERE Id={team.Id}";

            _logger.LogInformation($"Updating Football Team with ID:{team.Id}");

            using (var sqlConnection = new SqlConnection(_sqlConnection))
            {
                await sqlConnection.OpenAsync();
                            
                affectedRows = await sqlConnection.ExecuteAsync(query, null, null, 30, CommandType.Text);
                if (affectedRows > 0)
                {
                    _logger.LogInformation($"Successfully Updated Team with Id:{team.Id}");
                }
            }

            return affectedRows;
        }
    }
}