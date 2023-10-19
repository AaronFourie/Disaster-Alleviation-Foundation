using System.ComponentModel.DataAnnotations;

namespace ST10101067_APPR6312_POE_PART_2.Models
{
    public class Admin
    {
        [Key]
        [Display(Name = "Admin_ID")]
        public int ADMIN_ID { get; set; }

        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string EMAIL { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public String? PASSWORD { get; set; }

    }
}
