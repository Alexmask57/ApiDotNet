-- Initialisation de la Base de données d'authentification 
CREATE DATABASE JWTAuthentication;
USE JWTAuthentication;

CREATE TABLE Employee
(
    EmployeeID       int           NOT NULL
        PRIMARY KEY
        AUTO_INCREMENT,
    NationalIDNumber nvarchar(15)  NOT NULL,
    EmployeeName     nvarchar(100) NULL,
    LoginID          nvarchar(256) NOT NULL,
    JobTitle         nvarchar(50)  NOT NULL,
    BirthDate        date          NOT NULL,
    MaritalStatus    nchar(1)      NOT NULL,
    Gender           nchar(1)      NOT NULL,
    HireDate         date          NOT NULL,
    VacationHours    smallint      NOT NULL,
    SickLeaveHours   smallint      NOT NULL,
    Rowguid          CHAR(38) NOT NULL,
    ModifiedDate     datetime      NOT NULL
);
    
INSERT Employee
VALUES (1, '295847284', 'Michael Westover', 'adventure-works\ken0', 'Vice President of Sales',
        CAST('1969-01-29' AS Date), 'S', 'M', CAST('2009-01-14' AS Date), 99, 69,
        'f01251e5-96a3-448d-981e-0f99d789110d', CAST('2014-06-30 00:00:00.000' AS DateTime));
INSERT Employee
VALUES (2, '245797967', 'Raeann Santos', 'adventure-works\terri0', 'Vice President of Engineering',
        CAST('1971-08-01' AS Date), 'S', 'F', CAST('2008-01-31' AS Date), 1, 20,
        '45e8f437-670d-4409-93cb-f9424a40d6ee', CAST('2014-06-30 00:00:00.000' AS DateTime));
INSERT Employee
VALUES (3, '509647174', 'Pamela Wambsgans', 'adventure-works\roberto0', 'Engineering Manager',
        CAST('1974-11-12' AS Date), 'M', 'M', CAST('2007-11-11' AS Date), 2, 21,
        '9bbbfb2c-efbb-4217-9ab7-f97689328841', CAST('2014-06-30 00:00:00.000' AS DateTime));
    
-- Initialisation de la base de données pour le test du cache
-- CREATE DATABASE CachingDDB;