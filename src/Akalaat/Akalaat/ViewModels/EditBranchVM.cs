namespace Akalaat.ViewModels
{
    public class EditBranchVM
    {
        public int BranchId { get; set; }
        public int CityId {  get; set; }
        public int DistrictId {  get; set; }
        public int OpenHour { get; set; }
        public int CloseHour { get; set; }
        public string AddressDetails { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int RegionId { get; set; }
        public bool IsDelivery { get; set; }
        public bool IsDineIn { get; set; }
    }
}
