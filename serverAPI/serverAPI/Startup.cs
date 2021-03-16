using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using serverAPI.Database;
using System;
using System.IO;
using System.Reflection;
using Microsoft.OpenApi.Models;

namespace serverAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("Database");
            services.AddTransient<IUserRepository, UserRepository>(provider => new UserRepository(connectionString));
            services.AddTransient<IShopRepository, ShopRepository>(provider => new ShopRepository(connectionString));
            services.AddTransient<IProductRepository, ProductRepository>(provider => new ProductRepository(connectionString));
            services.AddTransient<IPurchaseRepository, PurchaseRepository>(provider => new PurchaseRepository(connectionString));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "serverAPI API",
                    Description = "Backend API for level 1 task\nfrom RTU IT LAB",
                    Contact = new OpenApiContact
                    {
                        Name = "Maxim Soluyanov",
                        Email = string.Empty,
                        Url = new Uri("https://vk.com/maxide201"),
                    },
                });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
