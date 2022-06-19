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
        //Criar uma Exchange
        canal.ExchangeDeclare(exchange: "topicos_empresa", type: ExchangeType.Topic);

        Console.WriteLine("Digite uma mensagem");

        while (true)
        {
            //Mensagem
            string mensagem = Console.ReadLine();

            if (mensagem == "!finalizar") break;

            //Convertendo a Mensagem em bytes
            var corpoMensagem = Encoding.UTF8.GetBytes(mensagem);

            //pegar uma routkey
            Console.WriteLine("Digite um topico");
            string rotaKey = Console.ReadLine();

            //registrar mensagem
            canal.BasicPublish(exchange: "topicos_empresa",
                                 routingKey: rotaKey,
                                 basicProperties: null,
                                 body: corpoMensagem);

            Console.WriteLine(" [x] Enviou {0}", mensagem);
        }
    }

    Console.WriteLine(" Pressione [enter] para sair.");
    Console.ReadLine();
}