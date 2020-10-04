using DigiPet.Web.Api.Application.Factory;
using DigiPet.Web.Api.Configs;
using DigiPet.Web.Api.Infrastructure.Messaging.Base;
using DigiPet.Web.Api.Infrastructure.Messaging.Contracts;
using DigiPet.Web.Api.Infrastructure.Persistence.Contracts;
using DigiPet.Web.Api.Infrastructure.Persistence.InMemory;
using DigiPet.Web.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace DigiPet.Web.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();
            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto);

            services.Configure<AnimalOptions>(Configuration.GetSection(AnimalOptions.ConfigSection));
            services.Configure<GameOptions>(Configuration.GetSection(GameOptions.ConfigSection));

            services.AddHostedService<HostedAdoptionHandler>();
            services.AddHostedService<HostedGameServer>();

            services.AddSingleton<IUserRepository, InMemoryUserRepository>();
            services.AddSingleton<IUniqueIdProvider, UniqueIdProvider>();
            services.AddSingleton<IMemoryCache, MemoryCache>();
            services.AddSingleton<IHub, Hub>();
            services.AddTransient<IAnimalFactory, AnimalFactory>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/api/health");
                endpoints.MapControllers();
            });
        }
    }
}
