USE [SiLabI];
SET NOCOUNT ON;
DECLARE @ADMIN_user_id INT,  @ADMIN_user_state INT, @ADMIN_admin_state INT;

SELECT @ADMIN_user_state = PK_State_Id FROM States WHERE Type = 'USER' AND Name = 'Activo';
SELECT @ADMIN_admin_state = PK_State_Id FROM States WHERE Type = 'ADMINISTRATOR' AND Name = 'Activo';

INSERT INTO Users (Name, Last_Name_1, Gender, Username, Password, FK_State_Id) VALUES
('admin', 'admin', 'Masculino', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', 'password', @ADMIN_user_state);

SET @ADMIN_user_id = SCOPE_IDENTITY();

INSERT INTO Professors (FK_User_Id) VALUES
(@ADMIN_user_id);

INSERT INTO Administrators (FK_User_Id, FK_State_Id) VALUES
(@ADMIN_user_id, @ADMIN_admin_state);

USE	[master];
