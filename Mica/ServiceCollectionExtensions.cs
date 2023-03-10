using Microsoft.Extensions.DependencyInjection;
using System.Collections.Specialized;
using System.Net;

namespace Mica
{


    public static class ServiceCollectionExtensions
    {
        public static void AddMicaScheduler(this IServiceCollection services, Action<MicaOptionsBuilder> options = null)
        {
            var builder = new MicaOptionsBuilder(services);
            options?.Invoke(builder);
            services.AddSingleton<MicaScheduler>();
            services.Configure<MicaOptions>(o => { 
            
            });
            
        }
    }
}
