// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.

// builder.Services.AddControllers();
// // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();
// var connectionString = builder.Configuration.GetConnectionString("SqlConnection");
// builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(connectionString));
// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
// }

// app.UseHttpsRedirection();

// app.UseAuthorization();

// app.MapControllers();

// app.Run();

// using BusinessLayer;
// using BusinessLayer.Interface;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.IdentityModel.Tokens;
// using RepositoryLayer;
// using RepositoryLayer.Interface;
// using RepositoryLayer.Service;
// using BusinessLayer.Service;
// using System.Text;

// var builder = WebApplication.CreateBuilder(args);

// // 🔐 JWT Settings
// var jwtSecretKey = builder.Configuration["JwtSettings:SecretKey"];
// var keyBytes = Encoding.UTF8.GetBytes(jwtSecretKey);

// // ✅ Add services to the container
// builder.Services.AddControllers();
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// // 🔄 Dependency Injection
// builder.Services.AddScoped<IUserRL, UserRL>();
// builder.Services.AddScoped<IUserBL>(provider =>
// {
//     var userRL = provider.GetRequiredService<IUserRL>();
//     return new UserBL(userRL, jwtSecretKey);
// });

// // 🔐 JWT Authentication Setup
// builder.Services.AddAuthentication(options =>
// {
//     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
// })
// .AddJwtBearer(options =>
// {
//     options.RequireHttpsMetadata = false; // For development only
//     options.SaveToken = true;
//     options.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidateIssuer = true,
//         ValidateAudience = true,
//         ValidateLifetime = true,
//         ValidateIssuerSigningKey = true,
//         ValidIssuer = "your-app",
//         ValidAudience = "your-app-users",
//         IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
//     };
// });

// // 🔓 Enable CORS (for frontend access)
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowAll", builder =>
//     {
//         builder
//         .AllowAnyOrigin()
//         .AllowAnyMethod()
//         .AllowAnyHeader();
//     });
// });

// var app = builder.Build();

// // 🔄 Middleware pipeline
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseCors("AllowAll");

// app.UseAuthentication(); // 
// app.UseAuthorization();

// app.MapControllers();

// app.Run();


using BusinessLayer;
using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using BusinessLayer.Service;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 🔐 JWT Settings
var jwtSecretKey = builder.Configuration["Jwt:Key"];

if (string.IsNullOrEmpty(jwtSecretKey))
{
    throw new Exception("JWT Secret Key is not configured in appsettings.json.");
}
var keyBytes = Encoding.UTF8.GetBytes(jwtSecretKey);

// ✅ Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔄 Dependency Injection
builder.Services.AddScoped<IUserRL, UserRL>();
builder.Services.AddScoped<IUserBL>(provider =>
{
    var userRL = provider.GetRequiredService<IUserRL>();
    var configuration = provider.GetRequiredService<IConfiguration>(); // Get IConfiguration
    return new UserBL(userRL, configuration); // Pass IConfiguration here
});
builder.Services.AddDbContext<FarmDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔐 JWT Authentication Setup
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // For development only
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "your-app",
        ValidAudience = "your-app-users",
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
    };
});

// 🔓 Enable CORS (for frontend access)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

var app = builder.Build();

// 🔄 Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Farm Scheme API V1");
    c.RoutePrefix = ""; // This sets Swagger at root: localhost:5129/
});
}

app.UseCors("AllowAll");

app.UseAuthentication(); // 
app.UseAuthorization();

app.MapControllers();

app.Run();
