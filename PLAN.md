# PLAN.md — Gestão de Ordens de Serviço

## Antes de Começar

### Pré-requisitos instalados manualmente (você faz)
- [x] .NET Framework 4.6
- [x] Visual Studio 2019+
- [x] Docker Desktop
- [x] SQLSysClrTypes.msi (x64)
- [x] Microsoft Report Viewer 2015 Runtime

### NuGet packages (você instala via Visual Studio)

| Projeto | Package | Versão |
|---|---|---|
| GestaoOrdensServico | Microsoft.ReportingServices.ReportViewerControl | 150.x |
| GestaoOrdensServico | Npgsql | 4.1.x |
| GestaoOrdensServico.Infrastructure | Npgsql | 4.1.x |
| GestaoOrdensServico.Application | Newtonsoft.Json | 13.x |

---

## Docker

### docker-compose.yml (raiz do repositório)

```yaml
services:
  postgres:
    image: postgres:16
    container_name: gestao_os_db
    environment:
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: postgres
      POSTGRES_DB: gestao_os
    ports:
      - "5432:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data
      - ./scripts/init.sql:/docker-entrypoint-initdb.d/init.sql

  pgadmin:
    image: dpage/pgadmin4
    container_name: gestao_os_pgadmin
    environment:
      PGADMIN_DEFAULT_EMAIL: admin@admin.com
      PGADMIN_DEFAULT_PASSWORD: admin
    ports:
      - "8080:80"
    depends_on:
      - postgres

volumes:
  postgres_data:
```

### Como subir

```bash
docker-compose up -d
```

PgAdmin: `http://localhost:8080` — login `admin@admin.com` / `admin`

String de conexão:
```
Host=localhost;Port=5432;Database=gestao_os;Username=postgres;Password=postgres
```

---

## Script SQL

### Localização

```
scripts/
└── init.sql   ← executado automaticamente ao subir o container
```

### init.sql

