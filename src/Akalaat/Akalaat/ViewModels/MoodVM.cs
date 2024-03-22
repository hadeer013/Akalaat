using Microsoft.AspNetCore.Mvc.Rendering;

namespace Akalaat.ViewModels
{
    public class MoodVM
    {
        public string Name { get; set; }
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public SelectList MoodList { get; set; }
        public int MoodId { get; set; }
        public IEnumerable<SelectListItem> RestaurantsList { get; set; }

        
        public List<SelectListItem> RestaurantsLists { get; set; } // Existing property
        public List<SelectListItem> MoodsList { get; set; }
    }
}
