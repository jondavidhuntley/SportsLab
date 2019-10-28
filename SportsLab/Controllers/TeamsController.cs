using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SportsLab.Models;
using SportsLabs.Services;
using SportsLabs.Services.Enums;
using System.Threading.Tasks;

namespace SportsLab.Controllers
{
    /// <summary>
    /// Teams Controller
    /// </summary>
    public class TeamsController : Controller
    {
        private readonly ITeamDataService _dataService;
        private readonly ILogger<TeamsController> _logger;

        /// <summary>
        /// Teams Controller
        /// </summary>
        /// <param name="dataService"></param>
        /// <param name="logger"></param>
        public TeamsController(ITeamDataService dataService, ILogger<TeamsController> logger)
        {
            _dataService = dataService;
            _logger = logger;
        }

        /// <summary>
        /// Teams List main Index View
        /// </summary>
        /// <returns>Teams List View Model</returns>
        [HttpGet]
        [Route("Teams")]
        public async Task<IActionResult> Index()
        {
            // Create View Model
            var model = new TeamsListViewModel
            {
                Teams = await _dataService.GetAllTeamsAsync(),
                SelectedCountry = "All"
            };

            return View(model);
        }

        /// <summary>
        /// Get Team Detail
        /// </summary>
        /// <param name="teamId">Team Id</param>
        /// <param name="returnUrl">Return Url (To Team List)</param>
        /// <returns>Team Detail View Model</returns>
        [HttpGet]
        [Route("Teams/Team/{teamId}")]
        public async Task<ActionResult> TeamDetail(int teamId)
        {            
            var team = await _dataService.GetFootballTeamAsync(teamId);

            if (team != null)
            {
                var model = new TeamViewModel
                {
                    Team = team,
                    Countries = await _dataService.GetCountryList(),
                    Action = CommandAction.Edit,
                    TeamSummary = $"{team.Name} from {team.Country}"                    
                };

                return View(model);
            }
            else
            {
                SetMessage("Team Not Found", $"Sorry we could NOT find a matching Team for TeamID:{teamId} !");

                return RedirectToAction("Index");
            }            
        }

        /// <summary>
        /// Handles Update from Team Detail View
        /// </summary>
        /// <param name="model">Team ViewModel</param>
        /// <returns>Redirected to Teams Index or back to Team Detail on Invalid Model State</returns>
        [HttpPost]
        [Route("Teams/Team/{teamId}")]
        public async Task<ActionResult> TeamDetail(TeamViewModel model)
        {                        
            if (ModelState.IsValid)
            {
                var team = model.Team;

                if (model.Action == CommandAction.Edit)
                {
                    var result = await _dataService.UpdateTeamAsync(team);

                    if (result.Success)
                    {
                        SetMessage("Update Confirmation", $"Your updates to Team:{team.Name} were successful!");
                        
                        // Clear any Cache                        
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        SetFailureMessage("Your update was NOT Successful!");
                        return RedirectToAction("Index");
                    }
                }
                else if (model.Action == CommandAction.New)
                {
                    // Add Call to Create new Team Here 
                    return RedirectToAction("Index");
                }
            }
            else
            {
                SetFailureMessage("Please supply the missing informtion!");

                // Fetch Countries Again - TODO Manage Cache in Session within Data Service
                model.Countries = await _dataService.GetCountryList();
            }

            return View(model);
        }

        /// <summary>
        /// Sets Failure Message in Temp Data
        /// </summary>
        /// <param name="content">Failure Detail</param>
        private void SetFailureMessage(string content)
        {
            TempData["MessageType"] = "FAIL";
            TempData["Message"] = content;
        }

        /// <summary>
        /// Sets Infomration/Confirmation message
        /// </summary>
        /// <param name="title">Message Title</param>
        /// <param name="content">Message Content</param>
        private void SetMessage(string title, string content)
        {
            TempData["MessageType"] = "INFO";
            TempData["Title"] = title;
            TempData["Message"] = content;
        }
    }
}