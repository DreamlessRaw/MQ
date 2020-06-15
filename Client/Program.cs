using RabbitMQ.Client;
using System;
using System.Text;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("客户端");
            IConnectionFactory factory = new ConnectionFactory//创建连接工厂对象
            {
                HostName = "127.0.0.1",//IP地址
                Port = 5672,//端口号
                UserName = "guest",//用户账号
                Password = "guest"//用户密码
            };
            IConnection con = factory.CreateConnection();//创建连接对象
            IModel channel = con.CreateModel();//创建连接会话对象
            string name = "CSharp";
            //声明一个队列
            channel.QueueDeclare(
              queue: name,//消息队列名称         
              durable: false,//是否持久化,true持久化,队列会保存磁盘,服务器重启时可以保证不丢失相关信息。
              exclusive: false,//是否排他,true排他的,如果一个队列声明为排他队列,该队列仅对首次声明它的连接可见,并在连接断开时自动删除.
              autoDelete: false,//是否自动删除。true是自动删除。自动删除的前提是：致少有一个消费者连接到这个队列，之后所有与这个队列连接的消费者都断开时,才会自动删除.
              arguments: null //设置队列的一些其它参数
               );

            channel.QueueBind(
                queue: name,
                exchange: "CSharp",
                routingKey: "test",
                arguments: null);


            string str;
            do
            {
                Console.WriteLine("发送内容:");
                str = Console.ReadLine();
                //消息内容
                byte[] body = Encoding.UTF8.GetBytes(str);
                //发送消息
                channel.BasicPublish(name, "", null, body);
                Console.WriteLine("成功发送消息:" + str);
            } while (str.Trim().ToLower() != "exit");
            con.Close();
            channel.Close();
        }
    }
}
