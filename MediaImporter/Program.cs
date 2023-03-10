using MediaImporter.Application;
using MediaImporter.Application.Jobs;
using MediaImporter.Application.Services;
using MediaImporter.Controllers;
using Mica;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

builder.Services.AddSingleton<FileService>();

builder.Services.AddDbContext<ApplicationDbContext>(options => options
    .UseLazyLoadingProxies()
    .UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddMicaScheduler(scheduler => scheduler
    .AddJob<ImportScanJob>(job => job
        .AddTrigger(trg => trg
            .Interval = TimeSpan.FromSeconds(10))));

builder.Services.Configure<Settings>(builder.Configuration.GetSection("Settings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.MapHub<JobHub>("/jobHub");

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}



app.Run();
