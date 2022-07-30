using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Reflection;

namespace Notes.WebApi
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                var apiVersion = description.ApiVersion.ToString();
                options.SwaggerDoc(description.GroupName, new OpenApiInfo
                {
                    Version = apiVersion,
                    Title = $"Notes Api {apiVersion}",
                    Description = "A simple Asp Net Core Web Api.",
                    TermsOfService = new Uri("https://revengston.com"),
                    Contact = new OpenApiContact
                    {
                        Name = "IgMo",
                        Email = "igmo71@mail.ru"
                    },
                    License = new OpenApiLicense
                    {
                        Name = "IgMo License",
                        Url = new Uri("https://revengston.com")
                    }
                });

                options.AddSecurityDefinition($"AuthToken {apiVersion}",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "bearer",
                        Name = "Authorization",
                        Description = "Authorization token"
                    });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = $"AuthToken {apiVersion}"
                            }
                    },
                    new string[] {}
                    }
                });

                options.CustomOperationIds(apiDescription =>                
                    apiDescription.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null );
            }
        }
    }
}
