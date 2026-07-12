using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TicketManagement.Server.DBContexts;
using TicketManagement.Server.Objects;
using TicketManagement.Server.Repositorys.OnlineEducation;
using TicketManagement.Server.Services;
using TicketManagement.Server.Services.OnlineEducation;
using Microsoft.AspNetCore.Identity;

//Start Create Builder Creates Web App Builder object "Initialize web server configuration" Web API startup configuration
var builder = WebApplication.CreateBuilder(args);
//End Create Builder

//string jwtSettings = builder.Configuration.GetValue<string>("JWT_KEY")!;
//string jwtKey = Environment.GetEnvironmentVariable("JWT_KEY")!;
//var jwtKey1 = builder.Configuration["JWT_KEY"];
//Console.WriteLine($"JWT KEY = {jwtKey1}");
var key = builder.Configuration["JWT_KEY"];
// Start Add DbContext Registers Entity Framework with SQL Server
builder.Services.AddDbContext<AppDatabaseContext>(options =>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//End Add DbContext

//Use it for JWT
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//.AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(
//            Encoding.UTF8.GetBytes(jwtKey!)),

//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,

//        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//        ValidAudience = builder.Configuration["Jwt:Audience"]
//    };
//});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            //ValidIssuer = builder.Configuration["Jwt:Issuer"], //this logic for appsettings.json file
            //ValidAudience = builder.Configuration["Jwt:Audience"], //this logic for appsettings.json file

            ValidIssuer = builder.Configuration["JWT_ISSUER"],//this logic for launchSttings.json file

            ValidAudience = builder.Configuration["JWT_AUDIENCE"],

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!))
        };
});
builder.Services.AddAuthorization();

//End Use it for JWT

//Start Add Controllers Without this → controllers won’t workUseSwagger
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler =
        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
//End Add Controllers

//Start Add OpenAPI (Swagger) for Enables Swagger documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();
//End Add OpenAPI

//Start Add CORS This allows: Angular (localhost:4200) Access .NET API Without this → CORS error
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", 
        policy =>{
            policy.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
             .AllowAnyHeader();
        });
});
//End Add CORS

//Start AddScoped registers a service in Dependency Injection container with Scoped lifetime [Scoped = one instance per HTTP request]
builder.Services.AddScoped<IListQuestions, ListQuestionsService>();
builder.Services.AddScoped<ISyllabus, ServicesSyllabus>();
builder.Services.AddScoped<QuestionsDatas>();

// Register Test service so ITestService can be resolved by DI
builder.Services.AddScoped<ITestService, TestService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserProfilesService, UserProfilesService>();
//End AddScoped
builder.Services.AddScoped<IPasswordHasher<TicketManagement.Server.Models.OnlineEducation.Users>, PasswordHasher<TicketManagement.Server.Models.OnlineEducation.Users>>();

//Start Build App for App instance is creating
var app = builder.Build();
//End Build App

//Start for UseSwagger
app.UseSwagger();
app.UseSwaggerUI();
//End for UseSwagger

//Static Files Support [Used when Angular is built and placed inside wwwroot]
app.UseDefaultFiles();
app.MapStaticAssets();
//End Static Files Support

//Start Development Mode Swagger Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
//End Development Mode Swagger


//app.UseAuthorization();

//Order matters:

//Start Redirects http → https.
app.UseHttpsRedirection();
//End Redirects http → https.
app.UseStaticFiles();
app.UseRouting();
// Enable CORS Activates the policy created earlier
//app.UseCors("AllowAngular");
app.UseCors("AllowAll");
//End Enable cross
app.UseAuthentication();
//Start Authorization Middleware
app.UseAuthorization();
//End Authorization Middleware
//Start Map Controllers Without this → API won't work.
app.MapControllers();
//End Map Controllers

//Start Fallback Used for Angular routing If user refreshes So Angular handles routing
app.MapFallbackToFile("/index.html");
//End Fallback

//Starts Kestrel web server ,Kestrel is ASP.NET Core’s cross-platform web server that listens for HTTP requests and sends responses.
app.Run();

/*Service Lifetime Types (Very Important)
 * AddTransient Every time  New instance each use lightweight services
 * AddScoped  Per request  Once per request DB services
 * AddSingleton  Whole app Once at app start caching, config
 */

// --- partial file with added DI registration for password hasher ---
// Add near other services registration

// Ensure IUserProfilesService already registered (you had it): keep as-is
//builder.Services.AddScoped<IUserProfilesService, UserProfilesService>();
