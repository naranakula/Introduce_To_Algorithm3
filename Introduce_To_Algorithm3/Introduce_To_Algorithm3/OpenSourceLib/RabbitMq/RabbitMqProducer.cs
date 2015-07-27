using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Introduce_To_Algorithm3.OpenSourceLib.RabbitMq
{
    public static class RabbitMqProducer
    {

        public static void Send(string message)
        {
            var factory = new ConnectionFactory(){HostName = "localhost"};
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                }
            }
        }
    }
}
