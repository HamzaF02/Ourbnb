using Microsoft.EntityFrameworkCore;
using Ourbnb.DAL;
using Ourbnb.Models;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<RentalDbContext>(options =>
{
    options.UseSqlite(
        builder.Configuration["ConnectionStrings:RentalDbContextConnection"]);

});
builder.Services.AddScoped<IRepository<Rental>, RentalRepository>();
builder.Services.AddScoped<IRepository<Customer>, CustomerRepository>();

var loggerConfig = new LoggerConfiguration().MinimumLevel.Information().WriteTo.File
    ($"Logs/app_{DateTime.Now:yyyyMMdd_HHmmss}.log");
loggerConfig.Filter.ByExcluding(e => e.Properties.TryGetValue("SourceContext", out var value) &&
    e.Level == LogEventLevel.Information &&
    e.MessageTemplate.Text.Contains("Executed DbCommand"));

var logger = loggerConfig.CreateLogger();
builder.Logging.AddSerilog(logger);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    DBInit.Seed(app);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
