using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Diagnostics;

namespace MyBlog.Engine
{
    public static class FilesService
    {
        #region Declarations

        private const string ConnectionString = "AzureStorageConnectionString";
        private const string ContainerName = "posts";

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methodes

        /// <summary>
        /// Initilize
        /// </summary>
        public static void Initilize()
        {
            try
            {
                CloudBlobContainer container = GetBlogContainer();

                container.CreateIfNotExists(BlobContainerPublicAccessType.Container);
            }
            catch (Exception ex)
            {
                Trace.TraceError("FilesService.Initilize:" + ex.Message);
            }
        }

        /// <summary>
        ///  Get blob container
        /// </summary>
        /// <returns></returns>
        private static CloudBlobContainer GetBlogContainer()
        {
            String account = CloudConfigurationManager.GetSetting(ConnectionString);
            // Get the configuration
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(account);
            // Get a new client
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            // Get the container
            CloudBlobContainer container = blobClient.GetContainerReference(ContainerName);
            return container;
        }

        /// <summary>
        /// Upload a file
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Uri Upload(String name, Byte[] bytes)
        {
            // Get the blog by name
            CloudBlockBlob blob = GetBlogContainer().GetBlockBlobReference(name);
            // upload bytes
            blob.UploadFromByteArray(bytes, 0, bytes.Length);

            // Return the blog uri
            return blob.Uri;
        }

        public static Uri Delete(String name)
        {
            // Get the blog by name
            CloudBlockBlob blob = GetBlogContainer().GetBlockBlobReference(name);
            // upload bytes
            blob.DeleteIfExists(DeleteSnapshotsOption.IncludeSnapshots);

            // Return the blog uri
            return blob.Uri;
        }

        #endregion
    }
}
