using System;
using System.Text;
using System.Threading.Channels;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory();

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "mytopicexchange", type:ExchangeType.Topic);

var queueName = channel.QueueDeclare().QueueName;

channel.QueueBind(queue: queueName, exchange: "mytopicexchange", routingKey: "#.payments");


var consumer = new EventingBasicConsumer(channel);

consumer.Received += (sender, args) => {
    var body = args.Body.ToArray();

    var message = Encoding.UTF8.GetString(body);

    Console.WriteLine($"Payment consumer: {message}");
};

channel.BasicConsume(queueName, autoAck:true, consumer);

Console.WriteLine("Paymet Consuming [#.payments]...");

Console.ReadKey();