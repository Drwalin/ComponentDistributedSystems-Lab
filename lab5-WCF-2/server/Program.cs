using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;


namespace server {

	public class Zadanie2Enum : ServiceReference1.IZadanie2Callback {
		public void Zadanie([MessageParameter(Name = "zadanie")] string zadanie1, int pkt, bool zaliczone) {
			Console.WriteLine("Zadanie: " + zadanie1 + ", points: " + pkt + (zaliczone ? ", passed" : ", not passed"));
		}
	}

	public class Zadanie7Res : ServiceReference2.IZadanie6Callback {
		public void Wynik(int wyn) {
			Console.WriteLine("Wynik: " + wyn);
		}
	}

	public class Zadanie3 : KSR_WCF2.IZadanie3 {
		public void TestujZwrotny() {
			var ctx = OperationContext.Current.GetCallbackChannel<KSR_WCF2.IZadanie3Zwrotny>();
			for(int x = 0; x <= 30; x++) {
				ctx.WolanieZwrotne(x, x * x * x - x * x);
			}
		}
	}

	[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
	public class Zadanie4 : KSR_WCF2.IZadanie4 {
		int count = 0;
		public int Dodaj(int v) {
			return count += v;
		}

		public void Ustaw(int v) {
			count = v;
		}
	}





	class Program {
		static void Main(string[] args) {

			// task 7
			var zad7_5 = new ServiceReference2.Zadanie5Client();
			string a = "Jeden", b = "dwa";
			Console.WriteLine("Scalone " + a + " + " + b + " = " + zad7_5.ScalNapisy(a, b));
			var zad7_6 = new ServiceReference2.Zadanie6Client(new InstanceContext(new Zadanie7Res()));
			int A = 13, B = 11;
			Console.WriteLine("Suma: " + A + " + " + B + " = ");
			zad7_6.Dodaj(A, B);


			// task 1
			{
				var zad = new ServiceReference1.Zadanie1Client();
				var taskLong = zad.DlugieObliczeniaAsync();
				System.Threading.Thread.Sleep(50);
				for(int x = 0; x <= 20; ++x)
					zad.Szybciej(x, 3 * x * x - 2 * x);
				((IDisposable)zad).Dispose();
				Console.WriteLine("Done task 1");
			}
			
			// task 2
			var zad2 = new ServiceReference1.Zadanie2Client(new InstanceContext(new Zadanie2Enum()));
			zad2.PodajZadania();

			// task 3
			var host3 = new ServiceHost(typeof(Zadanie3));
			host3.AddServiceEndpoint(typeof(KSR_WCF2.IZadanie3),
				new NetNamedPipeBinding(),
				"net.pipe://localhost/ksr-wcf2-zad3");
			host3.Open();

			// task 4
			var host4 = new ServiceHost(typeof(Zadanie4));
			host4.AddServiceEndpoint(typeof(KSR_WCF2.IZadanie4),
				new NetNamedPipeBinding(),
				"net.pipe://localhost/ksr-wcf2-zad4");
			host4.Open();



			Console.ReadKey();
			host3.Close();
			host4.Close();
			((IDisposable)zad2).Dispose();
		}
	}
}
