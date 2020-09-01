using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSE.Identidade.API.Data;
using NSE.Identidade.API.Extensions;
using NSE.WebAPI.Core.Identidade;

namespace NSE.Identidade.API.Configuration
{
    public static class IdentityConfig
    {
        public static void AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // Identity
            services.AddDbContext<ApplicationDBContext>(options =>
              options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>() //é o usuario padrão do identity
                    .AddRoles<IdentityRole>() // as roles do identity
                    .AddErrorDescriber<IdentityMensagemPortugues>() //classe q faz override de IdentityErrorDescriber
                    .AddEntityFrameworkStores<ApplicationDBContext>() // o banco que criamos o Identity
                    .AddDefaultTokenProviders(); // não tem nada haver com JWT. é o token dele para resetar senha, autenticar conta recem criada
                                                 // praticamente uma criptografia dentro de um token para te reconhecer

            services.AddJwtConfiguration(configuration);
        }


        //public static void UseIdentityConfiguration(this IApplicationBuilder app)
        //{
        //    app.UseJwtConfiguration();
        //}
    }
}
