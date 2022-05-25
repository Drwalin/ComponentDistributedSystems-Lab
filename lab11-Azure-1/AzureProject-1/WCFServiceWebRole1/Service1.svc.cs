using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;

namespace WCFServiceWebRole1 {
	public class Service1 : IService1 {
		public bool Create(string login, string password) {
			var users = Table("users");

            var a = TableOperation.Retrieve<Users>(login, password);
            var res = users.Execute(a);
			if(res.Result != null)
				return false;

			users.Execute(
				TableOperation.Insert(
					new Users(login, password) {
						UserName = login,
						Password = password
					})
				);
			return true;
		}

		public string Login(string login, string password) {
			var users = Table("users");

			var res = users.Execute(
					TableOperation.Retrieve<Users>(login, password));
			var e = res.Result as Users;
			if(e == null)
				return null;

			if(e.Password != password)
				return null;

			var sessions = Table("sessions");
			Guid id = Guid.NewGuid();
			var session = new Sessions(id.ToString(), "A") {
				UserName = login,
				SessionId = id
			};
			sessions.Execute(
					TableOperation.Insert(session)
					);
			return id.ToString();
		}

		public void Logout(string sessionId) {
			var sessions = Table("sessions");
			var res = sessions.Execute(
					TableOperation.Retrieve<Sessions>(sessionId, "A"));
			var e = res.Result as Sessions;
			if(e == null)
				return;

			sessions.Execute(
					TableOperation.Delete(e));
		}


		public bool Put(string fileName, string sessionId, string content) {
			var sessions = Table("sessions");
			var res = sessions.Execute(
					TableOperation.Retrieve<Sessions>(sessionId, "A"));
			var e = res.Result as Sessions;
			if(e == null)
				return false;

			if(fileName.Contains("@"))
				return false;

			GetBlob("file", fileName + "@" + e.UserName).UploadTextAsync(content);
			return true;
		}

		public string Get(string fileName, string sessionId) {
			var sessions = Table("sessions");
			var res = sessions.Execute(
					TableOperation.Retrieve<Sessions>(sessionId, "A"));
			var e = res.Result as Sessions;
			if(e == null)
				return null;

			if(fileName.Contains("@"))
				return null;

			return GetBlob("file", fileName + "@" + e.UserName)?.DownloadText();
		}



		private CloudTable Table(string tableName) {
			var table = CloudStorageAccount
                .DevelopmentStorageAccount
                .CreateCloudTableClient()
                .GetTableReference(tableName);
			table.CreateIfNotExists();
			return table;
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
