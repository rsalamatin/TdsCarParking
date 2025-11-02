using Microsoft.EntityFrameworkCore;
using TdsCarParking.Core.Contracts;
using TdsCarParking.Core.Services;
using TdsCarParking.Infrastructure.Dal;
using TdsCarParking.Infrastructure.Dal.Repositories;

namespace TdsCarParking.WebApi;

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
        services.AddScoped<IParkingChargesRepository, ParkingChargesRepository>();
        services.AddScoped<IParkingSlotRepository, ParkingSlotRepository>();
        services.AddScoped<IChargeService, ChargeService>();
        services.AddScoped<IParkingService, ParkingService>();
        services.AddDbContext<ParkingChargesContext>(options => options.UseInMemoryDatabase("parkingDb"));
        services.AddDbContext<ParkingSlotContext>(options => options.UseInMemoryDatabase("chargesDb"));
        services.AddControllers();
        services.AddSwaggerGen();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = string.Empty;
        });
        
        var scope = app.ApplicationServices.CreateScope();
        var parkingChargesContext = scope.ServiceProvider.GetRequiredService<ParkingChargesContext>();
        var slotContext = scope.ServiceProvider.GetRequiredService<ParkingSlotContext>();
        
        addTestData(parkingChargesContext, slotContext);
    }
    
    private static void addTestData(ParkingChargesContext chargesContext, ParkingSlotContext slotContext)
    {
        chargesContext.AddRange(InitialDataProvider.GetCharges());
        chargesContext.SaveChanges();
        slotContext.AddRange(InitialDataProvider.GetParkingSlots());
        slotContext.SaveChanges();
    }   
}