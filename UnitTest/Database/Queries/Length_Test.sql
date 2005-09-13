IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Length_Test')
BEGIN
	DROP  Procedure  Length_Test
END

GO

CREATE Procedure Length_Test
	@len int,
	@str varchar(100)
AS

if @len <> len(@str)
begin
	raiserror('lens do not match.', 16, 1)
end

GO

GRANT EXEC ON Length_Test TO PUBLIC
GO

