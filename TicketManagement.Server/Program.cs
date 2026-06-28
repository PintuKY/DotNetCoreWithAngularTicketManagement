using Microsoft.EntityFrameworkCore;
using TicketManagement.Server.DBContexts;
using TicketManagement.Server.Objects;
using TicketManagement.Server.Repositorys.OnlineEducation;
using TicketManagement.Server.Services.OnlineEducation;


//Start Create Builder Creates Web App Builder object "Initialize web server configuration" Web API startup configuration
var builder = WebApplication.CreateBuilder(args);
//End Create Builder

// Start Add DbContext Registers Entity Framework with SQL Server
builder.Services.AddDbContext<AppDatabaseContext>(options =>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//End Add DbContext

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
//End AddScoped

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

//Start Redirects http → https.
app.UseHttpsRedirection();
//End Redirects http → https.

// Enable CORS Activates the policy created earlier
//app.UseCors("AllowAngular");
app.UseCors("AllowAll");
//End Enable cross

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

