SET NOCOUNT ON;

DECLARE @semestre INT, @cuatrimestre INT, @trimestre INT, @bimestre INT;

SELECT @semestre = PK_Period_Type_Id FROM PeriodTypes WHERE Name = 'Semestre';
SELECT @cuatrimestre = PK_Period_Type_Id FROM PeriodTypes WHERE Name = 'Cuatrimestre';
SELECT @trimestre = PK_Period_Type_Id FROM PeriodTypes WHERE Name = 'Trimestre';
SELECT @bimestre = PK_Period_Type_Id FROM PeriodTypes WHERE Name = 'Bimestre';

INSERT INTO Periods (Value, FK_Period_Type_Id) VALUES
(1, @semestre),
(2, @semestre),
(1, @cuatrimestre),
(2, @cuatrimestre),
(3, @cuatrimestre),
(1, @trimestre),
(2, @trimestre),
(3, @trimestre),
(4, @trimestre),
(1, @bimestre),
(2, @bimestre),
(3, @bimestre),
(4, @bimestre),
(5, @bimestre),
(6, @bimestre);