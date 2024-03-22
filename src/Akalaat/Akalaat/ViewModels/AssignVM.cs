using Akalaat.DAL.Models;

namespace Akalaat.ViewModels
{
    public class AssignVM
    {
        public IEnumerable<Mood> Moods { get; set; }
        public IEnumerable<Resturant> Restaurants { get; set; }
    }
}
