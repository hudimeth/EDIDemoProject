using Microsoft.AspNetCore.Authentication.Cookies;

namespace EDIConverterWeb.Web
{
    public class Program
    {
        private static string CookieScheme = "EDIConverter";
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthentication(CookieScheme)
           .AddCookie(CookieScheme, options =>
           {
               options.Events = new CookieAuthenticationEvents
               {
                   OnRedirectToLogin = context =>
                   {
                       context.Response.StatusCode = 403;
                       context.Response.ContentType = "application/json";
                       var result = System.Text.Json.JsonSerializer.Serialize(new { error = "You are not authenticated" });
                       return context.Response.WriteAsync(result);
                   }
               };
           });

            builder.Services.AddSession();

            // Add services to the container.

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");

            app.MapFallbackToFile("index.html"); ;

            app.Run();
        }
    }
}