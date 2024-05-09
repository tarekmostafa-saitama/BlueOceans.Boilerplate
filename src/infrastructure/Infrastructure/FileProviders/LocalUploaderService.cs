using Application.Common.FileProviders;
using Application.Common.Helpers;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using ImageMagick;

namespace Infrastructure.FileProviders;

public class LocalUploaderService: IFileUploaderService
{
    private readonly ILogger<LocalUploaderService> _logger;

    public LocalUploaderService(ILogger<LocalUploaderService> logger)
    {
        _logger = logger;
    }
    public async Task<string> UploadAsync(Stream data, string fileName, string folderName = "uploads", bool compress = false)
    {
        try
        {
            var name = Utilities.GenerateRandomString(10)  + "." + folderName.Split('.').Last();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName, name);

            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName)))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName));
            }
            await using var bits = new FileStream(path, FileMode.Create);
            await data.CopyToAsync(bits);
            bits.Close();
            if (compress && IsImage(data,fileName))
            {
                var imageFile = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",folderName, name));
                var optimizer = new ImageOptimizer { OptimalCompression = true };
                optimizer.Compress(imageFile);
                imageFile.Refresh();
            }
            return name;
        }
        catch(Exception ex) 
        {
            _logger.LogError("Error while trying to upload file", ex);
            return null;
        }
    }

    public void ClearDirectory(string folderName = "uploads")
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);

        if (!Directory.Exists(path))
        {
            _logger.LogWarning("folder: {folderName} is not existing to be deleted",folderName);
            return;
        }
        var di = new DirectoryInfo(path);
        foreach (var file in di.EnumerateFiles())
        {
            file.Delete();
        }

    }



    private const int ImageMinimumBytes = 512;
    private static bool IsImageExtension(string extension)
    {
        var allowedExtensions = new List<string>
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".gif",
            ".bmp",
            ".tiff",
            ".webp"
        };

        return allowedExtensions.Contains(extension);
    }
    private static string GetMimeType(Stream stream)
    {
        // Check if the stream is null or doesn't support reading.
        if (stream == null || !stream.CanRead)
        {
            return string.Empty;
        }

        //-------------------------------------------
        //  Use a simple header checking method to get the MIME type
        //-------------------------------------------
        byte[] buffer = new byte[256];
        if (stream.Read(buffer, 0, 256) <= 0)
        {
            return string.Empty;
        }

        var mimeType = GetMimeTypeFromBytes(buffer);
        return mimeType;
    }

    private static string GetMimeTypeFromBytes(byte[] buffer)
    {
        var knownMimeTypes = new Dictionary<string, string>
        {
            { "ffd8ffe000104a464946", "image/jpeg" },
            { "89504e470d0a1a0a0000", "image/png" },
            { "47494638396126026f01", "image/gif" },
            { "424d" , "image/bmp" },
            { "49492a00" , "image/tiff" },
            { "52494646", "image/webp" }
        };

        var bytesAsString = BitConverter.ToString(buffer.Take(6).ToArray()).Replace("-", "").ToLower();

        if (knownMimeTypes.ContainsKey(bytesAsString))
        {
            return knownMimeTypes[bytesAsString];
        }

        return string.Empty;
    }

    private bool IsImage(Stream stream, string fileName)
    {
        //-------------------------------------------
        //  Check the image mime types
        //-------------------------------------------
        var allowedMimeTypes = new List<string>
        {
            "image/jpeg",
            "image/png",
            "image/gif",
            "image/bmp",
            "image/tiff",
            "image/webp"
        };

        var mimeType = GetMimeType(stream);

        if (!allowedMimeTypes.Contains(mimeType) || !IsImageExtension(Path.GetExtension(fileName)?.ToLower()))
        {
            return false;
        }

        //-------------------------------------------
        //  Attempt to read the file and check the first bytes
        //-------------------------------------------
        try
        {
            if (!stream.CanRead)
            {
                return false;
            }

            //------------------------------------------
            // Check whether the image size exceeds the limit or not
            //------------------------------------------
            if (stream.Length < ImageMinimumBytes)
            {
                return false;
            }

            //------------------------------------------
            // Check for any malicious content
            //------------------------------------------
            byte[] buffer = new byte[ImageMinimumBytes];
            stream.Read(buffer, 0, ImageMinimumBytes);
            string content = System.Text.Encoding.UTF8.GetString(buffer);
            if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
            {
                return false;
            }
        }
        catch (Exception)
        {
            return false;
        }
        finally
        {
            stream.Position = 0;
        }

        return true;
    }


}