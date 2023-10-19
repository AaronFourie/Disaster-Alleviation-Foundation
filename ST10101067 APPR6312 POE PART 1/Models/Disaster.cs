using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

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
        [Display(Name = "Start Date")]
        public DateTime? STARTDATE { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime? ENDDATE { get; set; }


        [Required]
        [Display(Name = "Location")]
        public String? LOCATION { get; set; }

        [Required]
        [Display(Name = "Aid Type")]
        public String? AID_TYPE { get; set; }

    }
}
