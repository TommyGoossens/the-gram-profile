using System.Reflection;
using AspNetCore.Firebase.Authentication.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using TheGramPost.EventBus;
using TheGramPost.Helpers;
using TheGramProfile.Helpers;
using TheGramProfile.Repository;
using TheGramProfile.Services;

namespace TheGramProfile
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
            services.AddFirebaseAuthentication(Configuration["Firebase:Issuer"], Configuration["Firebase:Audience"]);

            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddMemoryCache();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IUserContextHelper, UserContextHelper>();
            services.AddHttpContextAccessor();
            services.AddSingleton<RabbitMqPersistentConn>();
            services.AddDbContext<ProfileContext>(builder =>
            {
                builder.UseInMemoryDatabase(Configuration.GetConnectionString("ProfileContext"));
            });
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRabbitListener();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }


    public static class ApplicationBuilderExtensions
    {
        public static RabbitMqPersistentConn Listener { get; set; }

        public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
        {
            Listener = app.ApplicationServices.GetService<RabbitMqPersistentConn>();
            var life = app.ApplicationServices.GetService<IHostApplicationLifetime>();
            life.ApplicationStarted.Register(OnStarted);
            life.ApplicationStopping.Register(OnStopping);
            return app;
        }

        private static void OnStarted()
        {
            // Listener.CreateConsumerChannel();
        }

        private static void OnStopping()
        {
            Listener.Disconnect();
        }
    }
}