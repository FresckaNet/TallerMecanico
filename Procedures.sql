USE TallerMecanico
GO
/*
EXEC dbo.MassiveCharge 
*/

/*****************************************************************************************/
/*+                                                                                      */
/*+ Nombre  : dbo.MassiveCharge                                                          */
/*+ Objetivo: Insertar en la BD una serie de Respuestos con sus Precios                  */
/*+                                                                                      */
/*****************************************************************************************/

CREATE PROC [dbo].[MassiveCharge] AS
BEGIN

/*+ Creaci�n de la tabla Temporal que contendr� los Repuestos con sus precios*/

    CREATE TABLE #TMP_RESPUESTO (Nombre VARCHAR(100),
                                 Precio DECIMAL(18,6))

/*+ Se generan los registros en la tabla temporal que posteriormente se evaluar�n para ver si procede su carga en la tabla definitiva de Repuestos*/

    BEGIN /*+ BEGIN INSERT EN LA TEMPORAL DE RESPUESTOS*/
        INSERT INTO #TMP_RESPUESTO VALUES ('B356963821', 17.61)
        INSERT INTO #TMP_RESPUESTO VALUES ('B881468337', 40.88)
        INSERT INTO #TMP_RESPUESTO VALUES ('B867719836', 87.76)
        INSERT INTO #TMP_RESPUESTO VALUES ('B397571688', 13.97)
        INSERT INTO #TMP_RESPUESTO VALUES ('B852883143', 47.97)
        INSERT INTO #TMP_RESPUESTO VALUES ('B461882670', 22.68)
        INSERT INTO #TMP_RESPUESTO VALUES ('B333520964', 82.28)
        INSERT INTO #TMP_RESPUESTO VALUES ('B388445039', 50.71)
        INSERT INTO #TMP_RESPUESTO VALUES ('B648201513', 21.83)
        INSERT INTO #TMP_RESPUESTO VALUES ('B436759416', 35.39)
        INSERT INTO #TMP_RESPUESTO VALUES ('B317533243', 22.84)
        INSERT INTO #TMP_RESPUESTO VALUES ('B666592414', 58.67)
        INSERT INTO #TMP_RESPUESTO VALUES ('B443568817', 53.83)
        INSERT INTO #TMP_RESPUESTO VALUES ('B316416378', 17.74)
        INSERT INTO #TMP_RESPUESTO VALUES ('B252543362', 16.98)
        INSERT INTO #TMP_RESPUESTO VALUES ('B453148609', 14.23)
        INSERT INTO #TMP_RESPUESTO VALUES ('B254958806', 41.19)
        INSERT INTO #TMP_RESPUESTO VALUES ('B356963821', 62.58)
        INSERT INTO #TMP_RESPUESTO VALUES ('B846487171', 92.91)
        INSERT INTO #TMP_RESPUESTO VALUES ('B397571688', 1.04)
        INSERT INTO #TMP_RESPUESTO VALUES ('B535169105', 90.14)
        INSERT INTO #TMP_RESPUESTO VALUES ('B628263302', 78.64)
        INSERT INTO #TMP_RESPUESTO VALUES ('B608816685', 93.73)
        INSERT INTO #TMP_RESPUESTO VALUES ('B660755442', 43.62)
        INSERT INTO #TMP_RESPUESTO VALUES ('B659053715', 90.59)
        INSERT INTO #TMP_RESPUESTO VALUES ('B556344166', 71.62)
        INSERT INTO #TMP_RESPUESTO VALUES ('B216140665', 93.15)
        INSERT INTO #TMP_RESPUESTO VALUES ('B843858581', 66.52)
        INSERT INTO #TMP_RESPUESTO VALUES ('B790077756', 8.91)
        INSERT INTO #TMP_RESPUESTO VALUES ('B916071768', 85.46)
        INSERT INTO #TMP_RESPUESTO VALUES ('B317533243', 7.97)
        INSERT INTO #TMP_RESPUESTO VALUES ('B343454513', 22.91)
        INSERT INTO #TMP_RESPUESTO VALUES ('B986574036', 65.10)
        INSERT INTO #TMP_RESPUESTO VALUES ('B662139869', 3.50)
        INSERT INTO #TMP_RESPUESTO VALUES ('B618792223', 6.87)
        INSERT INTO #TMP_RESPUESTO VALUES ('B578485476', 49.70)
        INSERT INTO #TMP_RESPUESTO VALUES ('B132813434', 32.58)
        INSERT INTO #TMP_RESPUESTO VALUES ('B776163235', 73.64)
        INSERT INTO #TMP_RESPUESTO VALUES ('B215908676', 92.83)
        INSERT INTO #TMP_RESPUESTO VALUES ('B871139440', 31.83)
        INSERT INTO #TMP_RESPUESTO VALUES ('B564893705', 18.91)
        INSERT INTO #TMP_RESPUESTO VALUES ('B634131771', 70.35)
        INSERT INTO #TMP_RESPUESTO VALUES ('B321187273', 91.96)
        INSERT INTO #TMP_RESPUESTO VALUES ('B444737823', 78.73)
        INSERT INTO #TMP_RESPUESTO VALUES ('B413525993', 9.93)
        INSERT INTO #TMP_RESPUESTO VALUES ('B229547877', 97.08)
        INSERT INTO #TMP_RESPUESTO VALUES ('B545788950', 11.84)
        INSERT INTO #TMP_RESPUESTO VALUES ('B658514562', 8.84)
        INSERT INTO #TMP_RESPUESTO VALUES ('B736313138', 78.47)
        INSERT INTO #TMP_RESPUESTO VALUES ('B840888802', 93.85)
        INSERT INTO #TMP_RESPUESTO VALUES ('B883572821', 21.57)
        INSERT INTO #TMP_RESPUESTO VALUES ('B493478663', 76.98)
        INSERT INTO #TMP_RESPUESTO VALUES ('B718838840', 7.41)
        INSERT INTO #TMP_RESPUESTO VALUES ('B183671709', 45.53)
        INSERT INTO #TMP_RESPUESTO VALUES ('B908384721', 14.73)
        INSERT INTO #TMP_RESPUESTO VALUES ('B566417680', 44.04)
        INSERT INTO #TMP_RESPUESTO VALUES ('B633833113', 33.28)
        INSERT INTO #TMP_RESPUESTO VALUES ('B829258206', 41.74)
        INSERT INTO #TMP_RESPUESTO VALUES ('B350041352', 85.13)
        INSERT INTO #TMP_RESPUESTO VALUES ('B548168477', 7.44)
        INSERT INTO #TMP_RESPUESTO VALUES ('B765657146', 89.79)
        INSERT INTO #TMP_RESPUESTO VALUES ('B830231322', 81.42)
        INSERT INTO #TMP_RESPUESTO VALUES ('B816385774', 9.30)
        INSERT INTO #TMP_RESPUESTO VALUES ('B857448796', 77.36)
        INSERT INTO #TMP_RESPUESTO VALUES ('B302875266', 54.89)
        INSERT INTO #TMP_RESPUESTO VALUES ('B790507487', 50.41)
        INSERT INTO #TMP_RESPUESTO VALUES ('B723629401', 65.36)
        INSERT INTO #TMP_RESPUESTO VALUES ('B595728629', 19.94)
        INSERT INTO #TMP_RESPUESTO VALUES ('B472436824', 65.69)
        INSERT INTO #TMP_RESPUESTO VALUES ('B235859870', 66.44)
        INSERT INTO #TMP_RESPUESTO VALUES ('B874178252', 42.38)
        INSERT INTO #TMP_RESPUESTO VALUES ('B777713850', 40.26)
        INSERT INTO #TMP_RESPUESTO VALUES ('B550221285', 8.72)
        INSERT INTO #TMP_RESPUESTO VALUES ('B816043247', 73.97)
        INSERT INTO #TMP_RESPUESTO VALUES ('B607313788', 15.95)
        INSERT INTO #TMP_RESPUESTO VALUES ('B396482694', 45.17)
        INSERT INTO #TMP_RESPUESTO VALUES ('B504021331', 24.52)
        INSERT INTO #TMP_RESPUESTO VALUES ('B651475349', 86.77)
        INSERT INTO #TMP_RESPUESTO VALUES ('B470409863', 11.81)
        INSERT INTO #TMP_RESPUESTO VALUES ('B264135435', 62.58)
        INSERT INTO #TMP_RESPUESTO VALUES ('B755636151', 33.88)
        INSERT INTO #TMP_RESPUESTO VALUES ('B382183955', 0.92)
        INSERT INTO #TMP_RESPUESTO VALUES ('B667316286', 0.29)
        INSERT INTO #TMP_RESPUESTO VALUES ('B783117048', 41.57)
        INSERT INTO #TMP_RESPUESTO VALUES ('B812952354', 86.25)
        INSERT INTO #TMP_RESPUESTO VALUES ('B621838237', 80.54)
        INSERT INTO #TMP_RESPUESTO VALUES ('B665465223', 53.69)
        INSERT INTO #TMP_RESPUESTO VALUES ('B881682635', 64.78)
        INSERT INTO #TMP_RESPUESTO VALUES ('B646289861', 72.01)
        INSERT INTO #TMP_RESPUESTO VALUES ('B852115667', 48.73)
        INSERT INTO #TMP_RESPUESTO VALUES ('B144635415', 34.23)
        INSERT INTO #TMP_RESPUESTO VALUES ('B874863828', 24.70)
        INSERT INTO #TMP_RESPUESTO VALUES ('B333841476', 41.57)
        INSERT INTO #TMP_RESPUESTO VALUES ('B587386017', 45.27)
        INSERT INTO #TMP_RESPUESTO VALUES ('B874270576', 42.38)
        INSERT INTO #TMP_RESPUESTO VALUES ('B300733136', 25.55)
        INSERT INTO #TMP_RESPUESTO VALUES ('B611446656', 60.12)
        INSERT INTO #TMP_RESPUESTO VALUES ('B801300387', 61.04)
        INSERT INTO #TMP_RESPUESTO VALUES ('B845153562', 60.09)
        INSERT INTO #TMP_RESPUESTO VALUES ('B943846621', 37.05)
    END /*+ END INSERT EN LA TEMPORAL DE RESPUESTOS*/

