using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Project2.Data;
using Project2.WebAPI.Services.Category;
using Project2.WebAPI.Session;

namespace Project2.WebAPI
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
			services.AddScoped<IUserSession, UserSession>();
			services.AddScoped<ICategoriesService, CategoriesService>();

			services.AddDbContext<ConnectedOfficeDbContext>(options =>
			{
				options.UseSqlServer("name=ConnectionStrings:SqlServer",
					sqlServerOptionsAction: sqlOptions => { sqlOptions.EnableRetryOnFailure(maxRetryCount: 3); });
				options.EnableDetailedErrors(true);

			});

			services.AddSwaggerGen(options => { options.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Project 2 WebAPI", Version = "v2", Description = "The CMPG323 Project 2 WebAPI by 37016776", }); });
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			app.UseSwagger(); app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v2/swagger.json", "Project 2 WebAPI"));
		}
	}
}
