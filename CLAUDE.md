# CLAUDE.md — Gestão de Ordens de Serviço

## Visão Geral do Projeto

Sistema desktop de **Gestão de Ordens de Serviço (OS)** com controle financeiro, auditoria e relatórios. Desenvolvido em C# .NET Framework 4.6 com Windows Forms e PostgreSQL.

O sistema permite cadastrar clientes e serviços, abrir ordens de serviço, adicionar itens com cálculo automático de valores, controlar status, auditar alterações e gerar relatórios gerenciais com exportação PDF.

---

## Stack Obrigatória

- **C# .NET Framework 4.6**
- **Windows Forms** — interface desktop
- **PostgreSQL** — banco de dados (Docker)
- **Npgsql** — driver de conexão com PostgreSQL (sem ORM)
- **ReportViewer** — relatórios gerenciais

---

## Estrutura da Solution

```
GestaoOrdensServico.sln
│
├── GestaoOrdensServico              ← WinForms — telas e eventos
├── GestaoOrdensServico.Domain       ← entidades, enums, regras
├── GestaoOrdensServico.Application  ← services, interfaces, Result Pattern
└── GestaoOrdensServico.Infrastructure ← repositories, conexão, log
```

### Dependências entre projetos

```
WinForms       → depende de Application e Domain
Application    → depende de Domain e Infrastructure
Infrastructure → depende de Domain
Domain         → não depende de ninguém
```

---

## Estrutura de Pastas

### GestaoOrdensServico (WinForms)

```
GestaoOrdensServico/
├── Forms/
│   ├── FormPrincipal.cs
│   ├── FormClientes.cs
│   ├── FormClientesCadastro.cs
│   ├── FormServicos.cs
│   ├── FormServicosCadastro.cs
│   ├── FormOrdensServico.cs
│   ├── FormOrdensServicoCadastro.cs
│   └── FormRelatorio.cs
├── Program.cs
└── App.config
```

### GestaoOrdensServico.Domain

```
Domain/
├── Entities/
│   ├── Cliente.cs
│   ├── Servico.cs
│   ├── OrdemServico.cs
│   └── ItemOs.cs
├── Enums/
│   └── StatusOs.cs
└── Auditoria/
    └── AuditoriaRegistro.cs
```

### GestaoOrdensServico.Application

```
Application/
├── Services/
│   ├── ClienteService.cs
│   ├── ServicoService.cs
│   ├── OrdemServicoService.cs
│   └── RelatorioService.cs
├── Relatorios/
│   └── RelatorioOsItem.cs
└── ResultadoOperacao.cs
```

### GestaoOrdensServico.Infrastructure

```
Infrastructure/
├── Connection/
│   └── DbConnectionFactory.cs
├── Logging/
│   └── Logger.cs
└── Repositories/
    ├── ClienteRepository.cs
    ├── ServicoRepository.cs
    ├── OrdemServicoRepository.cs
    └── AuditoriaRepository.cs
```

---

## Banco de Dados

### Conexão

PostgreSQL rodando em Docker. String de conexão no `App.config`:

```xml
<connectionStrings>
  <add name="DefaultConnection"
       connectionString="Host=localhost;Port=5432;Database=gestao_os;Username=postgres;Password=postgres"
       providerName="Npgsql" />
</connectionStrings>
```

### Tabelas

- `clientes` — cadastro de clientes com constraint unique em documento
- `servicos` — cadastro de serviços com constraints de valor e percentual
- `ordens_servico` — OS com controle de status (ENUM) e campo `versao` para concorrência otimista
- `itens_os` — itens da OS com cópia dos valores do serviço no momento da criação
- `auditoria` — snapshot JSON de toda alteração relevante no sistema

### Índices obrigatórios

```sql
idx_clientes_documento
idx_os_cliente_id
idx_os_status
idx_os_data_abertura
idx_itens_os_ordem_servico_id
idx_auditoria_entidade
idx_auditoria_id_registro
```

---

## Padrões de Código

### Result Pattern

