using common.sismo.interfaces.repositories;
using common.sismo.interfaces.services;
using data.sismo;
using data.sismo.models;
using data.sismo.repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using services.sismo.services;

namespace api.mylayers
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public string MyAllowedPolicy = "_myAllowedSpecificOrigin";
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen();
            services.AddSingleton<IProjectRepository, ProjectRepository>();
            services.AddSingleton<IProjectService, ProjectService>();
            services.AddSingleton<ISurveyRepository, SurveyRepository>();
            services.AddSingleton<ISurveyService, SurveyService>();
            services.AddSingleton<IOperationalFrontRepository, OperationalFrontRepository>();
            services.AddSingleton<IOperationalFrontService, OperationalFrontService>();

            services.AddDbContextFactory<MyLayerContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("MyConnection"), x => x.UseNetTopologySuite());
            });

            services.AddCors(o => o.AddPolicy(MyAllowedPolicy, buider =>
               { buider.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }
            ));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            DefaultFilesOptions options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(options);
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseHttpsRedirection();
            app.UseCors(MyAllowedPolicy);
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
