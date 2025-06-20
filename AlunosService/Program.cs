using Microsoft.EntityFrameworkCore;
using AlunosService.Data;
using AlunosService.Repositories;
using AlunosService.Services;
using AlunosService.External;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configura��o do SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=alunos.db"));

// Configura��o do HttpClient para servi�os externos
builder.Services.AddHttpClient<IPagamentosServiceClient, PagamentosServiceClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ExternalServices:PagamentosService:BaseUrl"]
        ?? "https://localhost:7001");
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddHttpClient<ITreinosServiceClient, TreinosServiceClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ExternalServices:TreinosService:BaseUrl"]
        ?? "https://localhost:7002");
    client.Timeout = TimeSpan.FromSeconds(30);
});

// Inje��o de depend�ncia
builder.Services.AddScoped<IAlunoRepository, AlunoRepository>();
builder.Services.AddScoped<IAlunoService, AlunoService>();

// Configura��o de logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Configura��o de CORS se necess�rio
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configura��es de sa�de
builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppDbContext>();

var app = builder.Build();

// Aplicar migra��es automaticamente
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        context.Database.EnsureCreated();
        // Ou usar: context.Database.Migrate(); se estiver usando migra��es
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Erro ao criar/migrar banco de dados");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Alunos Service API V1");
        c.RoutePrefix = string.Empty; // Para acessar o Swagger na raiz
    });
}

app.UseHttpsRedirection();

// Usar CORS se configurado
if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowAll");
}

app.UseAuthorization();

app.MapControllers();

// Endpoint de sa�de
app.MapHealthChecks("/health");

// Endpoint de informa��es
app.MapGet("/info", () => new
{
    Service = "Alunos Service",
    Version = "1.0.0",
    Environment = app.Environment.EnvironmentName,
    Timestamp = DateTime.UtcNow
});

app.Run();