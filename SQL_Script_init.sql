CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    UserName NVARCHAR(100) NOT NULL
);

CREATE TABLE Groups (
    GroupId INT PRIMARY KEY IDENTITY(1,1),
    GroupName NVARCHAR(100) NOT NULL
);

CREATE TABLE UserGroups (
    UserId INT,
    GroupId INT,
    PRIMARY KEY (UserId, GroupId),
    FOREIGN KEY (UserId) REFERENCES Users(UserId),
    FOREIGN KEY (GroupId) REFERENCES Groups(GroupId)
);


CREATE PROCEDURE AddUser
    @UserName NVARCHAR(100)
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Users WHERE UserName = @UserName)
    BEGIN
        INSERT INTO Users (UserName)
        VALUES (@UserName);
    END
END

CREATE PROCEDURE AddGroup
    @GroupName NVARCHAR(100)
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Groups WHERE GroupName = @GroupName)
    BEGIN
        INSERT INTO Groups (GroupName)
        VALUES (@GroupName);
    END
END

CREATE PROCEDURE GetAvailableGroupsForUser
    @UserId INT
AS
BEGIN
    SELECT g.GroupId, g.GroupName
    FROM Groups g
    WHERE g.GroupId NOT IN (
        SELECT ug.GroupId
        FROM UserGroups ug
        WHERE ug.UserId = @UserId
    );
END;

CREATE PROCEDURE AddUserToGroup
    @UserId INT,
    @GroupId INT
AS
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM UserGroups
        WHERE UserId = @UserId AND GroupId = @GroupId
    )
    BEGIN
        INSERT INTO UserGroups (UserId, GroupId)
        VALUES (@UserId, @GroupId);
    END
END;

CREATE PROCEDURE GetUsersWithGroupsJson
AS
BEGIN
    SELECT 
        u.UserId,
        u.UserName,
        (
            SELECT 
                g.GroupId,
                g.GroupName
            FROM 
                UserGroups ug
            JOIN 
                Groups g ON ug.GroupId = g.GroupId
            WHERE 
                ug.UserId = u.UserId
            FOR JSON PATH
        ) AS Groups
    FROM 
        Users u
    FOR JSON PATH;
END;
