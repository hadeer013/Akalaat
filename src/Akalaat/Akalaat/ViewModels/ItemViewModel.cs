using Akalaat.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace Akalaat.ViewModels
{
    public class ItemViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The Name field is required.")]
        public string Name { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Likes must be a non-negative integer.")]
        public int Likes { get; set; } = 0;
        [StringLength(1000, ErrorMessage = "The Description field must not exceed 1000 characters.")]
        public string Description { get; set; }

        public string? ImageUrl { get; set; }
        public IFormFile Image { get; set; }

        public decimal? Discount { get; set; }
        [EnumDataType(typeof(OfferStatus), ErrorMessage = "Invalid value for IsOffer.")]
        public OfferStatus IsOffer { get; set; }
        
        public int MenuID { get; init; }
        public int CategoryID { get; set; }
  
        [Range(0, int.MaxValue, ErrorMessage = "Price must be a positive integer.")]
        public int? Price { get; set; }
    }
}
