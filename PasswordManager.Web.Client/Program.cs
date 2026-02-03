using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PasswordManager.Domain.Service;
using PasswordManager.Domain.Validator;
using PasswordManager.Infrastructure;
using PasswordManager.Infrastructure.Data;
using PasswordManager.Infrastructure.Entity;
using PasswordManager.Infrastructure.UnitOfWork;
using PasswordManager.Model.Builder;
using PasswordManager.Model.Mapper;
using PasswordManager.Web.Client.Conventions;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Add Authentication services
var jwtIssuer = Environment.GetEnvironmentVariable("PMJWT__Issuer", EnvironmentVariableTarget.Machine);
var jwtAudience = Environment.GetEnvironmentVariable("PMJWT__Audience", EnvironmentVariableTarget.Machine);
var jwtKey = Environment.GetEnvironmentVariable("PMJWT__Key", EnvironmentVariableTarget.Machine);

if (string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience) || string.IsNullOrEmpty(jwtKey))
{
    throw new Exception("Jwt Issuer Audience or Key not set in configuration.");
}
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
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var token = context.SecurityToken as JwtSecurityToken;
            var tokenString = string.Empty;

            if (context.SecurityToken is JwtSecurityToken jwtToken)
            {
                tokenString = jwtToken.RawData;
            }
            else
            {
                // Fallback: get the token from the header
                tokenString = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");                
            }

            var authTokenService = context.HttpContext.RequestServices.GetRequiredService<IAuthTokenService>();
            var authToken = await authTokenService.GetByTokenAsync(tokenString!);

            if (authToken == null || authToken.RevokedAt != null)
            {
                context.Fail("Token has been revoked.");
            }
        }
    };
});

//Add DB Context with Connection String
var connectionString = Environment.GetEnvironmentVariable(PasswordManager.Common.Constant.Database.DbConnectionName, EnvironmentVariableTarget.Machine);
if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception("Connection string is not set in the environment variables.");
}
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)
);

builder.Services.AddDbContextFactory<AppDbContext>(options => options
        .UseNpgsql(connectionString),
        ServiceLifetime.Scoped
);

//Custom token name 
builder.Services.AddAntiforgery(options => options.HeaderName = PasswordManager.Common.Constant.Authentication.AntiforgeryTokeName);

// To access HttpContext in services
builder.Services.AddHttpContextAccessor();

// Auto Mapper Configurations
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

// Dependency Injection for Repositories and Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IAuthTokenRepository, AuthTokenRepository>();
builder.Services.AddTransient<IEntryRepository, EntryRepository>();

// Dependency Injection for Services
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IAuthTokenService, AuthTokenService>();
builder.Services.AddTransient<IEntryService, EntryService>();

// Dependency Injection for Validators
builder.Services.AddTransient<IValidator<User>, UserValidator>();
builder.Services.AddTransient<IValidator<Entry>, EntryValidator>();

// Dependency Injection for Model Builders
builder.Services.AddTransient<IUserModelBuilder, UserModelBuilder>();
builder.Services.AddTransient<IAuthTokenModelBuilder, AuthTokenModelBuilder>();
builder.Services.AddTransient<IEntryModelBuilder, EntryModelBuilder>();


// Configure CORS to allow requests from the React app
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy => 
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// Add Controllers with Global Route Prefix
builder.Services.AddControllers(options =>
   {
       options.Conventions.Insert(0, new GlobalRoutePrefixConvention("api"));
   });

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    //builder.Services.AddEndpointsApiExplorer();
    //builder.Services.AddSwaggerGen();
}

app.UseCors("AllowReactApp");
//app.UseHttpsRedirection(); // TODO: Uncomment in production
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
