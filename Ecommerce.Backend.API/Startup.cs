using System.Collections.Generic;
using System.Text.Json;
using Ecommerce.Backend.API.Helpers;
using Ecommerce.Backend.API.Middlewares;
using Ecommerce.Backend.Common.Configurations;
using Ecommerce.Backend.Services.Abstractions;
using Ecommerce.Backend.Services.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace Ecommerce.Backend.API
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
      services.Configure<ApiBehaviorOptions>(Options => Options.SuppressModelStateInvalidFilter = true);
      services.Configure<DatabaseSetting>(Configuration.GetSection(nameof(DatabaseSetting)));
      services.Configure<JwtConfig>(Configuration.GetSection(nameof(JwtConfig)));

      services.AddSingleton<IDatabaseSetting>(s => s.GetRequiredService<IOptions<DatabaseSetting>>().Value);
      services.AddSingleton<IJwtConfig>(s => s.GetRequiredService<IOptions<JwtConfig>>().Value);

      var dbSetting = Configuration.GetSection(nameof(DatabaseSetting)).Get<DatabaseSetting>();
      services.InitiateDbConnection(dbSetting);
      services.RegisterAllIndex();

      // add services here
      services.AddScoped<IProductService, ProductService>();
      // services.AddScoped<IUserService, UserService>();
      // services.AddScoped<AuthService>();

      services.AddCors();
      services.AddControllers();

      // services.AddAutoMapper(config => config.AddProfile(new UserMappingProfile()), typeof(Startup));
      // services.AddDbContext<TokenStoreDbContext>(options => options.UseSqlite("Filename=./tokenstore.db"));

      services.AddMvc(Options => Options.Filters.Add(new ValidateModelStateAttribute()))
        .AddJsonOptions(options =>
        {
          options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
          options.JsonSerializerOptions.DictionaryKeyPolicy = null;
          options.JsonSerializerOptions.IgnoreNullValues = true;
        });

      // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      //   .AddJwtBearer(options =>
      //   {
      //     options.TokenValidationParameters = new TokenValidationParameters
      //     {
      //     ValidateIssuerSigningKey = true,
      //     IssuerSigningKey = new SymmetricSecurityKey(
      //     Encoding.UTF8.GetBytes(
      //     Configuration.GetSection("JwtConfig:AccessTokenSecretKey").Value
      //     )
      //     ),
      //     ValidateAudience = false,
      //     ValidateIssuer = false,
      //     RequireExpirationTime = true,
      //     ValidateLifetime = true
      //     };
      //   });

      // services.AddAuthorization(options =>
      // {
      //   var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);
      //   defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
      //   options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
      // });

      services.AddSwaggerGen(config =>
      {
        config.SwaggerDoc("v1", new OpenApiInfo { Title = "Ecommerce Backend API Documentations", Version = "v1" });
        config.AddSecurityDefinition("Bearer",
          new OpenApiSecurityScheme
          {
            Description =
              "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
              Name = "Authorization",
              In = ParameterLocation.Header,
              Type = SecuritySchemeType.ApiKey,
              Scheme = "Bearer"
          });
        config.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
          {
            new OpenApiSecurityScheme
            {
              Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
          }
        });
        // c.CustomSchemaIds(x => x.FullName);
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.UseForwardedHeaders(new ForwardedHeadersOptions //needed for nginx reverse proxy
        {
          ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });
      if (env.IsDevelopment())
      {
        app.UseCors(b => b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseRouting();
      // app.UseAuthentication();
      // app.UseAuthorization();
      app.UseSwagger();
      app.UseSwaggerUI(config =>
      {
        config.SwaggerEndpoint("/swagger/v1/swagger.json", "Ecommerce Backend API Documentations v1");
        config.RoutePrefix = string.Empty;
      });
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}