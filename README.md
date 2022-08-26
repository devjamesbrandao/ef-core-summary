### Estudos sobre Entity Framework Core

#### Como limpar/remover objetos trackeados na memória do Entity Framework Core?
```
contexto.ChangeTracker.Clear();
```

#### Como gerar script do banco de dados ?
```
// ExemploContext utilizado nesse e nos próximos exemplos pode ser encontrado na pasta 'Data'
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

#### Como criar filtros globais para consultas realizadas no banco de dados utilizando LINQ?
```
// Nós configuramos o filtro global no DbContext da nossa aplicação
public class ProdutoContext : DbContext
{
    public DbSet<Produto> Produtos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("CONNECTION_STRING");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aqui configuramos o filtro global
        // Nesse caso, toda vez que realizarmos uma query no banco de dados na tabela de produtos utilizando o LINQ do 
        // Entity Framework Core, será adicionado um filtro para trazer somente os produtos os quais o campo ativo 
        // seja igual a 'true' modelBuilder.Entity<Produto>().HasQueryFilter(p => p.Ativo == true);
    }
}

using var contexto = new ProdutoContext();

// Exemplo de query
var produtos = contexto.Produtos.AsNoTracking().ToListAsync();

// Como resultado da consulta acima, teremos uma query similar a 'SELECT [p].[Id], [p].[Descricao], [p].[Ativo], [p].[Preco] 
// FROM [Produtos] AS [p] WHERE [p].[Ativo] = true'. Embora nós não tenhamos adicionado nenhum filtro na consulta acima 
// utilizando LINQ, o filtro global adicionou um filtro de forma automática, pois configuramos isso no DbContext 
// (modelBuilder.Entity<Produto>().HasQueryFilter(p => p.Ativo == true)).
```

#### Como ignorar filtros globais para consultas realizadas no banco de dados utilizando LINQ?
```
using var contexto = new ProdutoContext();

// Nesse caso, o filtro global será ignorado
var produtos = contexto.Produtos.AsNoTracking().IgnoreQueryFilters().ToListAsync();
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

// ProdutoContext utilizado nesse e nos próximos exemplos pode ser encontrado na pasta 'Data'
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

#### Como inserir dados utilizando Stored Procedure com o EF Core?
```
using var contexto = new ProdutoContext();

contexto.Database.ExecuteSqlRaw("execute CriarProduto @p0, @p1, @p2, @p3", "Camisa do SHREK", true, 50, 1);
```
