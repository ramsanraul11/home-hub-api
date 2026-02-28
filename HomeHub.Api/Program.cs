// ... (Tus usings se mantienen igual)

using HomeHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// Services
// --------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HomeHub API", Version = "v1" });

    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Introduce: Bearer {tu_token}",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = JwtBearerDefaults.AuthenticationScheme
        }
    };

    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, jwtSecurityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

// CORS: Configurado para aceptar todo en producción sin problemas de Mixed Content
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("dev", p =>
        p.AllowAnyOrigin()
         .AllowAnyHeader()
         .AllowAnyMethod());
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// JWT (Configuración se mantiene igual)
var jwt = builder.Configuration.GetSection("Jwt");
var issuer = jwt["Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer missing");
var audience = jwt["Audience"] ?? throw new InvalidOperationException("Jwt:Audience missing");
var key = jwt["Key"] ?? throw new InvalidOperationException("Jwt:Key missing");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });

builder.Services.AddScoped<IAuthorizationHandler, HouseholdMemberHandler>();
builder.Services.AddScoped<IAuthorizationHandler, HouseholdAdminOrOwnerHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("HouseholdMember", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AddRequirements(new HouseholdMemberRequirement());
    });
    options.AddPolicy("HouseholdAdminOrOwner", p => { p.RequireAuthenticatedUser(); p.AddRequirements(new HouseholdAdminOrOwnerRequirement()); });
});

var app = builder.Build();

// --------------------
// Pipeline
// --------------------

// 1. Migraciones automáticas (Se mantiene igual, solo asegúrate de la conexión en Render)
if (!app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var db = services.GetRequiredService<AppDbContext>();
            await db.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while applying EF migrations.");
        }
    }
}

// 2. Swagger con forzado de HTTPS para evitar el "Failed to fetch"
app.UseSwagger(c =>
{
    c.PreSerializeFilters.Add((swagger, httpReq) =>
    {
        // Render usa el header X-Forwarded-Proto para indicar si la petición original era HTTPS
        var scheme = httpReq.Headers["X-Forwarded-Proto"].FirstOrDefault() ?? "https";
        swagger.Servers = new List<OpenApiServer>
        {
            new() { Url = $"{scheme}://{httpReq.Host.Value}" }
        };
    });
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "HomeHub API v1");
});

app.UseCors("dev");

// Importante: En Render, el proxy ya maneja HTTPS, pero esto asegura consistencia
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();