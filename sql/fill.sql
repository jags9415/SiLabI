SET NOCOUNT ON
GO

PRINT 'FILLING DATABASE'
:ON Error EXIT

:r C:\sql\fill\1-States.sql
:r C:\sql\fill\2-PeriodTypes.sql
:r C:\sql\fill\3-Periods.sql
:r C:\sql\fill\4-Users.sql
:r C:\sql\fill\5-Students.sql
:r C:\sql\fill\6-Professors.sql
:r C:\sql\fill\7-Operators.sql
:r C:\sql\fill\8-Administrators.sql
:r C:\sql\fill\9-Courses.sql
:r C:\sql\fill\10-Groups.sql
:r C:\sql\fill\11-Laboratories.sql
:r C:\sql\fill\12-Software.sql
:r C:\sql\fill\13-SoftwareByLaboratory.sql
:r C:\sql\fill\14-StudentsByGroup.sql

PRINT 'DATABASE FILLED'
GO