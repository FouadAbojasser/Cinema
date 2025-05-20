using Cinema.Models;
using Cinema.Repositories;
using Cinema.Repositories.IRepositories;
using Cinema.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Google;



namespace Cinema
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
                .AddCookie()
                .AddGoogle(options =>
                {
                    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                });

           


            builder.Services.AddDbContext<ApplicationDbContext>(options => 
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            
            builder.Services.AddTransient<IEmailSender, EmailSender>();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(option =>
            {
                option.Password.RequiredLength = 6;  //Default Lenght
                option.SignIn.RequireConfirmedEmail = true; //Change to true to allow only confioremd emails to login
            
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders(); ;

            builder.Services.AddDbContext<ApplicationDbContext>();

            //builder.Services.AddScoped<IActorRepository, ActorRepository>();
            //builder.Services.AddScoped<IDirectorRepository, DirectorRepository>();
            //builder.Services.AddScoped<IGenreRepository, GenreRepository>();
            //builder.Services.AddScoped<IMovieRepository, MovieRepository>();
            //builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
            //builder.Services.AddScoped<IOTPRepository, OTPRepository>();
            //builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();

            //بدل تسجيل كل ريبوزتري على حدة بنسجل الحاوية الخاصة بيهم
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();
           
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Guest}/{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
