DROP USER [func-nhse-lg-dev-001];
CREATE USER [func-nhse-lg-dev-001] FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER [func-nhse-lg-dev-001];
ALTER ROLE db_datawriter ADD MEMBER [func-nhse-lg-dev-001];

DROP USER [webapp-nhse-lg-dev-001];
CREATE USER [webapp-nhse-lg-dev-001] FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER [webapp-nhse-lg-dev-001];
ALTER ROLE db_datawriter ADD MEMBER [webapp-nhse-lg-dev-001];

CREATE USER [IPLSService@england.nhs.uk] FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER [IPLSService@england.nhs.uk];
ALTER ROLE db_datawriter ADD MEMBER [IPLSService@england.nhs.uk];

CREATE USER [auto-logicapp-nhse-ipls-dev-01] FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER [auto-logicapp-nhse-ipls-dev-01];
ALTER ROLE db_datawriter ADD MEMBER [auto-logicapp-nhse-ipls-dev-01];

CREATE USER [auto-logicapp-nhse-ipls-dev-02] FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER [auto-logicapp-nhse-ipls-dev-02];
ALTER ROLE db_datawriter ADD MEMBER [auto-logicapp-nhse-ipls-dev-02];

CREATE USER [auto-logicapp-nhse-ipls-dev-03] FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER [auto-logicapp-nhse-ipls-dev-03];
ALTER ROLE db_datawriter ADD MEMBER [auto-logicapp-nhse-ipls-dev-03];

CREATE USER [auto-logicapp-nhse-ipls-dev-04] FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER [auto-logicapp-nhse-ipls-dev-04];
ALTER ROLE db_datawriter ADD MEMBER [auto-logicapp-nhse-ipls-dev-04];

CREATE USER [auto-logicapp-nhse-ipls-dev-05] FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER [auto-logicapp-nhse-ipls-dev-05];
ALTER ROLE db_datawriter ADD MEMBER [auto-logicapp-nhse-ipls-dev-05];

CREATE USER [auto-logicapp-nhse-ipls-dev-06] FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER [auto-logicapp-nhse-ipls-dev-06];
ALTER ROLE db_datawriter ADD MEMBER [auto-logicapp-nhse-ipls-dev-06];

CREATE USER [auto-logicapp-nhse-ipls-dev-07] FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER [auto-logicapp-nhse-ipls-dev-07];
ALTER ROLE db_datawriter ADD MEMBER [auto-logicapp-nhse-ipls-dev-07];

CREATE USER [auto-logicapp-nhse-ipls-dev-08] FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER [auto-logicapp-nhse-ipls-dev-08];
ALTER ROLE db_datawriter ADD MEMBER [auto-logicapp-nhse-ipls-dev-08];

CREATE USER [auto-logicapp-nhse-ipls-dev-09] FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER [auto-logicapp-nhse-ipls-dev-09];
ALTER ROLE db_datawriter ADD MEMBER [auto-logicapp-nhse-ipls-dev-09];

CREATE USER [auto-logicapp-nhse-ipls-dev-10] FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER [auto-logicapp-nhse-ipls-dev-10];
ALTER ROLE db_datawriter ADD MEMBER [auto-logicapp-nhse-ipls-dev-10];

CREATE USER [auto-logicapp-nhse-ipls-dev-11] FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER [auto-logicapp-nhse-ipls-dev-11];
ALTER ROLE db_datawriter ADD MEMBER [auto-logicapp-nhse-ipls-dev-11];

CREATE USER [auto-logicapp-nhse-ipls-dev-12] FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER [auto-logicapp-nhse-ipls-dev-12];
ALTER ROLE db_datawriter ADD MEMBER [auto-logicapp-nhse-ipls-dev-12];

CREATE USER [auto-logicapp-nhse-ipls-dev-13] FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER [auto-logicapp-nhse-ipls-dev-13];
ALTER ROLE db_datawriter ADD MEMBER [auto-logicapp-nhse-ipls-dev-13];

CREATE USER [auto-logicapp-nhse-ipls-dev-14] FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER [auto-logicapp-nhse-ipls-dev-14];
ALTER ROLE db_datawriter ADD MEMBER [auto-logicapp-nhse-ipls-dev-14];

CREATE USER [auto-logicapp-nhse-ipls-dev-15] FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER [auto-logicapp-nhse-ipls-dev-15];
ALTER ROLE db_datawriter ADD MEMBER [auto-logicapp-nhse-ipls-dev-15];