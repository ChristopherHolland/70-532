﻿using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnection"));

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference("images");

            var blobs = new List<BlobImage>();

            foreach(var blob in container.ListBlobs())
            {
                if(blob.GetType() == typeof(CloudBlockBlob))
                {
                    var sas = container.GetSharedAccessSignature(null, "MySAP");
                    blobs.Add(new BlobImage { BlobUri = blob.Uri.ToString() + sas });
                    blobs.Add(new BlobImage { BlobStorageUri = blob.StorageUri.ToString() + sas });
                }
                
            }

            return View(blobs);
        }

        static string GetSASToken(CloudStorageAccount storageAccount)
        {
            SharedAccessAccountPolicy policy = new SharedAccessAccountPolicy()
            {
                Permissions = SharedAccessAccountPermissions.Read | SharedAccessAccountPermissions.Write,
                Services = SharedAccessAccountServices.Blob,
                ResourceTypes = SharedAccessAccountResourceTypes.Object,
                SharedAccessExpiryTime = DateTime.Now.AddMinutes(30),
                Protocols = SharedAccessProtocol.HttpsOnly
            };

            return storageAccount.GetSharedAccessSignature(policy);
        }
    }
}