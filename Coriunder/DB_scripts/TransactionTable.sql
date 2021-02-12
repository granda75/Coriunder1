/****** Object:  Table [dbo].[Transactions]    Script Date: 2/12/2021 3:21:06 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Transactions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CardHolderName] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[Phone] [nvarchar](50) NULL,
	[CardNumber] [int] NULL,
	[ExpDate] [char](4) NULL,
	[Cvv] [int] NULL,
	[Address] [nvarchar](250) NULL,
	[City] [nvarchar](50) NULL,
	[ZipCode] [varchar](10) NULL,
	[Country] [char](2) NULL,
	[ReplyCode] [char](3) NULL,
	[ReplyDescription] [nvarchar](250) NULL,
 CONSTRAINT [PK_Transactions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
