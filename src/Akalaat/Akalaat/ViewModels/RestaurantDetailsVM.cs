using Akalaat.DAL.Models;

namespace Akalaat.ViewModels
{
    public class RestaurantDetailsVM
    {
        public Resturant Restaurant { get; set; }
        public List<Mood> Moods { get; set; }
    }


}
