using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WCFServiceWebRole1 {
	[ServiceContract]
	public interface IService1 {
		
		[OperationContract]
		bool Create(string login, string password);

		[OperationContract]
		string Login(string login, string password);

		[OperationContract]
		void Logout(string sessionId);
		

		[OperationContract]
		bool Put(string fileName, string sessionId, string content);

		[OperationContract]
		string Get(string fileName, string sessionId);
	}
}
