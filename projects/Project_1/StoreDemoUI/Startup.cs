using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoStoreBusinessLayer;
using DemoStoreBusinessLayer.Interfaces;
using DemoStoreDbContext.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace StoreDemoUI
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

      services.AddControllers();
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "StoreDemoUI", Version = "v1" });
      });


      services.AddDbContext<Demo_08162021batchContext>(options =>
        {
          //if db options is already configured, done do anything..
          // otherwise use the Connection string I have in secrets.json
        //if (!options.IsConfigured)
        //{
            options.UseSqlServer("server=(localdb)\\MSSQLLocalDB;database=Demo_08162021batch;Trusted_Connection=true;");
        //}
        });


      //registering classes with the DI system.
      services.AddScoped<ICustomerRepository, CustomerRepository>();
      services.AddScoped<IModelMapper, ModelMapper>();

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StoreDemoUI v1"));
      }
      app.UseStatusCodePages();

      app.UseHttpsRedirection();

      app.UseRewriter(new RewriteOptions()
          .AddRedirect("^$", "index.html"));

      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}

