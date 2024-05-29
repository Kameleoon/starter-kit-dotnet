using Kameleoon;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var clientId = builder.Configuration["Kameleoon:ClientId"];
var clientSecret = builder.Configuration["Kameleoon:ClientSecret"];
var siteCode = builder.Configuration["Kameleoon:SiteCode"];
var topLevelDomain = builder.Configuration["Kameleoon:TopLevelDomain"];

// Kameleoon Integration
try
{
    var config = new KameleoonClientConfig(clientId, clientSecret, topLevelDomain: topLevelDomain);
    var client = KameleoonClientFactory.Create(siteCode, config);
    await client.WaitInit();
    builder.Services.AddSingleton(client);
}
catch (KameleoonException exception)
{
    if (exception is KameleoonException.ConfigCredentialsInvalid ||
        exception is KameleoonException.SiteCodeIsEmpty)
    {
        Console.WriteLine($"Expected KameleoonException: {exception}");
    }
    else
        Console.WriteLine($"Unexpected KameleoonException: {exception.Message}");
    throw;
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