```sql
-- ENUMS
CREATE TYPE status_os AS ENUM ('Aberta', 'EmAndamento', 'Concluida', 'Cancelada');

-- CLIENTES
CREATE TABLE clientes (
    id            UUID          NOT NULL DEFAULT gen_random_uuid(),
    nome          VARCHAR(200)  NOT NULL,
    documento     VARCHAR(14)   NOT NULL,
    tipo          VARCHAR(10)   NOT NULL,
    email         VARCHAR(200),
    telefone      VARCHAR(20),
    data_cadastro TIMESTAMP     NOT NULL DEFAULT NOW(),
    ativo         BOOLEAN       NOT NULL DEFAULT TRUE,

    CONSTRAINT pk_clientes              PRIMARY KEY (id),
    CONSTRAINT uk_clientes_documento    UNIQUE (documento),
    CONSTRAINT chk_clientes_tipo        CHECK (tipo IN ('Fisica', 'Juridica'))
);

CREATE INDEX idx_clientes_documento ON clientes(documento);

-- SERVICOS
CREATE TABLE servicos (
    id                  UUID           NOT NULL DEFAULT gen_random_uuid(),
    nome                VARCHAR(200)   NOT NULL,
    valor_base          NUMERIC(10, 2) NOT NULL,
    percentual_imposto  NUMERIC(5, 2)  NOT NULL DEFAULT 0,
    ativo               BOOLEAN        NOT NULL DEFAULT TRUE,

    CONSTRAINT pk_servicos                  PRIMARY KEY (id),
    CONSTRAINT chk_servicos_valor_base      CHECK (valor_base > 0),
    CONSTRAINT chk_servicos_percentual      CHECK (percentual_imposto BETWEEN 0 AND 100)
);

-- ORDENS DE SERVICO
CREATE TABLE ordens_servico (
    id             UUID           NOT NULL DEFAULT gen_random_uuid(),
    cliente_id     UUID           NOT NULL,
    data_abertura  TIMESTAMP      NOT NULL DEFAULT NOW(),
    data_conclusao TIMESTAMP,
    status         status_os      NOT NULL DEFAULT 'Aberta',
    observacao     TEXT,
    valor_total    NUMERIC(10, 2) NOT NULL DEFAULT 0,
    versao         INTEGER        NOT NULL DEFAULT 1,

    CONSTRAINT pk_ordens_servico  PRIMARY KEY (id),
    CONSTRAINT fk_os_cliente      FOREIGN KEY (cliente_id) REFERENCES clientes(id)
);

CREATE INDEX idx_os_cliente_id    ON ordens_servico(cliente_id);
CREATE INDEX idx_os_status        ON ordens_servico(status);
CREATE INDEX idx_os_data_abertura ON ordens_servico(data_abertura);

-- ITENS DA OS
CREATE TABLE itens_os (
    id                          UUID           NOT NULL DEFAULT gen_random_uuid(),
    ordem_servico_id            UUID           NOT NULL,
    servico_id                  UUID           NOT NULL,
    quantidade                  INTEGER        NOT NULL,
    valor_unitario              NUMERIC(10, 2) NOT NULL,
    percentual_imposto_aplicado NUMERIC(5, 2)  NOT NULL DEFAULT 0,
    valor_total_item            NUMERIC(10, 2) NOT NULL,

    CONSTRAINT pk_itens_os              PRIMARY KEY (id),
    CONSTRAINT fk_item_os               FOREIGN KEY (ordem_servico_id) REFERENCES ordens_servico(id),
    CONSTRAINT fk_item_servico          FOREIGN KEY (servico_id) REFERENCES servicos(id),
    CONSTRAINT chk_item_quantidade      CHECK (quantidade >= 1),
    CONSTRAINT chk_item_valor_unitario  CHECK (valor_unitario > 0)
);

CREATE INDEX idx_itens_os_ordem_servico_id ON itens_os(ordem_servico_id);

-- AUDITORIA
CREATE TABLE auditoria (
    id          UUID         NOT NULL DEFAULT gen_random_uuid(),
    entidade    VARCHAR(100) NOT NULL,
    id_registro UUID         NOT NULL,
    operacao    VARCHAR(10)  NOT NULL,
    data_hora   TIMESTAMP    NOT NULL DEFAULT NOW(),
    usuario     VARCHAR(100) NOT NULL,
    snapshot    TEXT         NOT NULL,

    CONSTRAINT pk_auditoria         PRIMARY KEY (id),
    CONSTRAINT chk_auditoria_op     CHECK (operacao IN ('INSERT', 'UPDATE', 'DELETE'))
);

CREATE INDEX idx_auditoria_entidade    ON auditoria(entidade);
CREATE INDEX idx_auditoria_id_registro ON auditoria(id_registro);
```

---

## Estrutura da Solution

### Criar manualmente no Visual Studio

```
GestaoOrdensServico.sln
├── GestaoOrdensServico              ← WinForms App (já existe)
├── GestaoOrdensServico.Domain       ← Class Library
├── GestaoOrdensServico.Application  ← Class Library
└── GestaoOrdensServico.Infrastructure ← Class Library
```

### Referências entre projetos

```
GestaoOrdensServico         → referencia Application + Domain
GestaoOrdensServico.Application → referencia Domain + Infrastructure
GestaoOrdensServico.Infrastructure → referencia Domain
GestaoOrdensServico.Domain  → sem referências
```

---

## Plano de Implementação

### FASE 0 — Estrutura de pastas e organização

**Objetivo:** projeto organizado e compilando antes de escrever qualquer lógica.

> ✅ Tudo nesta fase é feito pelo Claude Code.

