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
		SELECT @cdFonte CodResultado, 'Cartão de crédito vinculado a fonte com sucesso.' MsgRetorno
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

