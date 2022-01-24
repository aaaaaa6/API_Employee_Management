
-- Base de datos

CREATE DATABASE dbEmployeeManagement

GO

-- Usar Base de datos

USE dbEmployeeManagement
GO
---------------------------------------------
------------- Tablas-------------------------
---------------------------------------------
---- Role
---------------------------------------------
IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'Role'))
BEGIN

	CREATE TABLE [dbo].[Role](
		[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
		[Name] [varchar](100) NOT NULL,
	)
		
END
GO
INSERT INTO  [dbo].[Role] VALUES('ADMIN')
INSERT INTO  [dbo].[Role] VALUES('ASESOR')
GO
---------------------------------------
-- User
---------------------------------------
IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'User'))
BEGIN

	CREATE TABLE [dbo].[User](
		[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
		[RoleId] [int] NOT NULL,
		[Login] [varchar](100) NOT NULL,
		[Pass] [varchar](100) NOT NULL,
		CONSTRAINT fk_Role FOREIGN KEY (RoleId) REFERENCES [Role] (Id)
	)
		
END
GO
INSERT INTO  [dbo].[User] VALUES(1,'qwe','123')
INSERT INTO  [dbo].[User] VALUES(2,'asd','123')
GO
---------------------------------------------
---- Permission
---------------------------------------------
IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'Permission'))
BEGIN

	CREATE TABLE [dbo].[Permission](
		[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
		[Name] [varchar](100) NOT NULL,
	)
		
END
GO
INSERT INTO  [dbo].[Permission] VALUES('EMPLOYEE_LIST')
INSERT INTO  [dbo].[Permission] VALUES('EMPLOYEE_SEACH')
INSERT INTO  [dbo].[Permission] VALUES('EMPLOYEE_CREATE')
INSERT INTO  [dbo].[Permission] VALUES('EMPLOYEE_EDIT')
GO
---------------------------------------------
---- RolePermission
---------------------------------------------
IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'RolePermission'))
BEGIN

	CREATE TABLE [dbo].[RolePermission](
		[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
		[RoleId] [int] NOT NULL,
		[PermissionId] [int] NOT NULL,
		CONSTRAINT fk_RolePermission FOREIGN KEY (RoleId) REFERENCES [Role] (Id),
		CONSTRAINT fk_Permission FOREIGN KEY (PermissionId) REFERENCES [Permission] (Id)
	)
		
END
GO
INSERT INTO  [dbo].[RolePermission] VALUES(1,1)
INSERT INTO  [dbo].[RolePermission] VALUES(1,2)
INSERT INTO  [dbo].[RolePermission] VALUES(1,3)
INSERT INTO  [dbo].[RolePermission] VALUES(1,4)

INSERT INTO  [dbo].[RolePermission] VALUES(2,1)
INSERT INTO  [dbo].[RolePermission] VALUES(2,2)
GO

---------------------------------------------
---- RolePermission
---------------------------------------------

IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'Employee'))
BEGIN

	CREATE TABLE [dbo].[Employee](
		[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
		[Cedula] [varchar](100) NOT NULL,
		[Nombre] [varchar](100) NOT NULL,
		[Sexo] [varchar](2) NOT NULL,
		[FechaNacimiento] [datetime2](7) NULL,
		[Salario] [float] NOT NULL,
		[Vacuna] [bit] NOT NULL,
	)
		
END
GO

INSERT INTO  [dbo].[Employee] VALUES('123','Empleados 1','M',GETDATE(),2500000,1)
INSERT INTO  [dbo].[Employee] VALUES('456','Empleados 2','F',GETDATE(),3500000,0)
INSERT INTO  [dbo].[Employee] VALUES('789','Empleados 3','M',GETDATE(),2500000,1)
INSERT INTO  [dbo].[Employee] VALUES('101','Empleados 4','F',GETDATE(),3500000,0)
INSERT INTO  [dbo].[Employee] VALUES('102','Empleados 5','M',GETDATE(),2500000,1)
INSERT INTO  [dbo].[Employee] VALUES('103','Empleados 6','F',GETDATE(),3500000,0)
INSERT INTO  [dbo].[Employee] VALUES('104','Empleados 7','M',GETDATE(),2500000,1)
INSERT INTO  [dbo].[Employee] VALUES('105','Empleados 8','F',GETDATE(),3500000,0)
INSERT INTO  [dbo].[Employee] VALUES('106','Empleados 8','M',GETDATE(),2500000,1)
INSERT INTO  [dbo].[Employee] VALUES('107','Empleados 10','F',GETDATE(),3500000,0)
