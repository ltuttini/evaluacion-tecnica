CREATE TABLE Transaction (  
    ID int NOT NULL PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
    SourceAccountId UUID,
    TargetAccountId UUID,
    TransferTypeId int,
    State int,
    Value int
);