DECLARE @Repuesto TABLE(
    Nombre VARCHAR(100) NOT NULL,
    Precio DECIMAL(18,6) NOT NULL
)

DECLARE @NombreRepuesto VARCHAR(100), @PrecioRepuesto DECIMAL(18,6);

DECLARE [MassiveCursor] CURSOR FOR 
SELECT Nombre, Precio
FROM #TMP_RESPUESTO
--WHERE Precio<20
ORDER BY Nombre

OPEN MassiveCursor;
FETCH NEXT FROM MassiveCursor
INTO @NombreRepuesto, @PrecioRepuesto;

WHILE @@FETCH_STATUS = 0
BEGIN  
      IF(@PrecioRepuesto<20)
	  BEGIN
	        INSERT INTO @Repuesto (Nombre, Precio) VALUES (@NombreRepuesto, @PrecioRepuesto)
	  END

	  ELSE
	  BEGIN  
		   PRINT(@NombreRepuesto + ' no fue insertado.')
	  END

      FETCH NEXT FROM MassiveCursor
      INTO @NombreRepuesto, @PrecioRepuesto;
END 

CLOSE MassiveCursor
DEALLOCATE MassiveCursor

INSERT INTO Repuesto(Nombre, Precio)
SELECT Nombre, SUM(Precio)
FROM @Repuesto
Group BY Nombre

END
GO

--SELECT * FROM Repuesto