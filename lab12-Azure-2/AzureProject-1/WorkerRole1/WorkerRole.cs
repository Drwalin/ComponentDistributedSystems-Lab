using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;

namespace WorkerRole1 {
	public class WorkerRole : RoleEntryPoint {
		private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
		private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

		public override void Run() {
			Trace.TraceInformation("WorkerRole1 is running");

			try {
				this.RunAsync(this.cancellationTokenSource.Token).Wait();
			} finally {
				this.runCompleteEvent.Set();
			}
		}

		public override bool OnStart() {
			// Set the maximum number of concurrent connections
			ServicePointManager.DefaultConnectionLimit = 12;

			// For information on handling configuration changes
			// see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

			bool result = base.OnStart();

			Trace.TraceInformation("WorkerRole1 has been started");

			return result;
		}

		public override void OnStop() {
			Trace.TraceInformation("WorkerRole1 is stopping");

			this.cancellationTokenSource.Cancel();
			this.runCompleteEvent.WaitOne();

			base.OnStop();

			Trace.TraceInformation("WorkerRole1 has stopped");
		}

		private async Task RunAsync(CancellationToken cancellationToken) {
			// TODO: Replace the following with your own logic.
			while(!cancellationToken.IsCancellationRequested) {

				var msg = await Queue("koduj").GetMessageAsync();
				if(msg == null) {
					System.Threading.Thread.Sleep(1);
					continue;
				}
				string name = msg.AsString;

				string text = GetBlob("raw", name).DownloadText();
				string encoded = Rot13(text);
				GetBlob("encoded", name).UploadText(encoded);

				// przetwarzanie blobow



				/*
				Trace.TraceInformation("Working");
				await Task.Delay(1000);
				*/
			}
		}

		private string Rot13(string text) {
			string ret = "";
			for(int i=0; i<text.Length; ++i) {
				char cc = text[i];
				int c = cc;
				int b = -1;
				if(c >= 'a' && c <= 'z')
					b = 'a';
				if(c >= 'A' && c <= 'Z')
					b = 'A';
				if(b != -1) {
					c -= b;
					c += 13;
					c %= 26;
					c += b;
				}
				ret += ((char)c);
			}
			return ret;
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
