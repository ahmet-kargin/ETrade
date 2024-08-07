using ETrade.Application.Interfaces;
using ETrade.Domain.Entities;
using ETrade.Infrastructure.Connection;
using ETrade.Infrastructure.Repository;
using ETrade.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddSingleton<JwtTokenService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();


// Authentication (kimlik doğrulama) hizmetini yapılandırır. Bu, uygulamanın kimlik doğrulama işlemlerini nasıl yöneteceğini belirler.
builder.Services.AddAuthentication(options =>
{
    // Varsayılan kimlik doğrulama şeması olarak JWT Bearer (taşıyıcı) şemasını ayarlar.
    // Bu, gelen isteklerde JWT tabanlı kimlik doğrulamasının kullanılacağını belirtir.
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

    // Varsayılan zorlama (challenge) şeması olarak JWT Bearer (taşıyıcı) şemasını ayarlar.
    // Bu, kimlik doğrulama gerektiren bir istek yapıldığında JWT Bearer şemasının kullanılacağını belirtir.
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // JWT Bearer (taşıyıcı) kimlik doğrulama şemasını ekler ve yapılandırır.

    // Token doğrulama parametrelerini yapılandırır.
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,   // Issuer (yayımcı) bilgisini doğrular. Token'ın doğru yayımcı tarafından oluşturulup oluşturulmadığını kontrol eder.
        ValidateAudience = true, // Audience (hedef kitle) bilgisini doğrular. 
        ValidateLifetime = true, // Token'ın süresinin dolup dolmadığını doğrular.
        ValidateIssuerSigningKey = true, // Token'ın imzasının geçerli olup olmadığını doğrular.
        ValidIssuer = builder.Configuration["Jwt:Issuer"], // Geçerli yayımcı bilgisi. Token'ın bu yayımcı tarafından oluşturulmuş olması gerekir.
        ValidAudience = builder.Configuration["Jwt:Audience"], // Geçerli hedef kitle bilgisi. Token'ın bu hedef kitleye yönelik olması gerekir.
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // Token imzası için kullanılan anahtar.
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();
// Use session
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
