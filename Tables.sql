USE TallerMecanico

CREATE TABLE Vehiculo (
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,
    Marca VARCHAR(100) NOT NULL,
    Modelo VARCHAR(100) NOT NULL,
    Patente VARCHAR(50) NOT NULL
);

CREATE TABLE Automovil (
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,
    IdVehiculo BIGINT NOT NULL FOREIGN KEY REFERENCES Vehiculo(Id),
    Tipo SMALLINT,
    CantidadPuertas SMALLINT
);

CREATE TABLE Moto (
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,
    IdVehiculo BIGINT NOT NULL FOREIGN KEY REFERENCES Vehiculo(Id),
    Cilindrada VARCHAR(50)
);

CREATE TABLE Presupuesto (
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Apellido VARCHAR(100) NOT NULL,
    Email VARCHAR(100),
    Total DECIMAL(18,6) NOT NULL,
    IdVehiculo BIGINT NOT NULL FOREIGN KEY REFERENCES Vehiculo(Id)
);

CREATE TABLE Desperfecto (
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,
    IdPresupuesto BIGINT NOT NULL FOREIGN KEY REFERENCES Presupuesto(Id),
    Descripcion VARCHAR(255),
    ManoDeObra DECIMAL(18,6) NOT NULL,
    Tiempo INT
);
--DROP TABLE Repuesto
CREATE TABLE Repuesto (
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Precio DECIMAL(18,6) NOT NULL
);

CREATE TABLE DesperfectoRepuesto (
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,
    IdDesperfecto BIGINT NOT NULL FOREIGN KEY REFERENCES Desperfecto(Id),
    IdRepuesto BIGINT NOT NULL FOREIGN KEY REFERENCES Repuesto(Id)
);


--PROC Automoviles------------------------

CREATE PROC SelectAutomovil @id BIGINT AS
BEGIN
SELECT v.* ,a.tipo, a.cantidadPuertas
FROM Automovil a
INNER JOIN Vehiculo v ON v.id=a.IdVehiculo
WHERE v.Id=@id
END


CREATE PROC SelectAutomoviles AS
BEGIN
SELECT v.* ,a.tipo, a.cantidadPuertas
FROM Automovil a
INNER JOIN Vehiculo v ON v.id=a.IdVehiculo
END

--EXEC SelectAutomovil 1  --EXEC SelectAutomoviles

CREATE PROC NewAutomovil @marca VARCHAR(100), @modelo VARCHAR(100), @patente VARCHAR(50), @tipo SMALLINT, @puertas SMALLINT AS
BEGIN
DECLARE @id INT;
INSERT INTO Vehiculo 
VALUES (@marca, @modelo, @patente);
SET @id=@@IDENTITY;
INSERT INTO Automovil 
VALUES (@id, @tipo, @puertas);
EXEC SelectAutomoviles
END


CREATE PROC EditAutomovil @id BIGINT, @marca VARCHAR(100), @modelo VARCHAR(100), @patente VARCHAR(50), @tipo SMALLINT, @puertas SMALLINT AS
BEGIN
UPDATE Vehiculo SET Marca=@marca, Modelo=@modelo, Patente=@patente
WHERE id=@id;
UPDATE Automovil SET Tipo=@tipo, CantidadPuertas=@puertas
WHERE IdVehiculo=@id;
EXEC SelectAutomoviles
END

--EXEC NewAutomovil  --EXEC EditAutomovil


--PROC Motos ------------------------------
CREATE PROC SelectMoto  @id BIGINT AS
BEGIN
SELECT v.* ,m.Cilindrada
FROM Moto m
INNER JOIN Vehiculo v ON v.id=m.IdVehiculo
WHERE v.Id=@id
END

CREATE PROC SelectMotos AS
BEGIN
SELECT v.* ,m.Cilindrada
FROM Moto m
INNER JOIN Vehiculo v ON v.id=m.IdVehiculo
END

CREATE PROC NewMoto @marca VARCHAR(100), @modelo VARCHAR(100), @patente VARCHAR(50), @cilindrada SMALLINT AS
BEGIN
DECLARE @id INT;
INSERT INTO Vehiculo 
VALUES (@marca, @modelo, @patente);
SET @id=@@IDENTITY;
INSERT INTO Moto 
VALUES (@id, @cilindrada);
EXEC SelectMotos
END

