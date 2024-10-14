DROP USER [func-nhse-lh-dev-001];
CREATE USER [func-nhse-lh-dev-001] FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER [func-nhse-lh-dev-001];
ALTER ROLE db_datawriter ADD MEMBER [func-nhse-lh-dev-001];

DROP USER [webapp-nhse-lhg-dev-001];
CREATE USER [webapp-nhse-lhg-dev-001] FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER [webapp-nhse-lhg-dev-001];
ALTER ROLE db_datawriter ADD MEMBER [webapp-nhse-lhg-dev-001];

DROP USER [webapp-nhse-lg-dev-001];
CREATE USER [webapp-nhse-lg-dev-001] FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER [webapp-nhse-lg-dev-001];