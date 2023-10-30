using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10101067_APPR6312_POE_PART_2.Models
{
    public class Disaster
    {
        [Key]
        [Required]
        public int DISTATER_ID { get; set; }

        public string USERNAME { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")] // Store only the date part
        [Display(Name = "Start Date")]
        public DateTime? STARTDATE { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")] // Store only the date part
        [Display(Name = "End Date")]
        public DateTime? ENDDATE { get; set; }

        [Required]
        [Display(Name = "Location")]
        public string? LOCATION { get; set; }

        [Required]
        [Display(Name = "Aid Type")]
        public string? AID_TYPE { get; set; }

        [Display(Name = "Is Active")]
        public int IsActive { get; set; }
    }
}
