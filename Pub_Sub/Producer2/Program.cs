using System;
using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange:"pubsub2",type:ExchangeType.Fanout,durable:false,autoDelete:false,arguments:null);

Console.WriteLine("Producer 2");

while (true) { 
    Console.Write("Enter Your message: ");
    var message = Console.ReadLine();

    if (message != null)
    {
        var encodeMessage = Encoding.UTF8.GetBytes(message);


        channel.BasicPublish(exchange:"pubsub2", "", null, encodeMessage);
    }
}