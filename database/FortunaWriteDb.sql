USE master
GO

IF DB_ID(N'FortunaWriteDb') IS NULL
BEGIN
    CREATE DATABASE [FortunaWriteDb];
END
GO

USE [FortunaWriteDb];
GO

IF OBJECT_ID(N'[dbo].[Customers]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Customers]
    (
        [Id] UNIQUEIDENTIFIER NOT NULL,
        [FirstName] NVARCHAR(100) NOT NULL,
        [LastName] NVARCHAR(100) NOT NULL,
        [Email] NVARCHAR(200) NOT NULL,
        [PasswordHash] NVARCHAR(512) NOT NULL,
        [CreatedAtUtc] DATETIME2(7) NOT NULL,
        CONSTRAINT [PK_dbo_Customers] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END
GO

IF OBJECT_ID(N'[dbo].[BankAccounts]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[BankAccounts]
    (
        [Id] UNIQUEIDENTIFIER NOT NULL,
        [CustomerId] UNIQUEIDENTIFIER NOT NULL,
        [AccountNumber] NVARCHAR(34) NOT NULL,
        [AccountName] NVARCHAR(200) NOT NULL,
        [Balance] DECIMAL(18, 2) NOT NULL,
        [Currency] NVARCHAR(3) NOT NULL,
        [Status] INT NOT NULL,
        [CreatedAtUtc] DATETIME2(7) NOT NULL,
        CONSTRAINT [PKBankAccounts] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FKBankAccountsCustomers] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customers]([Id])
    );
END
GO

IF OBJECT_ID(N'[dbo].[Transactions]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Transactions]
    (
        [Id] UNIQUEIDENTIFIER NOT NULL,
        [BankAccountId] UNIQUEIDENTIFIER NOT NULL,
        [TransferId] UNIQUEIDENTIFIER NULL,
        [Type] INT NOT NULL,
        [Amount] DECIMAL(18, 2) NOT NULL,
        [Currency] NVARCHAR(3) NOT NULL,
        [Title] NVARCHAR(300) NOT NULL,
        [BookedAtUtc] DATETIME2(7) NOT NULL,
        CONSTRAINT [PKTransactions] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FKTransactionsBankAccounts] FOREIGN KEY ([BankAccountId]) REFERENCES [dbo].[BankAccounts]([Id])
    );
END
GO

IF OBJECT_ID(N'[dbo].[Transfers]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Transfers]
    (
        [Id] UNIQUEIDENTIFIER NOT NULL,
        [SourceAccountId] UNIQUEIDENTIFIER NOT NULL,
        [TargetAccountId] UNIQUEIDENTIFIER NOT NULL,
        [Amount] DECIMAL(18, 2) NOT NULL,
        [Currency] NVARCHAR(3) NOT NULL,
        [Title] NVARCHAR(300) NOT NULL,
        [Status] INT NOT NULL,
        [CreatedAtUtc] DATETIME2(7) NOT NULL,
        [CompletedAtUtc] DATETIME2(7) NULL,
        CONSTRAINT [PKTransfers] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END
GO

IF OBJECT_ID(N'[dbo].[OutboxMessages]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[OutboxMessages]
    (
        [Id] UNIQUEIDENTIFIER NOT NULL,
        [Type] NVARCHAR(500) NOT NULL,
        [Payload] NVARCHAR(MAX) NOT NULL,
        [OccurredOnUtc] DATETIME2(7) NOT NULL,
        [ProcessedOnUtc] DATETIME2(7) NULL,
        [Error] NVARCHAR(MAX) NULL,
        CONSTRAINT [PKOutboxMessages] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IXCustomersEmail'
      AND object_id = OBJECT_ID(N'[dbo].[Customers]')
)
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_dbo_Customers_Email]
        ON [dbo].[Customers] ([Email] ASC);
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IXBankAccountsCustomerId'
      AND object_id = OBJECT_ID(N'[dbo].[BankAccounts]')
)
BEGIN
    CREATE NONCLUSTERED INDEX [IX_dbo_BankAccounts_CustomerId]
        ON [dbo].[BankAccounts] ([CustomerId] ASC);
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IXTransactionsBankAccountId'
      AND object_id = OBJECT_ID(N'[dbo].[Transactions]')
)
BEGIN
    CREATE NONCLUSTERED INDEX [IXTransactionsBankAccountId]
        ON [dbo].[Transactions] ([BankAccountId] ASC);
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IXTransactionsTransferId'
      AND object_id = OBJECT_ID(N'[dbo].[Transactions]')
)
BEGIN
    CREATE NONCLUSTERED INDEX [IXTransactionsTransferId]
        ON [dbo].[Transactions] ([TransferId] ASC);
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IXTransfersSourceAccountId'
      AND object_id = OBJECT_ID(N'[dbo].[Transfers]')
)
BEGIN
    CREATE NONCLUSTERED INDEX [IXTransfersSourceAccountId]
        ON [dbo].[Transfers] ([SourceAccountId] ASC);
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IXTransfersTargetAccountId'
      AND object_id = OBJECT_ID(N'[dbo].[Transfers]')
)
BEGIN
    CREATE NONCLUSTERED INDEX [IXTransfersTargetAccountId]
        ON [dbo].[Transfers] ([TargetAccountId] ASC);
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IXOutboxMessagesProcessedOnUtc'
      AND object_id = OBJECT_ID(N'[dbo].[OutboxMessages]')
)
BEGIN
    CREATE NONCLUSTERED INDEX [IX_dbo_OutboxMessages_ProcessedOnUtc]
        ON [dbo].[OutboxMessages] ([ProcessedOnUtc] ASC);
END
GO
