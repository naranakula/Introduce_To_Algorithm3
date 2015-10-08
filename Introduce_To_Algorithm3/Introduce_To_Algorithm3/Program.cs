using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Timers;
using AutoMapper;
using Com.Utility.Commons;
using Introduce_To_Algorithm3.Common.Utils;
using Introduce_To_Algorithm3.Common.Utils.sockets;
using Introduce_To_Algorithm3.Common.Utils.Serial;
using Introduce_To_Algorithm3.Models;
using Introduce_To_Algorithm3.OpenSourceLib.Autofac;
using Introduce_To_Algorithm3.OpenSourceLib.RabbitMq;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;
using Quartz.Util;

namespace Introduce_To_Algorithm3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AutofacHelper.Test();
        }
    }
}
