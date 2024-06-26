using Refit;
using hackernewsapi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using AutoMapper;
using hackernewsapi.Profiles;
using System.Diagnostics.CodeAnalysis;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using hackernewsapi.Interfaces;

namespace hackernewsapi
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IHackerNewsService, HackerNewsService>();
            services.AddRefitClient<IHackerNews>().ConfigureHttpClient(
                c => c.BaseAddress = new Uri(Configuration["HackerNewsBaseApi"])
            );
            services.AddAutoMapper(typeof(HackerNewsMapper));
            services.AddSwaggerGen(c => {
                c.SwaggerDoc(
                    "v1", new OpenApiInfo{
                    Version = Configuration["ApiInfo:Version"],
                    Title = Configuration["ApiInfo:Title"],
                    Description = Configuration["ApiInfo:Description"],
                    Contact = new OpenApiContact{
                        Name = Configuration["ApiInfo:Contact:Name"],
                        Email = Configuration["ApiInfo:Contact:Email"],
                        Url = new Uri(Configuration["ApiInfo:Contact:Url"])
                    }
                });
            
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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hacker_News API");
                c.RoutePrefix = String.Empty;
            });
        }
    }
}
