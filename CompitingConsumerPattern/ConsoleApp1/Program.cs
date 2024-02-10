using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;


var factory = new ConnectionFactory { HostName="localhost"};

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.QueueDeclare(queue:"letterbox", durable: false, exclusive: false, autoDelete:false, arguments: null );

var num = 1;
var random = new Random();
while (true)
{

    var processing_time = random.Next(1, 2);

    var message = $"Message ID {num} sent at {processing_time} sec delay";

    var encodeMessage = Encoding.UTF8.GetBytes(message);

    Task.Delay(TimeSpan.FromSeconds(processing_time)).Wait();

    channel.BasicPublish("","letterbox",null,encodeMessage);

    Console.WriteLine($"{num}.Publisher sent: {message}");
    num++;
}

