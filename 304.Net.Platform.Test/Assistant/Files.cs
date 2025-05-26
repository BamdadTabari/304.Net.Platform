using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace _304.Net.Platform.Test.Assistant;
public static class Files
{
    // this will make a fake file for us in tests
    public static IFormFile CreateFakeFormFile(string fileName = "test.jpg", string contentType = "image/jpeg", string content = "fake image content")
    {
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        return new FormFile(stream, 0, stream.Length, "image_file", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        };
    }
}
