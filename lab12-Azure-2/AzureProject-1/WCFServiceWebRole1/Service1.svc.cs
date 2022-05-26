using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;

namespace WCFServiceWebRole1 {
	public class Service1 : IService1 {
		public void Koduj(string name, string text) {
			GetBlob("raw", name).UploadTextAsync(text);
			Queue("koduj").AddMessage(new CloudQueueMessage(name));
			// push queue encode
		}

		public string Pobierz(string name) {
			return GetBlob("encoded", name)?.DownloadText();
		}


		private CloudQueue Queue(string name) {
			CloudQueue queue = CloudStorageAccount
				.DevelopmentStorageAccount
				.CreateCloudQueueClient()
				.GetQueueReference(name);
			queue.CreateIfNotExists();
			return queue;
		}

		private CloudBlockBlob GetBlob(string containerName, string name) {
			CloudBlobContainer container = CloudStorageAccount
                .DevelopmentStorageAccount
                .CreateCloudBlobClient()
                .GetContainerReference(containerName);
			container.CreateIfNotExists();
			return container.GetBlockBlobReference(name);
		}

	}
}
