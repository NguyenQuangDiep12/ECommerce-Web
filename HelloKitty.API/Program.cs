
using HelloKitty.API.Middleware;
using HelloKitty.Application;
using HelloKitty.Domain.Users.Interfaces;
using HelloKitty.Infrastructure.Persistences;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger với JWT support
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HelloKitty API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập: Bearer {token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});
// Application + Infrastructure DI
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// CORS cho frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")  // Vite default port
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Middleware phải đúng thứ tự
app.UseMiddleware<ExceptionHandlingMiddleware>();  // 1. bắt lỗi trước

app.UseCors("AllowFrontend");                     // 2. CORS

app.UseAuthentication();                           // 3. xác thực JWT
app.UseAuthorization();                            // 4. phân quyền

app.MapControllers();

app.Run();