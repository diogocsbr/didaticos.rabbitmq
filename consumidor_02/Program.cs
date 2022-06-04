using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using System.Text;

//Servidor do Rabbitmq
var servidor = new ConnectionFactory() { HostName = "localhost", Port = 5672, UserName = "usuario", Password = "Senha@123" };

//Conexão com o servidor
var conexao = servidor.CreateConnection();
{
    //Canal
    using (var canal = conexao.CreateModel())
    {
        //declarar a fila que está escutando
        canal.QueueDeclare(queue: "fila_de_tarefas",
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);


        canal.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

        Console.WriteLine(" [*] Aguardando mensagens.");

        //consumidor
        var consumidor = new EventingBasicConsumer(canal);

        //evento que o cosumidor está escutando
        consumidor.Received += (model, ea) =>
        {
            //vamos receber o corpo do rabbit
            var corpo = ea.Body.ToArray();

            //pegar a mensagem que está em bytes e converter
            var mensagem = Encoding.UTF8.GetString(corpo);

            //mostrando a mensagem
            Console.WriteLine(" [x] Recebido {0}", mensagem);

            canal.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        };

        canal.BasicConsume(queue: "fila_de_tarefas",
                             autoAck: false,
                             consumer: consumidor);

        Console.WriteLine(" Pressione [enter] para finalizar.");
        Console.ReadLine();
    }

}