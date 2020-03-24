-- sql server
create table ApiResources
(
    Id           int identity
        constraint PK_ApiResources
            primary key,
    Enabled      bit           not null,
    Name         nvarchar(200) not null,
    DisplayName  nvarchar(200),
    Description  nvarchar(1000),
    Created      datetime2     not null,
    Updated      datetime2,
    LastAccessed datetime2,
    NonEditable  bit           not null
)
go

create table ApiClaims
(
    Id            int identity
        constraint PK_ApiClaims
            primary key,
    Type          nvarchar(200) not null,
    ApiResourceId int           not null
        constraint FK_ApiClaims_ApiResources_ApiResourceId
            references ApiResources
            on delete cascade
)
go

create index IX_ApiClaims_ApiResourceId
    on ApiClaims (ApiResourceId)
go

create table ApiProperties
(
    Id            int identity
        constraint PK_ApiProperties
            primary key,
    [Key]         nvarchar(250)  not null,
    Value         nvarchar(2000) not null,
    ApiResourceId int            not null
        constraint FK_ApiProperties_ApiResources_ApiResourceId
            references ApiResources
            on delete cascade
)
go

create index IX_ApiProperties_ApiResourceId
    on ApiProperties (ApiResourceId)
go

create unique index IX_ApiResources_Name
    on ApiResources (Name)
go

create table ApiScopes
(
    Id                      int identity
        constraint PK_ApiScopes
            primary key,
    Name                    nvarchar(200) not null,
    DisplayName             nvarchar(200),
    Description             nvarchar(1000),
    Required                bit           not null,
    Emphasize               bit           not null,
    ShowInDiscoveryDocument bit           not null,
    ApiResourceId           int           not null
        constraint FK_ApiScopes_ApiResources_ApiResourceId
            references ApiResources
            on delete cascade
)
go

create table ApiScopeClaims
(
    Id         int identity
        constraint PK_ApiScopeClaims
            primary key,
    Type       nvarchar(200) not null,
    ApiScopeId int           not null
        constraint FK_ApiScopeClaims_ApiScopes_ApiScopeId
            references ApiScopes
            on delete cascade
)
go

create index IX_ApiScopeClaims_ApiScopeId
    on ApiScopeClaims (ApiScopeId)
go

create index IX_ApiScopes_ApiResourceId
    on ApiScopes (ApiResourceId)
go

create unique index IX_ApiScopes_Name
    on ApiScopes (Name)
go

create table ApiSecrets
(
    Id            int identity
        constraint PK_ApiSecrets
            primary key,
    Description   nvarchar(1000),
    Value         nvarchar(4000) not null,
    Expiration    datetime2,
    Type          nvarchar(250)  not null,
    Created       datetime2      not null,
    ApiResourceId int            not null
        constraint FK_ApiSecrets_ApiResources_ApiResourceId
            references ApiResources
            on delete cascade
)
go

create index IX_ApiSecrets_ApiResourceId
    on ApiSecrets (ApiResourceId)
go

