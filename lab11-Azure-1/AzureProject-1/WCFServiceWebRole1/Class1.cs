using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace WCFServiceWebRole1 {
	public class UserTable : TableEntity {
		
		public UserTable(string rk, string pk) {
			this.PartitionKey = pk; // ustawiamy klucz partycji
			this.RowKey = rk; // ustawiamy klucz główny
		}
		public UserTable() {}
		public string UserName { get; set; }
		public string Password { get; set; }
	}
	
	public class SessionTable : TableEntity {
		public SessionTable(string rk, string pk) {
			this.PartitionKey = pk; // ustawiamy klucz partycji
			this.RowKey = rk; // ustawiamy klucz główny
		}
		public SessionTable() {}
		public string UserName { get; set; }
		public Guid SessionId { get; set; }
	}
	
	public class FilesTable : TableEntity {
		public FilesTable(string rk, string pk) {
			this.PartitionKey = pk; // ustawiamy klucz partycji
			this.RowKey = rk; // ustawiamy klucz główny
		}
		public FilesTable() {}
		public string UserName { get; set; }
		public string FileName { get; set; }
		public string Content { get; set; }
	}
}
