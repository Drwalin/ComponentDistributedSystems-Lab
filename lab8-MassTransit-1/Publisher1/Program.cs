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
	public class Message : IMessage {
		public string Msg { get; set; }
	}
	class Program {
		static void Main(string[] args) {
			var bus = Bus.Factory.CreateUsingRabbitMq(sbc => {
				sbc.Host(new Uri("rabbitmq://sparrow.rmq.cloudamqp.com/mapaayxd"),
					h => {
						h.Username("mapaayxd");
						h.Password("HmuXDb9Jj-crT9SGJ2XCRV2yTqQZg5EU");
					});
			});
			bus.StartAsync();
			Console.WriteLine("Nadawca");
			for(int i=0; i<10; ++i) {
				System.Threading.Thread.Sleep(1000);
				Message msg = new Message();
				msg.Msg = "Message: " + i;
				Console.WriteLine("Wyslano: " + msg.Msg);
				bus.Publish(msg, ctx=> {
					ctx.Headers.Set("a", "123");
					ctx.Headers.Set("b", "abc");
				});
			}
		}
	}
}
