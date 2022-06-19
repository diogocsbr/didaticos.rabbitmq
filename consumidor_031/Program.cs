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
        //Criando ou declara
        canal.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

        //criar fila 
        var nomeFila = canal.QueueDeclare().QueueName;

        //ligar fila no exchange
        canal.QueueBind(queue: nomeFila, exchange: "logs", routingKey: "");

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
        };

        canal.BasicConsume(queue: nomeFila, autoAck: true, consumer: consumidor);

        Console.WriteLine(" Pressione [enter] para finalizar.");
        Console.ReadLine();
    }

}