create table Clients
(
    Id                                int identity
        constraint PK_Clients
            primary key,
    Enabled                           bit           not null,
    ClientId                          nvarchar(200) not null,
    ProtocolType                      nvarchar(200) not null,
    RequireClientSecret               bit           not null,
    ClientName                        nvarchar(200),
    Description                       nvarchar(1000),
    ClientUri                         nvarchar(2000),
    LogoUri                           nvarchar(2000),
    RequireConsent                    bit           not null,
    AllowRememberConsent              bit           not null,
    AlwaysIncludeUserClaimsInIdToken  bit           not null,
    RequirePkce                       bit           not null,
    AllowPlainTextPkce                bit           not null,
    AllowAccessTokensViaBrowser       bit           not null,
    FrontChannelLogoutUri             nvarchar(2000),
    FrontChannelLogoutSessionRequired bit           not null,
    BackChannelLogoutUri              nvarchar(2000),
    BackChannelLogoutSessionRequired  bit           not null,
    AllowOfflineAccess                bit           not null,
    IdentityTokenLifetime             int           not null,
    AccessTokenLifetime               int           not null,
    AuthorizationCodeLifetime         int           not null,
    ConsentLifetime                   int,
    AbsoluteRefreshTokenLifetime      int           not null,
    SlidingRefreshTokenLifetime       int           not null,
    RefreshTokenUsage                 int           not null,
    UpdateAccessTokenClaimsOnRefresh  bit           not null,
    RefreshTokenExpiration            int           not null,
    AccessTokenType                   int           not null,
    EnableLocalLogin                  bit           not null,
    IncludeJwtId                      bit           not null,
    AlwaysSendClientClaims            bit           not null,
    ClientClaimsPrefix                nvarchar(200),
    PairWiseSubjectSalt               nvarchar(200),
    Created                           datetime2     not null,
    Updated                           datetime2,
    LastAccessed                      datetime2,
    UserSsoLifetime                   int,
    UserCodeType                      nvarchar(100),
    DeviceCodeLifetime                int           not null,
    NonEditable                       bit           not null
)
go

create table ClientClaims
(
    Id       int identity
        constraint PK_ClientClaims
            primary key,
    Type     nvarchar(250) not null,
    Value    nvarchar(250) not null,
    ClientId int           not null
        constraint FK_ClientClaims_Clients_ClientId
            references Clients
            on delete cascade
)
go

create index IX_ClientClaims_ClientId
    on ClientClaims (ClientId)
go

create table ClientCorsOrigins
(
    Id       int identity
        constraint PK_ClientCorsOrigins
            primary key,
    Origin   nvarchar(150) not null,
    ClientId int           not null
        constraint FK_ClientCorsOrigins_Clients_ClientId
            references Clients
            on delete cascade
)
go

create index IX_ClientCorsOrigins_ClientId
    on ClientCorsOrigins (ClientId)
go

create table ClientGrantTypes
(
    Id        int identity
        constraint PK_ClientGrantTypes
            primary key,
    GrantType nvarchar(250) not null,
    ClientId  int           not null
        constraint FK_ClientGrantTypes_Clients_ClientId
            references Clients
            on delete cascade
)
go

create index IX_ClientGrantTypes_ClientId
    on ClientGrantTypes (ClientId)
go

create table ClientIdPRestrictions
(
    Id       int identity
        constraint PK_ClientIdPRestrictions
            primary key,
    Provider nvarchar(200) not null,
    ClientId int           not null
        constraint FK_ClientIdPRestrictions_Clients_ClientId
            references Clients
            on delete cascade
)
go

create index IX_ClientIdPRestrictions_ClientId
    on ClientIdPRestrictions (ClientId)
go

create table ClientPostLogoutRedirectUris
(
    Id                    int identity
        constraint PK_ClientPostLogoutRedirectUris
            primary key,
    PostLogoutRedirectUri nvarchar(2000) not null,
    ClientId              int            not null
        constraint FK_ClientPostLogoutRedirectUris_Clients_ClientId
            references Clients
            on delete cascade
)
go

create index IX_ClientPostLogoutRedirectUris_ClientId
    on ClientPostLogoutRedirectUris (ClientId)
go

create table ClientProperties
(
    Id       int identity
        constraint PK_ClientProperties
            primary key,
    [Key]    nvarchar(250)  not null,
    Value    nvarchar(2000) not null,
    ClientId int            not null
        constraint FK_ClientProperties_Clients_ClientId
            references Clients
            on delete cascade
)
go

create index IX_ClientProperties_ClientId
    on ClientProperties (ClientId)
go

create table ClientRedirectUris
(
    Id          int identity
        constraint PK_ClientRedirectUris
            primary key,
    RedirectUri nvarchar(2000) not null,
    ClientId    int            not null
        constraint FK_ClientRedirectUris_Clients_ClientId
            references Clients
            on delete cascade
)
go

create index IX_ClientRedirectUris_ClientId
    on ClientRedirectUris (ClientId)
go

