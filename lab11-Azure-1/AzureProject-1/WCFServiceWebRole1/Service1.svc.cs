using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace WCFServiceWebRole1 {
	public class Service1 : IService1 {
		public void Create(LoginData login) {
			var account = CloudStorageAccount.DevelopmentStorageAccount;
			CloudTableClient cl = account.CreateCloudTableClient();
			var users = cl.GetTableReference("UserTable");
			users.CreateIfNotExists();
			users.Execute(
					TableOperation.Insert(
						new UserTable(login.Login, "") {
						UserName = login.Login,
						Password = login.Password
						}
						)
					);
		}

		public string Login(LoginData login) {
			var account = CloudStorageAccount.DevelopmentStorageAccount;
			CloudTableClient cl = account.CreateCloudTableClient();
			var users = cl.GetTableReference("UserTable");
			
			var res = users.Execute(
					TableOperation.Retrieve<UserTable>(login.Login, ""));
			var e = (UserTable)res.Result;
			if(e == null)
				return null;
			
			if(e.Password != login.Password)
				return null;
			
			var sessions = cl.GetTableReference("SessionTable");
			Guid id = Guid.NewGuid();
			var session = new SessionTable(id.ToString(), "") {
						UserName = login.Login,
						SessionId = id
						};
			users.Execute(
					TableOperation.Insert(session)
					);
			return id.ToString();
		}

		public void Logout(string sessionId) { 
			var account = CloudStorageAccount.DevelopmentStorageAccount;
			CloudTableClient cl = account.CreateCloudTableClient();
			var sessions = cl.GetTableReference("SessionTable");
			var res = sessions.Execute(
					TableOperation.Retrieve<SessionTable>(sessionId, ""));
			var e = (SessionTable)res.Result;
			if(e == null)
				return;
			
			sessions.Execute(
					TableOperation.Delete(e));
		}
		

		public void Put(PutFileData data) {
			var account = CloudStorageAccount.DevelopmentStorageAccount;
			CloudTableClient cl = account.CreateCloudTableClient();
			var sessions = cl.GetTableReference("SessionTable");
			var res = sessions.Execute(
					TableOperation.Retrieve<SessionTable>(data.SessionId, ""));
			var e = (SessionTable)res.Result;
			if(e == null)
				return;

			var f = new FilesTable(e.UserName, data.FileName) { 
				FileName = data.FileName,
				UserName = e.UserName,
				Content = data.Content
			};
			sessions.Execute(
					TableOperation.Insert(f)
					);
		}

		public string Get(GetFileData data) {
			var account = CloudStorageAccount.DevelopmentStorageAccount;
			CloudTableClient cl = account.CreateCloudTableClient();
			var sessions = cl.GetTableReference("SessionTable");
			var res = sessions.Execute(
					TableOperation.Retrieve<SessionTable>(data.SessionId, ""));
			var e = (SessionTable)res.Result;
			if(e == null)
				return null;


			var resf = sessions.Execute(
					TableOperation.Retrieve<FilesTable>(e.UserName, data.FileName));
			var f = (FilesTable)res.Result;
			if(f == null)
				return null;
			return f.Content;
		}
	}
}
