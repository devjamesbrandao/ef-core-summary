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
