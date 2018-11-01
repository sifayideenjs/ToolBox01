using ToolBox.Model;
using ToolBox.Models;
using ToolBox.Utilities;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompMatrixConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var accountName = ConfigurationManager.AppSettings["storage:account:name"];
            var accountKey = ConfigurationManager.AppSettings["storage:account:key"];
            var Container = ConfigurationManager.AppSettings["storage:account:container:name"];

            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(Container);

            //var blobList = ListdownBlobs(container);
            //DownloadStreamBlobs(container, blobList, @"C:\Users\h149041\Documents\CompMatrix\Blob");

            //var result = ReadStreamBlobs(container, "Config.xml");
            //if(result != null)
            //{

            //}

            WriteToBlobs(container, "Config.xml", @"C:\Users\Sifayideen\Source\Repos\CompMatrix\Config.xml");
        }

        private static List<string> ListdownBlobs(CloudBlobContainer container)
        {
            List<string> blobList = new List<string>();
            if (container.Exists())
            {
                // Loop over items within the container and output the length and URI.
                foreach (IListBlobItem item in container.ListBlobs(null, false))
                {
                    if (item.GetType() == typeof(CloudBlockBlob))
                    {
                        CloudBlockBlob blob = (CloudBlockBlob)item;
                        blobList.Add(blob.Uri.ToString());
                        Console.WriteLine("Block blob of length {0}: {1}", blob.Properties.Length, blob.Uri);
                    }
                    //else if (item.GetType() == typeof(CloudPageBlob))
                    //{
                    //    CloudPageBlob pageBlob = (CloudPageBlob)item;

                    //    Console.WriteLine("Page blob of length {0}: {1}", pageBlob.Properties.Length, pageBlob.Uri);
                    //}
                    //else if (item.GetType() == typeof(CloudBlobDirectory))
                    //{
                    //    CloudBlobDirectory directory = (CloudBlobDirectory)item;

                    //    Console.WriteLine("Directory: {0}", directory.Uri);
                    //}
                }
            }
            else
            {
                Console.WriteLine("Container does not exist!");
            }

            return blobList;
        }

        private static void DownloadBlobs(CloudBlobContainer container, List<string> blobList, string directory)
        {
            foreach(var blobName in blobList)
            {
                DownloadBlobs(container, blobName, directory);
            }
        }

        private static void DownloadBlobs(CloudBlobContainer container, string blobName, string directory)
        {
            // Retrieve reference to a blob name.
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);

            // Save blob contents to a file.
            string filePath = Path.Combine(directory, Path.GetFileName(blobName));
            using (var fileStream = System.IO.File.OpenWrite(filePath))
            {
                blockBlob.DownloadToStream(fileStream);
            }
        }

        private static void DownloadStreamBlobs(CloudBlobContainer container, List<string> blobList, string directory)
        {
            foreach (var blobName in blobList)
            {
                //var result = ReadStreamBlobs(container, blobName, directory);
            }
        }

        //private static string ReadStreamBlobs(CloudBlobContainer container, string blobName)
        //{
        //    string result = string.Empty;

        //    // Retrieve reference to a blob name.
        //    var maxRetryCount = 3;
        //    CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
        //    if(blockBlob.Exists())
        //    {
        //        var blobRequestOptions = new BlobRequestOptions
        //        {
        //            ServerTimeout = TimeSpan.FromSeconds(30),
        //            MaximumExecutionTime = TimeSpan.FromSeconds(120),
        //            RetryPolicy = new ExponentialRetry(TimeSpan.FromSeconds(3), maxRetryCount),
        //        };

        //        using (var memoryStream = new MemoryStream())
        //        {
        //            blockBlob.DownloadToStream(memoryStream, null, blobRequestOptions);
        //            result = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
        //        }
        //    }

        //    return result;
        //}

        private static Hub ReadStreamBlobs(CloudBlobContainer container, string blobName)
        {
            //List<CompMatrixModel> result = null;
            Hub result = null;

            // Retrieve reference to a blob name.
            var maxRetryCount = 3;
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
            if (blockBlob.Exists())
            {
                var blobRequestOptions = new BlobRequestOptions
                {
                    ServerTimeout = TimeSpan.FromSeconds(30),
                    MaximumExecutionTime = TimeSpan.FromSeconds(120),
                    RetryPolicy = new ExponentialRetry(TimeSpan.FromSeconds(3), maxRetryCount),
                };

                using (var memoryStream = new MemoryStream())
                {
                    //blockBlob.DownloadToStream(memoryStream, null, blobRequestOptions);
                    string text = blockBlob.DownloadText();
                    //CompMatrixDocumentParser parser = new CompMatrixDocumentParser();
                    //result = parser.Parse(memoryStream);

                    System.Xml.Serialization.XmlSerializer deserializer = new System.Xml.Serialization.XmlSerializer(typeof(Hub));
                    //TextReader reader = new StreamReader(memoryStream);
                    object obj = deserializer.Deserialize(new StringReader(text));
                    result = obj as Hub;
                }
            }

            return result;
        }

        private static void WriteToBlobs(CloudBlobContainer container, string blobName, string filePath)
        {
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
            if (blockBlob.Exists())
            {
                blockBlob.UploadFromFile(filePath);
            }
        }

        public void PopulateConfigDataSet()
        {
            //Tile tile01 = new Tile() { Name = "Comp Matrix 2015", View = "SearchView", Type = TileType.Excel, Url = "CompMatrix2015" };
            //Tile tile02 = new Tile() { Name = "Comp Matrix 2016", View = "SearchView", Type = TileType.Excel, Url = "CompMatrix2016" };
            //Tile tile03 = new Tile() { Name = "Comp Matrix 2017", View = "SearchView", Type = TileType.Excel };

            //HubSection hubSection01 = new HubSection();
            //hubSection01.Name = "Comp Matrix";
            //hubSection01.Tiles.AddRange(new List<Tile> { tile01, tile02, tile03 });

            //Tile tile04 = new Tile() { Name = "Tool 01", View = "Tool01View", Type = TileType.Tool, ShortDescription = "Calculate some stuff" };
            //Tile tile05 = new Tile() { Name = "Tool 02", View = "Tool02View", Type = TileType.Tool, ShortDescription = "Calculate some stuff" };
            //Tile tile06 = new Tile() { Name = "Tool 03", View = "Tool03View", Type = TileType.Tool, ShortDescription = "Calculate some stuff" };
            //Tile tile07 = new Tile() { Name = "Notepad", Type = TileType.Custom, ShortDescription = "Take some notes", Url = "notepad.exe" };
            //Tile tile08 = new Tile() { Name = "MS Paint", Type = TileType.Custom, ShortDescription = "Capture screen shots", Url = "mspaint.exe" };
            //Tile tile09 = new Tile() { Name = "Calc", Type = TileType.Custom, ShortDescription = "Perform some calculations", Url = "calc.exe" };

            //HubSection hubSection02 = new HubSection();
            //hubSection02.Name = "Tools";
            //hubSection02.Tiles.AddRange(new List<Tile> { tile04, tile05, tile06, tile07, tile08, tile09 });

            //Tile tile10 = new Tile() { Name = "Microsoft", Type = TileType.Web, ShortDescription = "www.bing.com", Url = "www.bing.com" };
            //Tile tile11 = new Tile() { Name = "Google", Type = TileType.Web, ShortDescription = "www.google.com", Url = "www.google.com" };
            //Tile tile12 = new Tile() { Name = "Twitter", Type = TileType.Web, ShortDescription = "www.twitter.com", Url = "www.twitter.com" };

            //HubSection hubSection03 = new HubSection();
            //hubSection03.Name = "Bookmarks";
            //hubSection03.Tiles.AddRange(new List<Tile> { tile10, tile11, tile12 });

            //Tile tile13 = new Tile() { Name = "Microsoft", Type = TileType.News, Description = "Microsoft is in the news today" };
            //Tile tile14 = new Tile() { Name = "Comp Matrix", Type = TileType.News, Description = "Search Comp Matrix for some usefull stuff" };
            //Tile tile15 = new Tile() { Name = "Chennai", Type = TileType.News, Description = "Chennai is again in the picture" };

            //HubSection hubSection04 = new HubSection();
            //hubSection04.Name = "News & Updates";
            //hubSection04.Tiles.AddRange(new List<Tile> { tile13, tile14, tile15 });

            //Hub hub = new Hub();
            //hub.HubSections.AddRange(new List<HubSection> { hubSection01, hubSection02, hubSection03, hubSection04 });

            //Configs configs = new Configs() { Hub = hub };

            //this.Hub = hub;

            //string xmlFile = @"C:\Users\Sifayideen\Source\Repos\CompMatrix\Config.xml";
            //System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Configs));
            //using (TextWriter writer = new StreamWriter(xmlFile))
            //{
            //    serializer.Serialize(writer, configs);
            //}
        }
    }
}
