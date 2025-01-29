using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NotificationCenter.API.SignalR;
using NotificationCenter.Application.Hubs;
using NotificationCenter.Application.Interfaces;
using NotificationCenter.Application.Services;
using NotificationCenter.Domain.Repositories;
using NotificationCenter.Infrastructure.Data;
using NotificationCenter.Infrastructure.Data.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<NotificationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .WithOrigins("http://localhost:4200"); 
    });
});


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<INotificationHub, NotificationHubService>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

builder.Services.AddSingleton<JwtService>(); 

var secretKey = builder.Configuration["JwtSettings:SecretKey"];
if (string.IsNullOrEmpty(secretKey))
{
    throw new InvalidOperationException("Secret key is not configured.");
}

var key = Encoding.UTF8.GetBytes(secretKey);
 
// Configure authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,        
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {            
            var accessToken = context.Request.Query["access_token"];
            
            if (!string.IsNullOrEmpty(accessToken))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();
builder.Services.AddSingleton<IUserIdProvider, ClientIdUserIdProvider>();


builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
    options.KeepAliveInterval = TimeSpan.FromSeconds(10);
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(60); 
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCors();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();  
app.UseAuthorization();

// Map SignalR hub
app.MapHub<NotificationHub>("/notificationHub");

app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
