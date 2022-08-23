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

#### Como verificar se é possível a aplicação conectar-se ao banco de dados?
```
using var contexto = new ExemploContext();

// Retorna um booleano
bool podeConectar = contexto.Database.CanConnect();
```

#### Como garantir que o banco de dados seja criado durante a inicialização da aplicação?
```
using var contexto = new ExemploContext();

contexto.Database.EnsureCreated();
```

#### Como garantir que o banco de dados seja deletado durante a inicialização da aplicação?
```
using var contexto = new ExemploContext();

contexto.Database.EnsureDeleted();
```

#### Como exeutar comandos SQL no banco de dados com Entity Framework Core
```
using var contexto = new ExemploContext();

var ativo = true;

var codFunc = 1;

// 1º Opção
contexto.Database.ExecuteSqlRaw("update Funcionarios set ativo = {0} where codFunc = {1}", ativo, codFunc);

// 2º Opção
contexto.Database.ExecuteSqlInterpolated($"update Funcionarios set ativo = {ativo} where codFunc = {codFunc}");

// 3º Opção
using (var cmd = contexto.Database.GetDbConnection().CreateCommand())
{
    cmd.CommandText = "update Funcionarios set ativo = @ativo where codFunc = @codFunc";

    var parametroAtivo = cmd.CreateParameter();
    parametroAtivo.ParameterName = "@ativo";
    parametroAtivo.Value = ativo;
    cmd.Parameters.Add(parametroAtivo);

    var parametroCodFunc = cmd.CreateParameter();
    parametroCodFunc.ParameterName = "@codFunc";
    parametroCodFunc.Value = codFunc;
    cmd.Parameters.Add(parametroCodFunc);

    cmd.ExecuteNonQuery();
}
```

#### Como criar Stored Procedure para buscar dados com o EF Core?
```
var criarProcedureBuscaProdutosPorId = @"
    CREATE OR ALTER PROCEDURE ObterProdutosPorId
        @Id int
    AS
    BEGIN
        SELECT * FROM Produtos Where Id = @Id
    END";

using var contexto = new ProdutoContext();

contexto.Database.ExecuteSqlRaw(criarProcedureBuscaProdutosPorId);
```
#### Como realizar consulta utilizando Stored Procedure com Entity Framework Core?
```
using var contexto = new ProdutoContext();

var idProduto = new SqlParameter("@Id", 1);

var produtos = contexto.Produtos
    .FromSqlInterpolated($"EXECUTE ObterProdutosPorId {idProduto}")
    .ToList();
```
#### Como criar Stored Procedure para inserir dados com o EF Core?
```
var criarProdutoComStoredProcedure = @"
    CREATE OR ALTER PROCEDURE CriarProduto
        @Descricao VARCHAR(50),
        @Ativo bit,
        @Preco decimal(10, 2),
        @Id int
    AS
    BEGIN
        INSERT INTO 
            Produtos(Descricao, Ativo, Preco, Id) 
        VALUES (@Descricao, @Ativo, @Preco, @Id)
    END ";

using var contexto = new ProdutoContext();

contexto.Database.ExecuteSqlRaw(criarProdutoComStoredProcedure);
```


