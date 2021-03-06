using API.Extensions;
using API.Middleware;
using API.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddApplicationServices(_config);
            services.AddControllers();
            services.AddCors();
            services.AddIdentityServices(_config);
            services.AddSignalR();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // if (env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage();
            // }

            //Always keep the exception middleware at the top
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            app.UseHttpsRedirection();

            app.UseRouting();
            // Always include CORS policy in same place between routing and the Authorization
            app.UseCors(policy =>
             policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            /// this allow credential used as it required for SIGNAL R
             .AllowCredentials()
            .WithOrigins("https://localhost:4200"));

            //This for Authorize Attribute to work
            app.UseAuthentication();
            app.UseAuthorization();
            // To User the Index.html as starting file
            app.UseDefaultFiles();
            // To say wwwroot folder content servered as static file
            app.UseStaticFiles();


            // We need to add the Signal R HUB end points 
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<PresenceHub>("hubs/presence");
                endpoints.MapHub<MessageHub>("hubs/message");
                //Here we mention the fall back controller for the angular deployment
                endpoints.MapFallbackToController("Index","Fallback");
            });
        }
    }
}
