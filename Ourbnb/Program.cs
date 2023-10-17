using Microsoft.EntityFrameworkCore;
using Ourbnb.DAL;
using Ourbnb.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<RentalDbContext>(options =>
{
    options.UseSqlite(
        builder.Configuration["ConnectionStrings:RentalDbContextConnection"]);

});
builder.Services.AddScoped<IRepository<Rental>, RentalRepository>();

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
