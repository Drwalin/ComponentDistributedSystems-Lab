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
				channel.QueueDeclare("message_queue", false, false, false, null);
				
				var consumer = new EventingBasicConsumer(channel);
				consumer.Received += (model, ea) => {
					var h = ea.BasicProperties.Headers;
					var body = ea.Body;
					var message = Encoding.UTF8.GetString(body.ToArray());

					var respQueueName = Encoding.UTF8.GetString((byte[])h["response"]);

					Console.WriteLine(pid + " odbiera: " + message + "   ``` headers: test: " + h["test"] + ", mleko: " + h["mleko"]);
					Thread.Sleep(2000);
					Console.WriteLine(pid + " odbiera: " + message + "   ``` wykonane, wysyla odpowiedz do: " + respQueueName);

					var msg = pid + " odpowiedz na: " + message;
					var bytes = Encoding.UTF8.GetBytes(msg);
					Console.WriteLine(msg);


					channel.BasicPublish("", respQueueName, null, bytes);
				};
				consumer.Model.BasicQos(0, 3, false);
				channel.BasicConsume("message_queue", true, consumer);

				Console.ReadKey();
			}
		}
	}
}
