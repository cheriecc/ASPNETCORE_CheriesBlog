using System.Text;
using CheriesBlog.Application.Services;
using CheriesBlog.Domain.Interface;
using CheriesBlog.Domain.Models;
using CheriesBlog.Infrastructure.Data;
using CheriesBlog.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

// Add services to the container.
builder.Services.AddDbContext<BlogDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        optionsBuilder => optionsBuilder.MigrationsAssembly("CheriesBlog.Infrastructure"));
});

//Add Identity services
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    // Configure password settings
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;

    // Configure user settings
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<BlogDbContext>()
.AddDefaultTokenProviders();


//Add services
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// Register IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Register repositories and services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IPostRepository, PostRepository>();


builder.Services.AddCors((options) =>
    {
        options.AddPolicy("DevCors", (corsBuilder) =>
            {
                corsBuilder.WithOrigins("http://localhost:4200", "http://localhost:3000", "http://localhost:8000")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        options.AddPolicy("ProdCors", (corsBuilder) =>
            {
                corsBuilder.WithOrigins("https://myProductionSite.com")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
    });


// string? tokenKeyString = builder.Configuration.GetSection("AppSettings:TokenKey").Value;

// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters()
//         {
//             ValidateIssuerSigningKey = true,
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
//                     tokenKeyString != null ? tokenKeyString : ""
//                 )),
//             ValidateIssuer = false,
//             ValidateAudience = false
//         };
//     });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
    // app.UseSwagger();
    // app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);
// using (var scope = app.Services.CreateScope())
// {
//     var context = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
//     context.Database.Migrate();
// }

app.Run();
