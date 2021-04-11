SET IDENTITY_INSERT dbo.[Users] ON;

INSERT INTO dbo.[Users] (Id, Username) VALUES
(1, 'Jack'),
(2, 'Jessica'),
(3, 'Johnny'),
(4, 'Luke'),
(5, 'Cassandra');

SET IDENTITY_INSERT dbo.[Users] OFF;
