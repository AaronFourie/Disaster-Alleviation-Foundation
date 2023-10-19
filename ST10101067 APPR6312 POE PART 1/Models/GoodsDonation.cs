using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10101067_APPR6312_POE_PART_2.Models
{
    public class GoodsDonation
    {
        [Key]
        [Required]
        public int GOODS_DONATION_ID { get; set; }

        public string USERNAME { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Donation Date")]
        public DateTime? DATE { get; set; }

        [Required]
        [Display(Name = "Number of items")]
        public int ITEM_COUNT { get; set; }

        [Required]
        [Display(Name = "Category")]
        public String? CATEGORY { get; set; }

        [Required]
        [Display(Name = "Description")]
        public String? DESCRIPTION { get; set; }

        [Display(Name = "Donor")]
        public String? DONOR { get; set; }
    }
}
