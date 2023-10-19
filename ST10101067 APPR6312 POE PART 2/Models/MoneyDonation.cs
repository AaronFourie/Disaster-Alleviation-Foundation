using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10101067_APPR6312_POE_PART_2.Models
{
    public class MoneyDonation
    {
        [Key]
        [Required]
        public int MONEY_DONATION_ID { get; set; }

        public string USERNAME { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime? DATE {get; set;}

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Amount")]
        public decimal AMOUNT { get; set;}

        [Display(Name = "Donor")]
        public string? DONOR { get; set;}
    }
}
