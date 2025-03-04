GO
USE AgendaTuLook;

GO
-- TABLA horario fecha:3/3/2025
CREATE TABLE dbo.tHorarios(
	HorariosId bigint primary key identity(1,1),
	HoraEntrada varchar(5) not null,
	HoraSalida varchar(5) not null,
	dia varchar(20) not null,
	Estado varchar(20) 

);

GO
-- Crear un horario fecha:3/3/2025
CREATE PROCEDURE dbo.RegistrarHorario
	@HoraEntrada varchar(5),
	@HoraSalida varchar(5),
	@dia varchar(20)
	AS
	DECLARE @Estado varchar(20) = 'Activo'
	BEGIN
	
		INSERT INTO  tHorarios(HoraEntrada,HoraSalida,dia,Estado)
		VALUES(@HoraEntrada, @HoraSalida,@dia,@Estado)
	
		
	END

GO
-- Eliminar un horario fecha:3/3/2025
CREATE PROCEDURE dbo.EliminarHorario
	@HorariosId  bigint
	AS
	BEGIN
		
	
		UPDATE tHorarios
		SET Estado = 'Inactivo'
		WHERE HorariosId = @HorariosId;
		

	
		
	END

GO
-- Actualizar un horario fecha:3/3/2025
CREATE PROCEDURE dbo.ActualizarHorario
	@HorariosId  bigint,
	@HoraEntrada varchar(5),
	@HoraSalida varchar(5),
	@dia varchar(20),
	@Estado varchar(20)
	AS

	BEGIN
		
	
		UPDATE tHorarios
		SET HoraEntrada = @HoraEntrada,
			HoraSalida = @HoraSalida,
			dia = @dia,
			Estado = @Estado
		WHERE HorariosId = @HorariosId;
		

	
		
	END
GO
CREATE PROCEDURE dbo.MostrarHorarios
	@Estado varchar(20)
AS
BEGIN
	SELECT HoraEntrada, HoraSalida, dia, Estado
	FROM tHorarios
	WHERE Estado = @Estado
END

--pruebas-------
--pruebas-------
--pruebas-------

GO
	EXEC dbo.RegistrarHorario
	@HoraEntrada = '8:30',
    @HoraSalida = '6:50',
	@dia = 'Lunes'

	EXEC dbo.RegistrarHorario
	@HoraEntrada = '8:30',
    @HoraSalida = '6:50',
	@dia = 'Martes'

	EXEC dbo.RegistrarHorario
	@HoraEntrada = '8:30',
    @HoraSalida = '6:50',
	@dia = 'Miercoles'

	EXEC dbo.RegistrarHorario
	@HoraEntrada = '8:30',
    @HoraSalida = '6:50',
	@dia = 'Jueves'

	EXEC dbo.RegistrarHorario
	@HoraEntrada = '8:30',
    @HoraSalida = '6:50',
	@dia = 'Viernes'

GO
	EXEC dbo.EliminarHorario
	@HorariosId = 2

GO
	EXEC dbo.ActualizarHorario
	@HorariosId = 1,
	@HoraEntrada = '8:30',
    @HoraSalida = '6:50',
	@dia = 'Lunes',
	@Estado = 'Activo'

GO
	EXEC dbo.MostrarHorarios
	@Estado = Activo
