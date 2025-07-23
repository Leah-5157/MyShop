using Entities;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using MyShop.Middleware;
using repositories;
using Repositories;
using Services;
using NLog.Web;
using PresidentsApp.Middlewares;
using MyShop;
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNLog();
// Add services to the container.

// Add JWT authentication (token will be injected to Authorization header by CookieMiddleware)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
    };
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    var redisHost = builder.Configuration["Redis:Host"] ?? "redis:6379";
    var redisPassword = Environment.GetEnvironmentVariable("REDIS_PASSWORD");
    options.Configuration = string.IsNullOrEmpty(redisPassword)
        ? redisHost
        : $"{redisHost},password={redisPassword}";
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IRatingService, RatingService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>(); 
builder.Services.AddDbContext<MyShopContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("home")));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<TokenService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRatingMiddleware();

app.UseErrorHandlingMiddleware();
app.UseMiddleware<MyShop.Middleware.CookieMiddleware>();
app.UseMiddleware<MyShop.Middleware.CspMiddleware>();
app.UseMiddleware<MyShop.Middleware.RateLimitMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
