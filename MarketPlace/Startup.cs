using MarketPlace.Entities.DBEntities;
using MarketPlace.Helpers;
using MarketPlace.Hubs;
using MarketPlace.JWT;
using MarketPlace.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using System.Threading.Tasks;

namespace MarketPlace
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



            services.AddDbContext<DBContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("MarketPlace")));
            services.AddDefaultIdentity<User>()
                   .AddRoles<IdentityRole>()
                   .AddEntityFrameworkStores<DBContext>();

            var builder = services.AddIdentityCore<User>(o =>
            {
                // configure identity options
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            });
            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
            builder.AddEntityFrameworkStores<DBContext>().AddDefaultTokenProviders();


            services.AddAuthentication(options =>
                    {
                        // Identity made Cookie authentication the default.
                        // However, we want JWT Bearer Auth to be the default.
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                  .AddJwtBearer(options =>
                  {
                      options.RequireHttpsMetadata = false;
                      options.TokenValidationParameters = new TokenValidationParameters
                      {
                          // укзывает, будет ли валидироваться издатель при валидации токена
                          ValidateIssuer = true,
                          // строка, представляющая издателя
                          ValidIssuer = AuthOptions.ISSUER,

                          // будет ли валидироваться потребитель токена
                          ValidateAudience = true,
                          // установка потребителя токена
                          ValidAudience = AuthOptions.AUDIENCE,
                          // будет ли валидироваться время существования
                          ValidateLifetime = true,

                          // установка ключа безопасности
                          IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                          // валидация ключа безопасности
                          ValidateIssuerSigningKey = true,
                      };
                      options.Events = new JwtBearerEvents
                      {
                          OnMessageReceived = context =>
                          {
                              var accessToken = context.Request.Query["access_token"];

                              // If the request is for our hub...
                              var path = context.HttpContext.Request.Path;
                              if (!string.IsNullOrEmpty(accessToken) &&
                                  (path.StartsWithSegments("/chat")))
                              {
                                  // Read the token out of the query string
                                  var token = accessToken.ToString();
                                  while (true)
                                  {
                                      int id = -1;
                                      id = token.IndexOf('"');
                                      if (id == -1)
                                          id = token.IndexOf('\\');
                                      if (id == -1)
                                          break;
                                      token = token.Remove(id, 1);


                                  }
                                  context.Token = token;
                              }
                              return Task.CompletedTask;
                          }
                      };
                  });


            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    b =>
                    {
                        b
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .SetIsOriginAllowed(_ => true);
                    });
            });






            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSignalR(o =>
            {
                o.EnableDetailedErrors = false;
            });
            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";

            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseDefaultFiles();
            var provider = new FileExtensionContentTypeProvider();
            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = provider // this is not set by default
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                     Path.Combine(Directory.GetCurrentDirectory(), "ClientApp/dist")),
            });
            
            // app.UseMiddleware<WebSocketsMiddleware>();
            app.UseAuthentication();
            app.UseCors("AllowAll");
            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chat");
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";
                
                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });

        }

    }
}