CREATE PROC EditMoto @id BIGINT, @marca VARCHAR(100), @modelo VARCHAR(100), @patente VARCHAR(50), @cilindrada SMALLINT AS
BEGIN
UPDATE Vehiculo SET Marca=@marca, Modelo=@modelo, Patente=@patente
WHERE id=@id;
UPDATE Moto SET Cilindrada=@cilindrada
WHERE IdVehiculo=@id;
EXEC SelectMotos
END


--PROC Desperfectos------------------------

CREATE PROC SelectDesperfecto @id BIGINT AS
BEGIN
SELECT * 
FROM Desperfecto
WHERE id=@id
END

CREATE PROC SelectDesperfectos @idPresupuesto BIGINT AS
BEGIN
SELECT id, IdPresupuesto, ManoDeObra, Tiempo 
FROM Desperfecto
WHERE IdPresupuesto=@idPresupuesto
END

CREATE PROC EditDesperfecto @idDesperfecto BIGINT, @idPresupuesto BIGINT, @descripcion VARCHAR(255), @manoObra DECIMAL(18,6), @tiempo INT AS
BEGIN
UPDATE Desperfecto SET Descripcion=@descripcion, ManoDeObra=@manoObra, Tiempo=@tiempo
WHERE id=@idDesperfecto;

EXEC CalculoTotal @idpresupuesto
EXEC SelectDesperfectos @idPresupuesto
END

CREATE PROC NewDesperfecto @idPresupuesto BIGINT, @descripcion VARCHAR(255), @manoObra DECIMAL(18,6), @tiempo INT AS
BEGIN
INSERT INTO Desperfecto
VALUES (@idPresupuesto ,@descripcion, @manoObra, @tiempo)
EXEC CalculoTotal @idPresupuesto
EXEC SelectDesperfectos @idPresupuesto
END

--PROC Repuestos--------------------------

CREATE PROC SelectRepuestos @idDesperfecto BIGINT AS
BEGIN
SELECT r.* FROM Repuesto r WHERE Id NOT IN(SELECT IdRepuesto FROM DesperfectoRepuesto WHERE IdDesperfecto=@idDesperfecto)
END

CREATE PROC NewRepuesto @nombre VARCHAR(100), @precio DECIMAL(18,6) AS
BEGIN
INSERT INTO Repuesto
VALUES (@nombre ,@precio)
EXEC SelectRepuestos 0
END

CREATE PROC EditRepuesto @idRepuesto BIGINT, @nombre VARCHAR(100), @precio DECIMAL(18,6) AS
BEGIN
UPDATE Repuesto SET Nombre=@nombre, Precio=@precio
WHERE id=@idRepuesto;
EXEC SelectRepuestos 0
END

--PROC RepuestoEnDefecto--------------------------

CREATE PROC SelectRepuestosEnDefecto @idDesperfecto BIGINT AS
BEGIN
SELECT r.Id, r.Nombre, r.Precio
FROM DesperfectoRepuesto d
INNER JOIN Repuesto r ON r.Id=d.IdRepuesto
WHERE d.IdDesperfecto = @idDesperfecto
ORDER BY d.IdRepuesto
END

CREATE PROC EliminarRepuestoEnDefecto @idDesperfecto BIGINT AS
BEGIN
DELETE DesperfectoRepuesto WHERE IdDesperfecto=@idDesperfecto
END

CREATE PROC SetRepuestoEnDefecto @idDesperfecto BIGINT, @idRepuesto BIGINT AS
BEGIN
INSERT INTO DesperfectoRepuesto
VALUES (@idDesperfecto, @idRepuesto)

DECLARE @idpresupuesto BIGINT

SELECT  @idpresupuesto=IdPresupuesto 
FROM Desperfecto
WHERE Id=@idDesperfecto;

EXEC CalculoTotal @idpresupuesto
END


-- PROC Presupuesto--------------------

CREATE PROC SelectPresupuesto @idVehiculo BIGINT AS
BEGIN
SELECT *
FROM Presupuesto
WHERE IdVehiculo=@idVehiculo
END

DELETE Presupuesto WHERE Id>1

CREATE PROC NewPresupuesto @idVehiculo BIGINT, @nombre VARCHAR(100), @apellido VARCHAR(100), @email VARCHAR(100) AS
BEGIN
INSERT INTO Presupuesto
VALUES (@nombre, @apellido, @email, 0, @idVehiculo)
SELECT @@IDENTITY AS Id
END

