using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WCFServiceWebRole1 {
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
	[ServiceContract]
	public interface IService1 {

		[OperationContract]
		void Create(LoginData login);

		[OperationContract]
		string Login(LoginData login);

		[OperationContract]
		void Logout(string sessionId);
		

		[OperationContract]
		void Put(PutFileData data);

		[OperationContract]
		string Get(GetFileData data);
	}

	[DataContract]
	public class GetFileData {
		string fileName;
		string sessionId;

		[DataMember]
		public string FileName {
			get { return fileName; }
			set { fileName = value; }
		}
		

		[DataMember]
		public string SessionId {
			get { return sessionId; }
			set { sessionId = value; }
		}
	}

	[DataContract]
	public class PutFileData {
		string fileName;
		string sessionId;
		string content;

		[DataMember]
		public string FileName {
			get { return fileName; }
			set { fileName = value; }
		}
		

		[DataMember]
		public string SessionId {
			get { return sessionId; }
			set { sessionId = value; }
		}
		
		[DataMember]
		public string Content {
			get { return content; }
			set { content = value; }
		}
	}

	[DataContract]
	public class LoginData {
		string login;
		string password;

		[DataMember]
		public string Login {
			get { return login; }
			set { login = value; }
		}
		

		[DataMember]
		public string Password {
			get { return password; }
			set { password = value; }
		}
	}
}
