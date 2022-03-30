// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Hello, World!");

List<string> files = new List<string> { @"C:\Users\bas\Desktop\testje\BL 1.jpg",
                                                @"C:\Users\bas\Desktop\testje\BL 2.jpg",
                                                @"C:\Users\bas\Desktop\testje\WA 1.jpeg" };

IConfiguration Configuration = null;

Host.CreateDefaultBuilder()
        .ConfigureAppConfiguration(Configure)
        .ConfigureServices(ConfigureServices)
        .ConfigureServices(services => services.AddSingleton<Scheduler>())
        .Build()
        .Services
        .GetService<Scheduler>()?
        .Execute();


void Configure(HostBuilderContext hostContext, IConfigurationBuilder builder)
{
    builder.AddJsonFile("appsettings.development.json");
    Configuration = builder.Build();
}

void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
{
    AppSettings settings = new AppSettings();
    Configuration.Bind(settings);
    services.AddSingleton(settings);


    services.AddDbContext<AppDBContext>(options=>
        options
        .UseMySQL(settings.ConnectionString)
        .UseLazyLoadingProxies());

}

