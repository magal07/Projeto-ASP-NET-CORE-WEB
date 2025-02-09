﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Services;


namespace SalesWebMvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<SalesWebMvcContext>(options =>
                    options.UseMySql(Configuration.GetConnectionString("SalesWebMvcContext"), builder =>
                    builder.MigrationsAssembly("SalesWebMvc")));

                                    // Inject Dependency
            services.AddScoped<SeedingService>();  // 
            services.AddScoped<SellerService>(); // Vendedor
            services.AddScoped<DepartmentService>(); // Departamento
            services.AddScoped<SalesRecordService>(); // Vendas

            }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, SeedingService seedingService)
        {
            // Criando uma instância de CultureInfo para o idioma "en-US" (inglês dos EUA)
            var enUS = new CultureInfo("en-US");

            // Cria uma instância de RequestLocalizationOptions para configurar as opções de localização
            var localizationOptions = new RequestLocalizationOptions
            {
                // Define a cultura padrão da requisição como "en-US"
                DefaultRequestCulture = new RequestCulture(enUS),
                // Define as culturas suportadas para a aplicação (neste caso, apenas "en-US")
                SupportedCultures = new List<CultureInfo> { enUS },
                // Define as culturas de interface do usuário suportadas (neste caso, apenas "en-US")
                SupportedUICultures = new List<CultureInfo> { enUS }
            };

            // Aplica as configurações de localização à aplicação
            app.UseRequestLocalization(localizationOptions);


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                seedingService.Seed();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
