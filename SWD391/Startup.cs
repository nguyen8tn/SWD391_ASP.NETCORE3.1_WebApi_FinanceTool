using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SWD391.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Builder;
using SWD391.Models;
using Microsoft.OData.Edm;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNet.OData.Formatter;
using System.Reflection;
using System.IO;
using static SWD391.Service.IAppServices;
using static SWD391.Service.AppServices;
using static SWD391.Models.EnumUtils;
using Microsoft.IdentityModel.Logging;
using System.Net;

namespace SWD391
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("http://financial-web-service.azurewebsites.net",
                                            "https://localhost:5001",
                                            "http://localhost:5000",
                                            "http://localhost:3000")
                                            .AllowAnyOrigin().AllowAnyHeader()
                                            .AllowAnyMethod();
                                  });
            });
            //Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Authority = "https://securetoken.google.com/swd391-d8680";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = "https://securetoken.google.com/swd391-d8680",
                    ValidateAudience = true,
                    ValidAudience = "swd391-d8680",
                    ValidateLifetime = true
                };
            });
            IdentityModelEventSource.ShowPII = true;
            //services.AddApiVersioning(options => options.RegisterMiddleware = false);
            services.AddControllers().AddNewtonsoftJson();

            services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddDbContext<SWD391Context>(options =>
                    options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                        .UseSqlServer(DBConnection.Deploy.ConStr));
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<CustomSwaggerAttribute>();

                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "mobile API", Version = "v1" });
            });


            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddOData();
            services.AddMvcCore(options =>
            {
                foreach (var outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
                foreach (var inputFormatter in options.InputFormatters.OfType<ODataInputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
            });
            //--------------------
            services.AddScoped(typeof(IBankService), typeof(BankService));
            //--------------------------------
            services.AddScoped(typeof(ITransactionService), typeof(TransactionService));
            //--------------------------------------
            services.AddScoped(typeof(IUserService), typeof(UserService));
            services.AddControllers();
            //------------------------------------
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls12;
            app.UseCors(MyAllowSpecificOrigins);
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            //app.UseApiVersioning();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });


            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.EnableDependencyInjection();
                endpoints.Select().Filter().OrderBy().Count().MaxTop(100);
                endpoints.MapODataRoute("api", "api", GetEdmModel());
            });

        }
        IEdmModel GetEdmModel()
        {
            var odataBuilder = new ODataConventionModelBuilder();
            odataBuilder.EntitySet<Bank>("Banks");
            odataBuilder.EntitySet<User>("Users");
            return odataBuilder.GetEdmModel();
        }
    }
}
