using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10101067_APPR6312_POE_PART_2.Models
{
    public class GoodsPurchase
    {
        [Key]
        public int GoodsPurchaseID { get; set; }

        [Required]
        [Display(Name = "Item Price")]
        public decimal GoodsPurchasePrice { get; set; }

        [Required]
        [Display(Name = "Number of items")]
        [Range(0, double.MaxValue, ErrorMessage = "The field {0} must be greater than or equal to {1}.")]
        public int ITEM_COUNT { get; set; }

        public decimal GoodsTotalPrice { get; set; }

        [Display(Name = "Category")]
        public String? CATEGORY { get; set; }

        // Add any other properties you need for MoneyAllocation
        [NotMapped]
        public int GOODS_INVENTORY_ID { get; set; }
    }
}
