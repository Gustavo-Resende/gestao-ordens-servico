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

    CONSTRAINT pk_clientes           PRIMARY KEY (id),
    CONSTRAINT uk_clientes_documento UNIQUE (documento),
    CONSTRAINT chk_clientes_tipo     CHECK (tipo IN ('Fisica', 'Juridica'))
);

CREATE INDEX idx_clientes_documento ON clientes(documento);

-- SERVICOS
CREATE TABLE servicos (
    id                  UUID           NOT NULL DEFAULT gen_random_uuid(),
    nome                VARCHAR(200)   NOT NULL,
    valor_base          NUMERIC(10, 2) NOT NULL,
    percentual_imposto  NUMERIC(5, 2)  NOT NULL DEFAULT 0,
    ativo               BOOLEAN        NOT NULL DEFAULT TRUE,

    CONSTRAINT pk_servicos              PRIMARY KEY (id),
    CONSTRAINT chk_servicos_valor_base  CHECK (valor_base > 0),
    CONSTRAINT chk_servicos_percentual  CHECK (percentual_imposto BETWEEN 0 AND 100)
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

    CONSTRAINT pk_ordens_servico PRIMARY KEY (id),
    CONSTRAINT fk_os_cliente     FOREIGN KEY (cliente_id) REFERENCES clientes(id)
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

    CONSTRAINT pk_itens_os             PRIMARY KEY (id),
    CONSTRAINT fk_item_os              FOREIGN KEY (ordem_servico_id) REFERENCES ordens_servico(id),
    CONSTRAINT fk_item_servico         FOREIGN KEY (servico_id) REFERENCES servicos(id),
    CONSTRAINT chk_item_quantidade     CHECK (quantidade >= 1),
    CONSTRAINT chk_item_valor_unitario CHECK (valor_unitario > 0)
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

    CONSTRAINT pk_auditoria     PRIMARY KEY (id),
    CONSTRAINT chk_auditoria_op CHECK (operacao IN ('INSERT', 'UPDATE', 'DELETE'))
);

CREATE INDEX idx_auditoria_entidade    ON auditoria(entidade);
CREATE INDEX idx_auditoria_id_registro ON auditoria(id_registro);
