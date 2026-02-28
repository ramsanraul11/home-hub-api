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

    // (Opcional) Config Swagger para JWT Bearer
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

// CORS (para frontend local)
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("dev", p =>
        p.AllowAnyOrigin()
         .AllowAnyHeader()
         .AllowAnyMethod());
    // .AllowCredentials() <-- ELIMINA esta línea si usas AllowAnyOrigin
});

// DI por capas
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// JWT
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
            ClockSkew = TimeSpan.FromSeconds(30) // pequeño margen
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
if (!app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var env = services.GetRequiredService<IHostEnvironment>();
        var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("DbMigrator");

        try
        {
            var db = services.GetRequiredService<AppDbContext>();

            // Aplica migrations pendientes
            logger.LogInformation("Applying EF migrations...");
            await db.Database.MigrateAsync();
            logger.LogInformation("EF migrations applied.");
        }
        catch (Exception ex)
        {
            // En producción puedes decidir si quieres "fallar" el arranque o no.
            logger.LogError(ex, "An error occurred while applying EF migrations.");
            throw; // recomendado: si falla migración, que no levante la app
        }
    }
}

// Swagger
app.UseSwagger(c =>
{
    // Fuerza a que Swagger use el mismo scheme/host con el que abriste /swagger (evita http/https mismatch)
    c.PreSerializeFilters.Add((swagger, httpReq) =>
    {
        swagger.Servers = new List<OpenApiServer>
        {
            new() { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" }
        };
    });
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "HomeHub API v1");
});

// CORS (antes de auth)
app.UseCors("dev");

// En dev, evita líos de redirección https (si quieres forzar https, quita este if)
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();