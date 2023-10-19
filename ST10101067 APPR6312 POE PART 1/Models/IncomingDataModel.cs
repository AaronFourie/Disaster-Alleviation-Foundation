namespace ST10101067_APPR6312_POE_PART_1.Models
{
    public class IncomingDataModel
    {
        public IEnumerable<Disaster> Disasters { get; set; }
        public IEnumerable<GoodsDonation> GoodsDonations { get; set; }
        public IEnumerable<MoneyDonation> MoneyDonations { get; set; }
    }
}
