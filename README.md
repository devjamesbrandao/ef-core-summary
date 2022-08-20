### Estudos sobre Entity Framework Core

#### Como limpar/remover objetos trackeados na memória do Entity Framework Core?
```
contexto.ChangeTracker.Clear();
```

#### Como gerar script do banco de dados ?
```
using var contexto = new ExemploContext();

var scriptSql = contexto.Database.GenerateCreateScript();

Console.WriteLine(scriptSql);
```
#### Como obter migrações aplicadas no banco de dados?
```
using var contexto = new ExemploContext();

var migracoes = contexto.Database.GetAppliedMigrations();
```
#### Como obter todas as migrações (aplicadas, pendentes) ?
```
using var contexto = new ExemploContext();

var migracoes = contexto.Database.GetMigrations();
```

#### Como aplicar migrações em tempo de execução (quando executa o comando dotnet run)?
```
using var contexto = new ExemploContext();

contexto.Database.Migrate();
```

#### Como obter migrações pendentes?
```
using var contexto = new ExemploContext();

var migracoesPendentes = contexto.Database.GetPendingMigrations();
```
