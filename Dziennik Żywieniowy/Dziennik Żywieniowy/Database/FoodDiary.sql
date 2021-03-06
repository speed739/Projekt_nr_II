USE [master]
GO
/****** Object:  Database [FoodDiary]    Script Date: 08.07.2020 14:43:59 ******/
CREATE DATABASE [FoodDiary]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'FoodDiary', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\FoodDiary.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'FoodDiary_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\FoodDiary_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [FoodDiary] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [FoodDiary].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [FoodDiary] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [FoodDiary] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [FoodDiary] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [FoodDiary] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [FoodDiary] SET ARITHABORT OFF 
GO
ALTER DATABASE [FoodDiary] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [FoodDiary] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [FoodDiary] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [FoodDiary] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [FoodDiary] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [FoodDiary] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [FoodDiary] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [FoodDiary] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [FoodDiary] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [FoodDiary] SET  ENABLE_BROKER 
GO
ALTER DATABASE [FoodDiary] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [FoodDiary] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [FoodDiary] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [FoodDiary] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [FoodDiary] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [FoodDiary] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [FoodDiary] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [FoodDiary] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [FoodDiary] SET  MULTI_USER 
GO
ALTER DATABASE [FoodDiary] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [FoodDiary] SET DB_CHAINING OFF 
GO
ALTER DATABASE [FoodDiary] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [FoodDiary] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [FoodDiary] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [FoodDiary] SET QUERY_STORE = OFF
GO
USE [FoodDiary]
GO
/****** Object:  Table [dbo].[Diary]    Script Date: 08.07.2020 14:44:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Diary](
	[ID_Diary] [int] IDENTITY(1,1) NOT NULL,
	[ID_User] [int] NOT NULL,
	[CreateDate] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Diary] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DiaryDetails]    Script Date: 08.07.2020 14:44:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DiaryDetails](
	[ID_Diary] [int] NULL,
	[ID_Product] [int] NULL,
	[Kcal] [decimal](6, 2) NULL,
	[AddData] [date] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 08.07.2020 14:44:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ID_Product] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](20) NOT NULL,
	[Weight] [float] NOT NULL,
	[Kcal] [float] NOT NULL,
	[Protein] [float] NULL,
	[Carbohydrates] [float] NULL,
	[Fat] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Product] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 08.07.2020 14:44:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[ID_User] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](20) NOT NULL,
	[Password] [nvarchar](20) NOT NULL,
	[Weight] [float] NOT NULL,
	[Height] [float] NOT NULL,
	[Sex] [nvarchar](7) NOT NULL,
	[Age] [tinyint] NOT NULL,
	[ActivityLevel] [nvarchar](10) NOT NULL,
	[BMI] [decimal](4, 2) NULL,
	[BMR] [decimal](6, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_User] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Diary] ON 

INSERT [dbo].[Diary] ([ID_Diary], [ID_User], [CreateDate]) VALUES (1, 6, CAST(N'2020-07-05' AS Date))
INSERT [dbo].[Diary] ([ID_Diary], [ID_User], [CreateDate]) VALUES (2, 7, CAST(N'2020-07-05' AS Date))
INSERT [dbo].[Diary] ([ID_Diary], [ID_User], [CreateDate]) VALUES (3, 1, CAST(N'2020-07-05' AS Date))
INSERT [dbo].[Diary] ([ID_Diary], [ID_User], [CreateDate]) VALUES (4, 8, CAST(N'2020-07-07' AS Date))
SET IDENTITY_INSERT [dbo].[Diary] OFF
INSERT [dbo].[DiaryDetails] ([ID_Diary], [ID_Product], [Kcal], [AddData]) VALUES (3, 2, CAST(55.20 AS Decimal(6, 2)), CAST(N'2020-07-05' AS Date))
INSERT [dbo].[DiaryDetails] ([ID_Diary], [ID_Product], [Kcal], [AddData]) VALUES (3, 4, CAST(44.50 AS Decimal(6, 2)), CAST(N'2020-07-06' AS Date))
INSERT [dbo].[DiaryDetails] ([ID_Diary], [ID_Product], [Kcal], [AddData]) VALUES (3, 7, CAST(292.00 AS Decimal(6, 2)), CAST(N'2020-07-06' AS Date))
INSERT [dbo].[DiaryDetails] ([ID_Diary], [ID_Product], [Kcal], [AddData]) VALUES (3, 3, CAST(121.00 AS Decimal(6, 2)), CAST(N'2020-07-06' AS Date))
INSERT [dbo].[DiaryDetails] ([ID_Diary], [ID_Product], [Kcal], [AddData]) VALUES (3, 7, CAST(365.00 AS Decimal(6, 2)), CAST(N'2020-07-06' AS Date))
INSERT [dbo].[DiaryDetails] ([ID_Diary], [ID_Product], [Kcal], [AddData]) VALUES (3, 2, CAST(69.00 AS Decimal(6, 2)), CAST(N'2020-07-06' AS Date))
INSERT [dbo].[DiaryDetails] ([ID_Diary], [ID_Product], [Kcal], [AddData]) VALUES (3, 13, CAST(128.00 AS Decimal(6, 2)), CAST(N'2020-07-06' AS Date))
INSERT [dbo].[DiaryDetails] ([ID_Diary], [ID_Product], [Kcal], [AddData]) VALUES (3, 3, CAST(181.50 AS Decimal(6, 2)), CAST(N'2020-07-07' AS Date))
INSERT [dbo].[DiaryDetails] ([ID_Diary], [ID_Product], [Kcal], [AddData]) VALUES (3, 4, CAST(44.50 AS Decimal(6, 2)), CAST(N'2020-07-07' AS Date))
INSERT [dbo].[DiaryDetails] ([ID_Diary], [ID_Product], [Kcal], [AddData]) VALUES (3, 2, CAST(48.30 AS Decimal(6, 2)), CAST(N'2020-07-07' AS Date))
INSERT [dbo].[DiaryDetails] ([ID_Diary], [ID_Product], [Kcal], [AddData]) VALUES (3, 6, CAST(255.50 AS Decimal(6, 2)), CAST(N'2020-07-07' AS Date))
INSERT [dbo].[DiaryDetails] ([ID_Diary], [ID_Product], [Kcal], [AddData]) VALUES (3, 3, CAST(121.00 AS Decimal(6, 2)), CAST(N'2020-07-07' AS Date))
INSERT [dbo].[DiaryDetails] ([ID_Diary], [ID_Product], [Kcal], [AddData]) VALUES (3, 5, CAST(25.00 AS Decimal(6, 2)), CAST(N'2020-07-07' AS Date))
INSERT [dbo].[DiaryDetails] ([ID_Diary], [ID_Product], [Kcal], [AddData]) VALUES (3, 2, CAST(69.00 AS Decimal(6, 2)), CAST(N'2020-07-07' AS Date))
INSERT [dbo].[DiaryDetails] ([ID_Diary], [ID_Product], [Kcal], [AddData]) VALUES (3, 6, CAST(365.00 AS Decimal(6, 2)), CAST(N'2020-07-08' AS Date))
INSERT [dbo].[DiaryDetails] ([ID_Diary], [ID_Product], [Kcal], [AddData]) VALUES (3, 3, CAST(121.00 AS Decimal(6, 2)), CAST(N'2020-07-08' AS Date))
INSERT [dbo].[DiaryDetails] ([ID_Diary], [ID_Product], [Kcal], [AddData]) VALUES (3, 7, CAST(219.00 AS Decimal(6, 2)), CAST(N'2020-07-08' AS Date))
INSERT [dbo].[DiaryDetails] ([ID_Diary], [ID_Product], [Kcal], [AddData]) VALUES (3, 3, CAST(121.00 AS Decimal(6, 2)), CAST(N'2020-07-08' AS Date))
INSERT [dbo].[DiaryDetails] ([ID_Diary], [ID_Product], [Kcal], [AddData]) VALUES (3, 6, CAST(730.00 AS Decimal(6, 2)), CAST(N'2020-07-08' AS Date))
INSERT [dbo].[DiaryDetails] ([ID_Diary], [ID_Product], [Kcal], [AddData]) VALUES (3, 6, CAST(730.00 AS Decimal(6, 2)), CAST(N'2020-07-08' AS Date))
INSERT [dbo].[DiaryDetails] ([ID_Diary], [ID_Product], [Kcal], [AddData]) VALUES (3, 1, CAST(53.00 AS Decimal(6, 2)), CAST(N'2020-07-08' AS Date))
INSERT [dbo].[DiaryDetails] ([ID_Diary], [ID_Product], [Kcal], [AddData]) VALUES (3, 1, CAST(53.00 AS Decimal(6, 2)), CAST(N'2020-07-08' AS Date))
INSERT [dbo].[DiaryDetails] ([ID_Diary], [ID_Product], [Kcal], [AddData]) VALUES (3, 1, CAST(53.00 AS Decimal(6, 2)), CAST(N'2020-07-08' AS Date))
INSERT [dbo].[DiaryDetails] ([ID_Diary], [ID_Product], [Kcal], [AddData]) VALUES (3, 4, CAST(80.10 AS Decimal(6, 2)), CAST(N'2020-07-05' AS Date))
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([ID_Product], [ProductName], [Weight], [Kcal], [Protein], [Carbohydrates], [Fat]) VALUES (1, N'Orange', 100, 53, 1, 16, 1)
INSERT [dbo].[Products] ([ID_Product], [ProductName], [Weight], [Kcal], [Protein], [Carbohydrates], [Fat]) VALUES (2, N'Jogurt naturalny', 100, 69, 5, 6, 3)
INSERT [dbo].[Products] ([ID_Product], [ProductName], [Weight], [Kcal], [Protein], [Carbohydrates], [Fat]) VALUES (3, N'Kurczak (pierś)', 100, 121, 22, 0, 4)
INSERT [dbo].[Products] ([ID_Product], [ProductName], [Weight], [Kcal], [Protein], [Carbohydrates], [Fat]) VALUES (4, N'Banan', 100, 89, 1, 22, 0)
INSERT [dbo].[Products] ([ID_Product], [ProductName], [Weight], [Kcal], [Protein], [Carbohydrates], [Fat]) VALUES (5, N'Apple', 100, 50, 0, 14, 1)
INSERT [dbo].[Products] ([ID_Product], [ProductName], [Weight], [Kcal], [Protein], [Carbohydrates], [Fat]) VALUES (6, N'CornFlakes Nestle', 100, 365, 7, 87, 1)
INSERT [dbo].[Products] ([ID_Product], [ProductName], [Weight], [Kcal], [Protein], [Carbohydrates], [Fat]) VALUES (7, N'Ryż długoziarnisty', 100, 365, 8, 79, 1)
INSERT [dbo].[Products] ([ID_Product], [ProductName], [Weight], [Kcal], [Protein], [Carbohydrates], [Fat]) VALUES (9, N'Mleko 2%', 100, 50, 3, 5, 2)
INSERT [dbo].[Products] ([ID_Product], [ProductName], [Weight], [Kcal], [Protein], [Carbohydrates], [Fat]) VALUES (10, N'Jajka', 100, 139, 13, 1, 10)
INSERT [dbo].[Products] ([ID_Product], [ProductName], [Weight], [Kcal], [Protein], [Carbohydrates], [Fat]) VALUES (11, N'Kasza jaglana', 100, 378, 11, 73, 4)
INSERT [dbo].[Products] ([ID_Product], [ProductName], [Weight], [Kcal], [Protein], [Carbohydrates], [Fat]) VALUES (12, N'Coca cola', 100, 37, 0, 10, 0)
INSERT [dbo].[Products] ([ID_Product], [ProductName], [Weight], [Kcal], [Protein], [Carbohydrates], [Fat]) VALUES (13, N'Ser biały półtłusty', 100, 128, 20, 3, 4)
INSERT [dbo].[Products] ([ID_Product], [ProductName], [Weight], [Kcal], [Protein], [Carbohydrates], [Fat]) VALUES (14, N'Ryba smażona', 100, 232, 15, 17, 12)
SET IDENTITY_INSERT [dbo].[Products] OFF
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([ID_User], [Username], [Password], [Weight], [Height], [Sex], [Age], [ActivityLevel], [BMI], [BMR]) VALUES (1, N'speed739', N'�璠硓﬒ꔱᩢ䨞떔镥', 90, 190, N'Man', 25, N'L', CAST(24.93 AS Decimal(4, 2)), CAST(2362.32 AS Decimal(6, 2)))
INSERT [dbo].[Users] ([ID_User], [Username], [Password], [Weight], [Height], [Sex], [Age], [ActivityLevel], [BMI], [BMR]) VALUES (2, N'asd', N'�璠硓﬒ꔱᩢ䨞떔镥', 75, 180, N'Man', 21, N'M', CAST(23.15 AS Decimal(4, 2)), CAST(2663.89 AS Decimal(6, 2)))
INSERT [dbo].[Users] ([ID_User], [Username], [Password], [Weight], [Height], [Sex], [Age], [ActivityLevel], [BMI], [BMR]) VALUES (3, N'qwe', N'�璠硓﬒ꔱᩢ䨞떔镥', 80, 190, N'Man', 29, N'M', CAST(22.16 AS Decimal(4, 2)), CAST(2773.53 AS Decimal(6, 2)))
INSERT [dbo].[Users] ([ID_User], [Username], [Password], [Weight], [Height], [Sex], [Age], [ActivityLevel], [BMI], [BMR]) VALUES (4, N'asdf', N'�璠硓﬒ꔱᩢ䨞떔镥', 60, 170, N'Man', 27, N'L', CAST(0.00 AS Decimal(4, 2)), CAST(0.00 AS Decimal(6, 2)))
INSERT [dbo].[Users] ([ID_User], [Username], [Password], [Weight], [Height], [Sex], [Age], [ActivityLevel], [BMI], [BMR]) VALUES (6, N'zxc', N'�璠硓﬒ꔱᩢ䨞떔镥', 75, 170, N'Man', 20, N'L', CAST(25.95 AS Decimal(4, 2)), CAST(2062.02 AS Decimal(6, 2)))
INSERT [dbo].[Users] ([ID_User], [Username], [Password], [Weight], [Height], [Sex], [Age], [ActivityLevel], [BMI], [BMR]) VALUES (7, N'qwer', N'�璠硓﬒ꔱᩢ䨞떔镥', 55, 160, N'Woman', 23, N'L', CAST(21.48 AS Decimal(4, 2)), CAST(1635.48 AS Decimal(6, 2)))
INSERT [dbo].[Users] ([ID_User], [Username], [Password], [Weight], [Height], [Sex], [Age], [ActivityLevel], [BMI], [BMR]) VALUES (8, N'Test', N'�璠硓﬒ꔱᩢ䨞떔镥', 66, 175, N'Woman', 22, N'L', CAST(21.55 AS Decimal(4, 2)), CAST(1800.24 AS Decimal(6, 2)))
SET IDENTITY_INSERT [dbo].[Users] OFF
/****** Object:  Index [UQ__Diary__ED4DE443CEC11882]    Script Date: 08.07.2020 14:44:00 ******/
ALTER TABLE [dbo].[Diary] ADD UNIQUE NONCLUSTERED 
(
	[ID_User] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Products__DD5A978A35EEF6E5]    Script Date: 08.07.2020 14:44:00 ******/
ALTER TABLE [dbo].[Products] ADD UNIQUE NONCLUSTERED 
(
	[ProductName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__536C85E4689D6798]    Script Date: 08.07.2020 14:44:00 ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Diary] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[DiaryDetails] ADD  DEFAULT (getdate()) FOR [AddData]
GO
ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF_Products_Weight]  DEFAULT ((100)) FOR [Weight]
GO
ALTER TABLE [dbo].[Diary]  WITH CHECK ADD FOREIGN KEY([ID_User])
REFERENCES [dbo].[Users] ([ID_User])
GO
ALTER TABLE [dbo].[DiaryDetails]  WITH CHECK ADD FOREIGN KEY([ID_Diary])
REFERENCES [dbo].[Diary] ([ID_Diary])
GO
ALTER TABLE [dbo].[DiaryDetails]  WITH CHECK ADD FOREIGN KEY([ID_Product])
REFERENCES [dbo].[Products] ([ID_Product])
GO
USE [master]
GO
ALTER DATABASE [FoodDiary] SET  READ_WRITE 
GO
