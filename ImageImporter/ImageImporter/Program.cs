using ImageImporter.Data;
using ImageImporter.Jobs;
using ImageImporter.Helpers.Quartz;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quartz;


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

builder.Services.AddQuartz(quartz => { 
    quartz.UseMicrosoftDependencyInjectionJobFactory();
    //quartz.AddJob<ImportImagesJob>(x => x
    //    .WithIntervalInSeconds(5)
    //    .RepeatForever());


    var conconcurrentJobKey = new JobKey("ConconcurrentJob");
    quartz.AddJob<ConconcurrentJob>(opts => opts.WithIdentity(conconcurrentJobKey));
    quartz.AddTrigger(opts => opts
        .ForJob(conconcurrentJobKey)
        .WithIdentity("ConconcurrentJob-trigger")
        .WithSimpleSchedule(x => x
            .WithIntervalInSeconds(5)
            .RepeatForever()));

    var nonConconcurrentJobKey = new JobKey("NonConconcurrentJob");
    quartz.AddJob<NonConconcurrentJob>(opts => opts.WithIdentity(nonConconcurrentJobKey));
    quartz.AddTrigger(opts => opts
        .ForJob(nonConconcurrentJobKey)
        .WithIdentity("NonConconcurrentJob-trigger")
        .WithSimpleSchedule(x => x
            .WithIntervalInSeconds(5)
            .RepeatForever()));

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
}

app.Run();
