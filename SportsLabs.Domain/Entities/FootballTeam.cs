using System.ComponentModel.DataAnnotations;

namespace SportsLabs.Domain.Entities
{
    /// <summary>
    /// Football Team
    /// </summary>
    public class FootballTeam
    {
        /// <summary>
        /// Gets or sets the Team Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Tam Name
        /// </summary>
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Team name is required")]
        [StringLength(40)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Teams Nation
        /// </summary>
        /// [Display(Name = "Name")]
        [Required(ErrorMessage = "Country is required")]
        [StringLength(50)]
        public string Country { get; set; }

        /// <summary>
        /// Indicates whether Team has been Eliminated
        /// </summary>
        [Display(Name = "Eliminated")]
        [Required(ErrorMessage = "Eliminated is required")]        
        public bool Eliminated { get; set; }

        /// <summary>
        /// Linked to CSS style for Status of Elimination
        /// </summary>
        public string Status
        {
            get
            {                
                if (!Eliminated)
                {
                    return "stillintherunning";
                }
                else
                {
                    return "eliminated";
                }               
            }
        }
    }
}