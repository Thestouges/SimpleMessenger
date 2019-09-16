ALTER PROCEDURE [dbo].[AddUser]
	-- Add the parameters for the stored procedure here
	@user nvarchar(30),
	@password nvarchar(max),
	@output int output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	set @output = 0

    -- Insert statements for procedure here
	if(exists(select * from UserAccount where [User] = @user))
		Begin
			set @output = 1
			return
		End

	insert into UserAccount([User],Password)
	values(@user,@password)
END
Go

ALTER PROCEDURE [dbo].[CheckLogin]
	-- Add the parameters for the stored procedure here
	@user nvarchar(30),
	@password nvarchar(max),
	@output int output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	set @output = 0
    -- Insert statements for procedure here
	if(not exists(select * from UserAccount as A where A.[User] = @user and A.Password = @password))
		Begin
			set @output = 1
		End

	return
	
END
Go

ALTER PROCEDURE [dbo].[EnterMessage]
	-- Add the parameters for the stored procedure here
	@user nvarchar(30),
	@message nvarchar(max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	insert into UserMessage([User], Message1,timestamp)
	values(@user, @message, GETDATE())
END
go

ALTER PROCEDURE [dbo].[GetLoggedInUserList]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select * from UserAccount where LoggedIn = 'Y'
END
go

ALTER PROCEDURE [dbo].[GetPastMessages]
	-- Add the parameters for the stored procedure here
	@messageid int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select top 50 * from UserMessage
	where MessageID < @messageid
	order by MessageID desc
END
go

ALTER PROCEDURE [dbo].[GetRecentMessages] 
	-- Add the parameters for the stored procedure here
	@messageid int = -1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	if @messageid = -1
		Begin
			select * from(
			select top 50 * from UserMessage
			order by MessageID desc) as a
			order by MessageID asc
		End
	else
		Begin
			select * from UserMessage
			where MessageID > @messageid
		End
END
go

ALTER PROCEDURE [dbo].[Login]
	-- Add the parameters for the stored procedure here
	@user nvarchar(30)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	update UserAccount set LoggedIn = 'Y'
	where [User] = @user
END
go

ALTER PROCEDURE [dbo].[Logout]
	-- Add the parameters for the stored procedure here
	@user nvarchar(30)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	update UserAccount set LoggedIn = 'N'
	where [User] = @user
END

