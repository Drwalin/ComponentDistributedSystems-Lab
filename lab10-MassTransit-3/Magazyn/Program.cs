using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Messages;

namespace Messages {
	public interface IStartZamowienia {
		int Ilosc { get; set; }
		string Login { get; set; }
	}
	public class StartZamowienia : IStartZamowienia {
		public int Ilosc { get; set; }
		public string Login { get; set; }
	}

	public interface IPytanieoPotwierdzenie : CorrelatedBy<Guid> {
		int Ilosc { get; set; }
		string Login { get; set; }
	}
	public class PytanieoPotwierdzenie : IPytanieoPotwierdzenie {
		public int Ilosc { get; set; }
		public Guid CorrelationId { get; set; }
		public string Login { get; set; }
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
		string Login { get; set; }
	}
	public class AkceptacjaZamowienia : IAkceptacjaZamowienia {
		public int Ilosc { get; set; }
		public Guid CorrelationId { get; set; }
		public string Login { get; set; }
	}

	public interface IOdrzucenieZamowienia : CorrelatedBy<Guid> {
		int Ilosc { get; set; }
		string Login { get; set; }
	}
	public class OdrzucenieZamowienia : IOdrzucenieZamowienia {
		public int Ilosc { get; set; }
		public Guid CorrelationId { get; set; }
		public string Login { get; set; }
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

namespace mag {
	public class Magazyn : IConsumer<Messages.IPytanieoWolne>, IConsumer<Messages.IAkceptacjaZamowienia>, IConsumer<Messages.IOdrzucenieZamowienia> {
		public int wolne = 0, zarezerwowane = 0;
		public HashSet<Guid> reservedTransactions = new HashSet<Guid>();
		public Task Consume(ConsumeContext<Messages.IPytanieoWolne> ctx) {
			if(ctx.Message.Ilosc > wolne) {
				ctx.RespondAsync(new Messages.OdpowiedzWolneNegatywna() { CorrelationId = ctx.Message.CorrelationId });
				return Print("OdpowiedzWolneNegatywna: " + ctx.Message.Ilosc);
			} else {
				wolne -= ctx.Message.Ilosc;
				zarezerwowane += ctx.Message.Ilosc;
				reservedTransactions.Add(ctx.Message.CorrelationId);
				ctx.RespondAsync(new Messages.OdpowiedzWolne() { CorrelationId = ctx.Message.CorrelationId });
				return Print("OdpowiedzWolne: " + ctx.Message.Ilosc);
			}
		}

		public Task Consume(ConsumeContext<Messages.IAkceptacjaZamowienia> ctx) {
			if(reservedTransactions.Contains(ctx.Message.CorrelationId)) {
				zarezerwowane -= ctx.Message.Ilosc;
				reservedTransactions.Remove(ctx.Message.CorrelationId);
			}
			return Print("IAkceptacjaZamowienia: " + ctx.Message.Ilosc);
		}

		public Task Consume(ConsumeContext<Messages.IOdrzucenieZamowienia> ctx) {
			if(reservedTransactions.Contains(ctx.Message.CorrelationId)) {
				zarezerwowane -= ctx.Message.Ilosc;
				wolne += ctx.Message.Ilosc;
				reservedTransactions.Remove(ctx.Message.CorrelationId);
			}
			return Print("IOdrzucenieZamowienia: " + ctx.Message.Ilosc);
		}


		public Task Print(string str = "") {
			return Console.Out.WriteLineAsync("Wolne: " + wolne + " zarezerwowane: " + zarezerwowane + " ; " + str);
		}
	}
	class Program {
		static void Main(string[] args) {
			Magazyn magazyn = new Magazyn();
			var bus = Bus.Factory.CreateUsingRabbitMq(sbc => {
				sbc.Host(new Uri("rabbitmq://sparrow.rmq.cloudamqp.com/mapaayxd"),
					h => {
						h.Username("mapaayxd");
						h.Password("HmuXDb9Jj-crT9SGJ2XCRV2yTqQZg5EU");
					});
				sbc.ReceiveEndpoint("KolejkaMagazyn", x => {
					x.Instance(magazyn);
				});
			});
			Console.WriteLine("Magazyn");
			bus.StartAsync();
			while(true) {
				try {
					int v = Convert.ToInt32(Console.ReadLine());
					magazyn.wolne += v;
					magazyn.Print("dodano: " + v);
				} catch(Exception) {
					Console.WriteLine("Proszę podać liczbę");
				}
			}
		}
	}
}