- [ ] Criar pasta `scripts/` na raiz do repositório
- [ ] Criar arquivo `scripts/init.sql` com o script SQL completo
- [ ] Criar arquivo `docker-compose.yml` na raiz do repositório
- [ ] Criar os 3 projetos Class Library na solution
- [ ] Configurar referências entre projetos
- [ ] Criar `App.config` com string de conexão
- [ ] Criar estrutura de pastas e arquivos conforme abaixo
- [ ] Verificar que a solution compila sem erros antes de avançar

**GestaoOrdensServico (WinForms)**
```
GestaoOrdensServico/
├── Forms/
│   ├── FormPrincipal.cs             ← classe vazia + construtor, SEM eventos
│   ├── FormClientes.cs              ← classe vazia + construtor, SEM eventos
│   ├── FormClientesCadastro.cs      ← classe vazia + construtor, SEM eventos
│   ├── FormServicos.cs              ← classe vazia + construtor, SEM eventos
│   ├── FormServicosCadastro.cs      ← classe vazia + construtor, SEM eventos
│   ├── FormOrdensServico.cs         ← classe vazia + construtor, SEM eventos
│   ├── FormOrdensServicoCadastro.cs ← classe vazia + construtor, SEM eventos
│   └── FormRelatorio.cs             ← classe vazia + construtor, SEM eventos
├── App.config
└── Program.cs                       ← wiring completo
```

**GestaoOrdensServico.Domain**
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

**GestaoOrdensServico.Application**
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

**GestaoOrdensServico.Infrastructure**
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

### FASE 1 — Infraestrutura base

**Objetivo:** projeto compilando, conectando no banco, sem tela ainda.

- [ ] Criar os 3 projetos Class Library na solution
- [ ] Configurar referências entre projetos
- [ ] Instalar NuGet packages
- [ ] Criar `App.config` com string de conexão
- [ ] Implementar `DbConnectionFactory`
- [ ] Implementar `Logger`
- [ ] Implementar `ResultadoOperacao<T>`
- [ ] Subir Docker e testar conexão

---

### FASE 2 — Domain

**Objetivo:** todas as entidades e enums criados.

- [ ] `StatusOs.cs` — enum
- [ ] `Cliente.cs` — entidade
- [ ] `Servico.cs` — entidade
- [ ] `OrdemServico.cs` — entidade
- [ ] `ItemOs.cs` — entidade
- [ ] `AuditoriaRegistro.cs` — entidade

---

### FASE 3 — Repositories

**Objetivo:** todas as operações de banco implementadas.

**ClienteRepository:**
- [ ] `Inserir`
- [ ] `Atualizar`
- [ ] `Excluir`
- [ ] `BuscarPorId`
- [ ] `Listar` (com filtros: nome, documento, ativo)
- [ ] `PossuiOsVinculada`

**ServicoRepository:**
- [ ] `Inserir`
- [ ] `Atualizar`
- [ ] `BuscarPorId`
- [ ] `Listar`

**OrdemServicoRepository:**
- [ ] `Inserir` (com transação + auditoria)
- [ ] `Atualizar` (com concorrência otimista)
- [ ] `BuscarPorId`
- [ ] `ListarPaginado` (com filtros: período, cliente, status)
- [ ] `InserirItem` (com transação + recalculo valor total + auditoria)
- [ ] `RemoverItem` (com transação + recalculo valor total + auditoria)
- [ ] `AlterarStatus` (com transação + auditoria)
- [ ] `BuscarItens`

**AuditoriaRepository:**
- [ ] `Inserir`

---

### FASE 4 — Services

**Objetivo:** todas as regras de negócio implementadas.

**ClienteService:**
- [ ] `Criar`
- [ ] `Atualizar`
- [ ] `Excluir` (valida OS vinculada)
- [ ] `BuscarPorId`
- [ ] `Listar`

**ServicoService:**
- [ ] `Criar`
- [ ] `Atualizar`
- [ ] `BuscarPorId`
- [ ] `Listar`