create table ClientScopes
(
    Id       int identity
        constraint PK_ClientScopes
            primary key,
    Scope    nvarchar(200) not null,
    ClientId int           not null
        constraint FK_ClientScopes_Clients_ClientId
            references Clients
            on delete cascade
)
go

create index IX_ClientScopes_ClientId
    on ClientScopes (ClientId)
go

create table ClientSecrets
(
    Id          int identity
        constraint PK_ClientSecrets
            primary key,
    Description nvarchar(2000),
    Value       nvarchar(4000) not null,
    Expiration  datetime2,
    Type        nvarchar(250)  not null,
    Created     datetime2      not null,
    ClientId    int            not null
        constraint FK_ClientSecrets_Clients_ClientId
            references Clients
            on delete cascade
)
go

create index IX_ClientSecrets_ClientId
    on ClientSecrets (ClientId)
go

create unique index IX_Clients_ClientId
    on Clients (ClientId)
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

create table IdentityResources
(
    Id                      int identity
        constraint PK_IdentityResources
            primary key,
    Enabled                 bit           not null,
    Name                    nvarchar(200) not null,
    DisplayName             nvarchar(200),
    Description             nvarchar(1000),
    Required                bit           not null,
    Emphasize               bit           not null,
    ShowInDiscoveryDocument bit           not null,
    Created                 datetime2     not null,
    Updated                 datetime2,
    NonEditable             bit           not null
)
go

create table IdentityClaims
(
    Id                 int identity
        constraint PK_IdentityClaims
            primary key,
    Type               nvarchar(200) not null,
    IdentityResourceId int           not null
        constraint FK_IdentityClaims_IdentityResources_IdentityResourceId
            references IdentityResources
            on delete cascade
)
go

create index IX_IdentityClaims_IdentityResourceId
    on IdentityClaims (IdentityResourceId)
go

create table IdentityProperties
(
    Id                 int identity
        constraint PK_IdentityProperties
            primary key,
    [Key]              nvarchar(250)  not null,
    Value              nvarchar(2000) not null,
    IdentityResourceId int            not null
        constraint FK_IdentityProperties_IdentityResources_IdentityResourceId
            references IdentityResources
            on delete cascade
)
go

create index IX_IdentityProperties_IdentityResourceId
    on IdentityProperties (IdentityResourceId)
go

create unique index IX_IdentityResources_Name
    on IdentityResources (Name)
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