CREATE PROC EditPresupuesto @idpresupuesto BIGINT, @nombre VARCHAR(100), @apellido VARCHAR(100), @email VARCHAR(100) AS
BEGIN
UPDATE Presupuesto
SET Nombre=@nombre, Apellido=@apellido, Email=@email
WHERE Id=@idpresupuesto
END

CREATE PROC CalculoTotal @idpresupuesto BIGINT AS
BEGIN
DECLARE @total DECIMAL(18,6);
SET @total=
(
	SELECT
	(
		(
			(
			SELECT SUM(r.Precio) FROM Repuesto r
			INNER JOIN DesperfectoRepuesto dr ON dr.IdRepuesto=r.Id
			INNER JOIN Desperfecto d ON d.Id=dr.IdDesperfecto
			INNER JOIN Presupuesto p ON p.Id=d.IdPresupuesto
			WHERE p.Id=@idpresupuesto
			)
			+
			(
			SELECT SUM(ManoDeObra+(Tiempo*130)) FROM Desperfecto
			WHERE IdPresupuesto=@idpresupuesto
			)
		)
		*1.1
	)
)
IF(@total IS NOT NULL)
	BEGIN
	UPDATE Presupuesto
	SET Total=@total
	WHERE Id=@idpresupuesto
	END
END

-- PROC Resumen--------------------

CREATE PROC ResumenMasUsado AS
BEGIN
SELECT v.Marca, v.Modelo, r.Id, r.Nombre, r.precio, COUNT(r.Nombre)AS 'Cantidad' FROM DesperfectoRepuesto dr
INNER JOIN Repuesto r ON r.Id=dr.IdRepuesto
INNER JOIN Desperfecto d ON d.Id=dr.IdDesperfecto
INNER JOIN Presupuesto p ON p.Id=d.IdPresupuesto
INNER JOIN Vehiculo v ON v.Id=p.IdVehiculo
GROUP BY r.Id, r.Nombre, r.precio, v.Marca, v.Modelo
HAVING COUNT(r.Nombre) > 1
ORDER BY Cantidad DESC
END

CREATE PROC ResumenPromedio AS
BEGIN
SELECT v.Marca, v.Modelo, AVG(p.Total)
FROM Presupuesto p
INNER JOIN Vehiculo v ON v.Id=p.IdVehiculo
GROUP BY v.Marca, v.Modelo
END

CREATE PROC ResumenSumatorias AS
BEGIN
SELECT
(SELECT SUM(p.Total)
FROM Presupuesto p
INNER JOIN Vehiculo v ON v.Id=p.IdVehiculo
INNER JOIN Automovil a ON a.IdVehiculo=v.Id
) AS 'Automoviles'
,
(SELECT SUM(p.Total)
FROM Presupuesto p
INNER JOIN Vehiculo v ON v.Id=p.IdVehiculo
INNER JOIN Moto m ON m.IdVehiculo=v.Id
) AS 'Motos'
END




--Ejemplos
USE TallerMecanico
INSERT INTO Vehiculo VALUES ('Mercedes', 'C200', 'MNA234');
INSERT INTO Vehiculo VALUES ('Yamaha', 'FZ250', '234LFP');
INSERT INTO Vehiculo VALUES ('Peugeot', '208', 'OPA214');
INSERT INTO Vehiculo VALUES ('Alfa Romeo (Italy and)', 'Geriund 20.0', 'AB 1456 FFFF');
SELECT * FROM Vehiculo

INSERT INTO Automovil VALUES (1,4,5)
INSERT INTO Automovil VALUES (3,2,5)
INSERT INTO Automovil VALUES (4,4,5)
SELECT * FROM Automovil

INSERT INTO Presupuesto VALUES ('Paloma', 'Sanches', 'paosanches@gmail.com', 18000.60, 1);
SELECT * FROM Presupuesto

INSERT INTO Desperfecto VALUES (1, 'Cambio de embrague en la caja.', 360 , 3);
SELECT * FROM Desperfecto
SELECT * FROM Repuesto

INSERT INTO DesperfectoRepuesto VALUES (1, 1);
INSERT INTO DesperfectoRepuesto VALUES (1, 2);
INSERT INTO DesperfectoRepuesto VALUES (1, 3);
SELECT * FROM DesperfectoRepuesto

SELECT * FROM Presupuesto
SELECT * FROM Repuesto

DELETe FROM Repuesto WHERE Id>23

EXEC SelectDesperfectos 1