using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HipercoreASPNETCORE.Interfaces;
using HipercoreASPNETCORE.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace HipercoreASPNETCORE
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        //----------------------Metodo de definicion de inyeccion de dependecias de servicios
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            //----inyeccion de dependecia del servicio de acceso a la BD cuando cualquier controlador lo solicite ------
            services.AddScoped<IServicioAccesoBD, ServicioaccesoDBSQLServer>();

            //----Inyeccion del servicio de envio de emails
            services.AddScoped<IServicioEnvioEmail, ServicioEnvioDeEmails>();

            //--------configuracion de seviocio de validacion de JWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                                    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters {
                                                                                                                                    ValidateIssuer = true,          //emisor 
                                                                                                                                    ValidateAudience= true,         //receptor
                                                                                                                                    ValidateLifetime=true,          //tiempo de vida del token
                                                                                                                                    ValidateIssuerSigningKey=true,  //se valida por key el token ? ...si
                                                                                                                                    ValidIssuer=" ",    //<----meter en variables de entorno
                                                                                                                                    ValidAudience=" ",
                                                                                                                                    IssuerSigningKey = new SymmetricSecurityKey(            //<---configuracion de la key
                                                                                                                                                                                 Encoding.UTF8.GetBytes(Configuration["JWT_Security_Key"])
                                                                                                                                                                                 ),
                                                                                                                                                                                 ClockSkew= TimeSpan.Zero //se ajusta a 0 para eviar ajustes temporales del token si expira
                                                                                                                                }
                                                   ); //fin de configurcion de JWT

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseAuthentication(); //<----
        }
    }
}
