using Akalaat.Helper;

namespace Akalaat.ViewModels
{
    public class viewBranchVM
    {
        public int Id { get; set; }
        public int ResturantId { get; set; }
        public string AddressDetails { get; set; }
        public int OpenHour { get; set; }
        public int CloseHour { get; set; }
        public bool LocationProvided { get; set; }
        public string RegionName { get; set; }
        public  DeliverToYou DeliveryState {  get; set; }
        public List<string> DeliveringAreas { get; set; } = new List<string>();
}
}

