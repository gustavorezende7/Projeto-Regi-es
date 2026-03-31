# RegioesApi

Projeto .NET 8 (C#) - API REST para cadastro de regiões (UF + Nome)

## Requisitos
- .NET 8 SDK
- PostgreSQL (padrão: postgres/postgres)
- Docker (opcional)

## Rodando com Docker
```bash
docker-compose up --build
```
A API ficará disponível em http://localhost:5000

## Rodando local (sem Docker)
1. Ajuste a connection string em appsettings.json se necessário.
2. Instale dotnet-ef:
   ```bash
   dotnet tool install --global dotnet-ef
   ```
3. Crie migração:
   ```bash
   dotnet ef migrations add Inicial
   dotnet ef database update
   ```
4. Rode:
   ```bash
   dotnet run
   ```

## Endpoints
- GET /regioes?incluirInativas=true
- GET /regioes/{id}
- POST /regioes
- PUT /regioes/{id}
- PATCH /regioes/{id}?ativo=false

