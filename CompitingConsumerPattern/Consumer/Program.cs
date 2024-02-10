using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName="localhost"};

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.QueueDeclare(queue:"letterbox", durable:false, exclusive: false, autoDelete:false, arguments:null);

channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);


var consumer =new EventingBasicConsumer(channel);

var random = new Random();

consumer.Received += (model, ea) =>
{
    var processing_time = random.Next(1, 6);
    var body = ea.Body.ToArray();

    var message = Encoding.UTF8.GetString(body);

    Console.WriteLine($"{message} recived with processing time {processing_time}");

    Task.Delay(TimeSpan.FromSeconds(processing_time)).Wait();

    channel.BasicAck(deliveryTag:ea.DeliveryTag, multiple:false);
};

channel.BasicConsume("letterbox",false,consumer:consumer);

Console.ReadKey();
