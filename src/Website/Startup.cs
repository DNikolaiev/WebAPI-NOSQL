using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;
using DataAccess.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SharpCompress.Archives.SevenZip;
using Website.Models;
using Settings = DataAccess.Models.Settings;

namespace Website
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
            services.AddControllersWithViews();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("Redis");
                options.InstanceName = "WebApp";
            });

            services.Configure<Settings>(
                options =>
                {
                    options.ConnectionString = Configuration.GetSection("MongoDb:ConnectionString").Value;
                    options.Database = Configuration.GetSection("MongoDb:CatalogDatabase").Value;
                    options.Container = Configuration.GetSection("MongoDb:Container").Value;
                    options.IsContained = Configuration["DOTNET_RUNNING_IN_CONTAINER"] != null;
                });
            services.AddSingleton<IDatabaseSettings>(sp => sp.GetRequiredService<IOptions<Settings>>().Value);
            services.AddScoped<ICatalog<Product>,ProductCatalog>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog API", Version = "v1" });
            });

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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog API V1");
            });
            
        }
    }
}

