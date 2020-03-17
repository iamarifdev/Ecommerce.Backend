using System.Collections.Generic;
using System.Text.Json;
using AutoMapper;
using Ecommerce.Backend.API.AutoMappingProfiles;
using Ecommerce.Backend.API.Helpers;
using Ecommerce.Backend.API.Middlewares;
using Ecommerce.Backend.API.Validators;
using Ecommerce.Backend.Common.Configurations;
using Ecommerce.Backend.Common.Helpers;
using Ecommerce.Backend.Services.Abstractions;
using Ecommerce.Backend.Services.Implementations;
using FluentValidation.AspNetCore;
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
      services.Configure<ApiConfig>(Configuration.GetSection(nameof(ApiConfig)));
      services.Configure<JwtConfig>(Configuration.GetSection(nameof(JwtConfig)));

      services.AddSingleton<IDatabaseSetting>(s => s.GetRequiredService<IOptions<DatabaseSetting>>().Value);
      services.AddSingleton<IApiConfig>(s => s.GetRequiredService<IOptions<ApiConfig>>().Value);
      services.AddSingleton<IJwtConfig>(s => s.GetRequiredService<IOptions<JwtConfig>>().Value);

      var dbSetting = Configuration.GetSection(nameof(DatabaseSetting)).Get<DatabaseSetting>();
      services.InitiateDbConnection(dbSetting);
      services.RegisterAllIndex();

      // add services here
      services.AddScoped<EcommerceHttpClient>();
      services.AddScoped<IProductService, ProductService>();
      services.AddScoped<ICartService, CartService>();
      services.AddScoped<IShippingMethodService, ShippingMethodService>();
      services.AddScoped<IPaymentMethodService, PaymentMethodService>();
      // services.AddScoped<AuthService>();

      services.AddCors();
      services.AddControllers();
      services.AddResponseCompression();
      services.AddAutoMapper(config => config.AddProfile(new ProductMappingProfile()), typeof(Startup));
      services.AddAutoMapper(config => config.AddProfile(new CartMappingProfile()), typeof(Startup));
      services.AddAutoMapper(config => config.AddProfile(new ShippingMethodMappingProfile()), typeof(Startup));
      services.AddAutoMapper(config => config.AddProfile(new PaymentMethodMappingProfile()), typeof(Startup));
      // services.AddDbContext<TokenStoreDbContext>(options => options.UseSqlite("Filename=./tokenstore.db"));

      services.AddMvc(Options => Options.Filters.Add(new ValidateModelStateAttribute()))
        .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AddCartProductValidator>())
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
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseHsts();
      }
      app.UseHttpsRedirection();
      app.UseCors(b => b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
      // app.UseStaticFiles();
      // app.UseStaticFiles(new StaticFileOptions()
      // {
      //   FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"uploads")),
      //     RequestPath = new PathString("/assets")
      // });
      app.UseRouting();
      app.UseResponseCompression();
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