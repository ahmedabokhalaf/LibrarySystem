using ITI.LibSys.Models.Data;
using ITI.LibSys.Presentation.Filteration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

public class Program
{
    public static int Main()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddDbContext<Context>(options =>
        {
            options.UseLazyLoadingProxies();
            options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConntection"));
        });
        builder.Services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<Context>()
            .AddDefaultTokenProviders();
        
        builder.Services.Configure<IdentityOptions>(options =>
        {
            //To give give option for the user for making his password in whatever style
            options.Password.RequireDigit = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;

            //To lockout login for 20 minutes if he tried 2 times 
            options.Lockout.MaxFailedAccessAttempts = 2;
            options.Lockout.DefaultLockoutTimeSpan= TimeSpan.FromSeconds(30);

            options.SignIn.RequireConfirmedEmail = true;
        });
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/User/Login";
        });
        builder.Services.AddControllersWithViews(options =>
        {
            //options.Filters.Add(new ExceptionFilteration());
        });
        builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
        var webApp=builder.Build();
        webApp.UseStaticFiles(new StaticFileOptions()
        {
            RequestPath = "/Content",
            FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(),"Content"))
        });


        webApp.UseAuthentication();
        webApp.UseAuthorization();
        webApp.MapControllerRoute("Default", "{controller=Home}/{action=Index}/{id?}");

        webApp.Run();
        return 0;
    }
}