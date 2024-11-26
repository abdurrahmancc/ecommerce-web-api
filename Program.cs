using Ecommerce_web_api.Controllers;
using Ecommerce_web_api.data;
using Ecommerce_web_api.Interfaces;
using Ecommerce_web_api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using CloudinaryDotNet;
using dotenv.net;
using Ecommerce_web_api.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables from .env file
DotEnv.Load();

// Add environment variables to Configuration
builder.Configuration.AddEnvironmentVariables();

// Dependency Injection (DI) Configuration
builder.Services.AddScoped<ICategoryServices, CategoryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IFilesUploadService, FilesUploadService>();
builder.Services.AddAutoMapper(typeof(Program));

// Configure JWT Settings
builder.Services.Configure<JwtSettings>(options =>
{
    options.Key = _env.JWT_SECRET_KEY;
    options.Issuer = _env.JWT_ISSUER;
    options.Audience = _env.JWT_AUDIENCE;
    options.ExpiryMinutes = _env.JWT_EXPIRY_MINUTES;
});

// Alternatively load from appsettings.json (for fallback settings)
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

// Configure Database Context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Cloudinary Settings
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

// Add Controllers to DI
builder.Services.AddControllers(); // Add this line

// Authentication with JWT Bearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Add Authorization Middleware
builder.Services.AddAuthorization();

// Configure API Behavior for Validation Errors
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value != null && e.Value.Errors.Count > 0)
            .SelectMany(e => e.Value.Errors.Select(x => x.ErrorMessage))
            .ToList();

        return new BadRequestObjectResult(ApiResponse<object>.ErrorResponse(errors, 400, "Validation failed"));
    };
});

// Configure CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add Swagger for API Documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Initialize Cloudinary API
Cloudinary cloudinary = new Cloudinary(_env.CLOUDINARY_URL)
{
    Api = { Secure = true }
};

// Build the application
var app = builder.Build();

/*================================ Middleware Configuration ================================*/

// Serve static files from "wwwroot"
app.UseStaticFiles();

// Enable Swagger UI in Development mode
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS
app.UseCors("AllowAll");

// Enable HTTPS redirection
app.UseHttpsRedirection();

// Enable Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

// Default test route
app.MapGet("/", () => Results.Json(new { status = "success", message = "API is working fine" }));

// Map Controllers (Make sure controllers are mapped after middleware setup)
app.MapControllers();

// Run the application
app.Run();
