using Akalaat.DAL.Models;

namespace Akalaat.ViewModels;

public class CategoryVM
{
    public IEnumerable<Category>? Categories { get; set; }
    public int Id { get; set; }

    public int Menu_ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string? Category_image { get; set; }
    public IFormFile CategoryImageFile { get; set; }
}