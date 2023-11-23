namespace ST10101067_APPR6312_POE_PART_2.Models
{
    public class IncomingDataModel
    {
        public IEnumerable<Disaster> Disasters { get; set; }
        public IEnumerable<GoodsDonation> GoodsDonations { get; set; }
        public IEnumerable<MoneyDonation> MoneyDonations { get; set; }

        public IEnumerable<Money> Moneys { get; set; }

        public IEnumerable<GoodsInventory> Inventory { get; set; }

        public IEnumerable<GoodsAllocation> GoodsAllocations { get; set; }

        public IEnumerable<MoneyAllocation> MoneyAllocations { get; set; }
    }
}
