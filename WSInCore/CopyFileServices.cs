using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ServiceProcess;
using System.Timers;

namespace WSInCore
{
   public  class CopyFileServices:ServiceBase
    {
        Timer timer = new Timer();
        string Destination = System.Configuration.ConfigurationManager.AppSettings["Destination"].ToString();
        string Source = System.Configuration.ConfigurationManager.AppSettings["Source"].ToString();
        string fileName = string.Empty;
        string destFile = string.Empty;

        public CopyFileServices()
        {
            ServiceName = "CoreServices";
        }

        public void OnDebug()
        {
            OnStart(null);
        }
        protected override void OnStart(string[] args)
        {
            WriteToFile("Service is started at " + DateTime.Now);
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 5000; //number in milisecinds 
            timer.Enabled = true;
            //CheckFolder(Source, Destination);
        }

        protected override void OnStop()
        {
            WriteToFile("Service is stopped at " + DateTime.Now);
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            WriteToFile("Service is recall at " + DateTime.Now);
            CheckFolder(Source, Destination);
        }

        #region Copy File Program

        public void CheckFolder(string Source, string Destination)
        {
            if (!Directory.Exists(Destination))
            {
                Directory.CreateDirectory(Destination);
                FileTransfer(Source, Destination);
            }
            else
            {
                FileTransfer(Source, Destination);
            }
        }

        private void FileTransfer(string Source, string Destination)
        {
            string[] filePaths = Directory.GetFiles(Source);
            foreach (var filename in filePaths)
            {
                string file = Path.GetFileName(filename);
                destFile = Path.Combine(Destination, file);
                if (!File.Exists(destFile))
                {
                    File.Copy(filename, destFile, true);
                }
            }
            //Subdirectory Copy
            string[] folders = Directory.GetDirectories(Source);
            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(Destination, name);
                CheckFolder(folder, dest);
            }
        }

        #endregion


        #region Generate Log File

        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.  
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }
        #endregion
    }
}