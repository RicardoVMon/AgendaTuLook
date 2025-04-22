USE [AgendaTuLook]
GO
/****** Object:  Table [dbo].[tCitas]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tCitas](
	[CitaId] [bigint] IDENTITY(1,1) NOT NULL,
	[UsuarioId] [bigint] NOT NULL,
	[ServicioId] [bigint] NOT NULL,
	[Fecha] [date] NOT NULL,
	[HoraInicio] [time](0) NOT NULL,
	[HoraFin] [time](0) NOT NULL,
	[Estado] [varchar](20) NOT NULL,
	[DiaTrabajoId] [bigint] NULL,
 CONSTRAINT [PK__tCitas__F0E2D9D2725D11AA] PRIMARY KEY CLUSTERED 
(
	[CitaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tDiasTrabajo]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tDiasTrabajo](
	[DiaTrabajoId] [bigint] IDENTITY(1,1) NOT NULL,
	[NombreDia] [varchar](20) NOT NULL,
	[HoraInicio] [time](0) NULL,
	[HoraFin] [time](0) NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK__tDiasTra__D272B90B806A453D] PRIMARY KEY CLUSTERED 
(
	[DiaTrabajoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tExcepciones]    Script Date: 4/22/2025 3:20:30 PM ******/
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
/****** Object:  Table [dbo].[tMetodosPago]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tMetodosPago](
	[MetodoPagoId] [bigint] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[Descripcion] [varchar](255) NULL,
	[Estado] [bit] NULL,
 CONSTRAINT [PK__tMetodos__A8FEAF545E2DFAAB] PRIMARY KEY CLUSTERED 
(
	[MetodoPagoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tNotificaciones]    Script Date: 4/22/2025 3:20:30 PM ******/
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
/****** Object:  Table [dbo].[tPagos]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tPagos](
	[PagoId] [bigint] IDENTITY(1,1) NOT NULL,
	[CitaId] [bigint] NOT NULL,
	[MetodoPagoId] [bigint] NOT NULL,
	[FechaPago] [datetime] NULL,
	[Comprobante] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[PagoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tProveedoresAuth]    Script Date: 4/22/2025 3:20:30 PM ******/
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
/****** Object:  Table [dbo].[tReviews]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tReviews](
	[ReviewId] [bigint] IDENTITY(1,1) NOT NULL,
	[CitaId] [bigint] NOT NULL,
	[ComentarioReview] [nvarchar](1000) NULL,
	[CalificacionReview] [int] NULL,
	[fecha] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ReviewId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tRoles]    Script Date: 4/22/2025 3:20:30 PM ******/
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
/****** Object:  Table [dbo].[tServicios]    Script Date: 4/22/2025 3:20:30 PM ******/
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
	[Estado] [bit] NOT NULL,
 CONSTRAINT [PK__tServici__D5AEECC25E23B642] PRIMARY KEY CLUSTERED 
