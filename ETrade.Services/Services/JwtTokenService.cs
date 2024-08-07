using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ETrade.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


namespace ETrade.Services.Services;

// JwtTokenService, JWT token oluşturmak için kullanılan bir servis sınıfıdır.
public class JwtTokenService
{
    // IConfiguration, uygulama ayarlarını okumak için kullanılan bir arayüzdür.
    private readonly IConfiguration _configuration;

    // Constructor, dependency injection kullanarak IConfiguration nesnesini alır.
    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // Kullanıcı bilgilerine dayalı olarak JWT token oluşturmak için kullanılan bir metod.
    public string GenerateToken(User user)
    {
        // JWT token içerisinde yer alacak claim'leri tanımlar.
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),  // Kullanıcının ID'si
            new Claim(JwtRegisteredClaimNames.Email, user.Email)  // Kullanıcının email adresi
        };

        // JWT token imzalama anahtarını oluşturur.
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // JWT token'ı oluşturur.
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],  // Token'ı oluşturan taraf
            audience: _configuration["Jwt:Audience"],  // Token'ın hedef kitlesi
            claims: claims,  // Token içerisine eklenen claim'ler
            expires: DateTime.Now.AddMinutes(30),  // Token'ın geçerlilik süresi
            signingCredentials: creds);  // Token'ı imzalamak için kullanılan imzalama bilgileri

        // Token'ı string formatında döner.
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

