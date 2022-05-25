using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace WCFServiceWebRole1 {
	public class Users : TableEntity {
		
		public Users(string pk, string rk) {
			this.PartitionKey = pk;
			this.RowKey = rk;
		}
		public Users() {}
		public string UserName { get; set; }
		public string Password { get; set; }
	}
	
	public class Sessions : TableEntity {
		public Sessions(string pk, string rk) {
			this.PartitionKey = pk;
			this.RowKey = rk;
		}
		public Sessions() {}
		public string UserName { get; set; }
		public Guid SessionId { get; set; }
	}
}
