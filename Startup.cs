using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestNikita.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.SpaServices.AngularCli;

namespace TestNikita
{
  public class Startup
  {

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
          .AddJwtBearer(options =>
          {
            options.RequireHttpsMetadata = false;

            options.TokenValidationParameters = new TokenValidationParameters
            {
              ValidateIssuer = true,

              ValidIssuer = AuthOption.ISSUER,

              ValidAudience = AuthOption.AUDIENCE,

              ValidateLifetime = true,

              IssuerSigningKey = AuthOption.GetSymmetricSecurityKey(),

              ValidateIssuerSigningKey = true
            };
          });

      services.AddTransient<DataService>();

      services.AddCors();

      services.AddControllers();

      services.AddSpaStaticFiles(configuration =>
      {
        configuration.RootPath = "AngularApp/dist/Angular";
      });
    }


    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();

      app.UseCors(builder => builder.AllowAnyOrigin());


      app.UseAuthentication();
      app.UseAuthorization();

      app.UseStaticFiles();
      if (!env.IsDevelopment())
      {
        app.UseSpaStaticFiles();
      }

      app.UseEndpoints(endpoints =>
           {
             endpoints.MapControllers();
           });

      app.UseSpa(spa =>
      {
        spa.Options.SourcePath = "AngularApp";

        if (env.IsDevelopment())
        {
          spa.UseAngularCliServer(npmScript: "start");
        }
      });
    }
  }
}
