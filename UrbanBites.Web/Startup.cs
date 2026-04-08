using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UrbanBites.Web.Data;
using UrbanBites.Web.Hubs;
using UrbanBites.Web.Jobs;
using UrbanBites.Web.Services;

namespace UrbanBites.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UrbanBitesDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers();
            services.AddSignalR();

            services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection")));

            services.AddHangfireServer();

            services.AddScoped<OrderService>();
            services.AddScoped<PaymentService>();
            services.AddScoped<DeliveryService>();
            services.AddScoped<NotificationService>();
            services.AddScoped<PricingService>();
            services.AddScoped<ReviewService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<DeliveryHub>("/hubs/delivery");
                endpoints.MapHangfireDashboard();
            });

            RecurringJob.AddOrUpdate<OrderTimeoutJob>(
                "order-timeout",
                job => job.Execute(),
                Cron.Minutely);

            RecurringJob.AddOrUpdate<DeliveryAssignmentJob>(
                "delivery-assignment",
                job => job.Execute(),
                Cron.Minutely);
        }
    }
}
