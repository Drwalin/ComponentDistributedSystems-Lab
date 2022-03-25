using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;


namespace server {
	[ServiceContract]
	public interface IZadanie2 {
		[OperationContract]
		string Test(string arg);
	}

	public class Zadanie2 : IZadanie2 {
		public string Test(string arg) {
			return "To jest zmodyfikowany string, przed: " + arg + " <<---;";
		}
	}

	[ServiceContract]
	public interface IZadanie7 {
		[OperationContract][FaultContract(typeof(Wyjatek7))]
		void RzucWyjatek7(string a, int b);
	}

	[DataContract]
	public class Wyjatek7 {
		public Wyjatek7(string o, string a, int b) {
			Opis = o;
			A = a;
			B = b;
		}
		[DataMember] string Opis { get; set; } 
		[DataMember] string A    { get; set; }
		[DataMember] int B       { get; set; } 
	}

	public class Zadanie7 : IZadanie7 {
		public void RzucWyjatek7(string a, int b) {
			throw new FaultException<Wyjatek7>(new Wyjatek7(null, a, b),
				new FaultReason("Wyjatek cos: " + a + "," + b));
		}
	}

	class Program {
		static void Main(string[] args) {
			var host = new ServiceHost(typeof(Zadanie2));
			{
				host.AddServiceEndpoint(typeof(IZadanie2),
					new NetNamedPipeBinding(),
					"net.pipe://localhost/ksr-wcf1-test");

				var beh = host.Description.Behaviors.Find<ServiceMetadataBehavior>();
				if(beh == null)
					beh = new ServiceMetadataBehavior();
				host.Description.Behaviors.Add(beh);
				host.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName,
					MetadataExchangeBindings.CreateMexNamedPipeBinding(),
					"net.pipe://localhost/metadane");

				host.AddServiceEndpoint(typeof(IZadanie2),
					new NetTcpBinding(),
					"net.tcp://127.0.0.1:55421");

				host.Open();
			}


			var host7 = new ServiceHost(typeof(Zadanie7));
			{
				host7.AddServiceEndpoint(typeof(IZadanie7),
					new NetNamedPipeBinding(),
					"net.pipe://localhost/ksr-wcf1-test7");

				var beh7 = host7.Description.Behaviors.Find<ServiceMetadataBehavior>();
				if(beh7 == null)
					beh7 = new ServiceMetadataBehavior();
				host7.Description.Behaviors.Add(beh7);
				host7.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName,
					MetadataExchangeBindings.CreateMexNamedPipeBinding(),
					"net.pipe://localhost/metadane7");
				host7.Open();
			}


			Console.ReadKey();
			host.Close();
			host7.Close();
			Console.ReadLine();
		}
	}
}
