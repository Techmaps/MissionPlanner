using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using Blazor.Extensions.Storage;
using Blazor.FileReader;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Harmony;
using Microsoft.JSInterop;
using MissionPlanner.ArduPilot;
using MissionPlanner.Utilities;


namespace wasm
{
    public class Startup
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddStorage();

            //services.Add(new ServiceDescriptor(typeof(IFileReaderService), typeof(FileReaderService), ServiceLifetime.Transient));

            var x = System.Runtime.CompilerServices.Unsafe.Unbox<int>(1);
            services.AddFileReaderService();

            //services.UseWebUSB(); // Makes IUSB available to the DI container
        }
        //IBlazorApplicationBuilder
        //IComponentsApplicationBuilder
        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");

            log4net.Repository.Hierarchy.Hierarchy hierarchy =
                (Hierarchy)log4net.LogManager.GetRepository(Assembly.GetAssembly(typeof(Startup)));

            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "%date [%thread] %-5level %logger - %message%newline";
            patternLayout.ActivateOptions();

            var cca = new ConsoleAppender();
            cca.Layout = patternLayout;
            cca.ActivateOptions();
            hierarchy.Root.AddAppender(cca);

            hierarchy.Root.Level = Level.Debug;
            hierarchy.Configured = true;

            log.Info("test");

            log.Info("Configure Done");

            //System.Net.WebRequest.get_InternalDefaultWebProxy

            //WebRequest.DefaultWebProxy = GlobalProxySelection.GetEmptyWebProxy();

            Directory.CreateDirectory(@"/home/web_user/Desktop");

            BinaryLog.onFlightMode += (firmware, modeno) =>
            {
                try
                {
                    if (firmware == "")
                        return null;

                    var modes = Common.getModesList((Firmwares)Enum.Parse(typeof(Firmwares), firmware));
                    string currentmode = null;

                    foreach (var mode in modes)
                    {
                        if (mode.Key == modeno)
                        {
                            currentmode = mode.Value;
                            break;
                        }
                    }

                    return currentmode;
                }
                catch
                {
                    return null;
                }
            };

            CustomMessageBox.ShowEvent += (text, caption, buttons, icon, yestext, notext) =>
            {
                Console.WriteLine("CustomMessageBox " + caption + " " + text);


                return CustomMessageBox.DialogResult.OK;
            };
        }
    }
}
