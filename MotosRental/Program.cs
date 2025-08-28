using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MotosRental;
using MotosRental.Application.Consumers;
using MotosRental.DTOs.Examples;
using MotosRental.Infraestructure.Repositories;
using MotosRental.Interfaces;
using MotosRental.Messaging;
using MotosRental.Middleware;
using MotosRental.MotosRental.Application.Mappers;
using MotosRental.Repositories;
using MotosRental.Services;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddEndpointsApiExplorer();

// JWT

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
    
    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse();

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            var result = System.Text.Json.JsonSerializer.Serialize(new
            {
                error = "Token inválido ou ausente. Faça login novamente."
            });

            return context.Response.WriteAsync(result);
        },
        OnAuthenticationFailed = context =>
        {
            context.NoResult();
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            var result = System.Text.Json.JsonSerializer.Serialize(new
            {
                error = "Falha na autenticação. Token expirado ou inválido."
            });

            return context.Response.WriteAsync(result);
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
});


// Swagger

builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.ExampleFilters();
    
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando Bearer. Exemplo: 'Bearer {token}'",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<DriverRequestExample>();
builder.Services.AddSwaggerGen();

// Controllers


builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddControllers();

// DbContext

var connectionString = builder.Configuration.GetConnectionString("AppDbConnectionString");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)
);

// RabbitMQ

builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddHostedService<MotorcycleEventConsumer>();

var rabbitMQHostname = builder.Configuration["RabbitMQ:Hostname"];
var rabbitMQUsername = builder.Configuration["RabbitMQ:Username"];
var rabbitMQPassword = builder.Configuration["RabbitMQ:Password"];

builder.Services.AddSingleton(new RabbitMQConnection(rabbitMQHostname, rabbitMQUsername, rabbitMQPassword));
builder.Services.AddScoped<IMessagePublisher, RabbitMQPublisher>();

// Services


builder.Services.AddScoped<IMotorcycleService, MotorcycleService>();
builder.Services.AddScoped<IDriverService, DriverService>();
builder.Services.AddScoped<IRentService, RentService>();

// Repositories

builder.Services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
builder.Services.AddScoped<IDriverRepository, DriverRepository>();
builder.Services.AddScoped<IRentRepository, RentRepository>();
builder.Services.AddScoped<IPlanRepository, PlanRepository>();


var app = builder.Build();

app.UseAuthentication(); 
app.UseAuthorization();


app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();