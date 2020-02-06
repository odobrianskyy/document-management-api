using DocumentManagement.API.Application;
using DocumentManagement.API.Application.Impl;
using DocumentManagement.API.Domain;
using DocumentManagement.API.Domain.Impl;
using DocumentManagement.API.Infrastructure;
using DocumentManagement.API.Infrastructure.Impl;
using DocumentManagement.API.Infrastructure.Impl.EfRepository;
using DocumentManagement.API.Presentation.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace DocumentManagement.API
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
            services.AddMvc(options =>
                {
                    options.Filters.Add(typeof(ValidationExceptionFilter));
                    options.Filters.Add(typeof(NotFoundExceptionFilter));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "Document Management API", Version = "v1" }));

            services.AddScoped<IDocumentsService, DocumentsService>();
            services.AddScoped<IDocumentsApplicationService, DocumentsApplicationService>();

            services.AddScoped<IIdGenerator, IdGenerator>();

            services.AddScoped<IBlobStorageService, BlobStorageService>();

            var connectionString = Configuration.GetConnectionString("SQL_SERVER_CONNECTION_STRING");
            services.AddDbContext<DocumentDbContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IDocumentRepository, EfDocumentRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Document Management API V1"));
            app.UseMvc();

            this.EnsureDatabaseCreated(app);
        }

        private void EnsureDatabaseCreated(IApplicationBuilder app)
        {
            var serviceProvider = app.ApplicationServices;
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<DocumentDbContext>();
                context.Database.EnsureCreated();
            }
        }
    }
}
