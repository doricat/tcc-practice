-- sql server
create table AspNetRoles
(
    Id               bigint not null
        constraint PK_AspNetRoles
            primary key,
    Name             nvarchar(256),
    NormalizedName   nvarchar(256),
    ConcurrencyStamp nvarchar(max)
)
go

create table AspNetRoleClaims
(
    Id         int identity
        constraint PK_AspNetRoleClaims
            primary key,
    RoleId     bigint not null
        constraint FK_AspNetRoleClaims_AspNetRoles_RoleId
            references AspNetRoles
            on delete cascade,
    ClaimType  nvarchar(max),
    ClaimValue nvarchar(max)
)
go

create index IX_AspNetRoleClaims_RoleId
    on AspNetRoleClaims (RoleId)
go

create unique index RoleNameIndex
    on AspNetRoles (NormalizedName)
    where [NormalizedName] IS NOT NULL
go

create table AspNetUsers
(
    Id                   bigint not null
        constraint PK_AspNetUsers
            primary key,
    UserName             nvarchar(256),
    NormalizedUserName   nvarchar(256),
    Email                nvarchar(256),
    NormalizedEmail      nvarchar(256),
    EmailConfirmed       bit    not null,
    PasswordHash         nvarchar(max),
    SecurityStamp        nvarchar(max),
    ConcurrencyStamp     nvarchar(max),
    PhoneNumber          nvarchar(max),
    PhoneNumberConfirmed bit    not null,
    TwoFactorEnabled     bit    not null,
    LockoutEnd           datetimeoffset,
    LockoutEnabled       bit    not null,
    AccessFailedCount    int    not null
)
go

create table AspNetUserClaims
(
    Id         int identity
        constraint PK_AspNetUserClaims
            primary key,
    UserId     bigint not null
        constraint FK_AspNetUserClaims_AspNetUsers_UserId
            references AspNetUsers
            on delete cascade,
    ClaimType  nvarchar(max),
    ClaimValue nvarchar(max)
)
go

create index IX_AspNetUserClaims_UserId
    on AspNetUserClaims (UserId)
go

create table AspNetUserLogins
(
    LoginProvider       nvarchar(450) not null,
    ProviderKey         nvarchar(450) not null,
    ProviderDisplayName nvarchar(max),
    UserId              bigint        not null
        constraint FK_AspNetUserLogins_AspNetUsers_UserId
            references AspNetUsers
            on delete cascade,
    constraint PK_AspNetUserLogins
        primary key (LoginProvider, ProviderKey)
)
go

create index IX_AspNetUserLogins_UserId
    on AspNetUserLogins (UserId)
go

create table AspNetUserRoles
(
    UserId bigint not null
        constraint FK_AspNetUserRoles_AspNetUsers_UserId
            references AspNetUsers
            on delete cascade,
    RoleId bigint not null
        constraint FK_AspNetUserRoles_AspNetRoles_RoleId
            references AspNetRoles
            on delete cascade,
    constraint PK_AspNetUserRoles
        primary key (UserId, RoleId)
)
go

create index IX_AspNetUserRoles_RoleId
    on AspNetUserRoles (RoleId)
go

create table AspNetUserTokens
(
    UserId        bigint        not null
        constraint FK_AspNetUserTokens_AspNetUsers_UserId
            references AspNetUsers
            on delete cascade,
    LoginProvider nvarchar(450) not null,
    Name          nvarchar(450) not null,
    Value         nvarchar(max),
    constraint PK_AspNetUserTokens
        primary key (UserId, LoginProvider, Name)
)
go

create index EmailIndex
    on AspNetUsers (NormalizedEmail)
go

create unique index UserNameIndex
    on AspNetUsers (NormalizedUserName)
    where [NormalizedUserName] IS NOT NULL
go

create table DeviceCodes
(
    UserCode     nvarchar(200) not null
        constraint PK_DeviceCodes
            primary key,
    DeviceCode   nvarchar(200) not null,
    SubjectId    nvarchar(200),
    ClientId     nvarchar(200) not null,
    CreationTime datetime2     not null,
    Expiration   datetime2     not null,
    Data         nvarchar(max) not null
)
go

create unique index IX_DeviceCodes_DeviceCode
    on DeviceCodes (DeviceCode)
go

create index IX_DeviceCodes_Expiration
    on DeviceCodes (Expiration)
go

create table PersistedGrants
(
    [Key]        nvarchar(200) not null
        constraint PK_PersistedGrants
            primary key,
    Type         nvarchar(50)  not null,
    SubjectId    nvarchar(200),
    ClientId     nvarchar(200) not null,
    CreationTime datetime2     not null,
    Expiration   datetime2,
    Data         nvarchar(max) not null
)
go

create index IX_PersistedGrants_Expiration
    on PersistedGrants (Expiration)
go

create index IX_PersistedGrants_SubjectId_ClientId_Type
    on PersistedGrants (SubjectId, ClientId, Type)
go

create table __EFMigrationsHistory
(
    MigrationId    nvarchar(150) not null
        constraint PK___EFMigrationsHistory
            primary key,
    ProductVersion nvarchar(32)  not null
)
go

INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed,
                                   PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed,
                                   TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount)
VALUES (11704070658064385, '123qwe@123qwe.com', '123QWE@123QWE.COM', '123qwe@123qwe.com', '123QWE@123QWE.COM',
        1, 'AQAAAAEAACcQAAAAEFPOf0veHrmHFo860fl4DpDpLmoeNg4bj/Vc5fHlVmYM4sxLHVdN0DDRwNysfRw4Gw==',
        'DR2GIWLXABVZL5WW4TY7WH6YBUR5OFU3', '9d016f86-aeb4-4860-a7a0-1fb89bc46ee0', null, 0, 0, null, 1, 0);
-- pwd = 123Qwe?
INSERT INTO AspNetUserClaims (UserId, ClaimType, ClaimValue)
VALUES (11704070658064385, 'email', 'email');