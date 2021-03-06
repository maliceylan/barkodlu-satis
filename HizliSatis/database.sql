USE [master]
GO
/****** Object:  Database [Hizlisatis]    Script Date: 19.06.2019 14:42:44 ******/
CREATE DATABASE [Hizlisatis]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Hizlisatis', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\Hizlisatis.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Hizlisatis_log', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\Hizlisatis_log.ldf' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Hizlisatis] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Hizlisatis].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Hizlisatis] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Hizlisatis] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Hizlisatis] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Hizlisatis] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Hizlisatis] SET ARITHABORT OFF 
GO
ALTER DATABASE [Hizlisatis] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Hizlisatis] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [Hizlisatis] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Hizlisatis] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Hizlisatis] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Hizlisatis] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Hizlisatis] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Hizlisatis] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Hizlisatis] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Hizlisatis] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Hizlisatis] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Hizlisatis] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Hizlisatis] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Hizlisatis] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Hizlisatis] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Hizlisatis] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Hizlisatis] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Hizlisatis] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Hizlisatis] SET RECOVERY FULL 
GO
ALTER DATABASE [Hizlisatis] SET  MULTI_USER 
GO
ALTER DATABASE [Hizlisatis] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Hizlisatis] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Hizlisatis] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Hizlisatis] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Hizlisatis', N'ON'
GO
USE [Hizlisatis]
GO
/****** Object:  Table [dbo].[CariHareket]    Script Date: 19.06.2019 14:42:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CariHareket](
	[MusteriID] [int] NOT NULL,
	[CariNo] [int] IDENTITY(1,1) NOT NULL,
	[Tur] [nvarchar](50) NULL,
	[Aciklama] [nvarchar](50) NULL,
	[SonOdemeTarihi] [date] NULL,
	[Borc] [float] NULL,
	[Tahsilat] [float] NULL,
	[IslemTarihi] [date] NULL,
 CONSTRAINT [PK_CariHareket] PRIMARY KEY CLUSTERED 
(
	[CariNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Fatura]    Script Date: 19.06.2019 14:42:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Fatura](
	[FaturaNo] [nvarchar](50) NULL,
	[BarkodNo] [nvarchar](50) NULL,
	[UrunAdi] [nvarchar](50) NULL,
	[Miktar] [float] NULL,
	[Birim] [nvarchar](50) NULL,
	[KDV] [int] NULL,
	[Fiyat] [float] NULL,
	[Tutar] [float] NULL,
	[Tarih] [smalldatetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hesaplar]    Script Date: 19.06.2019 14:42:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hesaplar](
	[MusteriID] [int] NOT NULL,
	[Adi] [nvarchar](50) NULL,
	[Tur] [nvarchar](50) NULL,
	[SonIslemTarihi] [date] NULL,
	[SonOdemeTarihi] [date] NULL,
	[Bakiye] [float] NULL,
 CONSTRAINT [PK_Hesaplar] PRIMARY KEY CLUSTERED 
(
	[MusteriID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Musteriler]    Script Date: 19.06.2019 14:42:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Musteriler](
	[MusteriID] [int] NOT NULL,
	[Adi] [nvarchar](50) NULL,
	[Grubu] [nvarchar](50) NULL,
	[Tur] [nvarchar](50) NULL,
	[Telefon] [nvarchar](50) NULL,
	[GSM] [nvarchar](50) NULL,
	[Adres] [nvarchar](200) NULL,
	[VergiDairesi] [nvarchar](50) NULL,
	[VergiNo] [nvarchar](50) NULL,
	[TCNo] [nvarchar](50) NULL,
	[SonIslemTarihi] [date] NULL,
	[SonOdemeTarihi] [date] NULL,
	[Bakiye] [float] NULL,
	[Hesap] [int] NULL,
 CONSTRAINT [PK_Musteriler] PRIMARY KEY CLUSTERED 
(
	[MusteriID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Stok]    Script Date: 19.06.2019 14:42:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stok](
	[StokID] [int] IDENTITY(1,1) NOT NULL,
	[Adi] [nvarchar](50) NULL,
	[Grubu] [nvarchar](50) NULL,
	[Barkod] [nvarchar](50) NULL,
	[StokKodu] [nvarchar](50) NULL,
	[Miktar] [float] NULL,
	[Birim] [nvarchar](50) NULL,
	[AlisFiyati] [float] NULL,
	[SatisFiyati1] [float] NULL,
	[SatisFiyati2] [float] NULL,
	[KDV] [int] NULL,
	[OTV] [int] NULL,
	[KritikSeviye] [int] NULL,
 CONSTRAINT [PK_Stok] PRIMARY KEY CLUSTERED 
(
	[StokID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StokHareket]    Script Date: 19.06.2019 14:42:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StokHareket](
	[HareketID] [int] IDENTITY(1,1) NOT NULL,
	[Adi] [nvarchar](50) NULL,
	[Barkod] [nvarchar](50) NULL,
	[Miktar] [nvarchar](50) NULL,
	[Tur] [nvarchar](50) NULL,
	[Islem] [nvarchar](50) NULL,
	[Aciklama] [nvarchar](200) NULL,
	[Tarih] [date] NULL,
 CONSTRAINT [PK_StokHareket] PRIMARY KEY CLUSTERED 
(
	[HareketID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
USE [master]
GO
ALTER DATABASE [Hizlisatis] SET  READ_WRITE 
GO
