namespace Akalaat.ViewModels
{
    public class AddReviewVM
    {
        public int Rating { get; set; }

        public string Comment { get; set; }

        public IFormFile ReviewImage { get; set; }
        public int No_of_Likes { get; set; }

        public string Customer_ID { get; set; }
        public int? Resturant_ID { get; set; }

    }
}
