
using Ecommerce_web_api.Controllers;
using Ecommerce_web_api.data;
using Ecommerce_web_api.Interfaces;
using Ecommerce_web_api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using dotenv.net;
using Ecommerce_web_api.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<ICategoryServices, CategoryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IFilesUploadService, FilesUploadService>();
builder.Services.AddAutoMapper(typeof (Program));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
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
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.Configure<ApiBehaviorOptions>(Options=>{
    Options.InvalidModelStateResponseFactory = context=>{
        var errors = context.ModelState
                .Where(e=> e.Value != null && e.Value.Errors.Count> 0)
                .SelectMany(e=> e.Value?.Errors != null ? e.Value.Errors.Select(x=>x.ErrorMessage): new List<string>()).ToList() ;

                return new BadRequestObjectResult(ApiResponse<object>.ErrorResponse(errors, 400, "validation failed"));
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        //policy.WithOrigins("http://localhost:3000")
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


/*================================= Cloudinary credentials set up  =================================*/
DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
Cloudinary cloudinary = new Cloudinary(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
cloudinary.Api.Secure = true;

var app = builder.Build();


// Enable static files to be served from wwwroot folder
app.UseStaticFiles();

if(app.Environment.IsDevelopment()){
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowSpecificOrigins");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapGet("/",()=>"Api is working fine");

app.MapControllers();
app.Run();


