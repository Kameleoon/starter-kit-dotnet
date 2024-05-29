using Kameleoon;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Read the Kameleoon configuration parameters needed for authentication and site identification
var clientId = builder.Configuration["Kameleoon:ClientId"];
var clientSecret = builder.Configuration["Kameleoon:ClientSecret"];
var siteCode = builder.Configuration["Kameleoon:SiteCode"];
// For tests, we're using 'localhost', but on the real website, you need to set your top-level domain, e.g., ".example.com"
var topLevelDomain = builder.Configuration["Kameleoon:TopLevelDomain"];

// Kameleoon Integration
try
{
    // Create KameleoonClientConfig
    var config = new KameleoonClientConfig(clientId, clientSecret, topLevelDomain: topLevelDomain);
    // Create KameleoonClient
    var client = KameleoonClientFactory.Create(siteCode, config);
    // Wait for initialization
    await client.WaitInit();
    // Bind KameleoonClient to Services
    builder.Services.AddSingleton(client);
} // Handle Kameleoon exceptions
catch (KameleoonException exception)
{
    if (exception is KameleoonException.ConfigCredentialsInvalid ||
        exception is KameleoonException.SiteCodeIsEmpty)
    {
        Console.WriteLine($"Expected KameleoonException: {exception}");
    }
    else
        Console.WriteLine($"Unexpected KameleoonException: {exception.Message}");
    throw; // Re-throwing exceptions is generally not recommended; this is for demonstration only.
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
