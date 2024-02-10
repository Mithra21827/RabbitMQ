using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory();

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "myroutingexchange",type:ExchangeType.Direct);

var queuename = channel.QueueDeclare().QueueName;

channel.QueueBind(exchange: "myroutingexchange", queue:queuename, routingKey:"paymentOnly");

channel.QueueBind(exchange: "myroutingexchange",queue:queuename, routingKey: "both");

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (sender, args) => { 
    var body = args.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Payment consumer: {message}");
};

channel.BasicConsume(queue: queuename, autoAck: true, consumer: consumer);

Console.WriteLine("Payment Consumer Consuming...");

Console.ReadKey();