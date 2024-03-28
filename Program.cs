using BunnyCDN.Net.Storage;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BunnyExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await UploadFileAsync();
            //await GetUploadedFilesAsync();
            //await RemoveUploadedFilesAsync();
        }

        private static async Task UploadFileAsync()
        {
            const string FILE_PATH = @"C:\tester-developer.jpg";  // Full path to your local file
            const string REGION = "uk";  // If German region, set this to an empty string: ""
            const string BASE_HOSTNAME = "storage.bunnycdn.com";
            string HOSTNAME = string.IsNullOrEmpty(REGION) ? BASE_HOSTNAME : $"{REGION}.{BASE_HOSTNAME}";
            const string STORAGE_ZONE_NAME = "test-77diamonds";
            const string FILENAME_TO_UPLOAD = "tester-developer-6.jpg";
            const string ACCESS_KEY = "c49c85b4-9d96-433b-996983534c70-fa42-44d1";
            const string CONTENT_TYPE = "application/octet-stream";

            string url = $"https://{HOSTNAME}/{STORAGE_ZONE_NAME}/{FILENAME_TO_UPLOAD}";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "PUT";
            request.ContentType = CONTENT_TYPE;
            request.Headers.Add("AccessKey", ACCESS_KEY);

            using (Stream fileStream = File.OpenRead(FILE_PATH))
            using (Stream requestStream = await request.GetRequestStreamAsync())
            {
                await fileStream.CopyToAsync(requestStream);
            }

            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(responseStream))
            {
                string responseString = await reader.ReadToEndAsync();
                Console.WriteLine(responseString);
            }
        }

        private static async Task GetUploadedFilesAsync()
        {
            const string REGION = "uk";  // If German region, set this to an empty string: ""
            const string BASE_HOSTNAME = "storage.bunnycdn.com";
            string HOSTNAME = string.IsNullOrEmpty(REGION) ? BASE_HOSTNAME : $"{REGION}.{BASE_HOSTNAME}";
            const string STORAGE_ZONE_NAME = "test-77diamonds";
            const string FILENAME_TO_RETRIEVE = "dummy-image-2";
            const string ACCESS_KEY = "c49c85b4-9d96-433b-996983534c70-fa42-44d1";
            const string CONTENT_TYPE = "application/octet-stream";

            string url = $"https://{HOSTNAME}/{STORAGE_ZONE_NAME}/{FILENAME_TO_RETRIEVE}";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = CONTENT_TYPE;
            request.Headers.Add("AccessKey", ACCESS_KEY);

            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            using (Stream responseStream = response.GetResponseStream())
            {
                // Create a FileStream to save the image locally
                using (FileStream fileStream = File.Create("downloaded_image.jpg"))
                {
                    // Copy the response stream to the FileStream
                    await responseStream.CopyToAsync(fileStream);
                }
            }

        }

        private static async Task RemoveUploadedFilesAsync()
        {
            const string REGION = "uk";  // If German region, set this to an empty string: ""
            const string BASE_HOSTNAME = "storage.bunnycdn.com";
            string HOSTNAME = string.IsNullOrEmpty(REGION) ? BASE_HOSTNAME : $"{REGION}.{BASE_HOSTNAME}";
            const string STORAGE_ZONE_NAME = "test-77diamonds";
            const string FILENAME_TO_RETRIEVE = "dummy-image-1";
            const string ACCESS_KEY = "c49c85b4-9d96-433b-996983534c70-fa42-44d1";
            const string CONTENT_TYPE = "application/octet-stream";

            string url = $"https://{HOSTNAME}/{STORAGE_ZONE_NAME}/{FILENAME_TO_RETRIEVE}";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "DELETE";
            request.ContentType = CONTENT_TYPE;
            request.Headers.Add("AccessKey", ACCESS_KEY);

            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(responseStream))
            {
                string responseString = await reader.ReadToEndAsync();
                Console.WriteLine(responseString);
            }

        }

    }
}
