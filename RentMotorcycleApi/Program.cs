using RentMotorcycle.Business.AppSettings;
using RentMotorcycleApi.Configuration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddSerilog(dispose: true);
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var appSettings = builder.Configuration.Get<AppSettings>()!;
builder.Services.ConfigureLifeTime(appSettings);

builder.Services.AddJwtConfig(builder.Configuration);

//start o processo do consumer
//Process.Start("dotnet", "run --project ..//RentMotorcycleRabbitMQ//RentMotorcycleRabbitMQ.csproj");



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
