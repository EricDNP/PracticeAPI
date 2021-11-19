using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PracticeAPI.Data;
using PracticeAPI.Models;
using PracticeAPI.Repositories;
using PracticeAPI.Services;

namespace PracticeAPI
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
            services.AddScoped<iUnitOfWork, UnitOfWork>();
            services.AddScoped<UtilityRepository, UtilityRepository>();
            services.AddScoped<iService<Product>, ProductService>();
            services.AddScoped<iService<PrivateClient>, ClientService<PrivateClient>>();
            services.AddScoped<iService<PublicClient>, ClientService<PublicClient>>();
            services.AddScoped<iService<ParentA>, ParentService<ParentA>>();
            services.AddScoped<iService<ParentB>, ParentService<ParentB>>();
            services.AddScoped<iService<ChildA>, ChildService<ChildA>>();
            services.AddScoped<iService<ChildB>, ChildService<ChildB>>();
            services.AddTransient(typeof(iRepository<>), typeof(GenericRepository<>));
            services.AddDbContext<ApiContext>(opt =>
            {
                var conString = Configuration.GetConnectionString("SqlServer");
                opt.UseSqlServer(conString);
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PracticeAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PracticeAPI v1"));
            }

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
