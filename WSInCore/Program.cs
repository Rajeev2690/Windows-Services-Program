using System;
using System.ServiceProcess;
namespace WSInCore
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            //Console.ReadLine();
            /*
            using (var service = new CopyFileServices())
            {
                ServiceBase.Run(service);
            }
            */
            CopyFileServices service = new CopyFileServices();
            service.OnDebug();

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new CopyFileServices()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
