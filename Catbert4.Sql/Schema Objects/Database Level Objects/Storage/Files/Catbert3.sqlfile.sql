ALTER DATABASE [$(DatabaseName)]
    ADD FILE (NAME = [Catbert3], FILENAME = '$(DefaultDataPath)$(DatabaseName).mdf', FILEGROWTH = 1024 KB) TO FILEGROUP [PRIMARY];

