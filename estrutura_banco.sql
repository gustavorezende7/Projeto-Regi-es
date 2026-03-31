-- Arquivo SQL com a estrutura da tabela regioes
CREATE TABLE IF NOT EXISTS regioes (
  id serial PRIMARY KEY,
  uf varchar(2) NOT NULL,
  nome varchar(200) NOT NULL,
  ativo boolean NOT NULL DEFAULT true,
  createdat timestamp without time zone NOT NULL DEFAULT now(),
  UNIQUE (uf, nome)
);
