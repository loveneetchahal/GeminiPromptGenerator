using DotnetGeminiSDK;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGeminiClient(config =>
{
    config.ApiKey = "AIzaSyB2ZFstqywbQ-Q7gfuMH7aGJ3L5uiVTglQ";
    config.ImageBaseUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro-vision";
    config.TextBaseUrl = "https://generativelanguage.googleapis.com/v1/models/gemini-pro";
});


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