INSERT INTO ApiResources (Id, Enabled, Name, DisplayName, Description, Created, Updated, LastAccessed, NonEditable) VALUES (1, 1, 'PetShop.Api', 'PetShop.Api', null, '2020-03-09 15:07:41.4854117', null, null, 0);
INSERT INTO ApiScopes (Id, Name, DisplayName, Description, Required, Emphasize, ShowInDiscoveryDocument, ApiResourceId) VALUES (1, 'PetShop.Api', 'PetShop.Api', null, 0, 0, 1, 1);
INSERT INTO ClientGrantTypes (Id, GrantType, ClientId) VALUES (2, 'authorization_code', 2);
INSERT INTO ClientPostLogoutRedirectUris (Id, PostLogoutRedirectUri, ClientId) VALUES (1, '{signout-callback-url}', 2);
INSERT INTO ClientRedirectUris (Id, RedirectUri, ClientId) VALUES (1, '{signin-url}', 2);
INSERT INTO ClientScopes (Id, Scope, ClientId) VALUES (2, 'openid', 2);
INSERT INTO ClientScopes (Id, Scope, ClientId) VALUES (3, 'profile', 2);
INSERT INTO ClientScopes (Id, Scope, ClientId) VALUES (4, 'PetShop.Api', 2);
INSERT INTO ClientScopes (Id, Scope, ClientId) VALUES (5, 'email', 2);
INSERT INTO ClientSecrets (Id, Description, Value, Expiration, Type, Created, ClientId) VALUES (2, null, 'K7gNU3sdo+OL0wNhqoVWhr3g6s1xYv72ol/pe/Unols=', null, 'SharedSecret', '2020-03-09 15:07:41.1667665', 2);
-- secret = secret
INSERT INTO Clients (Id, Enabled, ClientId, ProtocolType, RequireClientSecret, ClientName, Description, ClientUri, LogoUri, RequireConsent, AllowRememberConsent, AlwaysIncludeUserClaimsInIdToken, RequirePkce, AllowPlainTextPkce, AllowAccessTokensViaBrowser, FrontChannelLogoutUri, FrontChannelLogoutSessionRequired, BackChannelLogoutUri, BackChannelLogoutSessionRequired, AllowOfflineAccess, IdentityTokenLifetime, AccessTokenLifetime, AuthorizationCodeLifetime, ConsentLifetime, AbsoluteRefreshTokenLifetime, SlidingRefreshTokenLifetime, RefreshTokenUsage, UpdateAccessTokenClaimsOnRefresh, RefreshTokenExpiration, AccessTokenType, EnableLocalLogin, IncludeJwtId, AlwaysSendClientClaims, ClientClaimsPrefix, PairWiseSubjectSalt, Created, Updated, LastAccessed, UserSsoLifetime, UserCodeType, DeviceCodeLifetime, NonEditable) VALUES (2, 1, 'PetShop.Spa', 'oidc', 0, null, null, null, null, 0, 1, 0, 1, 0, 0, null, 1, null, 1, 1, 300, 3600, 300, null, 2592000, 1296000, 1, 0, 1, 0, 1, 0, 0, 'client_', null, '2020-03-09 15:07:41.1667622', null, null, null, null, 300, 0);
INSERT INTO IdentityClaims (Id, Type, IdentityResourceId) VALUES (1, 'website', 1);
INSERT INTO IdentityClaims (Id, Type, IdentityResourceId) VALUES (2, 'picture', 1);
INSERT INTO IdentityClaims (Id, Type, IdentityResourceId) VALUES (3, 'profile', 1);
INSERT INTO IdentityClaims (Id, Type, IdentityResourceId) VALUES (4, 'preferred_username', 1);
INSERT INTO IdentityClaims (Id, Type, IdentityResourceId) VALUES (5, 'nickname', 1);
INSERT INTO IdentityClaims (Id, Type, IdentityResourceId) VALUES (6, 'middle_name', 1);
INSERT INTO IdentityClaims (Id, Type, IdentityResourceId) VALUES (7, 'given_name', 1);
INSERT INTO IdentityClaims (Id, Type, IdentityResourceId) VALUES (8, 'family_name', 1);
INSERT INTO IdentityClaims (Id, Type, IdentityResourceId) VALUES (9, 'name', 1);
INSERT INTO IdentityClaims (Id, Type, IdentityResourceId) VALUES (10, 'gender', 1);
INSERT INTO IdentityClaims (Id, Type, IdentityResourceId) VALUES (11, 'birthdate', 1);
INSERT INTO IdentityClaims (Id, Type, IdentityResourceId) VALUES (12, 'zoneinfo', 1);
INSERT INTO IdentityClaims (Id, Type, IdentityResourceId) VALUES (13, 'locale', 1);
INSERT INTO IdentityClaims (Id, Type, IdentityResourceId) VALUES (14, 'updated_at', 1);
INSERT INTO IdentityClaims (Id, Type, IdentityResourceId) VALUES (15, 'sub', 2);
INSERT INTO IdentityResources (Id, Enabled, Name, DisplayName, Description, Required, Emphasize, ShowInDiscoveryDocument, Created, Updated, NonEditable) VALUES (1, 1, 'profile', 'User profile', 'Your user profile information (first name, last name, etc.)', 0, 1, 1, '2020-03-09 15:07:41.4238252', null, 0);
INSERT INTO IdentityResources (Id, Enabled, Name, DisplayName, Description, Required, Emphasize, ShowInDiscoveryDocument, Created, Updated, NonEditable) VALUES (2, 1, 'openid', 'Your user identifier', null, 1, 0, 1, '2020-03-09 15:07:41.3979692', null, 0);