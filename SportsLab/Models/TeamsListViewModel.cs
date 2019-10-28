using SportsLabs.Domain.Entities;
using System.Collections.Generic;

namespace SportsLab.Models
{
    public class TeamsListViewModel
    {
        /// <summary>
        /// Gets or sets the Teams List
        /// </summary>
        public List<FootballTeam> Teams { get; set; }

        /// <summary>
        /// Gets or sets the Selected Country
        /// Intended for Filtering Teams by Country
        /// </summary>
        public string SelectedCountry { get; set; }

        /// <summary>
        /// Gets the Total Number of Teams
        /// </summary>
        public int TotalTeams
        {
            get
            {
                if (Teams != null)
                {
                    return Teams.Count;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}