using SportsLabs.Domain.Entities;
using SportsLabs.Services.Enums;
using System.Collections.Generic;

namespace SportsLab.Models
{
    public class TeamViewModel
    {
        private const string LOGO_PATH = "http://img.uefa.com/imgml/TP/teams/logos/70x70/[ID].png";
        private const string UEFA_PROFILE = "https://www.uefa.com/teamsandplayers/teams/club=[ID]/profile/index.html";

        /// <summary>
        /// Team Details
        /// </summary>
        public FootballTeam Team { get; set; }

        /// <summary>
        /// Gets or sets the Update Mode
        /// </summary>
        public CommandAction Action { get; set; }

        /// <summary>
        /// Gets Team Logo URL
        /// </summary>
        public string LogoUrl
        {
            get
            {
                if (Team != null)
                {
                    return LOGO_PATH.Replace("[ID]", Team.Id.ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets the Teams UEFA Profile URL
        /// </summary>
        public string ProfileURL
        {
            get
            {
                if (Team != null)
                {
                    return UEFA_PROFILE.Replace("[ID]", Team.Id.ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets or sets List of Country
        /// </summary>
        public List<string> Countries { get; set; }        

        /// <summary>
        /// Gets or Sets the Teams Return Path
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Gets or sets the Team Summary
        /// </summary>
        public string TeamSummary { get; set; }
    }
}