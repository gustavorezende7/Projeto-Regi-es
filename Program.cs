using Microsoft.EntityFrameworkCore;
using RegioesApi.Data;
using RegioesApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configura serviços
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configura o DbContext com PostgreSQL
var conexao = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(conexao));

// Repositório
builder.Services.AddScoped<IRegiaoRepository, RegiaoRepository>();

var app = builder.Build();

// Aplica migrações automaticamente (apenas em dev)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
