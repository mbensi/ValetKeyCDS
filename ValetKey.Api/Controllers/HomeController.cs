using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Http;

namespace ValetKey.Api.Controllers
{
    public class HomeController : ApiController
    {
        private readonly CloudStorageAccount account;
        private string blobContainer;

        public HomeController()
        {
            this.account = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["Storage"]);
            this.blobContainer = "valetkeysample";
        }
        
        [HttpGet]
        [Route("api/Home/{Id}/{Name}")]
        public async Task<IHttpActionResult> Index(string id, string name)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    throw new ArgumentNullException("Id", "Value cannot be null.  Correct usage: /Home/{blob id}/{blob name}");
                }
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentNullException("Name", "Value cannot be null.  Correct usage: /Home/{blob id}/{blob name}");
                }

                this.blobContainer = id;

                var blobSas = await this.GetSharedAccessReferenceForDownload(id, name);
                return Redirect(string.Format("{0}{1}", blobSas.BlobUri, blobSas.Credentials));
            }
            catch (Exception ex)
            {
                var message = "Error: " + ex.Message;
                Trace.TraceError(message);

                bool devMode = false;
                if (bool.TryParse(ConfigurationManager.AppSettings["DevMode"], out devMode) && devMode)
                {
                    return InternalServerError(ex);
                }
                return InternalServerError(new Exception(message));
            }
        }
        
        private async Task<StorageEntitySas> GetSharedAccessReferenceForDownload(string blobId, string blobName)
        {
            var blobClient = this.account.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(this.blobContainer);

            var blob = container.GetBlockBlobReference(blobName);

            if (!await blob.ExistsAsync())
            {
                throw new Exception("Blob does not exist");
            }

            var policy = new SharedAccessBlobPolicy
            {
                Permissions = SharedAccessBlobPermissions.Read,

                // Create a signature for 5 min earlier to leave room for clock skew
                SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-5),

                // Create the signature for as long as necessary -  we can 
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(5)
            };

            var sas = blob.GetSharedAccessSignature(policy);

            return new StorageEntitySas
            {
                BlobUri = blob.Uri,
                Credentials = sas
            };
        }

        public struct StorageEntitySas
        {
            public string Credentials;
            public Uri BlobUri;
        }
    }
}
