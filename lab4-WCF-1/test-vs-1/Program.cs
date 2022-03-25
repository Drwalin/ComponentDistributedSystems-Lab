using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using KSR_WCF1;
using System.ServiceModel;

namespace server {
}

namespace test_vs_1 {

	[DataContract]
	public class Wyjatek7 {
		[DataMember] string Opis { get; set; }
		[DataMember] string A { get; set; }
		[DataMember] int B { get; set; }
	}

	class Program {
		static void Main(string[] args) {
			{
				var client7 = new ServiceReference4.Zadanie7Client();
				try {
					client7.RzucWyjatek7("Tekst", 4);
				} catch {//(FaultException<Wyjatek7> e) {
					Console.WriteLine("wyjatek 7");
				}
				((IDisposable)client7).Dispose();
			}

			{
				//var fact = new ChannelFactory<IZadanie1>(new NetNamedPipeBinding(),
				//	new EndpointAddress("net.pipe://localhost/ksr-wcf1-test-165421"));
				var fact = new ChannelFactory<IZadanie2>(new NetNamedPipeBinding(),
					new EndpointAddress("net.pipe://localhost/ksr-wcf1-test"));
				var client = fact.CreateChannel();
				Console.WriteLine(client.Test("Tekst"));
				((IDisposable)client).Dispose();
				fact.Close();
			}

			{
				var client2 = new ServiceReference1.Zadanie2Client();
				Console.WriteLine(client2.Test("Tekst2"));
				((IDisposable)client2).Dispose();
			}

			{
				var fact = new ChannelFactory<IZadanie1>(new NetNamedPipeBinding(),
					new EndpointAddress("net.pipe://localhost/ksr-wcf1-test-165421"));
				var client = fact.CreateChannel();
				try {
					client.RzucWyjatek(true);
				} catch(FaultException<Wyjatek> e) {
					Console.WriteLine(client.OtoMagia(e.Detail.magia));
				}
				((IDisposable)client).Dispose();
				fact.Close();
			}

			Console.ReadKey();
		}
	}
}
