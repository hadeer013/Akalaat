namespace Akalaat.ViewModels
{
    public class AddBranchVM
    {
        public int ResturantId {  get; set; }
        public int OpenHour {  get; set; }
        public int CloseHour {  get; set; }
        public string AddressDetails {  get; set; }
        public int RegionId { get; set; }
        public bool IsDelivery {  get; set; }
        public bool IsDineIn {  get; set; }
        public string Latitude {  get; set; }
        public string Longitude {  get; set; }
    }
}
