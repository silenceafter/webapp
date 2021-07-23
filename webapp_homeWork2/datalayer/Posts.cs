using System;
using System.IO;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace datalayer
{    
    public class Posts
    { 
        public int Id { get; set; }
        public string Content { get; set; } 
        
        public void A()
        {           
            var config = new LoggingConfiguration();
            var fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);
            fileTarget.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MyLogFile.txt");
            fileTarget.Layout = "[${date:format=dd\\.MM\\.yyyy HH\\:mm\\:ss}] ${level} [${callsite}] ${message}";
            fileTarget.CreateDirs = true;
            fileTarget.KeepFileOpen = true;
            //config.LoggingRules.Add(new LoggingRule("*", , fileTarget));
            LogManager.Configuration = config;
            return;
        }       
    }
}
