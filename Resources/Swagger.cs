using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace Resources
{
    public static class Swagger
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("20200518", new OpenApiInfo
                {
                    Title = "BusDepot (Resources)",
                    Version = "20200518",
                    Description = "Mikroserwis zarządzający zasobami zajezdni autobusowej",
                    Contact = new OpenApiContact
                    {
                        Name = "Jakub Sońta",
                        Email = string.Empty,
                        Url = new Uri("https://github.com/jsonta"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://github.com/jsonta/BusDepot_Part1/blob/master/LICENSE"),
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public static void UseCustomSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/20200518/swagger.json", "BusDepot (Resources)");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
