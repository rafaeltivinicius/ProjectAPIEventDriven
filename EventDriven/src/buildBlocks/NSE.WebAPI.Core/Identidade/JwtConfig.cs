using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace NSE.WebAPI.Core.Identidade
{
    public static class JwtConfig
    {
        public static void AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // ele obtem o AppSettings do projeto q esta rodando, pois ele mesmo não tem essa classe

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
                    ValidAudience = appSettings.ValidoEm, // onde esse token vai ser valido
                    ValidIssuer = appSettings.Emissor, // emissor do token que é essa api
                };
            });
        }

        public static void UseJwtConfiguration(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
