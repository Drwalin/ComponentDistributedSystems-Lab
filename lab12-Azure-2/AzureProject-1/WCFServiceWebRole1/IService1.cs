﻿using System;
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
		void Koduj(string name, string text);

		[OperationContract]
		string Pobierz(string name);
	}
}
