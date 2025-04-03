using SharpLogShield.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Aggiungi OpenAPI e Swagger UI
builder.Services.AddEndpointsApiExplorer();  // Necessario per OpenAPI
builder.Services.AddSwaggerGen();            // Aggiunge il supporto per la generazione della documentazione Swagger

// Aggiungi Logging
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddConsole();
    loggingBuilder.AddSharpLogShieldLogging();
});

var app = builder.Build();

// Configura la pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();  // Genera la documentazione OpenAPI
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"); // Definisce il punto finale di Swagger
        options.RoutePrefix = string.Empty; // Imposta la route di Swagger UI come home (opzionale)
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();