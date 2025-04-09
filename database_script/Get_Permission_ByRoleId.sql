IF EXISTS (
	SELECT *
	FROM sysobjects
	WHERE id = object_id(N'[Get_Permission_ByRoleId]')
		AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
BEGIN
	DROP PROCEDURE [Get_Permission_ByRoleId]
END
GO

CREATE PROCEDURE [Get_Permission_ByRoleId]
	@roleId varchar(50) NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM [Identity].Permissions
	WHERE RoleId = @roleId
END