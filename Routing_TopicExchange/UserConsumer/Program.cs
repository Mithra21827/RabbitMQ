using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();

using var Channel = connection.CreateModel();

Channel.ExchangeDeclare(exchange: "mytopicexchange", type: ExchangeType.Topic);

var queuename = Channel.QueueDeclare().QueueName;

Channel.QueueBind(queue: queuename, exchange: "mytopicexchange", routingKey: "user.#");

var consumer = new EventingBasicConsumer(Channel);

consumer.Received += (sender, args) => {
    var body = args.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    Console.WriteLine($"User Consumer: {message}");

};

Channel.BasicConsume(queuename, autoAck: true, consumer: consumer);

Console.WriteLine("User consuming [user.#]...");

Console.ReadKey();