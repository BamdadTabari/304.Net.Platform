using Azure.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Assistant.Helpers;
public static class FileHelper
{
    public static async Task<string> UploadImage(IFormFile file, string folderName= "images")
    {
        // Define the directory for uploads 
        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

        // Create directory if not Exist
        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }

        // Build file name
        var newFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var imagePath = Path.Combine(uploadPath, newFileName);

        // Save Image
        using (var stream = new FileStream(imagePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        return imagePath;
    }

    public static void DeleteImage(string imagePath)
    {
        if(File.Exists(imagePath))
            File.Delete(imagePath);
    }
}
