using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PetrovichBot.Database;
using PetrovichBot.Extensions;
using PetrovichBot.Services;
using PetrovichBot.Services.Interfaces;
using Serilog;
using Serilog.Events;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace PetrovichBot
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);

            CreateGlobalLoggerConfiguration();
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "config.json"))
            {
                Log.Warning("No config file found. Attempt to create new one...");
                CreateDefaultConfig();
                Log.Warning("New config has been created");
                Log.Fatal("Insert TokenBot and other necessary parametres to the config.json");
                Log.Warning($"Path to the config is: {AppDomain.CurrentDomain.BaseDirectory}config.json");
                return;
            }
            Log.Information("Starting host");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = new HostBuilder()
                  .ConfigureAppConfiguration(x =>
                  {
                      var conf = new ConfigurationBuilder()
                               .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                               .AddJsonFile("config.json", false, true)
                               .Build();

                      x.AddConfiguration(conf);
                  })
                  .UseSerilog()
                  .ConfigureServices((context, services) =>
                  {
                      services.AddHostedService<TelegramBotHostedService>();
                      services.AddTransient<IUpdateHandler, UpdateHandler>();
                      services.AddTransient<ITelegramBotClient>(_ => new TelegramBotClient(context.Configuration.GetSection(nameof(EnvsSettings))["TokenBot"]));

                      services.AddTransient<IApplicationServices, ApplicationServices>();
                      services.Configure<EnvsSettings>(context.Configuration.GetSection(nameof(EnvsSettings)));
                      services.AddTransient<IEnvsSettings>(sp => sp.GetRequiredService<IOptions<EnvsSettings>>().Value);


                      services.AddLocalization(options => options.ResourcesPath = "Resources");
                  });

            return builder;
        }
        private static void CreateDefaultConfig()
        {
            GlobalConfig globalConfig = new GlobalConfig()
            {
                EnvsSettings = new EnvsSettings()
                {
                    Admins              = new List<string>(),
                    ChatsToDevNotify    = new List<string>(),
                    DevNotifyEvery      = TimeSpan.FromMinutes(1),
                    DevExtraNotifyEvery = TimeSpan.FromMinutes(5),
                    TriggersEvery       = TimeSpan.FromSeconds(20),
                    BotstatApiKey       = "",
                    TokenBot            = "",
                    ConnectionString    = ""
                },
            };

            var configJSON = JsonConvert.SerializeObject(globalConfig);

            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "config.json", configJSON);
        }
        public static void CreateGlobalLoggerConfiguration()
        {
            string pathToLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                            "Logs",
                                            DateTime.Now.ToString("yyyy"),
                                            DateTime.Now.ToString("MM"),
                                            $"{DateTime.Now:dd}-.txt");

            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .WriteTo.File(pathToLog,
                          rollingInterval: RollingInterval.Day,
                          retainedFileCountLimit: null,
                          outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.Console(LogEventLevel.Debug)
            .CreateLogger();

            Log.Warning("Path to logs: " + pathToLog);
        }


        static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception) args.ExceptionObject;
            Log.Fatal(e, "MyHandler caught!");
        }

    }
}
