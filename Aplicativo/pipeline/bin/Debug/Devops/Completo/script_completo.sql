-- 06/03/2022 22:29:04 **** Script aplicado - E:\GitHub\dbdevops\Aplicativo\pipeline\bin\Debug\Devops\Pendentes\v1_FINANCAS.sql ****
USE master
GO

IF DB_ID('FINANCAS') IS NOT NULL
BEGIN
	ALTER DATABASE [FINANCAS] SET RESTRICTED_USER WITH ROLLBACK IMMEDIATE
	DROP DATABASE [FINANCAS]
END
GO

CREATE DATABASE [FINANCAS]
GO

ALTER DATABASE [FINANCAS] SET ALLOW_SNAPSHOT_ISOLATION ON
GO

-- 06/03/2022 22:29:04 **** Script aplicado - E:\GitHub\dbdevops\Aplicativo\pipeline\bin\Debug\Devops\Pendentes\v2_FINANCAS.sql ****
USE [FINANCAS]
GO

IF EXISTS(SELECT TOP 1 1 FROM sys.sysusers WHERE name = 'usr_financas')
BEGIN
	DROP USER [usr_financas]
END
GO

IF DATABASE_PRINCIPAL_ID('Client') IS NOT NULL
BEGIN
	DROP ROLE [Client]
END

IF SCHEMA_ID('lancamentos') IS NOT NULL
BEGIN
	DROP SCHEMA [lancamentos]
END
GO

USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM sys.syslogins WHERE name = 'lgn_financas')
BEGIN
	DROP LOGIN [lgn_financas]
END
GO

CREATE LOGIN [lgn_financas] WITH 
	PASSWORD = 'lgn_financas', 
	CHECK_POLICY = OFF, 
	CHECK_EXPIRATION = OFF, 
	DEFAULT_DATABASE = [FINANCAS]
GO

USE [FINANCAS]
GO

CREATE USER [usr_financas] FOR LOGIN [lgn_financas]
GO

CREATE SCHEMA [lancamentos] AUTHORIZATION [dbo]
GO

CREATE ROLE [Client] AUTHORIZATION [dbo]
GO

ALTER ROLE [Client] ADD MEMBER [usr_financas]
GO

GRANT EXECUTE ON SCHEMA :: [lancamentos] TO [Client]
GO

-- 06/03/2022 22:29:04 **** Script aplicado - E:\GitHub\dbdevops\Aplicativo\pipeline\bin\Debug\Devops\Pendentes\v3_FINANCAS.sql ****
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

-- 06/03/2022 22:29:04 **** Script aplicado - E:\GitHub\dbdevops\Aplicativo\pipeline\bin\Debug\Devops\Pendentes\v4_FINANCAS.sql ****
USE [FINANCAS]
GO

IF OBJECT_ID('lancamentos.cria_fonte','P') IS NOT NULL
	DROP PROCEDURE lancamentos.cria_fonte
GO

/*
lancamentos.cria_fonte
*/
CREATE PROCEDURE lancamentos.cria_fonte
	@dsFonte	VARCHAR(100),
	@tpFonte	CHAR(30)
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL SNAPSHOT
	SET NOCOUNT ON

	BEGIN TRAN

	DECLARE @cdFonte	INT = NEXT VALUE FOR lancamentos.seqFonte

	BEGIN TRY
		INSERT INTO dbo.Fonte(cdFonte, dsFonte, tpFonte)
		VALUES(@cdFonte, @dsFonte, @tpFonte)

		COMMIT
		SELECT @cdFonte CodResultado, 'Fonte criada com sucesso.' MsgRetorno
	END TRY
	BEGIN CATCH
		ROLLBACK
		SELECT -1 CodResultado, ERROR_MESSAGE() MsgRetorno
	END CATCH

	SET NOCOUNT OFF
END
GO


IF OBJECT_ID('lancamentos.cria_fonte_cartao_credito','P') IS NOT NULL
	DROP PROCEDURE lancamentos.cria_fonte_cartao_credito
GO

/*
lancamentos.cria_fonte_cartao_credito
*/
CREATE PROCEDURE lancamentos.cria_fonte_cartao_credito
	@cdFonte		INT,
	@dsBandeira		CHAR(30),
	@diaFechamento	INT,
	@diaFatura		INT
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL SNAPSHOT
	SET NOCOUNT ON

	BEGIN TRAN

	BEGIN TRY
		INSERT INTO dbo.FonteCartaoCredito(cdFonte, dsBandeira, diaFechamento, diaFatura)
		VALUES(@cdFonte, @dsBandeira, @diaFechamento, @diaFatura)

		COMMIT
		SELECT @cdFonte CodResultado, 'Cart�o de cr�dito vinculado a fonte com sucesso.' MsgRetorno
	END TRY
	BEGIN CATCH
		ROLLBACK
		SELECT -1 CodResultado, ERROR_MESSAGE() MsgRetorno
	END CATCH

	SET NOCOUNT OFF
END
GO

IF OBJECT_ID('lancamentos.lista_fontes_sem_filtro','P') IS NOT NULL
	DROP PROCEDURE lancamentos.lista_fontes_sem_filtro
GO

/*
lancamentos.lista_fontes_sem_filtro
*/
CREATE PROCEDURE lancamentos.lista_fontes_sem_filtro
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL SNAPSHOT
	SET NOCOUNT ON

	SELECT 
		F.cdFonte, F.dsFonte, F.tpFonte,
		ISNULL(FCC.diaFatura,0) diaFatura, 
		ISNULL(FCC.diaFechamento,0) diaFechamento, 
		ISNULL(FCC.dsBandeira,'') dsBandeira
	FROM dbo.Fonte F
		LEFT JOIN dbo.FonteCartaoCredito FCC ON FCC.cdFonte = F.cdFonte
	ORDER BY F.dsFonte

	SET NOCOUNT OFF
END
GO



