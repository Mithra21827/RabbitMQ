using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory();

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "request-queue", exclusive: false);

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (sender, args) =>
{
    Console.WriteLine($"Received Request: {args.BasicProperties.CorrelationId}");

    var replaymessage = $"This is yout reply: {args.BasicProperties.CorrelationId}";

    var body = Encoding.UTF8.GetBytes(replaymessage);

    Console.WriteLine("Reply sent");
    
    channel.BasicPublish("", args.BasicProperties.ReplyTo, null, body);
};

channel.BasicConsume("request-queue", autoAck: true, consumer: consumer);


Console.ReadKey();