using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using P2E.SSO.Domain.Repositories;
using P2E.SSO.Infra.Data.DataContext;
using P2E.SSO.Infra.Data.Repositories;
using Swashbuckle.AspNetCore.Swagger;

namespace P2E.SSO.API
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
            services.AddAutoMapper();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "P2E [SSO-API]", Version = "v1" });
            });

            services.AddScoped<SSOContext, SSOContext>();
            services.AddTransient<IModuloRepository, ModuloRepository>();
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<IRotinaRepository, RotinaRepository>();
            services.AddTransient<IParceiroNegocioRepository, ParceiroNegocioRepository>();
            services.AddTransient<IRotinaRepository, RotinaRepository>();
            services.AddTransient<IParceiroNegocioRepository, ParceiroNegocioRepository>();            
            services.AddTransient<IGrupoRepository, GrupoRepository>();
            services.AddTransient<IUsuarioModuloRepository, UsuarioModuloRepository>();
            services.AddTransient<IUsuarioGrupoRepository, UsuarioGrupoRepository>();
            services.AddTransient<IOperacaoRepository, OperacaoRepository>();
            services.AddTransient<IServicoRepository, ServicoRepository>();
            services.AddTransient<IUsuarioModuloRepository, UsuarioModuloRepository>();
            services.AddTransient<IUsuarioGrupoRepository, UsuarioGrupoRepository>();
            services.AddTransient<IOperacaoRepository, OperacaoRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
