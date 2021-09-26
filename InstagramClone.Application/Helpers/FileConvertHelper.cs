using Microsoft.AspNetCore.Http;
using System.IO;

namespace InstagramClone.Application.Helpers
{
    public class FileConvertHelper
    {
        public static byte[] ConvertToBytes(IFormFile file)
        {
            byte[] data = null;

            if (file != null)
            {
                using (var binaryReader = new BinaryReader(file.OpenReadStream()))
                {
                    data = binaryReader.ReadBytes((int)file.Length);
                }
            }
            return data;
        }
    }
}