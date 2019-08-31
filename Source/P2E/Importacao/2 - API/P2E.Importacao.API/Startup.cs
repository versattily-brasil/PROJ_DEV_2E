using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using P2E.Importacao.Domain.Repositories;
using P2E.Importacao.Infra.Data.Repository;
using P2E.Importacao.Infra.Data.DataContext;
using Swashbuckle.AspNetCore.Swagger;

namespace P2E.Importacao.API
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

            services.AddAutoMapper();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "P2E [Importação-API]", Version = "v1" });
            });

            services.AddScoped<ImportacaoContext, ImportacaoContext>();
            services.AddTransient<IImportacaoRepository, ImportacaoRepository>();
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

        //public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        //{
        //    if (env.IsDevelopment())
        //    {
        //        app.UseDeveloperExceptionPage();
        //    }
        //    else
        //    {
        //        app.UseHsts();
        //    }

        //    app.UseHttpsRedirection();


        //    // Enable middleware to serve generated Swagger as a JSON endpoint.
        //    app.UseSwagger();

        //    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
        //    // specifying the Swagger JSON endpoint.
        //    app.UseSwaggerUI(c =>
        //    {
        //        c.SwaggerEndpoint("/swagger/v1/swagger.json", "P2E Importação - API V1");
        //        c.RoutePrefix = "api/docs";
        //        c.DocumentTitle = "P2E Importação - API";
        //    });

        //    app.UseMvc();

        //}
    }
}
