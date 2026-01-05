using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SMEAppHouse.Core.TopshelfAdapter;
using SMEAppHouse.Core.TopshelfAdapter.Common;
using Topshelf;

namespace SMEAppHouse.Core.TopShelfAdapter.Test
{
    class Program
    {
        private static IConfiguration _configurations;

        static void Main(string[] args)
        {
            //HostFactory.Run(host => {
            //    host.SetServiceName("SMEAppHouseFreeIPProxyProviderSvc");
            //    host.SetDisplayName("SMEAppHouse Free IP Proxy Provider Service");
            //    host.SetDescription("Service providing free IP proxies from public sites");
            //    host.StartAutomatically();
            //    host.Service<TestWorker>();
            //});
            //Console.WriteLine("Hello World!");

            _configurations = LoadConfiguration();
            var runtimeBehaviorOptions = _configurations.GetSection("RuntimeBehaviorOptions");
            
            HostFactory.Run(configurator =>
            {
                HostFactory.Run(x =>
                {
                    x.Service<BryanTestService>(cfg =>
                    {
                        cfg.ConstructUsing(name =>
                        {
                            try
                            {
                                // initialize the service controller.
                                var serviceProvider = ConfigureServices(); // Retrieve the configured service provider

                                // Resolve dependencies using the service provider
                                var runtimeBehaviorOptions = serviceProvider.GetRequiredService<RuntimeBehaviorOptions>();
                                var logger = serviceProvider.GetRequiredService<ILogger<BryanTestService>>();

                                var svc = new BryanTestService(runtimeBehaviorOptions, logger);

                                svc.OnServiceInitialized += Svc_OnServiceInitialized;
                                svc.Resume();
                                return svc;
                            }
                            catch (Exception exception)
                            {
                                throw;
                            }
                        });

                        cfg.WhenStarted(svcCtrlr =>
                        {
                            svcCtrlr.Resume();
                        });

                        cfg.WhenStopped(svcCtrlr =>
                        {
                            svcCtrlr.Suspend();
                        });
                        cfg.WhenShutdown(svcCtrlr =>
                        {
                            svcCtrlr.Shutdown();
                        });

                    });

                    x.SetDisplayName("Bryan's Test Service");
                    x.SetDescription("Bryan's Test Service for checking SMS from time to time.");
                    x.SetServiceName("BryanTestService");

                    x.RunAsLocalSystem();
                    x.StartAutomatically();
                });
            });

        }

        private static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();            

            services.AddLogging(configure =>
            {
                configure.AddConfiguration(_configurations.GetSection("Logging"));
                configure.ClearProviders();
                configure.AddConsole();
                //configure.AddAzureWebAppDiagnostics();
                configure.AddDebug();       //only here to for Console logging toflush quicker!?
            }
                )
                .Configure<LoggerFilterOptions>(
                    options => options.MinLevel = LogLevel.Information);


            // Register your dependencies
            services.AddSingleton<RuntimeBehaviorOptions>();
            services.AddSingleton<ILogger, Logger<BryanTestService>>();
            return services.BuildServiceProvider();
        }

        public static IConfiguration LoadConfiguration()
        {
            var environmentName = Environment.GetEnvironmentVariable("ENVIRONMENT");

            // using AppContext.BaseDirectory to set the base path,
            // ensuring it's more reliable across different deployment scenarios.
            var baseDirectory = AppContext.BaseDirectory;

            var builder = new ConfigurationBuilder()
                .SetBasePath(baseDirectory) // Use application base directory
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true);

            if (!string.IsNullOrEmpty(environmentName))
            {
                builder.AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true);
            }

            builder.AddEnvironmentVariables();

            return builder.Build();
        }

        private static void Svc_OnServiceInitialized(object sender, ServiceInitializedEventArgs e)
        {
            var snder = (ITopshelfClient)sender;

            //Console.WriteLine("Test....");

            snder.Logger.LogInformation("Bryan's service initialized... ");


            //throw new NotImplementedException();
        }

    }
}
