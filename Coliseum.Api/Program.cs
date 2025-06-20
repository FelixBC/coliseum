var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Configure Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ColiseumContext>()
    .AddDefaultTokenProviders();

// Configure JWT
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Configure CORS for MAUI client
builder.Services.AddCors(options =>
{
    options.AddPolicy("MAUICorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Configure database
builder.Services.AddDbContext<ColiseumContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? 
        "Data Source=coliseum.db"));

// Register FileStorageService
builder.Services.AddScoped<IFileStorageService, FileStorageService>();

// Configure static files
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Create media directory if it doesn't exist
var webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
var mediaPath = Path.Combine(webRoot, "media");
Directory.CreateDirectory(mediaPath);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "media")),
    RequestPath = "/media"
});

// Add authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

app.UseCors("MAUICorsPolicy");
app.MapControllers();

app.Run();
