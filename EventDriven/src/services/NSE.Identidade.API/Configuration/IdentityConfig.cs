using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NSE.Identidade.API.Data;
using NSE.Identidade.API.Extensions;
using System.Text;

namespace NSE.Identidade.API.Configuration
{
    public static class IdentityConfig
    {
        public static void AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDBContext>(options =>
              options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>() //é o usuario padrão do identity
                    .AddRoles<IdentityRole>() // as roles do identity
                    .AddErrorDescriber<IdentityMensagemPortugues>() //classe q faz override de IdentityErrorDescriber
                    .AddEntityFrameworkStores<ApplicationDBContext>() // o banco que criamos o Identity
                    .AddDefaultTokenProviders(); // não tem nada haver com JWT. é o token dele para resetar senha, autenticar conta recem criada
                                                 // praticamente uma criptografia dentro de um token para te reconhecer
           
            var appSettingsSection = configuration.GetSection("AppSettings"); //obtendo pela chave
            services.Configure<AppSettings>(appSettingsSection); //mapeando

            var appSettings = appSettingsSection.Get<AppSettings>(); //obtendo
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(beareOptions =>
            {
                beareOptions.RequireHttpsMetadata = true;
                beareOptions.SaveToken = true;
                beareOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key), //aqui vem a chave, como vou embaralhar os dados dentro do token
                    ValidateIssuer = true, //valida dentro da api que eu quiser, não aceito um token emitido de outro emissor
                    ValidateAudience = true, //onde vai ser valido, em quais dominios q ele é valido
                    ValidIssuer = appSettings.Emissor, // emissor do token que é essa api
                    ValidAudience = appSettings.ValidoEm // onde esse token vai ser valido
                };
            });
        }

        public static IApplicationBuilder UseIdentityConfiguration(this IApplicationBuilder app)
        {
            app.UseAuthorization();
            app.UseAuthentication();

            return app;
        }
    }
}
