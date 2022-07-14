using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public static class ServiceCollectionExt
    {
        public static IServiceCollection RegisterService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<DatabaseContext>(opt => opt.UseInMemoryDatabase(databaseName: "MohanTest"));

            return serviceCollection;
        }

        public static IServiceCollection RegisterJWT(this IServiceCollection serviceCollection, IConfiguration Configuration)
        {

            serviceCollection.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            //Addding Jwt Bearer
            .AddJwtBearer(options =>
            {
                #region Commented Code
                //    //Suppose a token is not passed through header and passed through the url query string
                //    options.Events = new JwtBearerEvents()
                //    {
                //        OnMessageReceived = context =>
                //        {
                //            if (context.Request.Query.ContainsKey("access_token"))
                //            {
                //                context.Token = context.Request.Query["access_token"];
                //            }
                //            return Task.CompletedTask;
                //        }
                //    };
                #endregion Commented Code

                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"])),
                    ClockSkew = TimeSpan.Zero
                };
            });

            return serviceCollection;
        }
    }
}
