CREATE TABLE [dbo].[UserAccount](
	[User] [nvarchar](30) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[LoggedIn] [nvarchar](1) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[UserMessage](
	[User] [nvarchar](30) NULL,
	[Message1] [nvarchar](max) NULL,
	[timestamp] [timestamp] NOT NULL,
	[MessageID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_UserMessage] PRIMARY KEY CLUSTERED 
(
	[MessageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO