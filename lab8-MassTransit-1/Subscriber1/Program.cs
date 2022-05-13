using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;

namespace Publisher1 {
	public interface IMessage {
		string Msg { get; set; }
	}
	public class Odbiorca : IConsumer<IMessage> {
		int count = 0;
		public Task Consume(ConsumeContext<IMessage> ctx) {
			count++;
			string h1 = ctx.Headers.GetAll().Where(e => e.Key == "a").First().Value.ToString();
			string h2 = ctx.Headers.GetAll().Where(e => e.Key == "b").First().Value.ToString();
			return Console.Out.WriteLineAsync("Odebrano: " + ctx.Message.Msg + " ["+h1+","+h2+"]   c=" + count);
		}
	}
	class Program {
		static void Main(string[] args) {
			var bus = Bus.Factory.CreateUsingRabbitMq(sbc => {
				sbc.Host(new Uri("rabbitmq://sparrow.rmq.cloudamqp.com/mapaayxd"),
					h => {
						h.Username("mapaayxd");
						h.Password("HmuXDb9Jj-crT9SGJ2XCRV2yTqQZg5EU");
					});
				sbc.ReceiveEndpoint("Kolejka1", x => {
					x.Consumer<Odbiorca>();
					/*
					x.Handler<IMessage>(ep => {
						ep.Consumer<Odbiorca>();
						string h1 = msg.Headers.GetAll().Where(e => e.Key == "a").First().Value.ToString();
						string h2 = msg.Headers.GetAll().Where(e => e.Key == "b").First().Value.ToString();
						return Console.Out.WriteLineAsync("Odebrano: " + msg.Message.Msg + " ["+h1+","+h2+"]");
					});
					*/
				});
			});
			Console.WriteLine("Odbiorca 1");
			bus.Start();
			Console.ReadKey();
			bus.Stop();
		}
	}
}
