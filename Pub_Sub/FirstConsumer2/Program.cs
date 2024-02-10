using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare("pubsub2", type: ExchangeType.Fanout, durable: false, autoDelete: false, arguments: null);

var queueName = channel.QueueDeclare().QueueName;

channel.QueueBind(exchange: "pubsub2", queue:queueName, routingKey:"", arguments:null);

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, ea) => {
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Subscriber 1: {message}");
};

channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
Console.WriteLine("Consumer1 waiting for Producer2 for consuming...");


Console.ReadKey();
