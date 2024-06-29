CREATE TRIGGER [account].[UpdateDateTimeAccount]
ON [account].[Account]
FOR UPDATE 
AS 
BEGIN 
    IF NOT UPDATE(UTCUpdatedDateTime) 
        UPDATE [account].[Account] SET UTCUpdatedDateTime = GETDATE() 
        WHERE UUID IN (SELECT UUID FROM inserted);
END 
GO