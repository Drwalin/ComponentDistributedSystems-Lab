using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using RabbitMQ.Client;
using System.Diagnostics;
using RabbitMQ.Client.Events;

namespace Client_dotnetframework {
	class Program {
		static void Main(string[] args) {
			var factory = new ConnectionFactory() {
				UserName = "mapaayxd",
				Password = "HmuXDb9Jj-crT9SGJ2XCRV2yTqQZg5EU",
				HostName = "sparrow.rmq.cloudamqp.com",
				VirtualHost = "mapaayxd"
			};


			var pid = Process.GetCurrentProcess().Id;
			using(var connection = factory.CreateConnection())
			using(var channel = connection.CreateModel()) {
				IBasicProperties prop = channel.CreateBasicProperties();
				prop.Headers = new Dictionary<string, object>();
				prop.Headers.Add("test", 311);
				prop.Headers.Add("mleko", 10);

				channel.QueueDeclare("message_queue", false, false, false, null);
				string replyQueueName = channel.QueueDeclare("", false, false, false, null).QueueName;
				Console.WriteLine("Reply queue: " + replyQueueName);

				var consumer = new EventingBasicConsumer(channel);
				consumer.Received += (model, ea) => {
					var message = Encoding.UTF8.GetString(ea.Body.ToArray());

					Console.WriteLine(pid + " odbiera odpowiedz: " + message);
					// show message
				};
				channel.BasicConsume(replyQueueName, true, consumer);

				prop.Headers.Add("response", replyQueueName);

				for(int i = 0; i < 10; ++i) {
					var msg = pid + " wysyla wiadomosc " + i;
					var bytes = Encoding.UTF8.GetBytes(pid + " wysyla wiadomosc " + i);
					Console.WriteLine(msg);
					channel.BasicPublish("", "message_queue", prop, bytes);
					Thread.Sleep(1000);
				}


				Console.ReadKey();
				channel.QueueDelete(replyQueueName);
			}
		}
	}
}
