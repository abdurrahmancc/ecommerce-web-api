
using Ecommerce_web_api.Controllers;
using Ecommerce_web_api.data;
using Ecommerce_web_api.Interfaces;
using Ecommerce_web_api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<ICategoryServices, CategoryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddAutoMapper(typeof (Program));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.Configure<ApiBehaviorOptions>(Options=>{
    Options.InvalidModelStateResponseFactory = context=>{
        var errors = context.ModelState
                .Where(e=> e.Value != null && e.Value.Errors.Count> 0)
                .SelectMany(e=> e.Value?.Errors != null ? e.Value.Errors.Select(x=>x.ErrorMessage): new List<string>()).ToList() ;

                return new BadRequestObjectResult(ApiResponse<object>.ErrorResponse(errors, 400, "validation failed"));
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if(app.Environment.IsDevelopment()){
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/",()=>"Api is working fine");

app.MapControllers();
app.Run();


