using System.ComponentModel.DataAnnotations;

namespace ST10101067_APPR6312_POE_PART_2.Models
{
    public class GoodsInventory
    {
        [Key]
        public int GOODS_INVENTORY_ID { get; set; }

        public string CATGEORY { get; set; }

        public int? ITEM_COUNT {get; set; }

    }
}
