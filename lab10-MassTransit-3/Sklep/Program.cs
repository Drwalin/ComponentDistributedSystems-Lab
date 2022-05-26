using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;

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

namespace Sklep {


	public class RejestracjaZamowienie : SagaStateMachineInstance {
		public RejestracjaZamowienie() {
			CorrelationId = Guid.NewGuid();
		}
		public Guid CorrelationId { get; set; }
		public Guid? CorrelectionIdNullable { get { return CorrelationId; } set { CorrelationId = value.Value; } }
		public string CurrentState { get; set; }
		public int Ilosc { get; set; }
		public string Login { get; set; }
	}
	public class RejestracjaSklep : MassTransitStateMachine<RejestracjaZamowienie> {
		public State Niepotwierdzone { get; private set; }
		public State PotwierdzoneKlient { get; private set; }
		public State PotwierdzoneMagazyn { get; private set; }

		public Event<Messages.StartZamowienia> StartZamowienia { get; private set; }
		public Event<Messages.Potwierdzenie> Potwierdzenie { get; private set; }
		public Event<Messages.BrakPotwierdzenia> BrakPotwierdzenia { get; private set; }
		public Event<Messages.OdpowiedzWolne> OdpowiedzWolne { get; private set; }
		public Event<Messages.OdpowiedzWolneNegatywna> OdpowiedzWolneNegatywna { get; set; }
		public Event<Messages.Timeout> TimeoutEvent { get; private set; }
		public Schedule<RejestracjaZamowienie, Messages.Timeout> Timeout { get; set; }

