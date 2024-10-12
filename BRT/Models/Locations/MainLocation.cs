using System.ComponentModel.DataAnnotations;

namespace BRT.Models.Locations
{
    public class MainLocation
    {
        [Key]
        public int id { get; set; }
        [Required]
        [Display(Name ="Location Name")]
        public string LocationName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

    }
}
