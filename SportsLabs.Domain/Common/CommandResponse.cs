using SportsLabs.Services.Enums;
using System;

namespace SportsLabs.DataServices.Entities
{
    /// <summary>
    /// Command Response (Carries Command Operation Responses for Update, Delete and Add Operations)
    /// </summary>
    public class CommandResponse
    {
        public CommandResponse()
        {
            this.Completed = DateTime.Now;
        }

        public int RecordId { get; set; }

        public CommandAction Action { get; set; }

        public DateTime Completed { private set; get; }

        public bool Success { get; set; }

        public string FaultMessage { get; set; }
    }
}