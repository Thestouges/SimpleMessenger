CREATE TABLE [dbo].[UserAccount](
	[User] [nvarchar](30) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[LoggedIn] [nvarchar](1) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[UserMessage](
	[User] [nvarchar](30) NULL,
	[Message1] [nvarchar](max) NULL,
	[timestamp] [timestamp] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO