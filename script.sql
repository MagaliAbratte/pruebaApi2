CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    PRIMARY KEY (`MigrationId`)
);

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230911194842_Inicial')
BEGIN
    CREATE TABLE `Autores` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Nombre` longtext NOT NULL,
        PRIMARY KEY (`Id`)
    );
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230911194842_Inicial')
BEGIN
    CREATE TABLE `Libros` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Nombre` longtext NULL,
        PRIMARY KEY (`Id`)
    );
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230911194842_Inicial')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230911194842_Inicial', '7.0.10');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230912192748_Comentarios')
BEGIN
    ALTER TABLE `Libros` MODIFY `Nombre` longtext NULL;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230912192748_Comentarios')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230912192748_Comentarios', '7.0.10');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230913183025_AutoresLibros')
BEGIN
    CREATE TABLE `AutoresLibros` (
        `LibroId` int NOT NULL,
        `AutorId` int NOT NULL,
        `Order` int NOT NULL,
        PRIMARY KEY (`AutorId`, `LibroId`),
        CONSTRAINT `FK_AutoresLibros_Autores_AutorId` FOREIGN KEY (`AutorId`) REFERENCES `Autores` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_AutoresLibros_Libros_LibroId` FOREIGN KEY (`LibroId`) REFERENCES `Libros` (`Id`) ON DELETE CASCADE
    );
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230913183025_AutoresLibros')
BEGIN
    CREATE INDEX `IX_AutoresLibros_LibroId` ON `AutoresLibros` (`LibroId`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230913183025_AutoresLibros')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230913183025_AutoresLibros', '7.0.10');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230914120918_FechaPublicacionLibro')
BEGIN
    ALTER TABLE `Libros` ADD `FechaPublicacion` datetime(6) NULL;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230914120918_FechaPublicacionLibro')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230914120918_FechaPublicacionLibro', '7.0.10');
END;

COMMIT;

