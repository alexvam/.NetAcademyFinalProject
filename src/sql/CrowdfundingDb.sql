SET XACT_ABORT ON;  

CREATE DATABASE [Crowdfunding];
GO

BEGIN TRANSACTION;

USE [Crowdfunding];

CREATE TABLE [Member] (
	[MemberId] INT NOT NULL IDENTITY,
	[FirstName] NVARCHAR(40) NOT NULL,
	[LastName] NVARCHAR(40) NOT NULL,
	[Email] NVARCHAR(256) NOT NULL,
	[Password] NVARCHAR(256) NOT NULL,
	[ConfirmPassword] NVARCHAR(256) NOT NULL,
	[Birthday] DATE NOT NULL,

	CONSTRAINT [PK_Member] PRIMARY KEY ([MemberId])
);

CREATE TABLE [Creator] (
	[MemberId] INT NOT NULL IDENTITY,	
	[BankAccount] NVARCHAR(256) NOT NULL,

	CONSTRAINT [PK_Creator] PRIMARY KEY ([MemberId]),
	CONSTRAINT [FK_Member_Creator_MemberId]
		FOREIGN KEY ([MemberId]) REFERENCES [Member] ([MemberId])
);

CREATE TABLE [ProjectCategory] (
	[ProjectCategoryId] TINYINT NOT NULL,	
	[Category] NVARCHAR(64) NOT NULL,
	
	CONSTRAINT [PK_ProjectCategory] PRIMARY KEY ([ProjectCategoryId])
);

CREATE TABLE [Project] (
	[ProjectId] INT NOT NULL IDENTITY,
	[Title] NVARCHAR(100) NOT NULL,
	[Target] DECIMAL(14,2) NOT NULL,
	[PhotoUrl] NVARCHAR(256) NULL,
	[VideoUrl] NVARCHAR(256) NULL,
	[Description] NVARCHAR(1024) NULL,
	[ReleaseDate] DATE NULL,
	[EndDate] DATE NOT NULL,
	[MemberId] INT NOT NULL,
	[ProjectCategoryId] TINYINT NOT NULL,

	CONSTRAINT [PK_Project] PRIMARY KEY ([ProjectId]),
	CONSTRAINT [FK_Project_Creator_MemberId]
		 FOREIGN KEY ([MemberId]) REFERENCES [Creator] ([MemberId]),
	CONSTRAINT [FK_Project_ProjectCategory_ProjectCategoryId]
		 FOREIGN KEY ([ProjectCategoryId]) REFERENCES [ProjectCategory] ([ProjectCategoryId])
);


CREATE TABLE [Reward] (
	[RewardId] INT NOT NULL IDENTITY,
	[Title] NVARCHAR(100) NOT NULL,
	[Price] DECIMAL(5,2) NOT NULL,	
	[Description] NVARCHAR(1024) NULL,
	[ProjectId] INT NOT NULL,

	CONSTRAINT [PK_Reward] PRIMARY KEY ([RewardId]),
	CONSTRAINT [FK_Reward_Project_ProjectId]
		 FOREIGN KEY ([ProjectId]) REFERENCES [Project] ([ProjectId])
);

CREATE TABLE [Transaction] (
	[TransactionId] INT NOT NULL IDENTITY,
	[Method] NVARCHAR(100) NOT NULL,
	[Amount] DECIMAL(14,2) NOT NULL,	
	[Date] DATE NOT NULL,
	[MemberId] INT NOT NULL,
	[ProjectId] INT NOT NULL,

	CONSTRAINT [PK_Transaction] PRIMARY KEY ([TransactionId]),
	CONSTRAINT [FK_Transaction_Member_MemberId]
		 FOREIGN KEY ([MemberId]) REFERENCES [Member] ([MemberId]),
	CONSTRAINT [FK_Transaction_Project_ProjectId]
		 FOREIGN KEY ([ProjectId]) REFERENCES [Project] ([ProjectId])
);

CREATE TABLE [MemberProject] (
	[MemberId] INT NOT NULL,
	[ProjectId] INT NOT NULL,

	CONSTRAINT [PK_MemberProject] PRIMARY KEY ([MemberId], [ProjectId]),
	CONSTRAINT [FK_MemberProject_Member_MemberId]
		 FOREIGN KEY ([MemberId]) REFERENCES [Member] ([MemberId]),
	CONSTRAINT [FK_MemberProject_Project_ProjectId]
		 FOREIGN KEY ([ProjectId]) REFERENCES [Project] ([ProjectId]),
);




COMMIT TRANSACTION;