using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "myroutingexchange", type: ExchangeType.Direct);

var queuename = channel.QueueDeclare().QueueName;

channel.QueueBind(exchange: "myroutingexchange", queue:queuename, routingKey: "analyticsonly");

channel.QueueBind(exchange: "myroutingexchange",queue: queuename, routingKey: "both");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) => {
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Analytics Consumer: {message}");
};

channel.BasicConsume(queue: queuename, autoAck: true, consumer: consumer);

Console.WriteLine("Analytics consumer Consuming...");

Console.ReadKey();