using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using P2E.SSO.API.Helpers;
using P2E.SSO.Domain.Repositories;
using P2E.SSO.Infra.Data.DataContext;
using P2E.SSO.Infra.Data.Repositories;
using Swashbuckle.AspNetCore.Swagger;
using System.Text;

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
                c.SwaggerDoc("v1", new Info { Title = "P2E [SSO-API]", Version = "v1" });
            });

            services.AddScoped<SSOContext, SSOContext>();
            services.AddTransient<IModuloRepository, ModuloRepository>();
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<IRotinaRepository, RotinaRepository>();
            services.AddTransient<IParceiroNegocioRepository, ParceiroNegocioRepository>();
            services.AddTransient<IRotinaRepository, RotinaRepository>();
            services.AddTransient<IRotinaAssociadaRepository, RotinaAssociadaRepository>();
            services.AddTransient<IParceiroNegocioRepository, ParceiroNegocioRepository>();
            services.AddTransient<IGrupoRepository, GrupoRepository>();
            services.AddTransient<IUsuarioModuloRepository, UsuarioModuloRepository>();
            services.AddTransient<IUsuarioGrupoRepository, UsuarioGrupoRepository>();
            services.AddTransient<IOperacaoRepository, OperacaoRepository>();
            services.AddTransient<IServicoRepository, ServicoRepository>();
            services.AddTransient<IUsuarioModuloRepository, UsuarioModuloRepository>();
            services.AddTransient<IUsuarioGrupoRepository, UsuarioGrupoRepository>();
            services.AddTransient<IOperacaoRepository, OperacaoRepository>();
            services.AddTransient<IParceiroNegocioModuloRepository, ParceiroNegocioModuloRepository>();
            services.AddTransient<IRotinaServicoRepository, RotinaServicoRepository>();
            services.AddTransient<IRotinaGrupoOperacaoRepository, RotinaGrupoOperacaoRepository>();
            services.AddTransient<IRotinaUsuarioOperacaoRepository, RotinaUsuarioOperacaoRepository>();


            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();


            //var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var key = Encoding.ASCII.GetBytes("r5u8x/A?D(G+KbPe");

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

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
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