Toda operação de service retorna `ResultadoOperacao<T>`. Nunca lançar exceção de negócio — retornar falha com mensagem.

```csharp
// Sucesso
return ResultadoOperacao<Cliente>.Sucesso(cliente);

// Falha de negócio
return ResultadoOperacao<Cliente>.Falha("Cliente não encontrado.");

// No Form
var resultado = _service.Criar(...);
if (!resultado.Sucedeu)
{
    MessageBox.Show(resultado.Mensagem, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
    return;
}
```

### Repositories

- Recebem `DbConnectionFactory` no construtor
- Abrem e fecham conexão dentro de cada método com `using`
- Usam parâmetros nomeados `@parametro` — nunca concatenação de string
- Não contêm regra de negócio
- Método privado `Mapear...` para converter `NpgsqlDataReader` em entidade

```csharp
public TEntidade BuscarPorId(Guid id)
{
    using (var connection = _factory.CreateConnection())
    {
        connection.Open();
        using (var command = new NpgsqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@id", id);
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                    return MapearEntidade(reader);
                return null;
            }
        }
    }
}
```

### Transações

Toda operação que envolve mais de uma tabela usa transação. O `NpgsqlCommand` deve receber a `transaction` como terceiro parâmetro.

```csharp
using (var transaction = connection.BeginTransaction())
{
    try
    {
        // comandos com (sql, connection, transaction)
        transaction.Commit();
    }
    catch (Exception ex)
    {
        transaction.Rollback();
        throw;
    }
}
```

### Operações que exigem transação

- Inserir OS → insere OS + auditoria
- Adicionar item → insere item + atualiza valor total da OS + auditoria
- Remover item → remove item + atualiza valor total da OS + auditoria
- Alterar status → atualiza OS + insere auditoria

### Concorrência Otimista

Campo `versao` em `ordens_servico`. Todo UPDATE inclui `AND versao = @versaoAtual` e incrementa `versao = versao + 1`. Se `ExecuteNonQuery()` retornar 0 linhas afetadas — lançar exceção de concorrência.

```csharp
int linhasAfetadas = command.ExecuteNonQuery();
if (linhasAfetadas == 0)
    throw new Exception("A OS foi alterada por outro usuário. Recarregue e tente novamente.");
```

### Auditoria

Registrar na tabela `auditoria` ao:
- Alterar status da OS
- Inserir, alterar ou remover item
- Alterar valor total da OS

O snapshot é um JSON serializado com `System.Web.Script.Serialization.JavaScriptSerializer` ou `Newtonsoft.Json` representando o estado **antes** da alteração.

### Valores Nullable no Npgsql

Campos nullable no banco precisam de tratamento especial:

```csharp
// Escrita
command.Parameters.AddWithValue("@observacao", (object)os.Observacao ?? DBNull.Value);

// Leitura
Observacao = reader.IsDBNull(reader.GetOrdinal("observacao"))
    ? null
    : reader.GetString(reader.GetOrdinal("observacao"))
```

### Log

Usar `Logger` para:
- `LogInfo` — operações bem-sucedidas relevantes
- `LogErro` — toda exceção capturada no catch dos services

---

## Services

- Recebem repositories e Logger no construtor
- Aplicam todas as regras de negócio antes de chamar o repository
- Retornam `ResultadoOperacao<T>` — nunca void em operações que podem falhar
- Capturam exceções inesperadas com try/catch, logam e retornam falha

### Regras de negócio por entidade

**Cliente:**
- Não permitir exclusão se existir OS vinculada
- Documento único (capturar PostgresException com código 23505)

**Serviço:**
- ValorBase > 0
- PercentualImposto entre 0 e 100

**Ordem de Serviço:**
- Não permitir edição de itens se status for Concluída ou Cancelada
- Não permitir concluir OS com valor total = 0
- ValorTotal recalculado a cada alteração de item

**Item OS:**
- ValorUnitario copiado do Serviço no momento da criação
- PercentualImpostoAplicado copiado do Serviço no momento da criação
- ValorTotalItem = (Quantidade × ValorUnitario) × (1 + Percentual / 100)

