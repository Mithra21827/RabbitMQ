using System;
using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName="localhost"};

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange:"pubsub", ExchangeType.Fanout);

Console.WriteLine("Producer 1");
while (true)
{
    Console.Write("Enter Your message: ");
    var message = Console.ReadLine();

    if (message != null)
    {
        var encodeMessage = Encoding.UTF8.GetBytes(message);


        channel.BasicPublish(exchange: "pubsub", "", null, encodeMessage);
    }
}


