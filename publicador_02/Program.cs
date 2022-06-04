using RabbitMQ.Client;

using System.Text;

//Servidor do Rabbitmq
var servidor = new ConnectionFactory() { HostName = "localhost", Port = 5672, UserName = "usuario", Password = "Senha@123" };

//Conexão com o servidor
var conexao = servidor.CreateConnection();
{
    //Canal
    using (var canal = conexao.CreateModel())
    {
        //Criar fila ou escutar fila
        canal.QueueDeclare(queue: "fila_de_tarefas",
                      durable: true,
                      exclusive: false,
                      autoDelete: false,
                      arguments: null);

        Console.WriteLine("Digite uma mensagem");

        while (true)
        {
            //Mensagem
            string mensagem = Console.ReadLine();

            if (mensagem == "!finalizar") break;

            //Convertendo a Mensagem em bytes
            var corpoMensagem = Encoding.UTF8.GetBytes(mensagem);

            //propriedades
            var propriedades = canal.CreateBasicProperties();
            propriedades.Persistent = true;

            //registrar mensagem
            canal.BasicPublish(exchange: "",
                                 routingKey: "fila_de_tarefas",
                                 basicProperties: propriedades,
                                 body: corpoMensagem);

            Console.WriteLine(" [x] Enviou {0}", mensagem);
        }
    }

    Console.WriteLine(" Pressione [enter] para sair.");
    Console.ReadLine();
}