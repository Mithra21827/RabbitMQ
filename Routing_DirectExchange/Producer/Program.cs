using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "myroutingexchange", type: ExchangeType.Direct);

while (true) {
    Console.Write("Enter Your mesaage: ");
    var message =Console.ReadLine();
    var encodeMessage = Encoding.UTF8.GetBytes(message);
    Console.WriteLine($"Message: '{message}' sent");

    channel.BasicPublish("myroutingexchange", "both", null, encodeMessage);
};

