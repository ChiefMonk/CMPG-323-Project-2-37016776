using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Project2.Data;
using Project2.WebAPI.Services.Category;
using Project2.WebAPI.Session;
using System.IO;
using System.Reflection;
using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Project2.Data.Entities;
using Project2.WebAPI.Services.Security;
using Project2.WebAPI.Middleware;

namespace Project2.WebAPI
{
	/// <summary>
	/// 
	/// </summary>
	public class Startup
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Startup" /> class.
		/// </summary>
		/// <param name="environment">The environment.</param>
		public Startup(IWebHostEnvironment environment)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(environment.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

			builder.AddEnvironmentVariables();

			Configuration = builder.Build();
		}

		/// <summary>
		/// Gets the configuration.
		/// </summary>
		/// <value>
		/// The configuration.
		/// </value>
		public IConfiguration Configuration { get; }


		/// <summary>
		/// Configures the services.
		/// </summary>
		/// <param name="services">The services.</param>
		public void ConfigureServices(IServiceCollection services)
		{
			var webApiSettings = Configuration.Get<WebApiSettings>();

			services.AddControllers();

			//register services
			services.AddSingleton<IWebApiSettings>(webApiSettings);
			services.AddScoped<IUserSession, UserSession>();
			services.AddScoped<ISecurityService, SecurityService>();
			services.AddScoped<ICategoriesService, CategoriesService>();

			//register database context
			services.AddDbContext<ConnectedOfficeDbContext>(options =>
			{
				options.UseSqlServer(webApiSettings.SqlServerConnection,
					sqlServerOptionsAction: sqlOptions => { sqlOptions.EnableRetryOnFailure(maxRetryCount: 3); });
				options.EnableDetailedErrors(true);

			});

			// register Identity
			services.AddIdentity<EntitySystemUser, IdentityRole>()
				.AddEntityFrameworkStores<ConnectedOfficeDbContext>()
				.AddDefaultTokenProviders();

			// register Authentication
			services.AddAuthentication(options =>
			{
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(o =>
			{
				o.SaveToken = true;
				o.RequireHttpsMetadata = false;
				o.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateAudience = true,
					ValidAudience = webApiSettings.JwtAudience,

					ValidateIssuer = true,
					ValidIssuer = webApiSettings.JwtIssuer,

					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(webApiSettings.JwtSecret))
				};
			});

			services.AddAuthorization();

			// add swagger documentation
			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v2", new OpenApiInfo
				{
					Title = "Project 2 WebAPI",
					Version = "v2",
					Description = "The CMPG323 Project 2 WebAPI",
					Contact = new OpenApiContact
					{
						Name = "Chipo Hamayobe - 37016776",
						Url = new Uri("https://www.linkedin.com/in/chipo-hamayobe-459107247/")
					},

				});
				
				var jwtSecurityScheme = new OpenApiSecurityScheme
				{
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
					In = ParameterLocation.Header,
					Scheme = JwtBearerDefaults.AuthenticationScheme,
					BearerFormat = "JWT",
					//Description = "**_ONLY_** enter your JWT token in the textbox below...",
					Description = "JWT Auth header using the Bearer scheme.\r\n\r\n Enter 'Bearer {Your_JWT_Token}'",

					Reference = new OpenApiReference
					{
						Id = JwtBearerDefaults.AuthenticationScheme,
						Type = ReferenceType.SecurityScheme
					}
				};

				options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
				options.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{ jwtSecurityScheme, Array.Empty<string>() }
				});


				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				options.IncludeXmlComments(xmlPath);
			});
		}


		/// <summary>
		/// Configures the specified application.
		/// </summary>
		/// <param name="app">The application.</param>
		/// <param name="env">The env.</param>
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			//app.UseHttpsRedirection();
			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();
			app.UseCustomSessionMiddleware();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			app.UseSwagger(); 
			app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v2/swagger.json", "Project 2 WebAPI"));
		}
	}
}
