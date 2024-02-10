using System;
using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "mytopicexchange", type: ExchangeType.Topic);

while (true)
{

    Console.Write("Enter your message: ");

    var message = Console.ReadLine();

    Console.Write("Enter your routingKey: ");

    var routingKey = Console.ReadLine();

    if (message != null)
    {
            
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "mytopicexchange", routingKey: routingKey, body:body);
    }
}