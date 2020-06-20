using System.Reflection;
using AspNetCore.Firebase.Authentication.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using TheGramPost.Helpers;
using TheGramProfile.Helpers;
using TheGramProfile.Repository;

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
            
            services.AddScoped<IUserContextHelper, UserContextHelper>();
            services.AddHttpContextAccessor();
            services.AddDbContext<ProfileContext>(builder =>
            {
                builder.UseMySQL(Configuration.GetConnectionString("ProfileContext"));
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
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}