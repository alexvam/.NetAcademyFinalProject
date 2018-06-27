SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET XACT_ABORT ON; 

CREATE DATABASE [Crowdfunding6];
GO

BEGIN TRANSACTION;

USE [Crowdfunding6];

CREATE TABLE dbo.ProjectStatus
	(
	StatusID tinyint NOT NULL,
	StatusDescription nvarchar(250) NULL,
	StatusCategory nvarchar(50) not NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.ProjectStatus ADD CONSTRAINT
	PK_ProjectStatus PRIMARY KEY CLUSTERED 
	(
	StatusID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]




CREATE TABLE dbo.MaterialType
	(
	MaterialTypeID bigint NOT NULL,
	TypeName nvarchar(250) NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.MaterialType ADD CONSTRAINT
	PK_MaterialType PRIMARY KEY CLUSTERED 
	(
	MaterialTypeID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]





CREATE TABLE dbo.ProjectCategory
	(
	CategoryID tinyint NOT NULL,
	CategoryDescription nvarchar(250)not NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.ProjectCategory ADD CONSTRAINT
	PK_ProjectCategory PRIMARY KEY CLUSTERED 
	(
	CategoryID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]



CREATE TABLE dbo.Member
	(
	MemberID bigint NOT NULL IDENTITY,
	PhoneNumber nvarchar(30) NULL,
	Address nvarchar(30) NULL,
	Country nvarchar(30) NULL,
	City nvarchar(30) NULL,
	PostCode nvarchar(30) NULL,
	FirstName nvarchar(30) Not NULL,
	LastName nvarchar(50) Not NULL,
	EmailAddress nvarchar(30) Not NULL,
	Password nvarchar(30) Not NULL,
	ConfirmPassword nvarchar(30) Not NULL,
	Birthday date NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Member ADD CONSTRAINT
	PK_Member PRIMARY KEY CLUSTERED 
	(
	MemberID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]



CREATE TABLE dbo.Project
	(
	ProjectID bigint NOT NULL IDENTITY,
	ProjectName nvarchar(50) Not NULL,
	Target decimal(14, 2) not NULL,
	MemberId bigint not NULL,
	ProjectDescription nvarchar(250) NULL,
	ProjectCategoryID tinyint not NULL,
	StartDate datetime not NULL,
	EndDate datetime not NULL,
	ProjectLocation nvarchar(50) NULL,
	Status tinyint not NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Project ADD CONSTRAINT
	PK_Project PRIMARY KEY CLUSTERED 
	(
	ProjectID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Project ADD CONSTRAINT
	FK_Project_Member FOREIGN KEY
	(
	MemberId
	) REFERENCES dbo.Member
	(
	MemberID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Project ADD CONSTRAINT
	FK_Project_ProjectCategory FOREIGN KEY
	(
	ProjectCategoryID
	) REFERENCES dbo.ProjectCategory
	(
	CategoryID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Project ADD CONSTRAINT
	FK_Project_ProjectStatus FOREIGN KEY
	(
	Status
	) REFERENCES dbo.ProjectStatus
	(
	StatusID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	


CREATE TABLE dbo.Reward
	(
	RewardsID bigint NOT NULL IDENTITY(1,1),
	ProjectID bigint not NULL,
	Title nvarchar(250) NULL,
	Description nvarchar(250) NULL,
	ItemsIncluded nvarchar(250) NULL,
	DeliveryDate datetime NULL,
	Amount int Not null
	)  ON [PRIMARY]
GO

ALTER TABLE dbo.Reward ADD CONSTRAINT
	PK_Reward PRIMARY KEY CLUSTERED
	(
	RewardsID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE dbo.Reward ADD CONSTRAINT
	FK_Reward_Project FOREIGN KEY
	(
	ProjectID
	) REFERENCES dbo.Project
	(
	ProjectID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	



CREATE TABLE dbo.Package
	(
	PackagesID bigint NOT NULL IDENTITY(1,1),
	ProjectID bigint not  NULL,
	RewardsID bigint NULL,
	Price decimal(18, 4) not NULL,
	Title nvarchar (50) null
	)  ON [PRIMARY]
	 
GO
ALTER TABLE dbo.Package ADD CONSTRAINT
	PK_Package PRIMARY KEY CLUSTERED 
	(
	PackagesID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Package ADD CONSTRAINT
	FK_Package_Project FOREIGN KEY
	(
	ProjectID
	) REFERENCES dbo.Project
	(
	ProjectID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
ALTER TABLE dbo.Package ADD CONSTRAINT
	FK_Package_Reward FOREIGN KEY
	(
	RewardsID
	) REFERENCES dbo.Reward
	(
	RewardsID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 






CREATE TABLE dbo.[Transaction]
	(
	TransactionID bigint NOT NULL IDENTITY,
	MemberID bigint not NULL,
	ProjectID bigint not NULL,
	Contribution decimal(18, 4) not  NULL,
	Date datetime not NULL,
	PackagesID bigint NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.[Transaction] ADD CONSTRAINT
	PK_Transaction PRIMARY KEY CLUSTERED 
	(
	TransactionID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.[Transaction] ADD CONSTRAINT
	FK_Transaction_Project FOREIGN KEY
	(
	ProjectID
	) REFERENCES dbo.Project
	(
	ProjectID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.[Transaction] ADD CONSTRAINT
	FK_Transaction_Member FOREIGN KEY
	(
	MemberID
	) REFERENCES dbo.Member
	(
	MemberID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.[Transaction] ADD CONSTRAINT
	FK_Transaction_Package FOREIGN KEY
	(
	PackagesID
	) REFERENCES dbo.Package
	(
	PackagesID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 









CREATE TABLE dbo.[Update]
	(
	UpdateID bigint NOT NULL IDENTITY,
	ProjectID bigint not NULL,
	MemberID bigint not NULL,
	Update_Text text NOT NULL,
	Date datetime NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.[Update] ADD CONSTRAINT
	PK_Update PRIMARY KEY CLUSTERED 
	(
	UpdateID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.[Update] ADD CONSTRAINT
	FK_Update_Project FOREIGN KEY
	(
	ProjectID
	) REFERENCES dbo.Project
	(
	ProjectID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.[Update] ADD CONSTRAINT
	FK_Update_Member FOREIGN KEY
	(
	MemberID
	) REFERENCES dbo.Member
	(
	MemberID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	





CREATE TABLE dbo.Comment
	(
	CommentID bigint NOT NULL IDENTITY,
	MemberID bigint not NULL,
	ProjectID bigint not NULL,
	Comment text not NULL,
	Date datetime not NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Comment ADD CONSTRAINT
	PK_Comment PRIMARY KEY CLUSTERED 
	(
	CommentID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Comment ADD CONSTRAINT
	FK_Comment_Member FOREIGN KEY
	(
	MemberID
	) REFERENCES dbo.Member
	(
	MemberID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Comment ADD CONSTRAINT
	FK_Comment_Project FOREIGN KEY
	(
	ProjectID
	) REFERENCES dbo.Project
	(
	ProjectID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	




CREATE TABLE dbo.Material
	(
	MaterialID bigint NOT NULL,
	ProjectID bigint not NULL,
	MaterialTypeID bigint NULL,
	Description text NULL,
	Link text NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Material ADD CONSTRAINT
	PK_Material PRIMARY KEY CLUSTERED 
	(
	MaterialID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Material ADD CONSTRAINT
	FK_Material_Project FOREIGN KEY
	(
	ProjectID
	) REFERENCES dbo.Project
	(
	ProjectID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Material ADD CONSTRAINT
	FK_Material_MaterialType FOREIGN KEY
	(
	MaterialTypeID
	) REFERENCES dbo.MaterialType
	(
	MaterialTypeID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 



COMMIT TRANSACTION;