---

## WinForms

### Convenções de nomenclatura de componentes

| Tipo | Prefixo | Exemplo |
|---|---|---|
| TextBox | txt | txtNome |
| ComboBox | cmb | cmbTipo |
| Button | btn | btnSalvar |
| DataGridView | dgv | dgvClientes |
| Label | lbl | lblNome |
| DateTimePicker | dtp | dtpInicio |
| CheckBox | chk | chkAtivo |
| ReportViewer | reportViewer | reportViewer1 |

### Padrão de Form de listagem

```
Topo    → filtros + botão pesquisar
Centro  → DataGridView com binding (DataSource)
Rodapé  → botões Novo, Editar, Excluir
```

### Padrão de Form de cadastro

```
Campos  → TextBox, ComboBox, DateTimePicker
Rodapé  → botões Salvar e Cancelar
```

### Binding

- **DataGridView** — usar `DataSource` para binding da lista
- **Campos individuais** — preencher e ler manualmente (sem binding)

### Navegação entre Forms

```csharp
// Abre form de cadastro como modal
var form = new FormClientesCadastro(_clienteService, cliente);
if (form.ShowDialog() == DialogResult.OK)
    CarregarClientes(); // recarrega a grid após salvar
```

### Paginação na listagem de OS

Listagem principal de OS deve ter paginação (LIMIT/OFFSET). Itens da OS só carregam ao abrir a OS — não na grid principal.

---

## Relatório

### Objeto de projeção

```csharp
public class RelatorioOsItem
{
    public string ClienteNome    { get; set; }
    public string OsId           { get; set; }
    public DateTime DataAbertura { get; set; }
    public string Status         { get; set; }
    public decimal ValorTotal    { get; set; }
    public decimal TotalImpostos { get; set; }
}
```

### Filtros disponíveis

- Período (data início e data fim)
- Cliente (opcional)
- Status (opcional)

### Estrutura do relatório

- Agrupado por cliente
- Total por cliente
- Total geral
- Total de impostos
- Quantidade total de OS no período

### Exportação PDF

```csharp
byte[] bytes = reportViewer1.LocalReport.Render("PDF");
// salvar com SaveFileDialog
```

---

## Tratamento de Erros

### Constraint unique (documento duplicado)

```csharp
catch (PostgresException ex) when (ex.SqlState == "23505")
{
    return ResultadoOperacao<Cliente>.Falha("Já existe um cliente com este documento.");
}
```

### Constraint FK (exclusão bloqueada)

```csharp
catch (PostgresException ex) when (ex.SqlState == "23503")
{
    return ResultadoOperacao<Cliente>.Falha("Não é possível excluir. Existem OS vinculadas a este cliente.");
}
```

### Concorrência

```csharp
catch (Exception ex) when (ex.Message.Contains("alterada por outro usuário"))
{
    return ResultadoOperacao<OrdemServico>.Falha(ex.Message);
}
```

---

## Wiring Manual (Program.cs)

```csharp
static void Main()
{
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);

    string connectionString = ConfigurationManager
        .ConnectionStrings["DefaultConnection"].ConnectionString;

    var factory = new DbConnectionFactory(connectionString);
    var logger  = new Logger();

    var clienteRepo      = new ClienteRepository(factory);
    var servicoRepo      = new ServicoRepository(factory);
    var ordemServicoRepo = new OrdemServicoRepository(factory);
    var auditoriaRepo    = new AuditoriaRepository(factory);

    var clienteService      = new ClienteService(clienteRepo, logger);
    var servicoService      = new ServicoService(servicoRepo, logger);
    var ordemServicoService = new OrdemServicoService(ordemServicoRepo, clienteRepo, servicoRepo, auditoriaRepo, logger);
    var relatorioService    = new RelatorioService(ordemServicoRepo, logger);

    Application.Run(new FormPrincipal(clienteService, servicoService, ordemServicoService, relatorioService));
}
```
