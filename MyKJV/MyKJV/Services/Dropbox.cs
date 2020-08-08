using Acr.UserDialogs;

using Dropbox.Api;
using Dropbox.Api.Files;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyKJV.Services
{
    public class Dropbox
    {
        DropboxClient client;
        public Dropbox(string apikey = API_KEY)
        {
            client = GetClient().Result;
        }
        public const string API_KEY = "corum5ks4e4sv4r";
        public const string API_ACCESS_TOKEN = "azyowstd60AAAAAAAAAAAT2V7o4fUrBj8Md_C9AVoJcYVfZfy0TlDl5Ys5CcanET";
        public async Task<DropboxClient> GetClient(string apikey = API_KEY)
        {
            var dbx = new DropboxClient(apikey);
            var full = await dbx.Users.GetCurrentAccountAsync();
            Console.WriteLine("{0} - {1}", full.Name.DisplayName, full.Email);
            return dbx;
        }
        public async Task Upload(DropboxClient dbx, string folder, string file, byte[] content)
        {
            await Task.Delay(1);
            //using (var mem = new System.IO.FileStream(System.IO.MemoryStream(content))
            //{
            //    var updated = await dbx.Files.UploadAsync(
            //        folder + "/" + file,
            //        WriteMode.Overwrite.Instance,
            //        body: mem);
            // Console.WriteLine("Saved {0}/{1} rev {2}", folder, file, updated.Rev);
        }
        public async Task SendDbToDropbox()
        {

        }
    }
}
