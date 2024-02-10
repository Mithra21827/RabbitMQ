using System;
using System.Runtime.InteropServices;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

var replayQueue = channel.QueueDeclare(queue:"", exclusive: true);

channel.QueueDeclare(queue: "request-queue", exclusive: false);

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (sender, args) => {
    var body = args.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Replay message: {message}");
};

channel.BasicConsume(queue: replayQueue.QueueName, autoAck: true, consumer:consumer );

var message = "Can I request a replay";
var body = Encoding.UTF8.GetBytes(message);

var properties = channel.CreateBasicProperties();
properties.ReplyTo = replayQueue.QueueName;
properties.CorrelationId = Guid.NewGuid().ToString();

channel.BasicPublish(exchange: "", "request-queue", properties, body: body) ;

Console.WriteLine($"Sending requiest: {properties.CorrelationId}");

Console.ReadKey();