USE [FINANCAS]
GO

IF OBJECT_ID('lancamentos.seqLancamento','SO') IS NOT NULL
	DROP SEQUENCE lancamentos.seqLancamento
GO

CREATE SEQUENCE lancamentos.seqLancamento AS INT
	START WITH 1
	INCREMENT BY 1
	MINVALUE 1
	NO MAXVALUE
	CYCLE
	CACHE 100
GO

IF OBJECT_ID('lancamentos.seqFonte','SO') IS NOT NULL
	DROP SEQUENCE lancamentos.seqFonte
GO

CREATE SEQUENCE lancamentos.seqFonte AS INT
	START WITH 1
	INCREMENT BY 1
	MINVALUE 1
	NO MAXVALUE
	CYCLE
	CACHE 100
GO

IF OBJECT_ID('dbo.Fonte','U') IS NOT NULL
	DROP TABLE dbo.Fonte
GO

CREATE TABLE dbo.Fonte
(
	cdFonte	INT				NOT NULL,
	dsFonte	VARCHAR(100)	NOT NULL,
	tpFonte	CHAR(30)		NOT NULL,
	CONSTRAINT PKFonte01 PRIMARY KEY(cdFonte),
	CONSTRAINT UQFonte01 UNIQUE(dsFonte)
)
GO

CREATE TABLE dbo.FonteCartaoCredito
(
	cdFonte			INT			NOT NULL,
	dsBandeira		CHAR(30)	NOT NULL,
	diaFechamento	INT			NOT NULL,
	diaFatura		INT			NOT NULL,
	CONSTRAINT PKFonteCartaoCredito01 PRIMARY KEY(cdFonte),
	CONSTRAINT FKFonte_FonteCartaoCredito01 FOREIGN KEY(cdFonte) REFERENCES dbo.Fonte(cdFonte)
)
GO

IF OBJECT_ID('dbo.Movimentacao','U') IS NOT NULL
	DROP TABLE dbo.Movimentacao
GO

IF OBJECT_ID('dbo.Lancamento','U') IS NOT NULL
	DROP TABLE dbo.Lancamento
GO

CREATE TABLE dbo.Lancamento
(
	cdLancamento	INT				NOT NULL,
	cdFonte			INT				NOT NULL,
	dsLancamento	VARCHAR(70)		NOT NULL,
	dsResponsavel	VARCHAR(100)	NOT NULL,
	flFixo			BIT				NOT NULL,
	qtParcelas		INT				NOT NULL,
	valorSugerido	DECIMAL(18,2)	NOT NULL,
	flQuitado		BIT				NOT NULL,
	CONSTRAINT PKLancamento01 PRIMARY KEY(cdLancamento),
	CONSTRAINT FKFonte_Lancamento01 FOREIGN KEY(cdFonte) REFERENCES dbo.Fonte(cdFonte)
)
GO

CREATE TABLE dbo.Movimentacao
(
	dataHoraMovimentacao	DATETIME		NOT NULL,
	cdLancamento			INT				NOT NULL,
	valorMovimentacao		DECIMAL(18,2)	NOT NULL,
	nuParcela				INT				NOT NULL,
	flQuitado				BIT				NOT NULL,
	CONSTRAINT PKMovimentacao01 PRIMARY KEY(dataHoraMovimentacao,cdLancamento),
	CONSTRAINT FKLancamento_Movimentacao01 FOREIGN KEY(cdLancamento) REFERENCES dbo.Lancamento(cdLancamento)
)
GO