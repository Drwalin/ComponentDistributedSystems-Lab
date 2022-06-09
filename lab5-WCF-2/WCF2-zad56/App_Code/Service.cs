using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in code, svc and config file together.
	public class Service : KSR_WCF2.IZadanie5, KSR_WCF2.IZadanie6 {
		public string ScalNapisy(string a, string b) {
			return a + b;
		}

		public void Dodaj(int a, int b) {
			OperationContext.Current.GetCallbackChannel<KSR_WCF2.IZadanie6Zwrotny>().Wynik(a + b);
		}
	}