**OrdemServicoService:**
- [ ] `Criar`
- [ ] `AdicionarItem` (calcula valores + copia preço do serviço)
- [ ] `RemoverItem`
- [ ] `AlterarStatus` (valida transições)
- [ ] `Concluir` (valida valor total > 0)
- [ ] `BuscarPorId`
- [ ] `ListarPaginado`

**RelatorioService:**
- [ ] `GerarRelatorio` (com filtros período, cliente, status)

---

### FASE 5A — WinForms (Claude Code cria a estrutura)

**Objetivo:** Forms criados com estrutura, construtores e métodos de evento vazios.

> ✅ Claude Code cria os arquivos. Nenhuma ação manual necessária nesta sub-fase.

- [ ] Preencher cada `Form.cs` com estrutura completa:
  - Campos privados dos services
  - Construtor recebendo os services
  - Métodos de evento **vazios** com a assinatura correta
  - Métodos auxiliares como `CarregarDados()` e `Mapear()` **vazios**

> ⚠️ Os métodos ficam vazios de propósito — o código interno só é gerado na Fase 5B, depois que você fizer o Designer.

---

### FASE 5B — WinForms (você faz o Designer, Claude Code preenche os eventos)

**Objetivo:** eventos implementados após você montar as telas no Visual Studio.

> ⚠️ **ATENÇÃO — só execute esta fase depois de fazer o Designer de cada Form.**
>
> **Para cada Form, siga essa ordem:**
> 1. Abra o Form no Visual Studio
> 2. Arraste cada componente da tabela "Componentes por Form" (no final deste documento)
> 3. Defina o `Name` de cada componente exatamente como está na tabela
> 4. Clique duas vezes em cada Button para criar os métodos de evento no Designer
> 5. Só depois peça ao Claude Code para preencher o código dos eventos

#### FormPrincipal
- [ ] **[VOCÊ PRIMEIRO]** Designer — componentes + eventos criados
- [ ] **[CLAUDE CODE]** Preencher eventos e wiring no `Program.cs`

#### Módulo Clientes
- [ ] **[VOCÊ PRIMEIRO]** Designer do `FormClientes` — componentes + eventos
- [ ] **[CLAUDE CODE]** Preencher eventos do `FormClientes`
- [ ] **[VOCÊ PRIMEIRO]** Designer do `FormClientesCadastro` — componentes + eventos
- [ ] **[CLAUDE CODE]** Preencher eventos do `FormClientesCadastro`

#### Módulo Serviços
- [ ] **[VOCÊ PRIMEIRO]** Designer do `FormServicos` — componentes + eventos
- [ ] **[CLAUDE CODE]** Preencher eventos do `FormServicos`
- [ ] **[VOCÊ PRIMEIRO]** Designer do `FormServicosCadastro` — componentes + eventos
- [ ] **[CLAUDE CODE]** Preencher eventos do `FormServicosCadastro`

#### Módulo Ordens de Serviço
- [ ] **[VOCÊ PRIMEIRO]** Designer do `FormOrdensServico` — componentes + eventos
- [ ] **[CLAUDE CODE]** Preencher eventos do `FormOrdensServico`
- [ ] **[VOCÊ PRIMEIRO]** Designer do `FormOrdensServicoCadastro` — componentes + eventos
- [ ] **[CLAUDE CODE]** Preencher eventos do `FormOrdensServicoCadastro`

#### Módulo Relatório
- [ ] **[VOCÊ PRIMEIRO]** Designer do `FormRelatorio` — componentes + eventos
- [ ] **[VOCÊ PRIMEIRO]** Criar arquivo `.rdlc` e configurar DataSet no Designer
- [ ] **[CLAUDE CODE]** Preencher eventos do `FormRelatorio`

---

### FASE 6 — Ajustes finais

- [ ] Testar todos os fluxos
- [ ] Testar concorrência otimista
- [ ] Testar constraints (documento duplicado, FK)
- [ ] Verificar logs gerados
- [ ] Atualizar README

