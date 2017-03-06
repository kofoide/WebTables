IF OBJECT_ID(N'dbo.UserAdditionalUpsert', N'P') IS NOT NULL
  DROP PROCEDURE dbo.UserAdditionalUpsert
GO

CREATE PROC dbo.UserAdditionalUpsert
	@UserId INT
,	@IsTest	BIT
,	@Department VARCHAR(50)
AS

MERGE dbo.UserAdditional AS T
USING (SELECT @UserId AS UserId) AS S
ON T.UserId = S.UserId
WHEN MATCHED THEN UPDATE SET T.IsTest = @IsTest, T.Department = @Department
WHEN NOT MATCHED THEN INSERT (UserId, IsTest, Department) VALUES(@UserId, @IsTest, @Department)
;

GO