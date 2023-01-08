using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<Survey_App.ContextModels.SurveyContext>(options =>
 options.UseSqlServer(builder.Configuration.GetConnectionString("SurveyContextDB")));

    services.AddCors();
}

builder.Services.AddDbContext<Survey_App.ContextModels.SurveyContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("SurveyContextDB")));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseCors(options => options.AllowAnyOrigin().WithMethods("GET"));
app.UseCors(options => options.AllowAnyOrigin().WithMethods("POST"));
app.UseCors(options => options.AllowAnyOrigin().WithMethods("PATCH"));
app.UseCors(options => options.AllowAnyOrigin().WithMethods("PUT"));

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


