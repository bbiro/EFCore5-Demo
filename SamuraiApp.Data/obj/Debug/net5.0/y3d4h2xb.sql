ALTER DATABASE CHARACTER SET utf8mb4;
CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `Samurais` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_Samurais` PRIMARY KEY (`Id`)
) CHARACTER SET utf8mb4;

CREATE TABLE `Servers` (
    `Id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `Prefix` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_Servers` PRIMARY KEY (`Id`)
) CHARACTER SET utf8mb4;

CREATE TABLE `Quotes` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Text` longtext CHARACTER SET utf8mb4 NULL,
    `SamuraiId` int NOT NULL,
    CONSTRAINT `PK_Quotes` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Quotes_Samurais_SamuraiId` FOREIGN KEY (`SamuraiId`) REFERENCES `Samurais` (`Id`) ON DELETE CASCADE
) CHARACTER SET utf8mb4;

CREATE INDEX `IX_Quotes_SamuraiId` ON `Quotes` (`SamuraiId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20210528054845_init', '5.0.6');

COMMIT;

