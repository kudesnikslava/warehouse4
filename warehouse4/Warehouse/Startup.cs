using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.XPath;
using AutoMapper;
using CommonLibrary.Models;
using CommonLibrary.Repositories;
using CommonLibrary.Repositories.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Warehouse.Models;
using Warehouse.Models.Requests;
using Warehouse.Models.Responses;

namespace Warehouse
{
    public class Startup
    {
	    private readonly string ApiName = "Warehouse";

	    private readonly IHostingEnvironment _hostingEnv;

	    /// <summary>
	    /// Constructor
	    /// </summary>
	    /// <param name="configuration"></param>
	    /// <param name="hostingEnv"></param>
	    public Startup( IHostingEnvironment hostingEnv)
	    {
		    _hostingEnv = hostingEnv;
	    }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			
	        services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddJsonOptions(opts =>
	        {
		        opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
	        });

			services.AddSwaggerGen(options => { SetupSwagger(options, ApiName, _hostingEnv); });

	        Mapper.Initialize(c =>
	        {
		        c.CreateMissingTypeMaps = true;
		        c.CreateMap<Customer, CustomerResponse>();
		        c.CreateMap<CustomerCreateRequest, Customer>().ForMember(m => m.Id, expression => expression.Ignore());
		        c.CreateMap<Entity, EntityResponse>();
		        c.CreateMap<EntityCreateRequest, Entity>().ForMember(m => m.Id, expression => expression.Ignore());
				//TODO Почему здесь нет CustomerUpdateRequest. Маппинг в контроллере есть в методе update
			});

			services.AddSingleton(typeof(IDiskCache), typeof(RedisDiskCache));
	        services.AddSingleton<CustomersRepository>();
			services.AddSingleton<EntitiesRepository>();
			services.AddSingleton<DataManager>();
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

			//app.Run(async (context) =>
			//{
			//	await context.Response.WriteAsync("Hello World!");
			//});

	        app.UseHttpsRedirection();
	        app.UseMvc();

			app.UseSwagger();
	        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", ApiName));
		}

	    private void SetupSwagger(SwaggerGenOptions options, string apiName, IHostingEnvironment hostingEnv)
	    {
		    options.SwaggerDoc("v1", new Info
		    {
			    Title = apiName,
			    Description = $"{apiName} (ASP.NET Core 2.2)",
			    Version = "1.0.BUILD_NUMBER"
		    });

		    options.DescribeAllEnumsAsStrings();

		    var comments =
			    new XPathDocument($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{hostingEnv.ApplicationName}.xml");
		    options.OperationFilter<XmlCommentsOperationFilter>(comments);

		    //options.AddSecurityDefinition("Bearer", new ApiKeyScheme()
		    //{
			   // Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
			   // Name = "Authorization",
			   // In = "header",
			   // Type = "apiKey"
		    //});

		    options.IgnoreObsoleteActions();
		}
	}
}
