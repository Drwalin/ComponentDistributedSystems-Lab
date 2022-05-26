using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;


namespace Messages {
	public interface IStartZamowienia {
		int Ilosc { get; set; }
		string Username { get; set; }
	}
	public class StartZamowienia : IStartZamowienia {
		public int Ilosc { get; set; }
		public string Username { get; set; }
	}

	public interface IPytanieoPotwierdzenie : CorrelatedBy<Guid> {
		int Ilosc { get; set; }
		string Username { get; set; }
	}
	public class PytanieoPotwierdzenie : IPytanieoPotwierdzenie {
		public int Ilosc { get; set; }
		public Guid CorrelationId { get; set; }
		public string Username { get; set; }
	}

	public interface IPytanieoWolne : CorrelatedBy<Guid> {
		int Ilosc { get; set; }
	}
	public class PytanieoWolne : IPytanieoWolne {
		public int Ilosc { get; set; }
		public Guid CorrelationId { get; set; }
	}

	public interface IAkceptacjaZamowienia : CorrelatedBy<Guid> {
		int Ilosc { get; set; }
		string Username { get; set; }
	}
	public class AkceptacjaZamowienia : IAkceptacjaZamowienia {
		public int Ilosc { get; set; }
		public Guid CorrelationId { get; set; }
		public string Username { get; set; }
	}

	public interface IOdrzucenieZamowienia : CorrelatedBy<Guid> {
		int Ilosc { get; set; }
		string Username { get; set; }
	}
	public class OdrzucenieZamowienia : IOdrzucenieZamowienia {
		public int Ilosc { get; set; }
		public Guid CorrelationId { get; set; }
		public string Username { get; set; }
	}


	public interface IPotwierdzenie : CorrelatedBy<Guid> {
	}
	public class Potwierdzenie : IPotwierdzenie {
		public Guid CorrelationId { get; set; }
	}

	public interface IBrakPotwierdzenia : CorrelatedBy<Guid> {
	}
	public class BrakPotwierdzenia : IBrakPotwierdzenia {
		public Guid CorrelationId { get; set; }
	}

	public interface IOdpowiedzWolne : CorrelatedBy<Guid> {
	}
	public class OdpowiedzWolne : IOdpowiedzWolne {
		public Guid CorrelationId { get; set; }
	}

	public interface IOdpowiedzWolneNegatywna : CorrelatedBy<Guid> {
	}
	public class OdpowiedzWolneNegatywna : IOdpowiedzWolneNegatywna {
		public Guid CorrelationId { get; set; }
	}

	public interface ITimeout : CorrelatedBy<Guid> {
	}
	public class Timeout : ITimeout {
		public Guid CorrelationId { get; set; }
	}
}

namespace cli {

	public class Klient : IConsumer<Messages.IPytanieoPotwierdzenie>, IConsumer<Messages.IAkceptacjaZamowienia>, IConsumer<Messages.IOdrzucenieZamowienia> {
		public Task Consume(ConsumeContext<Messages.IPytanieoPotwierdzenie> ctx) {
			Console.WriteLine("Czy potwierdzasz zamówienie o ilości: " + ctx.Message.Ilosc + " (Y/N)");
			var c = Console.ReadKey();
			if(c.KeyChar == 'y' || c.KeyChar == 'Y') {
				ctx.RespondAsync(new Messages.Potwierdzenie() { CorrelationId = ctx.Message.CorrelationId });
				return Console.Out.WriteLineAsync("Odrzucono zamowienie o ilosci: " + ctx.Message.Ilosc);
			} else {
				ctx.RespondAsync(new Messages.BrakPotwierdzenia() { CorrelationId = ctx.Message.CorrelationId });
				return Console.Out.WriteLineAsync("Odrzucono zamowienie o ilosci: " + ctx.Message.Ilosc);
			}
		}

		public Task Consume(ConsumeContext<Messages.IAkceptacjaZamowienia> ctx) {
			return Console.Out.WriteLineAsync("Udało się wykonać zamówienie o ilości: " + ctx.Message.Ilosc);
		}

		public Task Consume(ConsumeContext<Messages.IOdrzucenieZamowienia> ctx) {
			return Console.Out.WriteLineAsync("Nie udało się wykonać zamówienie o ilości: " + ctx.Message.Ilosc);
		}
	}

	class Program {
		static void Main(string[] args) {
			string username = "Klient" + new Random().Next().ToString();
			var bus = Bus.Factory.CreateUsingRabbitMq(sbc => {
				sbc.Host(new Uri("rabbitmq://sparrow.rmq.cloudamqp.com/mapaayxd"),
					h => {
						h.Username("mapaayxd");
						h.Password("HmuXDb9Jj-crT9SGJ2XCRV2yTqQZg5EU");
					});
				sbc.ReceiveEndpoint("Kolejka"+ username, x => {
					x.Instance(new Klient());
				});
			});
			bus.StartAsync();
			Console.WriteLine(username);
			while(true) {
				try {
					int v = Convert.ToInt32(Console.ReadLine());
					bus.Publish(new Messages.StartZamowienia() {Username = username, Ilosc = v });
				} catch(Exception) {
					Console.WriteLine("Proszę podać liczbę");
				}
			}
		}
	}
}
