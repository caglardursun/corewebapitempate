CREATE DATABASE [DBName]
GO
USE [DBName]
GO

/****** Object:  Table [dbo].[GeneralUsers]    Script Date: 26.02.2020 17:46:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GeneralUsers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](20) NOT NULL,
	[Password] [nvarchar](128) NOT NULL,
	[Token] [nvarchar](256) NULL,
	[ExpiredDate] [datetime] NULL,
	[Phone] [nvarchar](20) NULL,
	[Email] [nvarchar](50) NULL,
	[IsAdmin] [bit] NOT NULL,
	[CreatedById] [int] NULL,
	[ModifiedById] [int] NULL,
	[CreatedDateTime] [datetime] NULL,
	[ModifiedDateTime] [datetime] NULL,
 CONSTRAINT [PK_GeneralUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
