using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Akalaat.Utilities;

public class FileManagement
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FileManagement(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<string?> AddFileAsync(string RelativePath, string FileName, IFormFile File)
    {
        try
        {
            var DirectoryPath = Path.Combine(_webHostEnvironment.WebRootPath, RelativePath);
            var ImagePath = Path.Combine(DirectoryPath, FileName);
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);

            }

            using (var stream = new FileStream(ImagePath, FileMode.Create))
            {
                await File.CopyToAsync(stream);
            }
            
            return '/'+RelativePath+FileName;
        }
        catch (Exception e)
        {
            return null;
        }
    }
    public void DeleteImageFile(string imageUrl)
    {
        if (!string.IsNullOrEmpty(imageUrl))
        {
            //remove first '/' because it cause problems when i combine 
            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, imageUrl.Substring(1));

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }
    }



}