---

## Componentes por Form

### FormPrincipal

| Componente | Tipo | Nome |
|---|---|---|
| Botão Clientes | Button | btnClientes |
| Botão Serviços | Button | btnServicos |
| Botão Ordens de Serviço | Button | btnOrdensServico |
| Botão Relatório | Button | btnRelatorio |

---

### FormClientes

| Componente | Tipo | Nome |
|---|---|---|
| Campo nome | TextBox | txtFiltroNome |
| Campo documento | TextBox | txtFiltroDocumento |
| Checkbox ativo | CheckBox | chkFiltroAtivo |
| Botão pesquisar | Button | btnPesquisar |
| Tabela clientes | DataGridView | dgvClientes |
| Botão novo | Button | btnNovo |
| Botão editar | Button | btnEditar |
| Botão excluir | Button | btnExcluir |

---

### FormClientesCadastro

| Componente | Tipo | Nome |
|---|---|---|
| Campo nome | TextBox | txtNome |
| Campo documento | TextBox | txtDocumento |
| Tipo pessoa | ComboBox | cmbTipo |
| Campo email | TextBox | txtEmail |
| Campo telefone | TextBox | txtTelefone |
| Checkbox ativo | CheckBox | chkAtivo |
| Botão salvar | Button | btnSalvar |
| Botão cancelar | Button | btnCancelar |

---

### FormServicos

| Componente | Tipo | Nome |
|---|---|---|
| Tabela serviços | DataGridView | dgvServicos |
| Botão novo | Button | btnNovo |
| Botão editar | Button | btnEditar |

---

### FormServicosCadastro

| Componente | Tipo | Nome |
|---|---|---|
| Campo nome | TextBox | txtNome |
| Campo valor base | TextBox | txtValorBase |
| Campo percentual imposto | TextBox | txtPercentualImposto |
| Checkbox ativo | CheckBox | chkAtivo |
| Botão salvar | Button | btnSalvar |
| Botão cancelar | Button | btnCancelar |

---

### FormOrdensServico

| Componente | Tipo | Nome |
|---|---|---|
| Data início | DateTimePicker | dtpInicio |
| Data fim | DateTimePicker | dtpFim |
| Combo status | ComboBox | cmbStatus |
| Botão pesquisar | Button | btnPesquisar |
| Tabela OS | DataGridView | dgvOrdensServico |
| Botão nova OS | Button | btnNova |
| Botão abrir OS | Button | btnAbrir |
| Botão anterior | Button | btnAnterior |
| Botão próximo | Button | btnProximo |
| Label página | Label | lblPagina |

---

### FormOrdensServicoCadastro

| Componente | Tipo | Nome |
|---|---|---|
| Combo cliente | ComboBox | cmbCliente |
| Campo observação | TextBox | txtObservacao |
| Label status | Label | lblStatus |
| Label valor total | Label | lblValorTotal |
| Tabela itens | DataGridView | dgvItens |
| Combo serviço | ComboBox | cmbServico |
| Campo quantidade | TextBox | txtQuantidade |
| Botão adicionar item | Button | btnAdicionarItem |
| Botão remover item | Button | btnRemoverItem |
| Combo novo status | ComboBox | cmbNovoStatus |
| Botão alterar status | Button | btnAlterarStatus |
| Botão salvar | Button | btnSalvar |
| Botão cancelar | Button | btnCancelar |

---

### FormRelatorio

| Componente | Tipo | Nome |
|---|---|---|
| Data início | DateTimePicker | dtpInicio |
| Data fim | DateTimePicker | dtpFim |
| Combo cliente | ComboBox | cmbCliente |
| Combo status | ComboBox | cmbStatus |
| Botão gerar | Button | btnGerar |
| Botão exportar PDF | Button | btnExportarPdf |
| ReportViewer | ReportViewer | reportViewer1 |
