-- DB_Vakantieparken --
CREATE DATABASE DB_Vakantieparken;
GO

USE DB_Vakantieparken;
GO

-- Park
CREATE TABLE [dbo].Park (
    [id] [int] NOT NULL,
    [naam] [nvarchar] (200) NOT NULL,
	[locatie] [nvarchar] (200) NOT NULL,
    CONSTRAINT [PK_Park] PRIMARY KEY CLUSTERED ([id] ASC));
	GO
------------------------------------------------------------------------------------
-- Faciliteit
CREATE TABLE [dbo].Faciliteit (
    [id] [int] NOT NULL,
	[beschrijving] [nvarchar] (200) NOT NULL,
    CONSTRAINT [PK_Faciliteit] PRIMARY KEY CLUSTERED ([id] ASC));
	GO
------------------------------------------------------------------------------------
-- Huis
CREATE TABLE [dbo].Huis (
    [id] [int] NOT NULL,
	[straat] [nvarchar] (200) NOT NULL,
	[nummer] [nvarchar] (10) NOT NULL,
	[isActief] [bit] NOT NULL,
	[capaciteit] [int] NOT NULL,
	[park_id] [int] NOT NULL,
    CONSTRAINT [PK_Huis] PRIMARY KEY CLUSTERED ([id] ASC));

ALTER TABLE [dbo]. [Huis] WITH CHECK ADD CONSTRAINT [FK_Huis_Park] FOREIGN KEY([park_id])
REFERENCES [dbo]. [Park] ([id]);

ALTER TABLE [dbo]. [Huis] CHECK CONSTRAINT [FK_Huis_Park];
GO
------------------------------------------------------------------------------------
-- Klant
CREATE TABLE [dbo].Klant (
    [id] [int] NOT NULL,
	[naam] [nvarchar] (200) NOT NULL,
	[adres] [nvarchar] (200) NOT NULL,
    CONSTRAINT [PK_Klant] PRIMARY KEY CLUSTERED ([id] ASC));
	GO
------------------------------------------------------------------------------------
-- Park_Faciliteit
CREATE TABLE [dbo].Park_Faciliteit (
    [park_id] [int] NOT NULL,
	[faciliteit_id] [int] NOT NULL,
    CONSTRAINT [PK_Park_Faciliteit] PRIMARY KEY CLUSTERED ([park_id] ASC, [faciliteit_id] ASC));

ALTER TABLE [dbo]. [Park_Faciliteit] WITH CHECK ADD CONSTRAINT [FK_ParkFaciliteit_Park] FOREIGN KEY([park_id])
REFERENCES [dbo]. [Park] ([id]);

ALTER TABLE [dbo]. [Park_Faciliteit] CHECK CONSTRAINT [FK_ParkFaciliteit_Park];

ALTER TABLE [dbo]. [Park_Faciliteit] WITH CHECK ADD CONSTRAINT [FK_ParkFaciliteit_Faciliteit] FOREIGN KEY([faciliteit_id])
REFERENCES [dbo]. [Faciliteit] ([id]);

ALTER TABLE [dbo]. [Park_Faciliteit] CHECK CONSTRAINT [FK_ParkFaciliteit_Faciliteit];
GO
------------------------------------------------------------------------------------
-- Reservatie
CREATE TABLE [dbo].Reservatie (
    [id] [int] IDENTITY(1,1) NOT NULL,
	[klant_id] [int] NOT NULL,
	[startDatum] [date] NOT NULL,
	[eindDatum] [date] NOT NULL,
	[isGeaffecteerd] [bit] NOT NULL,
	[huis_id] [int] NOT NULL,
    CONSTRAINT [PK_Reservatie] PRIMARY KEY NONCLUSTERED ([id]));

ALTER TABLE [dbo]. [Reservatie] WITH CHECK ADD CONSTRAINT [FK_Reservatie_Klant] FOREIGN KEY([klant_id])
REFERENCES [dbo]. [Klant] ([id]);

ALTER TABLE [dbo]. [Reservatie] CHECK CONSTRAINT [FK_Reservatie_Klant];

ALTER TABLE [dbo]. [Reservatie] WITH CHECK ADD CONSTRAINT [FK_Reservatie_Huis] FOREIGN KEY([huis_id])
REFERENCES [dbo]. [Huis] ([id]);

ALTER TABLE [dbo]. [Reservatie] CHECK CONSTRAINT [FK_Reservatie_Huis];
GO