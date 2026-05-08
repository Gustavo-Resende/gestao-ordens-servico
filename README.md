# Gestão de Ordens de Serviço

Sistema desktop de gestão de ordens de serviço com controle financeiro, auditoria e relatórios. Desenvolvido em C# .NET Framework 4.6 com Windows Forms e PostgreSQL.

---

## Stack

- **C# .NET Framework 4.6**
- **Windows Forms** — interface desktop
- **PostgreSQL** — banco de dados
- **Npgsql** — acesso a dados sem ORM
- **ReportViewer** — relatórios gerenciais com exportação PDF

---

## Arquitetura

O projeto segue os princípios do Clean Architecture com separação em 4 camadas:
src/
├── GestaoOrdensServico/              ← UI (Windows Forms)
├── GestaoOrdensServico.Application/  ← Services e regras de negócio
├── GestaoOrdensServico.Domain/       ← Entidades e enums
└── GestaoOrdensServico.Infrastructure/ ← Repositories, conexão e log

**Fluxo de uma requisição:**
Form → Service → Repository → PostgreSQL

- **Domain** — entidades puras sem dependências externas
- **Application** — services com regras de negócio, Result Pattern sem bibliotecas externas
- **Infrastructure** — acesso a dados com Npgsql puro, log em arquivo
- **UI** — Windows Forms, sem lógica de negócio, apenas eventos e chamadas aos services

---

## Decisões Técnicas

### Acesso a dados
Npgsql puro com NpgsqlConnection e NpgsqlCommand. Sem ORM. Parâmetros nomeados em todas as queries para prevenção de SQL Injection. Mapeamento manual das entidades via NpgsqlDataReader.

### Result Pattern
Todas as operações de service retornam ResultadoOperacao<T> com Sucedeu e Mensagem. Exceções de negócio são tratadas com if — exceptions apenas para erros inesperados.

### Injeção de Dependência
Manual — sem container. O wiring é feito em Program.cs de dentro para fora: banco → repositories → services → handlers → forms.

### Interfaces
Definidas no lado do consumidor (Application), não no lado da implementação (Infrastructure). Duck typing implícito.

### Log
Logger próprio gravando em arquivo diário na pasta logs/ do executável. Formato: [data hora] NIVEL mensagem.

---

## Estratégia de Concorrência

Controle de concorrência otimista via campo `versao` (integer) na tabela `ordens_servico`.

**Funcionamento:**
1. Usuário A abre a OS — versao = 1
2. Usuário B abre a mesma OS — versao = 1
3. Usuário A salva → UPDATE WHERE id = @id AND versao = 1 → incrementa versao para 2
4. Usuário B tenta salvar → UPDATE WHERE id = @id AND versao = 1 → 0 linhas afetadas → erro de concorrência

O segundo usuário recebe mensagem: "A OS foi alterada por outro usuário. Recarregue e tente novamente."

---

## Transações

Operações que envolvem múltiplas tabelas são executadas em transação única com rollback obrigatório em caso de falha:

- Inserir OS → OS + auditoria
- Adicionar item → item + recalculo valor total + auditoria
- Remover item → item + recalculo valor total + auditoria
- Alterar status → status + auditoria

---

## Auditoria

Toda alteração relevante gera um registro na tabela `auditoria` com:
- Entidade e ID do registro alterado
- Operação (INSERT/UPDATE/DELETE)
- Data/hora
- Usuário
- Snapshot JSON do estado anterior

---

## Como Rodar

### Pré-requisitos

- Visual Studio 2022+
- .NET Framework 4.6.1 Developer Pack
- Docker Desktop
- SQLSysClrTypes.msi (x64) - dependência do ReportViewer 2015, disponível no SQL Server 2014 Feature Pack da Microsoft
- Microsoft Report Viewer 2015 Runtime

### 1. Subir o banco de dados

```bash
docker-compose up -d
```

O script `scripts/init.sql` é executado automaticamente na primeira inicialização, criando todas as tabelas, constraints e índices.

PgAdmin disponível em: `http://localhost:8080`
- Email: admin@admin.com
- Senha: admin

### 2. String de conexão

No arquivo `src/GestaoOrdensServico/App.config`:

```xml
<connectionStrings>
  <add name="DefaultConnection"
       connectionString="Host=localhost;Port=5432;Database=gestao_os;Username=postgres;Password=postgres"
       providerName="Npgsql" />
</connectionStrings>
```

### 3. Rodar a aplicação

Abra `GestaoOrdensServico.slnx` no Visual Studio e pressione F5.

---

## Estrutura do Banco de Dados

| Tabela | Descrição |
|---|---|
| clientes | Cadastro de clientes com constraint unique no documento |
| servicos | Cadastro de serviços com constraints de valor e percentual |
| ordens_servico | OS com controle de status e campo versao para concorrência |
| itens_os | Itens da OS com cópia dos valores do serviço no momento da criação |
| auditoria | Registro de todas as alterações relevantes com snapshot JSON |

---

## Funcionalidades

- Cadastro de clientes com pesquisa por múltiplos filtros
- Cadastro de serviços
- Abertura e gestão de ordens de serviço
- Adição e remoção de itens com cálculo automático de valores
- Controle de status com transições válidas
- Auditoria completa de alterações
- Relatório gerencial com filtros e exportação PDF
- Paginação nas listagens
- Controle de concorrência otimista
