using AsZero.Traces.Core;
using AsZero.Traces.DAL;
using AsZero.Traces.WebApi;
using AsZero.WebApi.DataConfigs;
using AsZero.WebApi.Models.DBModels;
using MediatR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using StdUnit.One;
using StdUnit.One.DAL;
using StdUnit.One.FuncResources;
using StdUnit.One.WebApi.Auth;
using StdUnit.Outbox.Core;
using StdUnit.Outbox.WebApi;
using StdUnit.Reviews.WebApi;
using StdUnit.Zero.DAL;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;

namespace AsZero.WebApi
{
    public class Startup
    {
        public IHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }


        public void AddDataProtection(IServiceCollection services, HostBuilderContext context)
        {
            var keyloc = context.Configuration.GetValue<string>("PersistKeyStorageDirectory");

            DirectoryInfo path;
            if (string.IsNullOrEmpty(keyloc))
            {
                var temp = Directory.GetParent(Assembly.GetExecutingAssembly().Location);
                temp ??= Directory.GetParent(Assembly.GetExecutingAssembly().Location)!;
                path = temp;
            }
            else
            {
                path = new DirectoryInfo(keyloc);
            }
            services.AddDataProtection()
                .PersistKeysToFileSystem(path);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMemoryCache();
            services.AddCors();
            services.AddHttpClient();

            #region infra
            services.AddLocalization();

            services.AddDbContextOnModelCreating<AsZeroDbContext, SystemLog, SystemLogEntityConfiguration>(() => new SystemLogEntityConfiguration(), 1);
            services.AddZeroDal(opts =>
            {
                var s = Configuration.GetConnectionString("AsZeroDbContext");
                //opts.UseNpgsql(s!, builder =>
                //{
                //    var thisAssembly = typeof(Startup).Assembly;
                //    builder.MigrationsAssembly(thisAssembly.GetName().Name);
                //});
                opts.UseSqlite(s!, builder =>
                {
                    var thisAssembly = typeof(Startup).Assembly;
                    builder.MigrationsAssembly(thisAssembly.GetName().Name);
                });
                opts.EnableSensitiveDataLogging();

                //opts.UseSqlServer(s!, builder => {
                //    var thisAssembly = typeof(Startup).Assembly;
                //    builder.MigrationsAssembly(thisAssembly.GetName().Name);
                //});
            });

            services.AddMediatR(typeof(Startup).Assembly);

            services.AddOneAuth(ob => { });


            #endregion

            #region web
            var mvcBuilder = services.AddRazorPages();
            services
                .AddControllersWithViews(option =>
                {
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    //options.SerializerSettings.ContractResolver = new DefaultContractResolver();    
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Unspecified;
                    options.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
                });
            services.AddSignalR()
                .AddNewtonsoftJsonProtocol(opts =>
                {
                    opts.PayloadSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    opts.PayloadSerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Unspecified;
                    opts.PayloadSerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
                });

            // Forwarded headers
            services.Configure<ForwardedHeadersOptions>(opts =>
            {
                opts.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
                opts.KnownNetworks.Clear();
                opts.KnownProxies.Clear();
            });


            services.AddSwaggerGen(options =>
            {
            });
            services.AddSwaggerGenNewtonsoftSupport();

            #endregion

            #region Add One
            services.AddOneCore();
            services.AddOneDAL();
            mvcBuilder.AddOneWebApi();
            #endregion

            #region
            services.AddOutboxCore(opts =>
            {
                opts.OptionsBuilder.Configure(o =>
                {
                    o.IsPostEnabled = true;
                    o.IsConsumptionEnabled = true;
                    o.Interval = 100;
                    o.ReadBatchSize = 20;
                });
            });
            services.AddOutboxDAL("Microsoft.EntityFrameworkCore.SqlServer");
            mvcBuilder.AddOutboxWebApi();
            #endregion

            #region Traces
            services.AddTraceCore();
            services.AddTraceDAL();
            mvcBuilder.AddTraceWebApi();
            services.AddDbContextOnModelCreating<FuncResource, TracesFunResourceEntityTypeConfigurer>(100);
            #endregion

        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //todo:测试可以允许任意跨域，正式环境要加权限
            app.UseCors(builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());


            #region
            ServeStaticAssets(app, env);
            ServeStaticLayouts(app, env);
            app.UseOneMfeStatics();
            app.UseTraceMfeStatics();
            #endregion


            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseForwardedHeaders();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
                c.DocExpansion(DocExpansion.None);
            });
        }

        private static void ServeStaticAssets(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var fileprovider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "StaticFiles"));
            app.UseDefaultFiles(new DefaultFilesOptions
            {
                FileProvider = fileprovider,
            });

            var staticfile = new StaticFileOptions
            {
                FileProvider = fileprovider,
            };
            app.UseStaticFiles(staticfile);
        }

        private static void ServeStaticLayouts(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var fileprovider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "StaticLayouts"));
            app.UseDefaultFiles(new DefaultFilesOptions
            {
                FileProvider = fileprovider,
            });

            var staticfile = new StaticFileOptions
            {
                RequestPath = "/StaticLayouts",
                FileProvider = fileprovider,
            };
            app.UseStaticFiles(staticfile);
        }
    }
}