		public RejestracjaSklep() {
			InstanceState(x => x.CurrentState);

			Event(() => StartZamowienia,
				x => x.CorrelateBy(
						s => s.Login,
						ctx => { Console.WriteLine("CorrelateBy: ctx.Message.Login"); return ctx.Message.Login; }
					).SelectId(ctx => { Console.WriteLine("Getting new guid"); return Guid.NewGuid(); })
				);

			Schedule(() => Timeout,
					ctx => ctx.CorrelectionIdNullable,
					ctx => { ctx.Delay = TimeSpan.FromSeconds(10); Console.WriteLine("schedule timeout"); }
				);

			Initially(
				When(StartZamowienia)
				.Schedule(Timeout, ctx => { Console.WriteLine("Start zamowienie timeout");  return new Messages.Timeout() { CorrelationId = ctx.Saga.CorrelationId }; })
				.Then(ctx => ctx.Saga.Login = ctx.Message.Login)
				.Then(ctx => ctx.Saga.Ilosc = ctx.Message.Ilosc)
				.ThenAsync(ctx => { return Console.Out.WriteLineAsync($"zamowienie w ilosci {ctx.Message.Ilosc}"); })
				.Respond(ctx => { Console.WriteLine("respond"); return new Messages.PytanieoPotwierdzenie() { CorrelationId = ctx.Saga.CorrelationId, Login = ctx.Saga.Login }; })
				.Respond(ctx => { Console.WriteLine("respond"); return new Messages.PytanieoWolne() { CorrelationId = ctx.Saga.CorrelationId, Ilosc = ctx.Saga.Ilosc }; })
				.TransitionTo(Niepotwierdzone)
				);

			During(Niepotwierdzone,
				When(TimeoutEvent)
				.ThenAsync(ctx => { return Console.Out.WriteLineAsync($"TIMEOUT: na zamowienie {ctx.Message.CorrelationId}"); })
				.Respond(ctx => { Console.WriteLine("respond"); return new Messages.OdrzucenieZamowienia() { CorrelationId = ctx.Saga.CorrelationId, Ilosc = ctx.Saga.Ilosc, Login = ctx.Saga.Login }; })
				.Finalize(),

				When(Potwierdzenie)
				.ThenAsync(ctx => { return Console.Out.WriteLineAsync($"potwierdzil zamowienie {ctx.Message.CorrelationId}"); })
				.Unschedule(Timeout)
				.TransitionTo(PotwierdzoneKlient),

				When(BrakPotwierdzenia)
				.ThenAsync(ctx => { return Console.Out.WriteLineAsync($" nie potwierdzil zamowienia {ctx.Message.CorrelationId}"); })
				.Respond(ctx => { Console.WriteLine("respond"); return new Messages.OdrzucenieZamowienia() { CorrelationId = ctx.Saga.CorrelationId, Ilosc = ctx.Saga.Ilosc, Login = ctx.Saga.Login }; })
				.Finalize(),

				When(OdpowiedzWolne)
				.ThenAsync(ctx => { return Console.Out.WriteLineAsync($"Magazyn moze zrealizowac zamowienie {ctx.Message.CorrelationId}"); })
				.TransitionTo(PotwierdzoneMagazyn),

				When(OdpowiedzWolneNegatywna)
				.ThenAsync(ctx => { return Console.Out.WriteLineAsync($"Magazyn nie moze zrealizowac zamowienia {ctx.Message.CorrelationId}"); })
				.Respond(ctx => { Console.WriteLine("respond"); return new Messages.OdrzucenieZamowienia() { CorrelationId = ctx.Saga.CorrelationId, Ilosc = ctx.Saga.Ilosc }; })
				.Finalize()
				);

			During(PotwierdzoneKlient,
				When(OdpowiedzWolne)
				.ThenAsync(ctx => { return Console.Out.WriteLineAsync($"Magazyn moze zrealizowac zamowienie {ctx.Message.CorrelationId}"); })
				.Respond(ctx => { Console.WriteLine("respond"); return new Messages.AkceptacjaZamowienia() { CorrelationId = ctx.Saga.CorrelationId, Ilosc = ctx.Saga.Ilosc }; })
				.Finalize(),

				When(OdpowiedzWolneNegatywna)
				.ThenAsync(ctx => { return Console.Out.WriteLineAsync($"Magazyn nie moze zrealizowac zamowienia {ctx.Message.CorrelationId}"); })
				.Respond(ctx => { Console.WriteLine("respond"); return new Messages.OdrzucenieZamowienia() { CorrelationId = ctx.Saga.CorrelationId, Ilosc = ctx.Saga.Ilosc }; })
				.Finalize()
				);

			During(PotwierdzoneMagazyn,
				When(TimeoutEvent)
				.ThenAsync(ctx => { return Console.Out.WriteLineAsync($"TIMEOUT: na zamowienie {ctx.Message.CorrelationId}"); })
				.Respond(ctx => { Console.WriteLine("respond"); return new Messages.OdrzucenieZamowienia() { CorrelationId = ctx.Saga.CorrelationId, Ilosc = ctx.Saga.Ilosc, Login = ctx.Saga.Login }; })
				.Finalize(),

				When(Potwierdzenie)
				.ThenAsync(ctx => { return Console.Out.WriteLineAsync($"potwierdzil zamowienie {ctx.Message.CorrelationId}"); })
				.Respond(ctx => { Console.WriteLine("respond"); return new Messages.AkceptacjaZamowienia() { CorrelationId = ctx.Saga.CorrelationId, Ilosc = ctx.Saga.Ilosc, Login = ctx.Saga.Login }; })
				.Unschedule(Timeout)
				.Finalize(),

				When(BrakPotwierdzenia)
				.ThenAsync(ctx => { return Console.Out.WriteLineAsync($"nie potwierdzil zamowienia {ctx.Message.CorrelationId}"); })
				.Respond(ctx => { Console.WriteLine("respond"); return new Messages.OdrzucenieZamowienia() { CorrelationId = ctx.Saga.CorrelationId, Ilosc = ctx.Saga.Ilosc, Login = ctx.Saga.Login }; })
				.Finalize()
				);

			SetCompletedWhenFinalized();
		}
	}


	class Program {
		static void Main(string[] args) {
			var rep = new InMemorySagaRepository<RejestracjaZamowienie>();
			var saga = new RejestracjaSklep();
			var bus = Bus.Factory.CreateUsingRabbitMq(
			 sbc => {
				 sbc.Host(new Uri("rabbitmq://sparrow.rmq.cloudamqp.com/mapaayxd"),
					 h => {
						 h.Username("mapaayxd");
						 h.Password("HmuXDb9Jj-crT9SGJ2XCRV2yTqQZg5EU");
					 });
				 sbc.ReceiveEndpoint("KolejkaSklep",
					 x => x.StateMachineSaga(saga, rep));
				 sbc.UseDelayedMessageScheduler();
			 });
			bus.Start();
			Console.WriteLine("Sklep");
			while(true) { }
			Console.ReadKey();
			bus.Stop();
		}
	}
}
