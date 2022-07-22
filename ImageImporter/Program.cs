using ImageImporter.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quartz;
using DB4045.Data.Seeds;
using ImageImporter.Services.Quartz.JobTracker;
using ImageImporter.Services.Quartz;
using ImageImporter.Application.Jobs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

//https://damienbod.com/2021/11/08/asp-net-core-scheduling-with-quartz-net-and-signalr-monitoring/
builder.Services.AddSignalR();

builder.Services.AddSingleton<JobsTracker>();
builder.Services.AddScoped<Settings>();

builder.Services.AddQuartz(quartz => { 
    quartz.UseMicrosoftDependencyInjectionJobFactory();
    quartz.AddJob<FileImportJob>(x => x
        .WithIntervalInMinutes(2)
        .RepeatForever());
});

builder.Services.AddQuartzHostedService(q => q
    .WaitForJobsToComplete = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<JobsHub>("/jobshub");
});

app.MapRazorPages();


using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dataContext.Database.Migrate();

    var seeders = new List<ISeeder> {};

    foreach (var seeder in seeders)
        seeder.Execute();
}

app.Run();
