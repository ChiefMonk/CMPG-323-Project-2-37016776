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

namespace Project2.WebAPI
{
	/// <summary>
	/// 
	/// </summary>
	public class Startup
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Startup"/> class.
		/// </summary>
		/// <param name="configuration">The configuration.</param>
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
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
				options.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo
				{
					Title = "Project 2 WebAPI",
					Version = "v2",
					Description = "The CMPG323 Project 2 WebAPI",
					Contact = new OpenApiContact
					{
						Name = "Chipo Hamayobe - 37016776",
						//Email = "chipo@outlook.com",
						Url = new Uri("https://www.linkedin.com/in/chipo-hamayobe-459107247/")
					},

				});

				var jwtSecurityScheme = new OpenApiSecurityScheme
				{
					Name = "Authorization",
					Scheme = "bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http,
					Description = "**_ONLY_** enter your JWT token in the textbox below...",

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

			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			app.UseSwagger(); 
			app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v2/swagger.json", "Project 2 WebAPI"));
		}
	}
}
