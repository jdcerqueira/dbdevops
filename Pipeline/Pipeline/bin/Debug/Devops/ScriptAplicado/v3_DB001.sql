USE DB001;

IF OBJECT_ID('dbo.Alunos','U') IS NOT NULL
	DROP TABLE dbo.Alunos;

CREATE TABLE dbo.Alunos
(
	idAluno	INT PRIMARY KEY,
	nomeAluno	VARCHAR(50)
);