(
	[ServicioId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tUsuarios]    Script Date: 4/22/2025 3:20:30 PM ******/
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
	[CodigoVerificacion] [varchar](10) NULL,
	[FechaVencimientoVerificacion] [datetime] NULL,
 CONSTRAINT [PK__tUsuario__2B3DE7B86517B5AB] PRIMARY KEY CLUSTERED 
(
	[UsuarioId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[tCitas] ON 
GO
INSERT [dbo].[tCitas] ([CitaId], [UsuarioId], [ServicioId], [Fecha], [HoraInicio], [HoraFin], [Estado], [DiaTrabajoId]) VALUES (10021, 10033, 1, CAST(N'2025-04-11' AS Date), CAST(N'09:00:00' AS Time), CAST(N'10:00:00' AS Time), N'Finalizada', 5)
GO
INSERT [dbo].[tCitas] ([CitaId], [UsuarioId], [ServicioId], [Fecha], [HoraInicio], [HoraFin], [Estado], [DiaTrabajoId]) VALUES (20011, 10033, 1, CAST(N'2025-04-11' AS Date), CAST(N'13:00:00' AS Time), CAST(N'14:00:00' AS Time), N'Cancelada', 5)
GO
INSERT [dbo].[tCitas] ([CitaId], [UsuarioId], [ServicioId], [Fecha], [HoraInicio], [HoraFin], [Estado], [DiaTrabajoId]) VALUES (20012, 10033, 1, CAST(N'2025-04-14' AS Date), CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), N'Confirmada', 1)
GO
INSERT [dbo].[tCitas] ([CitaId], [UsuarioId], [ServicioId], [Fecha], [HoraInicio], [HoraFin], [Estado], [DiaTrabajoId]) VALUES (20013, 10033, 1, CAST(N'2025-04-15' AS Date), CAST(N'08:00:00' AS Time), CAST(N'09:00:00' AS Time), N'Finalizada', 2)
GO
INSERT [dbo].[tCitas] ([CitaId], [UsuarioId], [ServicioId], [Fecha], [HoraInicio], [HoraFin], [Estado], [DiaTrabajoId]) VALUES (20014, 10033, 1, CAST(N'2025-04-14' AS Date), CAST(N'11:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Finalizada', 1)
GO
INSERT [dbo].[tCitas] ([CitaId], [UsuarioId], [ServicioId], [Fecha], [HoraInicio], [HoraFin], [Estado], [DiaTrabajoId]) VALUES (20015, 10035, 1, CAST(N'2025-04-14' AS Date), CAST(N'13:00:00' AS Time), CAST(N'14:00:00' AS Time), N'Finalizada', 1)
GO
INSERT [dbo].[tCitas] ([CitaId], [UsuarioId], [ServicioId], [Fecha], [HoraInicio], [HoraFin], [Estado], [DiaTrabajoId]) VALUES (20016, 10036, 1, CAST(N'2025-04-18' AS Date), CAST(N'09:00:00' AS Time), CAST(N'10:00:00' AS Time), N'Finalizada', 5)
GO
INSERT [dbo].[tCitas] ([CitaId], [UsuarioId], [ServicioId], [Fecha], [HoraInicio], [HoraFin], [Estado], [DiaTrabajoId]) VALUES (20017, 10033, 1, CAST(N'2025-03-11' AS Date), CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), N'Confirmada', 2)
GO
INSERT [dbo].[tCitas] ([CitaId], [UsuarioId], [ServicioId], [Fecha], [HoraInicio], [HoraFin], [Estado], [DiaTrabajoId]) VALUES (20018, 10033, 1, CAST(N'2025-03-19' AS Date), CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), N'Confirmada', 3)
GO
INSERT [dbo].[tCitas] ([CitaId], [UsuarioId], [ServicioId], [Fecha], [HoraInicio], [HoraFin], [Estado], [DiaTrabajoId]) VALUES (20019, 10033, 2, CAST(N'2025-04-15' AS Date), CAST(N'09:00:00' AS Time), CAST(N'10:00:00' AS Time), N'Finalizada', 2)
GO
INSERT [dbo].[tCitas] ([CitaId], [UsuarioId], [ServicioId], [Fecha], [HoraInicio], [HoraFin], [Estado], [DiaTrabajoId]) VALUES (20020, 10037, 1, CAST(N'2025-04-29' AS Date), CAST(N'11:00:00' AS Time), CAST(N'11:30:00' AS Time), N'Confirmada', 5)
GO
INSERT [dbo].[tCitas] ([CitaId], [UsuarioId], [ServicioId], [Fecha], [HoraInicio], [HoraFin], [Estado], [DiaTrabajoId]) VALUES (20021, 10037, 4, CAST(N'2025-04-30' AS Date), CAST(N'13:00:00' AS Time), CAST(N'15:00:00' AS Time), N'Cancelada', 3)
GO
INSERT [dbo].[tCitas] ([CitaId], [UsuarioId], [ServicioId], [Fecha], [HoraInicio], [HoraFin], [Estado], [DiaTrabajoId]) VALUES (20022, 10037, 2, CAST(N'2025-04-22' AS Date), CAST(N'08:00:00' AS Time), CAST(N'09:00:00' AS Time), N'Cancelada', 2)
GO
SET IDENTITY_INSERT [dbo].[tCitas] OFF
GO
SET IDENTITY_INSERT [dbo].[tDiasTrabajo] ON 
GO
INSERT [dbo].[tDiasTrabajo] ([DiaTrabajoId], [NombreDia], [HoraInicio], [HoraFin], [Activo]) VALUES (1, N'Lunes', CAST(N'10:00:00' AS Time), CAST(N'18:00:00' AS Time), 0)
GO
INSERT [dbo].[tDiasTrabajo] ([DiaTrabajoId], [NombreDia], [HoraInicio], [HoraFin], [Activo]) VALUES (2, N'Martes', CAST(N'08:00:00' AS Time), CAST(N'13:00:00' AS Time), 1)
GO
INSERT [dbo].[tDiasTrabajo] ([DiaTrabajoId], [NombreDia], [HoraInicio], [HoraFin], [Activo]) VALUES (3, N'Miércoles', CAST(N'08:00:00' AS Time), CAST(N'16:00:00' AS Time), 1)
GO
INSERT [dbo].[tDiasTrabajo] ([DiaTrabajoId], [NombreDia], [HoraInicio], [HoraFin], [Activo]) VALUES (4, N'Jueves', CAST(N'06:00:00' AS Time), CAST(N'15:00:00' AS Time), 1)
GO
INSERT [dbo].[tDiasTrabajo] ([DiaTrabajoId], [NombreDia], [HoraInicio], [HoraFin], [Activo]) VALUES (5, N'Viernes', CAST(N'08:00:00' AS Time), CAST(N'16:00:00' AS Time), 1)
GO
INSERT [dbo].[tDiasTrabajo] ([DiaTrabajoId], [NombreDia], [HoraInicio], [HoraFin], [Activo]) VALUES (6, N'Sábado', CAST(N'08:00:00' AS Time), CAST(N'16:00:00' AS Time), 0)
GO
INSERT [dbo].[tDiasTrabajo] ([DiaTrabajoId], [NombreDia], [HoraInicio], [HoraFin], [Activo]) VALUES (7, N'Domingo', CAST(N'08:00:00' AS Time), CAST(N'16:00:00' AS Time), 0)
GO
SET IDENTITY_INSERT [dbo].[tDiasTrabajo] OFF
GO
SET IDENTITY_INSERT [dbo].[tExcepciones] ON 
GO
INSERT [dbo].[tExcepciones] ([ExcepcionId], [Mensaje], [Fecha], [Origen], [UsuarioId]) VALUES (1, N'Cannot implicitly convert type ''decimal'' to ''double''. An explicit conversion exists (are you missing a cast?)', CAST(N'2025-04-21T05:21:09.317' AS DateTime), N'/api/Citas/ObtenerCitaParaEditar', 10037)
GO
INSERT [dbo].[tExcepciones] ([ExcepcionId], [Mensaje], [Fecha], [Origen], [UsuarioId]) VALUES (2, N'Cannot implicitly convert type ''decimal'' to ''double''. An explicit conversion exists (are you missing a cast?)', CAST(N'2025-04-21T05:27:05.423' AS DateTime), N'/api/Citas/ObtenerCitaParaEditar', 10037)
GO
INSERT [dbo].[tExcepciones] ([ExcepcionId], [Mensaje], [Fecha], [Origen], [UsuarioId]) VALUES (3, N'Cannot implicitly convert type ''decimal'' to ''double''. An explicit conversion exists (are you missing a cast?)', CAST(N'2025-04-21T05:27:52.807' AS DateTime), N'/api/Citas/ObtenerCitaParaEditar', 10037)
GO
INSERT [dbo].[tExcepciones] ([ExcepcionId], [Mensaje], [Fecha], [Origen], [UsuarioId]) VALUES (4, N'Cannot implicitly convert type ''decimal'' to ''double''. An explicit conversion exists (are you missing a cast?)', CAST(N'2025-04-21T05:30:28.693' AS DateTime), N'/api/Citas/ObtenerCitaParaEditar', 10037)
GO
INSERT [dbo].[tExcepciones] ([ExcepcionId], [Mensaje], [Fecha], [Origen], [UsuarioId]) VALUES (5, N'Object reference not set to an instance of an object.', CAST(N'2025-04-21T22:31:57.920' AS DateTime), N'/api/Citas/ActualizarCita', 10037)
GO
INSERT [dbo].[tExcepciones] ([ExcepcionId], [Mensaje], [Fecha], [Origen], [UsuarioId]) VALUES (6, N'Object reference not set to an instance of an object.', CAST(N'2025-04-21T22:43:11.950' AS DateTime), N'/api/Citas/ActualizarCita', 10032)
GO
INSERT [dbo].[tExcepciones] ([ExcepcionId], [Mensaje], [Fecha], [Origen], [UsuarioId]) VALUES (10002, N'Could not find a part of the path ''C:\FIDELITAS\7 cuatri\Progra Avanzada Web\AgendaTuLook\AgendaTuLookAPI\AgendaTuLookAPI\wwwroot\img\servicios\servicio_4_20250422120950.jpg''.', CAST(N'2025-04-22T12:09:50.747' AS DateTime), N'/api/Servicios/EditarServicio', 10032)
GO
INSERT [dbo].[tExcepciones] ([ExcepcionId], [Mensaje], [Fecha], [Origen], [UsuarioId]) VALUES (10003, N'Could not find a part of the path ''C:\FIDELITAS\7 cuatri\Progra Avanzada Web\AgendaTuLook\AgendaTuLookAPI\AgendaTuLookAPI\wwwroot\img\servicios\servicio_5_20250422121815.jpg''.', CAST(N'2025-04-22T12:18:15.697' AS DateTime), N'/api/Servicios/CrearServicio', 10032)
GO
INSERT [dbo].[tExcepciones] ([ExcepcionId], [Mensaje], [Fecha], [Origen], [UsuarioId]) VALUES (10004, N'Could not find a part of the path ''C:\FIDELITAS\7 cuatri\Progra Avanzada Web\AgendaTuLook\AgendaTuLookAPI\AgendaTuLookAPI\wwwroot\img\servicios\servicio_5_20250422124147.jpg''.', CAST(N'2025-04-22T12:41:47.763' AS DateTime), N'/api/Servicios/EditarServicio', 10032)
GO
INSERT [dbo].[tExcepciones] ([ExcepcionId], [Mensaje], [Fecha], [Origen], [UsuarioId]) VALUES (20002, N'Procedure or function CrearServicio has too many arguments specified.', CAST(N'2025-04-22T14:49:10.347' AS DateTime), N'/api/Servicios/CrearServicio', 10032)
GO
SET IDENTITY_INSERT [dbo].[tExcepciones] OFF
GO
SET IDENTITY_INSERT [dbo].[tMetodosPago] ON 
GO
INSERT [dbo].[tMetodosPago] ([MetodoPagoId], [Nombre], [Descripcion], [Estado]) VALUES (1, N'Tarjeta Débito/Crédito', N'Descripcion Tarjeta', 1)
GO
INSERT [dbo].[tMetodosPago] ([MetodoPagoId], [Nombre], [Descripcion], [Estado]) VALUES (2, N'SINPE Móvil', N'Descripción SINPE', 1)
GO
SET IDENTITY_INSERT [dbo].[tMetodosPago] OFF
GO
SET IDENTITY_INSERT [dbo].[tPagos] ON 
GO
INSERT [dbo].[tPagos] ([PagoId], [CitaId], [MetodoPagoId], [FechaPago], [Comprobante]) VALUES (10018, 10021, 2, CAST(N'2025-04-10T21:02:53.630' AS DateTime), N'3135768.png')
GO
INSERT [dbo].[tPagos] ([PagoId], [CitaId], [MetodoPagoId], [FechaPago], [Comprobante]) VALUES (20008, 20011, 2, CAST(N'2025-04-11T12:58:03.627' AS DateTime), N'3135768.png')
GO
INSERT [dbo].[tPagos] ([PagoId], [CitaId], [MetodoPagoId], [FechaPago], [Comprobante]) VALUES (20009, 20012, 2, CAST(N'2025-04-11T13:03:54.870' AS DateTime), N'3135768.png')
GO
INSERT [dbo].[tPagos] ([PagoId], [CitaId], [MetodoPagoId], [FechaPago], [Comprobante]) VALUES (20010, 20013, 2, CAST(N'2025-04-11T13:06:33.087' AS DateTime), N'3135768.png')
GO
INSERT [dbo].[tPagos] ([PagoId], [CitaId], [MetodoPagoId], [FechaPago], [Comprobante]) VALUES (20011, 20014, 2, CAST(N'2025-04-11T15:57:59.537' AS DateTime), N'3135768.png')
GO
INSERT [dbo].[tPagos] ([PagoId], [CitaId], [MetodoPagoId], [FechaPago], [Comprobante]) VALUES (20012, 20015, 2, CAST(N'2025-04-12T14:33:08.440' AS DateTime), N'ReporteAuditoriaPacientes.pdf')
GO
INSERT [dbo].[tPagos] ([PagoId], [CitaId], [MetodoPagoId], [FechaPago], [Comprobante]) VALUES (20013, 20016, 2, CAST(N'2025-04-13T13:39:07.407' AS DateTime), N'ReporteAuditoriaPacientes.pdf')
GO
INSERT [dbo].[tPagos] ([PagoId], [CitaId], [MetodoPagoId], [FechaPago], [Comprobante]) VALUES (20014, 20019, 2, CAST(N'2025-04-14T14:13:06.600' AS DateTime), N'3135768.png')
GO
INSERT [dbo].[tPagos] ([PagoId], [CitaId], [MetodoPagoId], [FechaPago], [Comprobante]) VALUES (20015, 20020, 1, CAST(N'2025-04-19T21:32:02.050' AS DateTime), NULL)
GO
SET IDENTITY_INSERT [dbo].[tPagos] OFF
GO
SET IDENTITY_INSERT [dbo].[tProveedoresAuth] ON 
GO
INSERT [dbo].[tProveedoresAuth] ([ProveedorAuthId], [Nombre]) VALUES (1, N'Email')
GO
INSERT [dbo].[tProveedoresAuth] ([ProveedorAuthId], [Nombre]) VALUES (2, N'Google')
GO
SET IDENTITY_INSERT [dbo].[tProveedoresAuth] OFF
GO
SET IDENTITY_INSERT [dbo].[tReviews] ON 
GO
INSERT [dbo].[tReviews] ([ReviewId], [CitaId], [ComentarioReview], [CalificacionReview], [fecha]) VALUES (7, 10021, N'Hola!', 4, CAST(N'2025-04-14T12:01:42.937' AS DateTime))
GO
INSERT [dbo].[tReviews] ([ReviewId], [CitaId], [ComentarioReview], [CalificacionReview], [fecha]) VALUES (8, 20014, N'Calificacion', 4, CAST(N'2025-04-14T12:03:42.430' AS DateTime))
GO
INSERT [dbo].[tReviews] ([ReviewId], [CitaId], [ComentarioReview], [CalificacionReview], [fecha]) VALUES (9, 10021, N'prueba review negativa', 3, CAST(N'2025-03-20T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[tReviews] ([ReviewId], [CitaId], [ComentarioReview], [CalificacionReview], [fecha]) VALUES (10, 10021, N'prueba review negativa', 3, CAST(N'2025-03-14T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[tReviews] ([ReviewId], [CitaId], [ComentarioReview], [CalificacionReview], [fecha]) VALUES (11, 10021, N'prueba review negativa', 3, CAST(N'2025-04-14T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[tReviews] ([ReviewId], [CitaId], [ComentarioReview], [CalificacionReview], [fecha]) VALUES (12, 10021, N'prueba review positiva', 5, CAST(N'2025-03-13T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[tReviews] ([ReviewId], [CitaId], [ComentarioReview], [CalificacionReview], [fecha]) VALUES (13, 10021, N'prueba review positiva', 4, CAST(N'2025-03-15T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[tReviews] ([ReviewId], [CitaId], [ComentarioReview], [CalificacionReview], [fecha]) VALUES (14, 10021, N'prueba review positiva', 5, CAST(N'2025-03-18T00:00:00.000' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[tReviews] OFF
GO
SET IDENTITY_INSERT [dbo].[tRoles] ON 
GO
INSERT [dbo].[tRoles] ([RolId], [Nombre]) VALUES (1, N'Administrador')
GO
INSERT [dbo].[tRoles] ([RolId], [Nombre]) VALUES (2, N'Cliente')
GO
SET IDENTITY_INSERT [dbo].[tRoles] OFF
GO
SET IDENTITY_INSERT [dbo].[tServicios] ON 
GO
INSERT [dbo].[tServicios] ([ServicioId], [NombreServicio], [Descripcion], [Precio], [Duracion], [Imagen], [Estado]) VALUES (1, N'Corte + Barba', N'"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', CAST(10000.00 AS Decimal(10, 2)), 30, N'https://blog.clover.com/wp-content/uploads/2024/03/barber-shaving-customer-with-straight-razor.jpg', 1)
GO
INSERT [dbo].[tServicios] ([ServicioId], [NombreServicio], [Descripcion], [Precio], [Duracion], [Imagen], [Estado]) VALUES (2, N'Barba + Exfoliación', N'"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', CAST(15000.00 AS Decimal(10, 2)), 11, N'https://blog.clover.com/wp-content/uploads/2024/03/barber-shaving-customer-with-straight-razor.jpg', 1)
GO
INSERT [dbo].[tServicios] ([ServicioId], [NombreServicio], [Descripcion], [Precio], [Duracion], [Imagen], [Estado]) VALUES (3, N'124123123', N'123123', CAST(112323.00 AS Decimal(10, 2)), 141, NULL, 0)
GO
INSERT [dbo].[tServicios] ([ServicioId], [NombreServicio], [Descripcion], [Precio], [Duracion], [Imagen], [Estado]) VALUES (4, N'Alisado', N'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc euismod commodo ex id consectetur. Nunc vel iaculis enim. Integer sit amet auctor dui. Duis auctor libero leo, eu tincidunt augue consectetur in. Pellentesque lacus dui, sagittis sit amet auctor et, dignissim quis leo. Ut sodales dolor erat, vitae posuere tortor varius ut.', CAST(5000.00 AS Decimal(10, 2)), 120, NULL, 1)
GO
INSERT [dbo].[tServicios] ([ServicioId], [NombreServicio], [Descripcion], [Precio], [Duracion], [Imagen], [Estado]) VALUES (5, N'Tinte', N'servicio tinte para pelito', CAST(70000.00 AS Decimal(10, 2)), 180, N'/img/servicios/108c34f4-7e2e-4f82-9c8a-405ec4219390.jpg', 1)
GO
INSERT [dbo].[tServicios] ([ServicioId], [NombreServicio], [Descripcion], [Precio], [Duracion], [Imagen], [Estado]) VALUES (10005, N'prueba imagen ', N'pruebaaaaaaaa', CAST(40000.00 AS Decimal(10, 2)), 180, NULL, 0)
GO
INSERT [dbo].[tServicios] ([ServicioId], [NombreServicio], [Descripcion], [Precio], [Duracion], [Imagen], [Estado]) VALUES (10006, N'pruebaaaa', N'xsxxxxxxxassd', CAST(4000.00 AS Decimal(10, 2)), 180, NULL, 0)
GO
INSERT [dbo].[tServicios] ([ServicioId], [NombreServicio], [Descripcion], [Precio], [Duracion], [Imagen], [Estado]) VALUES (10007, N'abc', N'abc', CAST(7500.00 AS Decimal(10, 2)), 30, N'/img/servicios/32373c4d-8f02-48f6-8e01-a9fb0fe20590.jpg', 0)
GO
SET IDENTITY_INSERT [dbo].[tServicios] OFF
GO
SET IDENTITY_INSERT [dbo].[tUsuarios] ON 
GO
INSERT [dbo].[tUsuarios] ([UsuarioId], [Nombre], [Correo], [Contrasennia], [GoogleId], [Telefono], [RolId], [FechaRegistro], [ProveedorAuthId], [CodigoVerificacion], [FechaVencimientoVerificacion]) VALUES (10032, N'Ricardo Vargas', N'ricardovm2803@gmail.com', N'klVMA1ubTkEMAthQ6Np0mA==', N'103966479926404496735', N'84622823', 1, CAST(N'2025-03-20T12:35:05.977' AS DateTime), 2, NULL, NULL)
GO
INSERT [dbo].[tUsuarios] ([UsuarioId], [Nombre], [Correo], [Contrasennia], [GoogleId], [Telefono], [RolId], [FechaRegistro], [ProveedorAuthId], [CodigoVerificacion], [FechaVencimientoVerificacion]) VALUES (10033, N'Ricardo Manuel', N'123hijo36@gmail.com', NULL, N'107552496017882252155', NULL, 2, CAST(N'2025-03-20T12:36:01.610' AS DateTime), 2, NULL, NULL)
GO
INSERT [dbo].[tUsuarios] ([UsuarioId], [Nombre], [Correo], [Contrasennia], [GoogleId], [Telefono], [RolId], [FechaRegistro], [ProveedorAuthId], [CodigoVerificacion], [FechaVencimientoVerificacion]) VALUES (10034, N'Ricardo Vargas', N'aaronvm09@gmail.com', NULL, N'113782116621441987697', NULL, 2, CAST(N'2025-04-05T14:00:58.130' AS DateTime), 2, NULL, NULL)
GO
INSERT [dbo].[tUsuarios] ([UsuarioId], [Nombre], [Correo], [Contrasennia], [GoogleId], [Telefono], [RolId], [FechaRegistro], [ProveedorAuthId], [CodigoVerificacion], [FechaVencimientoVerificacion]) VALUES (10035, N'Fiorella Hernández M.', N'fiorellah0996@gmail.com', NULL, N'104377169569895940430', N'72482156', 1, CAST(N'2025-04-12T14:32:01.670' AS DateTime), 2, NULL, NULL)
GO
INSERT [dbo].[tUsuarios] ([UsuarioId], [Nombre], [Correo], [Contrasennia], [GoogleId], [Telefono], [RolId], [FechaRegistro], [ProveedorAuthId], [CodigoVerificacion], [FechaVencimientoVerificacion]) VALUES (10036, N'Fiorella Hernández Miranda', N'fiorellaproyectos9@gmail.com', NULL, N'100180894516735730497', NULL, 2, CAST(N'2025-04-12T15:01:39.557' AS DateTime), 2, NULL, NULL)
GO
INSERT [dbo].[tUsuarios] ([UsuarioId], [Nombre], [Correo], [Contrasennia], [GoogleId], [Telefono], [RolId], [FechaRegistro], [ProveedorAuthId], [CodigoVerificacion], [FechaVencimientoVerificacion]) VALUES (10037, N'amr', N'amata90989@ufide.ac.cr', N'klVMA1ubTkEMAthQ6Np0mA==', NULL, N'88888888', 2, CAST(N'2025-04-19T21:27:32.483' AS DateTime), 1, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[tUsuarios] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__tMetodos__75E3EFCF887DB8F8]    Script Date: 4/22/2025 3:20:30 PM ******/
ALTER TABLE [dbo].[tMetodosPago] ADD  CONSTRAINT [UQ__tMetodos__75E3EFCF887DB8F8] UNIQUE NONCLUSTERED 
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__tUsuario__60695A1915D2DBE0]    Script Date: 4/22/2025 3:20:30 PM ******/
ALTER TABLE [dbo].[tUsuarios] ADD  CONSTRAINT [UQ__tUsuario__60695A1915D2DBE0] UNIQUE NONCLUSTERED 
(
	[Correo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tDiasTrabajo] ADD  CONSTRAINT [DF__tDiasTrab__Activ__208CD6FA]  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[tExcepciones] ADD  DEFAULT (getdate()) FOR [Fecha]
GO
ALTER TABLE [dbo].[tMetodosPago] ADD  CONSTRAINT [DF__tMetodosP__Estad__4CA06362]  DEFAULT ((1)) FOR [Estado]
GO
ALTER TABLE [dbo].[tNotificaciones] ADD  DEFAULT (getdate()) FOR [FechaEnvio]
GO
ALTER TABLE [dbo].[tNotificaciones] ADD  DEFAULT ((0)) FOR [Leido]
GO
ALTER TABLE [dbo].[tPagos] ADD  DEFAULT (getdate()) FOR [FechaPago]
GO
ALTER TABLE [dbo].[tUsuarios] ADD  CONSTRAINT [DF__tUsuarios__Fecha__3A81B327]  DEFAULT (getdate()) FOR [FechaRegistro]
GO
ALTER TABLE [dbo].[tCitas]  WITH CHECK ADD  CONSTRAINT [FK_Citas_DiasTrabajo] FOREIGN KEY([DiaTrabajoId])
REFERENCES [dbo].[tDiasTrabajo] ([DiaTrabajoId])
GO
ALTER TABLE [dbo].[tCitas] CHECK CONSTRAINT [FK_Citas_DiasTrabajo]
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
ALTER TABLE [dbo].[tReviews]  WITH CHECK ADD  CONSTRAINT [FK_tReviews_tCitas] FOREIGN KEY([CitaId])
REFERENCES [dbo].[tCitas] ([CitaId])
GO
ALTER TABLE [dbo].[tReviews] CHECK CONSTRAINT [FK_tReviews_tCitas]
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
ALTER TABLE [dbo].[tCitas]  WITH CHECK ADD  CONSTRAINT [CK__tCitas__Estado__5AEE82B9] CHECK  (([estado]='Cancelada' OR [estado]='Confirmada' OR [estado]='Finalizada'))
GO
ALTER TABLE [dbo].[tCitas] CHECK CONSTRAINT [CK__tCitas__Estado__5AEE82B9]
GO
ALTER TABLE [dbo].[tReviews]  WITH CHECK ADD CHECK  (([CalificacionReview]>=(1) AND [CalificacionReview]<=(5)))
GO
/****** Object:  StoredProcedure [dbo].[ActualizarCita]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ActualizarCita]
    @CitaId BIGINT,
    @ServicioId BIGINT,
    @Fecha DATE,
    @HoraInicio TIME(0),
    @HoraFin TIME(0),
    @DiaTrabajoId BIGINT,
    @NuevoMetodoPagoId BIGINT = NULL,
    @Comprobante VARCHAR(255) = NULL,
    @ServicioCambiado BIT
AS
BEGIN
    DECLARE @ServicioOriginalId BIGINT;
    
    -- Obtener el servicio original
    SELECT @ServicioOriginalId = ServicioId 
    FROM tCitas 
    WHERE CitaId = @CitaId;
    
    -- Actualizar la cita
    UPDATE tCitas
    SET 
        ServicioId = @ServicioId,
        Fecha = @Fecha,
        HoraInicio = @HoraInicio,
        HoraFin = @HoraFin,
        DiaTrabajoId = @DiaTrabajoId
    WHERE CitaId = @CitaId;
    
    -- Si el servicio cambió, borrar el pago anterior y crear uno nuevo
    IF @ServicioCambiado = 1 AND @NuevoMetodoPagoId IS NOT NULL
    BEGIN
        -- Borrar pago anterior
        DELETE FROM tPagos WHERE CitaId = @CitaId;
        
        -- Crear nuevo pago
        INSERT INTO tPagos (CitaId, MetodoPagoId, FechaPago, Comprobante)
        VALUES (@CitaId, @NuevoMetodoPagoId, CURRENT_TIMESTAMP, @Comprobante);
    END
END
GO
/****** Object:  StoredProcedure [dbo].[ActualizarHorasDiaTrabajo]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ActualizarHorasDiaTrabajo]
	@DiaTrabajoId BIGINT,
	@HoraInicio TIME(0),
	@HoraFin TIME(0)
AS
BEGIN
	UPDATE tDiasTrabajo 
	SET HoraInicio = @HoraInicio,
	HoraFin = @HoraFin
	WHERE DiaTrabajoId = @DiaTrabajoId
END

GO
/****** Object:  StoredProcedure [dbo].[ActualizarServicio]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================
-- Template generated from Template Explorer using:
CREATE PROCEDURE [dbo].[ActualizarServicio]
	@ServicioId BIGINT,
	@NombreServicio VARCHAR(100),
	@Descripcion TEXT,
	@Precio DECIMAL(10,2),
	@Duracion INT,
	@Imagen VARCHAR(255)
AS
BEGIN
	UPDATE tServicios
	SET NombreServicio = @NombreServicio,
	Descripcion = @Descripcion,
	Precio = @Precio,
	Duracion = @Duracion,
	Imagen = @Imagen
	WHERE ServicioId = @ServicioId;
END
GO
/****** Object:  StoredProcedure [dbo].[ActualizarUsuario]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ActualizarUsuario]
    @UsuarioId BIGINT,
    @Nombre VARCHAR(100),
    @Telefono VARCHAR(15),  
    @Correo VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
  
    -- Verificar si el usuario existe
    IF EXISTS (SELECT 1 FROM tUsuarios WHERE UsuarioId = @UsuarioId)
    BEGIN
        -- Actualizar los datos del usuario
        UPDATE tUsuarios
        SET Nombre = ISNULL(@Nombre, Nombre),  -- Solo cambia si no es NULL
            Telefono = ISNULL(@Telefono, Telefono),  -- Solo cambia si no es NULL
            Correo = ISNULL(@Correo, Correo)
        WHERE UsuarioId = @UsuarioId;
        
        -- Retornar resultado exitoso
        SELECT 1 AS Resultado, 'Usuario actualizado correctamente' AS Mensaje;
    END
    ELSE
    BEGIN
        -- Retornar error si el usuario no existe
        SELECT 0 AS Resultado, 'El usuario no existe' AS Mensaje;
    END
END
GO
/****** Object:  StoredProcedure [dbo].[CambiarContrasenna]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CambiarContrasenna]
    @Correo VARCHAR(100),
    @NuevaContrasennia VARCHAR(255)
AS
BEGIN
    UPDATE tUsuarios
    SET Contrasennia = @NuevaContrasennia,
        FechaVencimientoVerificacion = NULL,
		CodigoVerificacion = NULL
    WHERE Correo = @Correo;
END
GO
/****** Object:  StoredProcedure [dbo].[CambiarEstadoDiaTrabajo]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CambiarEstadoDiaTrabajo]
	@DiaTrabajoId BIGINT
AS
BEGIN
	UPDATE tDiasTrabajo 
	SET Activo = Activo ^ 1
	WHERE DiaTrabajoId = @DiaTrabajoId
END

GO
/****** Object:  StoredProcedure [dbo].[CambiarEstadoServicio]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CambiarEstadoServicio]
	@ServicioId BIGINT
AS
BEGIN
	UPDATE tServicios 
	SET Estado = Estado ^ 1
	WHERE ServicioId = @ServicioId
END
GO
/****** Object:  StoredProcedure [dbo].[CancelarCita]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CancelarCita]
    @CitaId BIGINT
AS
BEGIN
    DECLARE @RowsAffected INT = 0;
    
    UPDATE tCitas
    SET Estado = 'Cancelada'
    WHERE CitaId = @CitaId AND Estado != 'Cancelada';
    
    SET @RowsAffected = @@ROWCOUNT;
    
    IF @RowsAffected > 0
    BEGIN
        DELETE FROM tPagos WHERE CitaId = @CitaId;
    END
    
    RETURN @RowsAffected;
END
GO
/****** Object:  StoredProcedure [dbo].[ConsultarCitasCalendario]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ConsultarCitasCalendario]
    @UsuarioId BIGINT,
    @Completadas INT
AS
BEGIN
    DECLARE @EsAdmin BIT;
    
    -- Determinar si el usuario es administrador
    SELECT @EsAdmin = CASE WHEN u.RolId = r.RolId THEN 1 ELSE 0 END
    FROM tUsuarios u
    INNER JOIN tRoles r ON r.Nombre = 'Administrador'
    WHERE u.UsuarioId = @UsuarioId;
    
    -- Consulta única con condiciones dinámicas
    SELECT 
        c.CitaId, 
        u.Nombre AS NombreCliente, 
        s.NombreServicio, 
        c.Fecha, 
        c.HoraInicio, 
        c.HoraFin, 
        c.Estado,
		r.ReviewId,
		r.CalificacionReview,
		r.ComentarioReview
    FROM 
        tCitas c
    JOIN 
        tUsuarios u ON u.UsuarioId = c.UsuarioId
    JOIN 
        tServicios s ON s.ServicioId = c.ServicioId
	LEFT JOIN 
        tReviews r ON r.CitaId = c.CitaId
    WHERE 
        -- Filtro de usuario (solo si no es administrador)
        (@EsAdmin = 1 OR u.UsuarioId = @UsuarioId)
        -- Filtro de estado usando CASE WHEN
        AND CASE 
            WHEN @Completadas = 0 THEN 1
            WHEN @Completadas = 1 AND c.Estado IN ('Finalizada', 'Cancelada') THEN 1
            WHEN @Completadas = 2 AND c.Estado = 'Confirmada' THEN 1
            ELSE 0
        END = 1;
END
GO
/****** Object:  StoredProcedure [dbo].[ConsultarDatosConfirmar]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ConsultarDatosConfirmar]
	@UsuarioId BIGINT,
	@ServicioId BIGINT,
	@Fecha DATE
AS
BEGIN
	SET LANGUAGE Español;
	SELECT u.Nombre, u.Correo, u.Telefono,
		   s.NombreServicio, s.Precio, s.Duracion,
		   (SELECT DiaTrabajoId 
			FROM tDiasTrabajo 
			WHERE NombreDia = DATENAME(WEEKDAY, @Fecha)) AS DiaTrabajoId
	FROM tUsuarios u
	JOIN tServicios s ON s.ServicioId = @ServicioId
	WHERE u.UsuarioId = @UsuarioId;
END
GO
/****** Object:  StoredProcedure [dbo].[ConsultarDiasTrabajo]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ConsultarDiasTrabajo]
	@DiaTrabajoId BIGINT
AS
BEGIN
	IF (@DiaTrabajoId = 0)
		SET @DiaTrabajoId = NULL
		SELECT DiaTrabajoId, NombreDia, Activo, HoraInicio, HoraFin
		FROM tDiasTrabajo
		WHERE DiaTrabajoId = ISNULL(@DiaTrabajoId, DiaTrabajoId)
END
GO
/****** Object:  StoredProcedure [dbo].[ConsultarHorasDisponibles]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ConsultarHorasDisponibles]
	@Fecha DATE
AS
BEGIN

	SET LANGUAGE Español;

	DECLARE @DiaTrabajoId BIGINT;
	SELECT @DiaTrabajoId = DiaTrabajoId
	FROM tDiasTrabajo
	WHERE NombreDia = DATENAME(WEEKDAY, @Fecha);

	WITH Horas AS (
    SELECT CAST(H.Hora AS TIME(0)) AS Hora
    FROM (VALUES ('00:00'), ('01:00'), ('02:00'), ('03:00'), ('04:00'), 
                ('05:00'), ('06:00'), ('07:00'), ('08:00'), ('09:00'), 
                ('10:00'), ('11:00'), ('12:00'), ('13:00'), ('14:00'),
                ('15:00'), ('16:00'), ('17:00'), ('18:00'), ('19:00'), 
                ('20:00'), ('21:00'), ('22:00'), ('23:00')) AS H(Hora)
)
	SELECT h.Hora
	FROM Horas h
	JOIN tDiasTrabajo dt ON dt.DiaTrabajoId = @DiaTrabajoId
	WHERE h.Hora >= dt.HoraInicio AND h.Hora <= DATEADD(HOUR, -1, dt.HoraFin)
	AND (
    -- Si la fecha seleccionada es hoy, muestra solo horas >= hora actual
    (@Fecha = CONVERT(DATE, GETDATE()) AND h.Hora >= CAST(GETDATE() AS TIME))
    -- Si la fecha es futura, muestra todas las horas disponibles
    OR @Fecha > CONVERT(DATE, GETDATE())
)
	AND NOT EXISTS (
		SELECT 1
		FROM tCitas c
		WHERE c.Fecha = @Fecha -- esto es clave, la fecha
		AND h.Hora >= c.HoraInicio AND h.Hora < c.HoraFin
	);
END
GO
/****** Object:  StoredProcedure [dbo].[ConsultarReviewsPorFecha]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ConsultarReviewsPorFecha]
	@fechaInicio datetime,
	@fechaFinal datetime
	AS
BEGIN
	SELECT r.ReviewId,cu.Nombre,r.ComentarioReview, r.CalificacionReview, r.fecha
	FROM tReviews r
	INNER JOIN [dbo].[tCitas] c ON r.CitaId = c.CitaId
    INNER JOIN [dbo].[tUsuarios] cu ON c.UsuarioId = cu.UsuarioId
	WHERE r.fecha BETWEEN @fechaInicio AND @fechaFinal;
	
END
GO
/****** Object:  StoredProcedure [dbo].[ConsultarServicios]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ConsultarServicios]
	@ServicioId BIGINT
AS
BEGIN
	IF (@ServicioId = 0)
		SET @ServicioId = NULL
		SELECT ServicioId, NombreServicio, Descripcion, Precio, Duracion, Imagen, Estado
		FROM tServicios
		WHERE ServicioId = ISNULL(@ServicioId, ServicioId)
END
GO
/****** Object:  StoredProcedure [dbo].[ConsultarUsuario]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE[dbo].[ConsultarUsuario]
	@UsuarioId BIGINT
AS
BEGIN
	SELECT Nombre,Telefono,Correo,Contrasennia
	FROM tUsuarios
	WHERE UsuarioId = @UsuarioId
END
GO
/****** Object:  StoredProcedure [dbo].[CrearCita]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CrearCita]
	@UsuarioId BIGINT,
	@ServicioId BIGINT,
	@Fecha DATE,
	@HoraInicio TIME(0),
	@HoraFin TIME(0),
	@DiaTrabajoId BIGINT,
	@MetodoPagoId BIGINT,
	@Comprobante VARCHAR(255)
AS
BEGIN

	DECLARE @CitaId BIGINT;

	INSERT INTO tCitas (UsuarioId, ServicioId, Fecha, HoraInicio, HoraFin, Estado, DiaTrabajoId)
	VALUES (@UsuarioId, @ServicioId, @Fecha, @HoraInicio, @HoraFin, 'Confirmada', @DiaTrabajoId)

	SET @CitaId = SCOPE_IDENTITY();

	INSERT INTO tPagos (CitaId, MetodoPagoId, FechaPago, Comprobante)
	VALUES (@CitaId, @MetodoPagoId, CURRENT_TIMESTAMP, @Comprobante)
END
GO
/****** Object:  StoredProcedure [dbo].[CrearServicio]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CrearServicio]
	@NombreServicio VARCHAR(100),
	@Descripcion TEXT,
	@Precio DECIMAL(10,2),
	@Duracion INT,
	@Imagen VARCHAR(255)
AS
BEGIN
	INSERT INTO tServicios (NombreServicio, Descripcion, Precio, Duracion, Imagen, Estado)
	VALUES (@NombreServicio, @Descripcion, @Precio, @Duracion, @Imagen, 1)

	DECLARE @ServicioId BIGINT;
	SET @ServicioId = SCOPE_IDENTITY();
	SELECT @ServicioId AS ServicioId;
END
GO
/****** Object:  StoredProcedure [dbo].[DesvincularGoogleId]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DesvincularGoogleId] 
    @Correo VARCHAR(100)
AS
BEGIN
    DECLARE @Contrasennia VARCHAR(255);
    SELECT @Contrasennia = Contrasennia FROM tUsuarios WHERE Correo = @Correo;

    IF @Contrasennia IS NULL
    BEGIN
        RETURN;
    END

    UPDATE tUsuarios
    SET GoogleId = NULL,
        ProveedorAuthId = (SELECT ProveedorAuthId FROM tProveedoresAuth WHERE Nombre = 'Email')
    WHERE Correo = @Correo;
END
GO
/****** Object:  StoredProcedure [dbo].[Estadisticas]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Estadisticas] 
	 @fechaInicio date,
     @fechaFinal date 
AS
BEGIN
	SET NOCOUNT ON;
	WITH CitasNuevas AS (
        SELECT 
            COUNT(*) as CitasNuevas
        FROM tcitas
        WHERE fecha >= @fechaInicio AND fecha < @fechaFinal
    ),
    UsuariosNuevos AS (
        SELECT 
            COUNT(*) as UsuariosNuevos
        FROM tUsuarios
        WHERE FechaRegistro >= @fechaInicio AND FechaRegistro < @fechaFinal
    ),
    UsuariosAtendidos AS (
        SELECT 
            COUNT(*) as UsuariosAtendidos
        FROM tcitas
        WHERE (Fecha >= @fechaInicio AND Fecha < @fechaFinal)
		AND (tCitas.Estado = 'Confirmada')
    ),
    TotalReviews AS (
        SELECT 
            COUNT(*) as ReviewsTotales
        FROM tReviews
        WHERE Fecha >= @fechaInicio AND Fecha < @fechaFinal
    ),
    TotalReviewsPositivas AS (
        SELECT 
            COUNT(*) as ReviewsPositivasTotales
        FROM tReviews
        WHERE (Fecha >= @fechaInicio AND Fecha < @fechaFinal) 
		AND (CalificacionReview >= 4)
    ),
    TotalReviewsNegativas AS (
        SELECT 
            COUNT(*) as ReviewsNegativasTotales
        FROM tReviews
        WHERE (Fecha >= @fechaInicio AND Fecha < @fechaFinal) 
		AND (CalificacionReview <= 3)
    ),
	CitasAnteriores AS (
    SELECT COUNT(*) as CitasAnteriores
    FROM tcitas
    WHERE fecha >= DATEADD(MONTH, -1, @fechaInicio) 
    AND fecha < DATEADD(MONTH, -1, @fechaFinal)
	),
	UsuariosNuevosAnteriores AS (
    SELECT COUNT(*) as UsuariosNuevosAnteriores
    FROM tUsuarios
    WHERE FechaRegistro >= DATEADD(MONTH, -1, @fechaInicio) 
    AND FechaRegistro < DATEADD(MONTH, -1, @fechaFinal)
	),
	UsuariosAtendidosAnteriores AS (
    SELECT COUNT(*) as UsuariosAtendidosAnteriores
    FROM tcitas
    WHERE (Fecha >= DATEADD(MONTH, -1, @fechaInicio) 
    AND Fecha < DATEADD(MONTH, -1, @fechaFinal)) 
	AND (tCitas.Estado = 'Confirmada')
	),
	TotalReviewsAnteriores AS (
    SELECT COUNT(*) as ReviewsAnteriores
    FROM tReviews
    WHERE Fecha >= DATEADD(MONTH, -1, @fechaInicio) 
    AND Fecha < DATEADD(MONTH, -1, @fechaFinal)
	),
	TotalReviewsPositivasAnteriores AS (
    SELECT COUNT(*) as ReviewsPositivasAnteriores
    FROM tReviews
    WHERE (Fecha >= DATEADD(MONTH, -1, @fechaInicio) 
    AND Fecha < DATEADD(MONTH, -1, @fechaFinal))
	AND (CalificacionReview >= 4)
	),
	TotalReviewsNegativasAnteriores AS (
    SELECT COUNT(*) as ReviewsNegativasAnteriores
    FROM tReviews
    WHERE (Fecha >= DATEADD(MONTH, -1, @fechaInicio) 
    AND Fecha < DATEADD(MONTH, -1, @fechaFinal))
	AND (CalificacionReview <= 3)
	)SELECT
	CitasNuevas.CitasNuevas,
	UsuariosNuevos.UsuariosNuevos,
	UsuariosAtendidos.UsuariosAtendidos,
	TotalReviews.ReviewsTotales,
	TotalReviewsPositivas.ReviewsPositivasTotales,
	TotalReviewsNegativas.ReviewsNegativasTotales,
        CASE 
            WHEN CitasAnteriores.CitasAnteriores > 0 
            THEN FLOOR((CitasNuevas.CitasNuevas - CitasAnteriores.CitasAnteriores) * 100.0 / CitasAnteriores.CitasAnteriores)
            ELSE NULL 
        END AS PorcentajeCitas,
		CASE 
            WHEN  UsuariosNuevosAnteriores.UsuariosNuevosAnteriores > 0 
            THEN FLOOR((UsuariosNuevos.UsuariosNuevos - UsuariosNuevosAnteriores.UsuariosNuevosAnteriores) * 100.0 / UsuariosNuevosAnteriores.UsuariosNuevosAnteriores)
            ELSE NULL 
        END AS PorcentajeUsuarioNuevos,
		CASE 
            WHEN  UsuariosAtendidosAnteriores.UsuariosAtendidosAnteriores > 0 
            THEN FLOOR((UsuariosAtendidos.UsuariosAtendidos - UsuariosAtendidosAnteriores.UsuariosAtendidosAnteriores) * 100.0 / UsuariosAtendidosAnteriores.UsuariosAtendidosAnteriores)
            ELSE NULL 
        END AS PorcentajeUsuariosAtendidos,
		CASE 
            WHEN  TotalReviewsAnteriores.ReviewsAnteriores > 0 
            THEN FLOOR((TotalReviews.ReviewsTotales - TotalReviewsAnteriores.ReviewsAnteriores) * 100.0 / TotalReviewsAnteriores.ReviewsAnteriores)
            ELSE NULL 
        END AS PorcentajeTotalReviews,
		CASE 
            WHEN  TotalReviewsPositivasAnteriores.ReviewsPositivasAnteriores > 0 
            THEN FLOOR((TotalReviewsPositivas.ReviewsPositivasTotales - TotalReviewsPositivasAnteriores.ReviewsPositivasAnteriores) * 100.0 / TotalReviewsPositivasAnteriores.ReviewsPositivasAnteriores)
            ELSE NULL 
        END AS PorcentajeTotalReviewsPositivas,
		CASE 
            WHEN  TotalReviewsNegativasAnteriores.ReviewsNegativasAnteriores > 0 
            THEN FLOOR((TotalReviewsNegativas.ReviewsNegativasTotales - TotalReviewsNegativasAnteriores.ReviewsNegativasAnteriores) * 100.0 / TotalReviewsNegativasAnteriores.ReviewsNegativasAnteriores)
            ELSE NULL 
        END AS PorcentajeTotalReviewsNegativas
	FROM CitasNuevas
	CROSS JOIN UsuariosNuevos
	CROSS JOIN UsuariosAtendidos
	CROSS JOIN TotalReviews
	CROSS JOIN TotalReviewsPositivas
	CROSS JOIN TotalReviewsNegativas
	CROSS JOIN CitasAnteriores
	CROSS JOIN UsuariosNuevosAnteriores
	CROSS JOIN UsuariosAtendidosAnteriores
	CROSS JOIN TotalReviewsAnteriores
	CROSS JOIN TotalReviewsPositivasAnteriores
	CROSS JOIN TotalReviewsNegativasAnteriores;
	
END
GO
/****** Object:  StoredProcedure [dbo].[EstadisticasFinancieras]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EstadisticasFinancieras]
	@fechaInicio DATE,
	@fechaFinal DATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Ingresos por servicio
    SELECT 
        s.ServicioId,
        s.NombreServicio,
        ISNULL(SUM(s.Precio), 0) AS Ingresos
    FROM tPagos p
    INNER JOIN tCitas c ON p.CitaId = c.CitaId
    INNER JOIN tServicios s ON c.ServicioId = s.ServicioId
    WHERE c.Fecha >= @fechaInicio AND c.Fecha < @fechaFinal
      AND c.Estado = 'Finalizada'
    GROUP BY s.ServicioId, s.NombreServicio;

    -- Total de ingresos
    SELECT 
        ISNULL(SUM(s.Precio), 0) AS IngresosTotales
    FROM tPagos p
    INNER JOIN tCitas c ON p.CitaId = c.CitaId
    INNER JOIN tServicios s ON c.ServicioId = s.ServicioId
    WHERE c.Fecha >= @fechaInicio AND c.Fecha < @fechaFinal
      AND c.Estado = 'Finalizada';
END
GO
/****** Object:  StoredProcedure [dbo].[ExisteCorreo]    Script Date: 4/22/2025 3:20:30 PM ******/
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
/****** Object:  StoredProcedure [dbo].[GuardarCalificacionCita]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GuardarCalificacionCita]
	@CitaId BIGINT,
	@CalificacionReview INT,
	@ComentarioReview NVARCHAR(1000)
AS
BEGIN
	INSERT INTO tReviews (CitaId, CalificacionReview, ComentarioReview, Fecha)
	VALUES (@CitaId, @CalificacionReview, @ComentarioReview, CURRENT_TIMESTAMP)
END
GO
/****** Object:  StoredProcedure [dbo].[GuardarCodigoVerificacion]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GuardarCodigoVerificacion]
	@UsuarioId INT,
    @CodigoVerificacion NVARCHAR(10),
    @FechaVencimiento DATETIME
AS
BEGIN
    UPDATE tUsuarios
    SET CodigoVerificacion = @CodigoVerificacion,
        FechaVencimientoVerificacion = @FechaVencimiento
    WHERE UsuarioId = @UsuarioId
END
GO
/****** Object:  StoredProcedure [dbo].[Login]    Script Date: 4/22/2025 3:20:30 PM ******/
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
			RolId
	FROM	tUsuarios
	WHERE   Correo = @Correo
	AND Contrasennia = @Contrasennia
END
GO
/****** Object:  StoredProcedure [dbo].[ObtenerCitaParaEditar]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ObtenerCitaParaEditar]
    @CitaId BIGINT
AS
BEGIN
    SELECT 
        c.CitaId,
        c.UsuarioId,
        c.ServicioId,
        c.Fecha,
        c.HoraInicio,
        c.HoraFin,
        c.Estado,
        c.DiaTrabajoId,
        s.NombreServicio,
        CAST(s.Precio AS FLOAT) AS Precio,
        s.Duracion,
        s.Imagen,
        s.Descripcion,
        p.MetodoPagoId,
        mp.Nombre AS MetodoPagoNombre,
        u.Nombre AS UsuarioNombre,
        u.Correo AS UsuarioCorreo,
        u.Telefono AS UsuarioTelefono
    FROM tCitas c
    JOIN tServicios s ON c.ServicioId = s.ServicioId
    JOIN tPagos p ON c.CitaId = p.CitaId
    JOIN tMetodosPago mp ON p.MetodoPagoId = mp.MetodoPagoId
    JOIN tUsuarios u ON c.UsuarioId = u.UsuarioId
    WHERE c.CitaId = @CitaId;
END
GO
/****** Object:  StoredProcedure [dbo].[ObtenerDetalleCita]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[ObtenerDetalleCita]
    @CitaId BIGINT
AS
BEGIN
    SELECT 
        c.CitaId,
        c.Fecha,
        c.HoraInicio,
        c.HoraFin,
        c.Estado,
        c.DiaTrabajoId,

        -- Datos del usuario
        u.UsuarioId,
        u.Nombre AS NombreUsuario,
        u.Correo,
        u.Telefono,

        -- Datos del servicio
        s.ServicioId,
        s.NombreServicio,
        s.Precio,
        s.Duracion
    FROM tCitas c
    JOIN tUsuarios u ON u.UsuarioId = c.UsuarioId
    JOIN tServicios s ON s.ServicioId = c.ServicioId
    WHERE c.CitaId = @CitaId
END
GO
/****** Object:  StoredProcedure [dbo].[ObtenerIdUsuarioConCorreo]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ObtenerIdUsuarioConCorreo]
	@Correo VARCHAR(100)
AS
BEGIN
	SELECT UsuarioId, Nombre, RolId
	FROM tUsuarios
	WHERE Correo = @Correo
END
GO
/****** Object:  StoredProcedure [dbo].[ObtenerMetodosPago]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ObtenerMetodosPago]
AS
BEGIN
	SELECT MetodoPagoId, Nombre, Descripcion FROM tMetodosPago;
END
GO
/****** Object:  StoredProcedure [dbo].[ObtenerProveedorAuthConCorreo]    Script Date: 4/22/2025 3:20:30 PM ******/
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
/****** Object:  StoredProcedure [dbo].[ObtenerReviewsDestacados]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[ObtenerReviewsDestacados]
AS
BEGIN
    SELECT 
        r.ReviewId,
        r.CitaId,
        r.ComentarioReview,
        r.CalificacionReview,
        r.Fecha,
        u.Nombre
        
    FROM tReviews r
    INNER JOIN tCitas c ON r.CitaId = c.CitaId
    INNER JOIN tUsuarios u ON c.UsuarioId = u.UsuarioId  
    WHERE r.CalificacionReview >= 4
    ORDER BY r.Fecha DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[RegistrarError]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RegistrarError]
			@Mensaje text,
           @Origen varchar(50),
           @UsuarioId bigint
AS
BEGIN



INSERT INTO [dbo].[tExcepciones]
           ([Mensaje]
           ,[Origen]
		   ,[Fecha]
           ,[UsuarioId])
     VALUES
           (@Mensaje,
			@Origen,
            GETDATE(),
           @UsuarioId)



END
GO
/****** Object:  StoredProcedure [dbo].[RegistroUsuario]    Script Date: 4/22/2025 3:20:30 PM ******/
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

	INSERT INTO tUsuarios (Nombre, Correo, Contrasennia, GoogleId, Telefono, ProveedorAuthId, RolId)
	VALUES (@Nombre, @Correo, @Contrasennia, @GoogleId, @Telefono, @ProveedorAuthId, @RolId)

	SET @UsuarioId = SCOPE_IDENTITY();
	SELECT @UsuarioId AS UsuarioId;
END
GO
/****** Object:  StoredProcedure [dbo].[TieneCitaAgendada]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[TieneCitaAgendada]
	@UsuarioId BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	IF EXISTS (
		SELECT 1
		FROM Citas
		WHERE UsuarioId = @UsuarioId AND Fecha >= GETDATE()
	)
		SELECT 1
	ELSE
		SELECT 0
END
GO
/****** Object:  StoredProcedure [dbo].[ValidarContrasenniaActual]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ValidarContrasenniaActual]
	@UsuarioId BIGINT,
	@ContrasenniaEnviada VARCHAR(255)
AS
BEGIN
	DECLARE @ContrasenniaAlmacenada VARCHAR(255);
	SELECT @ContrasenniaAlmacenada = Contrasennia
	FROM tUsuarios
	WHERE UsuarioId = @UsuarioId;

	IF @ContrasenniaAlmacenada = @ContrasenniaEnviada
	BEGIN
		SELECT 1 AS Coincide; -- Las contraseñas coinciden
	END
	ELSE
	BEGIN
		SELECT 0 AS Coincide; -- Las contraseñas no coinciden
	END
END
GO
/****** Object:  StoredProcedure [dbo].[VerificarCitaEditable]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[VerificarCitaEditable]
    @CitaId BIGINT
AS
BEGIN
    DECLARE @FechaCita DATETIME;
    DECLARE @HoraInicio TIME;
    DECLARE @FechaHoraCita DATETIME;
    
    SELECT @FechaCita = Fecha, @HoraInicio = HoraInicio
    FROM tCitas
    WHERE CitaId = @CitaId;
    
    SET @FechaHoraCita = CAST(@FechaCita AS DATETIME) + CAST(@HoraInicio AS DATETIME);
    
    IF DATEDIFF(HOUR, GETDATE(), @FechaHoraCita) > 24
        SELECT 1 AS EsEditable;
    ELSE
        SELECT 0 AS EsEditable;
END
GO
/****** Object:  StoredProcedure [dbo].[VerificarCodigoRecuperacion]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[VerificarCodigoRecuperacion]
    @Correo VARCHAR(100),
    @Codigo VARCHAR(10)
AS
BEGIN
    SELECT UsuarioId, Correo, CodigoVerificacion, FechaVencimientoVerificacion
    FROM tUsuarios
    WHERE Correo = @Correo AND CodigoVerificacion = @Codigo
END
GO
/****** Object:  StoredProcedure [dbo].[VincularGoogleId]    Script Date: 4/22/2025 3:20:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[VincularGoogleId] 
	@GoogleId VARCHAR(255),
	@Correo VARCHAR(100)
AS
BEGIN
	UPDATE tUsuarios
	SET GoogleId = @GoogleId,
    ProveedorAuthId = (SELECT ProveedorAuthId FROM tProveedoresAuth WHERE Nombre = 'Google')
	WHERE Correo = @Correo
END
GO
USE [master]
GO
ALTER DATABASE [AgendaTuLook] SET  READ_WRITE 
GO
