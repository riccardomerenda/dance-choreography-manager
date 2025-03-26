using System.Text;
using DanceChoreographyManager.Services.Authentication.Data;
using DanceChoreographyManager.Services.Authentication.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults from .NET Aspire
builder.AddServiceDefaults();

// Configure Kestrel
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5001); // HTTP endpoint
});

// Add database context with fallback connections
string[] connectionStrings = {
    builder.Configuration.GetConnectionString("DefaultConnection"),
    // Alternative connections if the first one fails
    "Host=auth-db;Port=5432;Database=authdb;Username=postgres;Password=postgres;Include Error Detail=true;Trust Server Certificate=true;SSL Mode=Prefer",
    "Host=localhost;Port=55805;Database=authdb;Username=postgres;Password=postgres;Include Error Detail=true;Trust Server Certificate=true;SSL Mode=Prefer"
};

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    // We'll try different connection strings during initialization
    options.UseNpgsql(connectionStrings[0]);
});

// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;

    // User settings
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Add JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"] ?? "temporarysecretkeythatneedstobechangedlater12345");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Set to true in production
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        ClockSkew = TimeSpan.Zero
    };
});

// Add controllers
builder.Services.AddControllers();

// Add API Explorer and Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Dance Choreography Manager - Authentication API",
        Version = "v1",
        Description = "API for user authentication and profile management"
    });

    // Configure Swagger to use JWT authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Add simple health checks
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth API V1");
        c.RoutePrefix = string.Empty; // Serve the Swagger UI at the app's root
    });
    
    // Initialize database with retry logic
    try 
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();
            
            logger.LogInformation("Initializing database...");
            
            // Try each connection string until one works
            bool connected = false;
            Exception lastException = null;
            
            foreach (var connectionString in connectionStrings)
            {
                try
                {
                    logger.LogInformation("Attempting to connect with connection string: {ConnectionStart}...", 
                        connectionString.Substring(0, Math.Min(20, connectionString.Length)) + "...");
                    
                    // Create a new options instance with the current connection string
                    var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                    optionsBuilder.UseNpgsql(connectionString);
                    
                    // Create a new context with these options
                    using var dbContext = new ApplicationDbContext(optionsBuilder.Options);
                    
                    // Try to connect
                    dbContext.Database.EnsureCreated();
                    connected = true;
                    
                    logger.LogInformation("Successfully connected to the database!");
                    
                    // Initialize roles and admin user
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    
                    logger.LogInformation("Creating default roles and admin user...");
                    
                    // Create default roles
                    string[] roles = new[] { "Admin", "User" };
                    foreach (var role in roles)
                    {
                        if (!await roleManager.RoleExistsAsync(role))
                        {
                            await roleManager.CreateAsync(new IdentityRole(role));
                            logger.LogInformation("Created role: {Role}", role);
                        }
                    }
                    
                    // Create default admin user
                    var adminUser = await userManager.FindByEmailAsync("admin@example.com");
                    if (adminUser == null)
                    {
                        adminUser = new ApplicationUser
                        {
                            UserName = "admin@example.com",
                            Email = "admin@example.com",
                            FirstName = "Admin",
                            LastName = "User",
                            EmailConfirmed = true
                        };
                        
                        var result = await userManager.CreateAsync(adminUser, "Admin123!");
                        if (result.Succeeded)
                        {
                            logger.LogInformation("Created admin user");
                            await userManager.AddToRoleAsync(adminUser, "Admin");
                            logger.LogInformation("Added admin user to Admin role");
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                logger.LogError("Error creating admin user: {Error}", error.Description);
                            }
                        }
                    }
                    
                    break; // Exit the loop if we successfully connected
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    logger.LogWarning(ex, "Failed to connect with this connection string. Trying next one if available...");
                }
            }
            
            if (!connected)
            {
                logger.LogError(lastException, "All connection attempts failed!");
                throw new Exception("Could not connect to the database with any of the provided connection strings", lastException);
            }
            
            logger.LogInformation("Database setup completed");
        }
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred while initializing the database");
    }
}

// Use default endpoints (health checks, etc.)
app.MapDefaultEndpoints();

// Remove HTTPS redirection in development
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();