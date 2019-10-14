using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using P2E.Administrativo.Domain.Repositories;
using P2E.Administrativo.Infra.Data.DataContext;
using P2E.Administrativo.Infra.Data.Repositories;
using Swashbuckle.AspNetCore.Swagger;

namespace P2E.Administrativo.API
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
            services.AddMvc().ConfigureApiBehaviorOptions(o =>
            {
                o.InvalidModelStateResponseFactory = context =>
                {
                    var error = new
                    {
                        Detail = "Custom error"
                    };

                    return new BadRequestObjectResult(error);
                };
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "P2E [Administração-API]", Version = "v1" });
            });

            services.AddScoped<AdmContext, AdmContext>();
            services.AddTransient<IAgendaRepository, AgendaRepository>();
            services.AddTransient<IAgendaExecRepository, AgendaExecRepository>();
            services.AddTransient<IAgendaExecLogRepository, AgendaExecLogRepository>();
            services.AddTransient<IAgendaBotRepository, AgendaBotRepository>();
            services.AddTransient<IBotRepository, BotRepository>();
            services.AddTransient<IBotExecRepository, BotExecRepository>();
            services.AddTransient<IBotExecLogRepository, BotExecLogRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseMvc();
        }
    }
}
