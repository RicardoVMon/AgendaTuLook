CREATE DATABASE [AgendaTuLook]

USE [AgendaTuLook]
GO
/****** Object:  Table [dbo].[tCalificaciones]    Script Date: 3/9/2025 10:00:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tCalificaciones](
	[CalificacionId] [bigint] IDENTITY(1,1) NOT NULL,
	[CitaId] [bigint] NOT NULL,
	[Puntuacion] [int] NOT NULL,
	[Comentario] [text] NULL,
	[Fecha] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[CalificacionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tCitas]    Script Date: 3/9/2025 10:00:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tCitas](
	[CitaId] [bigint] IDENTITY(1,1) NOT NULL,
	[UsuarioId] [bigint] NOT NULL,
	[ServicioId] [bigint] NOT NULL,
	[FechaInicio] [datetime] NOT NULL,
	[FechaFin] [datetime] NOT NULL,
	[Estado] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CitaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tExcepciones]    Script Date: 3/9/2025 10:00:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tExcepciones](
	[ExcepcionId] [bigint] IDENTITY(1,1) NOT NULL,
	[Mensaje] [text] NOT NULL,
	[Fecha] [datetime] NULL,
	[Origen] [varchar](50) NOT NULL,
	[UsuarioId] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[ExcepcionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tMetodosPago]    Script Date: 3/9/2025 10:00:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tMetodosPago](
	[MetodoPagoId] [bigint] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[Estado] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[MetodoPagoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tNotificaciones]    Script Date: 3/9/2025 10:00:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tNotificaciones](
	[NotificacionId] [bigint] IDENTITY(1,1) NOT NULL,
	[UsuarioId] [bigint] NOT NULL,
	[Mensaje] [varchar](255) NOT NULL,
	[FechaEnvio] [datetime] NULL,
	[Leido] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[NotificacionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tPagos]    Script Date: 3/9/2025 10:00:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tPagos](
	[PagoId] [bigint] IDENTITY(1,1) NOT NULL,
	[CitaId] [bigint] NOT NULL,
	[MetodoPagoId] [bigint] NOT NULL,
	[Monto] [decimal](10, 2) NOT NULL,
	[FechaPago] [datetime] NULL,
	[TransaccionId] [varchar](255) NULL,
	[EstadoPago] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[PagoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tProveedoresAuth]    Script Date: 3/9/2025 10:00:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tProveedoresAuth](
	[ProveedorAuthId] [bigint] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](6) NOT NULL,
 CONSTRAINT [PK_tProveedoresAuth] PRIMARY KEY CLUSTERED 
(
	[ProveedorAuthId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tRoles]    Script Date: 3/9/2025 10:00:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tRoles](
	[RolId] [bigint] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RolId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tServicios]    Script Date: 3/9/2025 10:00:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tServicios](
	[ServicioId] [bigint] IDENTITY(1,1) NOT NULL,
	[NombreServicio] [varchar](100) NOT NULL,
	[Descripcion] [text] NULL,
	[Precio] [decimal](10, 2) NOT NULL,
	[Duracion] [int] NOT NULL,
	[Imagen] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[ServicioId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tUsuarios]    Script Date: 3/9/2025 10:00:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tUsuarios](
	[UsuarioId] [bigint] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Correo] [varchar](100) NOT NULL,
	[Contrasennia] [varchar](255) NULL,
	[GoogleId] [varchar](255) NULL,
	[Telefono] [varchar](15) NULL,
	[RolId] [bigint] NOT NULL,
	[FechaRegistro] [datetime] NULL,
	[ProveedorAuthId] [bigint] NULL,
	[TieneContrasennaTemp] [bit] NULL,
	[FechaVencimientoTemp] [datetime] NULL,
 CONSTRAINT [PK__tUsuario__2B3DE7B86517B5AB] PRIMARY KEY CLUSTERED 
(
	[UsuarioId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[tProveedoresAuth] ON 
GO
INSERT [dbo].[tProveedoresAuth] ([ProveedorAuthId], [Nombre]) VALUES (1, N'Email')
GO
INSERT [dbo].[tProveedoresAuth] ([ProveedorAuthId], [Nombre]) VALUES (2, N'Google')
GO
SET IDENTITY_INSERT [dbo].[tProveedoresAuth] OFF
GO
SET IDENTITY_INSERT [dbo].[tRoles] ON 
GO
INSERT [dbo].[tRoles] ([RolId], [Nombre]) VALUES (1, N'Administrador')
GO
INSERT [dbo].[tRoles] ([RolId], [Nombre]) VALUES (2, N'Cliente')
GO
SET IDENTITY_INSERT [dbo].[tRoles] OFF
GO
SET IDENTITY_INSERT [dbo].[tUsuarios] ON 
GO
INSERT [dbo].[tUsuarios] ([UsuarioId], [Nombre], [Correo], [Contrasennia], [GoogleId], [Telefono], [RolId], [FechaRegistro], [ProveedorAuthId], [TieneContrasennaTemp], [FechaVencimientoTemp]) VALUES (10018, N'Ricardo Vargas', N'ricardovm2803@gmail.com', NULL, N'103966479926404496735', NULL, 2, CAST(N'2025-03-03T10:42:42.103' AS DateTime), 2, 0, NULL)
GO
INSERT [dbo].[tUsuarios] ([UsuarioId], [Nombre], [Correo], [Contrasennia], [GoogleId], [Telefono], [RolId], [FechaRegistro], [ProveedorAuthId], [TieneContrasennaTemp], [FechaVencimientoTemp]) VALUES (10019, N'Manolito Jiménez', N'manolete@gmail.com', N'klVMA1ubTkEMAthQ6Np0mA==', NULL, N'11223344', 2, CAST(N'2025-03-03T10:56:21.487' AS DateTime), 1, 0, NULL)
GO
INSERT [dbo].[tUsuarios] ([UsuarioId], [Nombre], [Correo], [Contrasennia], [GoogleId], [Telefono], [RolId], [FechaRegistro], [ProveedorAuthId], [TieneContrasennaTemp], [FechaVencimientoTemp]) VALUES (10020, N'RICARDO AARON VARGAS MONTERO', N'123hijo36@gmail.com', N'klVMA1ubTkEMAthQ6Np0mA==', N'107552496017882252155', N'11223344', 2, CAST(N'2025-03-03T11:05:19.867' AS DateTime), 1, 0, NULL)
GO
INSERT [dbo].[tUsuarios] ([UsuarioId], [Nombre], [Correo], [Contrasennia], [GoogleId], [Telefono], [RolId], [FechaRegistro], [ProveedorAuthId], [TieneContrasennaTemp], [FechaVencimientoTemp]) VALUES (10021, N'Ari Prueba', N'amata90989@ufide.ac.cr', N'klVMA1ubTkEMAthQ6Np0mA==', NULL, N'88888888', 2, CAST(N'2025-03-09T15:01:26.800' AS DateTime), 1, 0, NULL)
GO
INSERT [dbo].[tUsuarios] ([UsuarioId], [Nombre], [Correo], [Contrasennia], [GoogleId], [Telefono], [RolId], [FechaRegistro], [ProveedorAuthId], [TieneContrasennaTemp], [FechaVencimientoTemp]) VALUES (10022, N'Arianna Mata', N'arianna.j3009@gmail.com', NULL, N'116997312620363857961', NULL, 2, CAST(N'2025-03-09T15:02:57.527' AS DateTime), 2, 0, NULL)
GO
INSERT [dbo].[tUsuarios] ([UsuarioId], [Nombre], [Correo], [Contrasennia], [GoogleId], [Telefono], [RolId], [FechaRegistro], [ProveedorAuthId], [TieneContrasennaTemp], [FechaVencimientoTemp]) VALUES (10023, N'Jose Juan', N'jose@correo', N'klVMA1ubTkEMAthQ6Np0mA==', NULL, N'88888888', 2, CAST(N'2025-03-09T19:20:39.560' AS DateTime), 1, 0, NULL)
GO
SET IDENTITY_INSERT [dbo].[tUsuarios] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__tMetodos__75E3EFCF909AA73D]    Script Date: 3/9/2025 10:00:17 PM ******/
ALTER TABLE [dbo].[tMetodosPago] ADD UNIQUE NONCLUSTERED 
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__tUsuario__60695A1915D2DBE0]    Script Date: 3/9/2025 10:00:17 PM ******/
ALTER TABLE [dbo].[tUsuarios] ADD  CONSTRAINT [UQ__tUsuario__60695A1915D2DBE0] UNIQUE NONCLUSTERED 
(
	[Correo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tCalificaciones] ADD  DEFAULT (getdate()) FOR [Fecha]
GO
ALTER TABLE [dbo].[tExcepciones] ADD  DEFAULT (getdate()) FOR [Fecha]
GO
ALTER TABLE [dbo].[tMetodosPago] ADD  DEFAULT ((1)) FOR [Estado]
GO
ALTER TABLE [dbo].[tNotificaciones] ADD  DEFAULT (getdate()) FOR [FechaEnvio]
GO
ALTER TABLE [dbo].[tNotificaciones] ADD  DEFAULT ((0)) FOR [Leido]
GO
ALTER TABLE [dbo].[tPagos] ADD  DEFAULT (getdate()) FOR [FechaPago]
GO
ALTER TABLE [dbo].[tUsuarios] ADD  CONSTRAINT [DF__tUsuarios__Fecha__3A81B327]  DEFAULT (getdate()) FOR [FechaRegistro]
GO
ALTER TABLE [dbo].[tCalificaciones]  WITH CHECK ADD  CONSTRAINT [FK_tCalificaiones_Cita] FOREIGN KEY([CitaId])
REFERENCES [dbo].[tCitas] ([CitaId])
GO
ALTER TABLE [dbo].[tCalificaciones] CHECK CONSTRAINT [FK_tCalificaiones_Cita]
GO
ALTER TABLE [dbo].[tCitas]  WITH CHECK ADD  CONSTRAINT [FK_tCitas_Servicio] FOREIGN KEY([ServicioId])
REFERENCES [dbo].[tServicios] ([ServicioId])
GO
ALTER TABLE [dbo].[tCitas] CHECK CONSTRAINT [FK_tCitas_Servicio]
GO
ALTER TABLE [dbo].[tCitas]  WITH CHECK ADD  CONSTRAINT [FK_tCitas_Usuario] FOREIGN KEY([UsuarioId])
REFERENCES [dbo].[tUsuarios] ([UsuarioId])
GO
ALTER TABLE [dbo].[tCitas] CHECK CONSTRAINT [FK_tCitas_Usuario]
GO
ALTER TABLE [dbo].[tExcepciones]  WITH CHECK ADD  CONSTRAINT [FK_tExcepciones_Usuarios] FOREIGN KEY([UsuarioId])
REFERENCES [dbo].[tUsuarios] ([UsuarioId])
GO
ALTER TABLE [dbo].[tExcepciones] CHECK CONSTRAINT [FK_tExcepciones_Usuarios]
GO
ALTER TABLE [dbo].[tNotificaciones]  WITH CHECK ADD  CONSTRAINT [FK_tNotificaciones_Usuario] FOREIGN KEY([UsuarioId])
REFERENCES [dbo].[tUsuarios] ([UsuarioId])
GO
ALTER TABLE [dbo].[tNotificaciones] CHECK CONSTRAINT [FK_tNotificaciones_Usuario]
GO
ALTER TABLE [dbo].[tPagos]  WITH CHECK ADD  CONSTRAINT [FK_tPagos_Cita] FOREIGN KEY([CitaId])
REFERENCES [dbo].[tCitas] ([CitaId])
GO
ALTER TABLE [dbo].[tPagos] CHECK CONSTRAINT [FK_tPagos_Cita]
GO
ALTER TABLE [dbo].[tPagos]  WITH CHECK ADD  CONSTRAINT [FK_tPagos_MetodoPago] FOREIGN KEY([MetodoPagoId])
REFERENCES [dbo].[tMetodosPago] ([MetodoPagoId])
GO
ALTER TABLE [dbo].[tPagos] CHECK CONSTRAINT [FK_tPagos_MetodoPago]
GO
ALTER TABLE [dbo].[tUsuarios]  WITH CHECK ADD  CONSTRAINT [FK_tUsuarios_Rol] FOREIGN KEY([RolId])
REFERENCES [dbo].[tRoles] ([RolId])
GO
ALTER TABLE [dbo].[tUsuarios] CHECK CONSTRAINT [FK_tUsuarios_Rol]
GO
ALTER TABLE [dbo].[tUsuarios]  WITH CHECK ADD  CONSTRAINT [FK_tUsuarios_tProveedoresAuth] FOREIGN KEY([ProveedorAuthId])
REFERENCES [dbo].[tProveedoresAuth] ([ProveedorAuthId])
GO
ALTER TABLE [dbo].[tUsuarios] CHECK CONSTRAINT [FK_tUsuarios_tProveedoresAuth]
GO
ALTER TABLE [dbo].[tCalificaciones]  WITH CHECK ADD CHECK  (([puntuacion]>=(1) AND [puntuacion]<=(5)))
GO
ALTER TABLE [dbo].[tCitas]  WITH CHECK ADD CHECK  (([estado]='Cancelada' OR [estado]='Confirmada' OR [estado]='Finalizada'))
GO
/****** Object:  StoredProcedure [dbo].[ActualizarContrasennaTemp]    Script Date: 3/9/2025 10:00:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ActualizarContrasennaTemp]
    @UsuarioId BIGINT,
    @ContrasennaTemp VARCHAR(255),
    @TieneContrasennaTemp BIT,
    @FechaVencimientoTemp DATETIME
AS
BEGIN
    UPDATE tUsuarios
    SET Contrasennia = @ContrasennaTemp, 
        TieneContrasennaTemp = @TieneContrasennaTemp,
        FechaVencimientoTemp = @FechaVencimientoTemp
    WHERE UsuarioId = @UsuarioId;
END
GO
/****** Object:  StoredProcedure [dbo].[ActualizarGoogleId]    Script Date: 3/9/2025 10:00:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ActualizarGoogleId] 
	@GoogleId VARCHAR(255),
	@Correo VARCHAR(100)
AS
BEGIN
	UPDATE tUsuarios
	SET GoogleId = @GoogleId
	WHERE Correo = @Correo
END
GO
/****** Object:  StoredProcedure [dbo].[CambiarContrasennaTemp]    Script Date: 3/9/2025 10:00:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CambiarContrasennaTemp]
    @Correo VARCHAR(100),
    @NuevaContrasenna VARCHAR(255)
AS
BEGIN
    UPDATE tUsuarios
    SET Contrasennia = @NuevaContrasenna,
        TieneContrasennaTemp = 0,
        FechaVencimientoTemp = NULL
    WHERE Correo = @Correo;
END
GO
/****** Object:  StoredProcedure [dbo].[ExisteCorreo]    Script Date: 3/9/2025 10:00:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ExisteCorreo]
	@Correo VARCHAR(100)
AS
BEGIN
	 IF EXISTS (SELECT 1 FROM tUsuarios WHERE Correo = @Correo)
	 BEGIN
		 SELECT 1 AS Existe
	 END
	 ELSE
	 BEGIN
		 SELECT 0 AS Existe
	 END
END
GO
/****** Object:  StoredProcedure [dbo].[Login]    Script Date: 3/9/2025 10:00:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Login]
	@Correo VARCHAR(100),
	@Contrasennia VARCHAR(255)
AS
BEGIN
	SELECT	UsuarioId,
			Nombre,
			Correo,
			RolId,
			TieneContrasennaTemp,
			FechaVencimientoTemp
	FROM	tUsuarios
	WHERE   Correo = @Correo
	AND Contrasennia = @Contrasennia
END
GO
/****** Object:  StoredProcedure [dbo].[ObtenerIdUsuarioConCorreo]    Script Date: 3/9/2025 10:00:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ObtenerIdUsuarioConCorreo]
	@Correo VARCHAR(100)
AS
BEGIN
	SELECT UsuarioId,
	Correo
	FROM tUsuarios
	WHERE Correo = @Correo
END
GO
/****** Object:  StoredProcedure [dbo].[ObtenerProveedorAuthConCorreo]    Script Date: 3/9/2025 10:00:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ObtenerProveedorAuthConCorreo]
	@Correo VARCHAR(100)
AS
BEGIN
	SELECT PA.Nombre,
	CASE
		WHEN U.Contrasennia IS NOT NULL THEN 1
		ELSE 0
	END AS TieneContrasennia,
	CASE
		WHEN U.GoogleId IS NOT NULL THEN 1
		ELSE 0
	END AS TieneGoogleId
	FROM tUsuarios U
	INNER JOIN tProveedoresAuth PA ON PA.ProveedorAuthId = U.ProveedorAuthId
	WHERE U.Correo = @Correo
END
GO
/****** Object:  StoredProcedure [dbo].[RegistroUsuario]    Script Date: 3/9/2025 10:00:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RegistroUsuario]
	@Nombre VARCHAR(100),
	@Correo VARCHAR(100),
	@Contrasennia VARCHAR(255),
	@GoogleId VARCHAR(255),
	@Telefono VARCHAR(15),
	@ProveedorAuth VARCHAR(6)
AS
BEGIN
	DECLARE @RolId BIGINT;
	DECLARE @ProveedorAuthId BIGINT;
	DECLARE @UsuarioId BIGINT;

	SET @RolId = (SELECT RolId FROM tRoles WHERE Nombre = 'Cliente');
	SET @ProveedorAuthId = (SELECT ProveedorAuthId FROM tProveedoresAuth WHERE Nombre = @ProveedorAuth);

	INSERT INTO tUsuarios (Nombre, Correo, Contrasennia, GoogleId, Telefono, ProveedorAuthId, RolId, TieneContrasennaTemp, FechaVencimientoTemp)
	VALUES (@Nombre, @Correo, @Contrasennia, @GoogleId, @Telefono, @ProveedorAuthId, @RolId, 0, NULL)

	SET @UsuarioId = SCOPE_IDENTITY();
	SELECT @UsuarioId AS UsuarioId;
END
GO
USE [master]
GO
ALTER DATABASE [AgendaTuLook] SET  READ_WRITE 
GO
