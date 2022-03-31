// See https://aka.ms/new-console-template for more information

using ImageImporter.Application.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


IConfiguration Configuration = null;

Host.CreateDefaultBuilder()
        .ConfigureAppConfiguration(Configure)
        .ConfigureServices(ConfigureServices)
        .ConfigureServices(services => services.AddSingleton<Application>())
        .Build()
        .Services
        .GetService<Application>()?
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

