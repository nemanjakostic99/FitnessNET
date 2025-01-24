using FitnessNET.Data;
using FitnessNET.Hubs;
using FitnessNET.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>(); 
builder.Services.AddScoped<FriendshipService>();
builder.Services.AddScoped<MessageService>();
builder.Services.AddSingleton<MongoDbService>();
builder.Services.AddScoped<IImageProcessingService, ImageProcessingService>();
builder.WebHost.ConfigureKestrel(options =>
{
    options.Configure(builder.Configuration.GetSection("Kestrel"));
});
builder.Services.AddDbContext<FitnessNetContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularFrontend",
        policy => policy.WithOrigins("http://localhost:4200") 
                        .AllowAnyMethod() 
                        .AllowAnyHeader() 
                        .AllowCredentials()); 
});

// JWT configuration
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);

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
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync("{\"error\":\"Authentication failed.\"}");
        },
        OnChallenge = context =>
        {
            context.Response.StatusCode = 403;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync("{\"error\":\"Access forbidden.\"}");
        },
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            // Only extract the token for SignalR WebSocket connections
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chatHub"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IImageProcessingService, ImageProcessingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularFrontend");

app.UseHttpsRedirection();
app.MapHub<ChatHub>("/chatHub");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
