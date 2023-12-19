using Microsoft.EntityFrameworkCore;
using Website_Bookmark_Desktop_App_API.Data;
using Website_Bookmark_Desktop_App_API.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Website_Bookmark_Desktop_App_API.Models;
using Microsoft.OpenApi.Models;


// Project reference video: https://www.youtube.com/watch?v=9zJn3a7L1uE&t=2597s
// Saving database changes reference video: https://www.youtube.com/watch?v=8pH5Lv4d5-g
// User auth reference video: https://www.youtube.com/watch?v=KRVjIgr-WOU&t=9281s

// After having changed database from SQL to SQLite, had to go into migrations and change any "nvarchar(max)" to "nvarchar(255)" because SQLite does not like "max"

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddControllersWithViews();
//builder.Services.AddRazorPages();

// This step lets the API know to connect to the database denoted by "DefaultConnection" in the "appsettings.json"
//builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Changed from SQL Server to SQLite instead, enables me to keep the database contained with the main backend application
builder.Services.AddDbContext<DataContext>(x => x.UseSqlite(builder.Configuration.GetConnectionString("localDB")));
builder.Services.AddDbContext<UserDataContext>(x => x.UseSqlite(builder.Configuration.GetConnectionString("localDB")));

// Adding identity user class
builder.Services.AddIdentity<User, IdentityRole>(identity =>
{
	// Configuring identity user class
	identity.SignIn.RequireConfirmedAccount = false;
    identity.User.RequireUniqueEmail = false;
    identity.Password.RequireNonAlphanumeric = false;
    identity.Password.RequireDigit = false;
    identity.Password.RequireLowercase = false;
    identity.Password.RequireUppercase = false;
	identity.Password.RequiredLength = 4;
})
.AddEntityFrameworkStores<UserDataContext>()
.AddDefaultTokenProviders();

// Assing authentification and JWT Bearer
builder.Services.AddAuthentication(options =>
{
	options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.SaveToken = true;
	options.RequireHttpsMetadata = false;
	options.TokenValidationParameters = new TokenValidationParameters()
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
		ValidAudience = builder.Configuration["JWT:ValidAudience"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
	};
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(ctx =>
{
	// Reference: https://www.pragimtech.com/blog/azure/how-to-use-swagger-in-asp.net-core-web-api/
	ctx.SwaggerDoc("v1", new OpenApiInfo
	{
		Version = "v1",
		Title = "Desktop Bookmark API",
		Description = "Mock-up API for a webapp idea which involves saving website bookmarks to a separate website which displays them in a desktop-like manner.\n" +
		"This would make website bookmarks browser agnostic and enable users to access their website bookmarks on from any browser anywhere!"
	});
	// Add authorize button in Swagger UI ref: https://code-maze.com/swagger-authorization-aspnet-core/
	// This step tells Swagger the type of protection is wanted for the API
	ctx.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Please enter token",
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		BearerFormat = "JWT",
		Scheme = "bearer"
	});
	// This step adds the global security requirement
	ctx.AddSecurityRequirement(new OpenApiSecurityRequirement()
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
			new string[]{}
        }
	});
	// With these steps, Authorize-button should appear in Swagger UI, a good thing is to have [Authorize] set up for controllers as well
});

// Auto-mapper from Microsoft to map slightly differing data objects automatically (useful between data-classes and DTO's)
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Makes sure that the API controller knows that each time a request is received whose operation needs the IDesktopBookmarkService dependency injection, that DesktopBookmarkService is used as the worker class
// This makes it easy to switch worker class for a given interface dependency injection -- literally just swap in for the worker class wanted
// AddScoped -- creates a new instance of the requested service for every incoming request
// AddTransient -- provides a new instance to every controller and service, even within the same request
// AddSingleton -- creates one instance which is used for every request
builder.Services.AddScoped<IDesktopBookmarkService, DesktopBookmarkService>();
builder.Services.AddScoped<IUserAuthService, UserAuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
else if (app.Environment.IsProduction())
{
	app.UseSwagger();

	// Reference: https://www.pragimtech.com/blog/azure/how-to-use-swagger-in-asp.net-core-web-api/
	app.UseSwaggerUI(ctx =>
	{
		// Think this enables the endpoint to connect in "production"
		ctx.SwaggerEndpoint("/swagger/v1/swagger.json", "Desktop Bookmark API V1");
		// This enables to connect via "localhost:{port}" instead of having to use "localhost:{port}/swagger"
		ctx.RoutePrefix = string.Empty;
	});
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
