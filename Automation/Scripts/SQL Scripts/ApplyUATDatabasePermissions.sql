CREATE USER [auto-logicapp-nhse-ipls-uat-01]
FROM
EXTERNAL
PROVIDER;

ALTER
ROLE db_datareader
ADD MEMBER [auto-logicapp-nhse-ipls-uat-01];

ALTER
ROLE db_datawriter
ADD MEMBER [auto-logicapp-nhse-ipls-uat-01];

CREATE USER [auto-logicapp-nhse-ipls-uat-02]
FROM
EXTERNAL
PROVIDER;

ALTER
ROLE db_datareader
ADD MEMBER [auto-logicapp-nhse-ipls-uat-02];

ALTER
ROLE db_datawriter
ADD MEMBER [auto-logicapp-nhse-ipls-uat-02];

CREATE USER [auto-logicapp-nhse-ipls-uat-03]
FROM
EXTERNAL
PROVIDER;

ALTER
ROLE db_datareader
ADD MEMBER [auto-logicapp-nhse-ipls-uat-03];

ALTER
ROLE db_datawriter
ADD MEMBER [auto-logicapp-nhse-ipls-uat-03];

CREATE USER [auto-logicapp-nhse-ipls-uat-04]
FROM
EXTERNAL
PROVIDER;

ALTER
ROLE db_datareader
ADD MEMBER [auto-logicapp-nhse-ipls-uat-04];

ALTER
ROLE db_datawriter
ADD MEMBER [auto-logicapp-nhse-ipls-uat-04];

CREATE USER [auto-logicapp-nhse-ipls-uat-05]
FROM
EXTERNAL
PROVIDER;

ALTER
ROLE db_datareader
ADD MEMBER [auto-logicapp-nhse-ipls-uat-05];

ALTER
ROLE db_datawriter
ADD MEMBER [auto-logicapp-nhse-ipls-uat-05];

CREATE USER [auto-logicapp-nhse-ipls-uat-06]
FROM
EXTERNAL
PROVIDER;

ALTER
ROLE db_datareader
ADD MEMBER [auto-logicapp-nhse-ipls-uat-06];

ALTER
ROLE db_datawriter
ADD MEMBER [auto-logicapp-nhse-ipls-uat-06];

CREATE USER [auto-logicapp-nhse-ipls-uat-07]
FROM
EXTERNAL
PROVIDER;

ALTER
ROLE db_datareader
ADD MEMBER [auto-logicapp-nhse-ipls-uat-07];

ALTER
ROLE db_datawriter
ADD MEMBER [auto-logicapp-nhse-ipls-uat-07];

CREATE USER [auto-logicapp-nhse-ipls-uat-08]
FROM
EXTERNAL
PROVIDER;

ALTER
ROLE db_datareader
ADD MEMBER [auto-logicapp-nhse-ipls-uat-08];

ALTER
ROLE db_datawriter
ADD MEMBER [auto-logicapp-nhse-ipls-uat-08];

CREATE USER [auto-logicapp-nhse-ipls-uat-09]
FROM
EXTERNAL
PROVIDER;

ALTER
ROLE db_datareader
ADD MEMBER [auto-logicapp-nhse-ipls-uat-09];

ALTER
ROLE db_datawriter
ADD MEMBER [auto-logicapp-nhse-ipls-uat-09];

CREATE USER [auto-logicapp-nhse-ipls-uat-10]
FROM
EXTERNAL
PROVIDER;

ALTER
ROLE db_datareader
ADD MEMBER [auto-logicapp-nhse-ipls-uat-10];

ALTER
ROLE db_datawriter
ADD MEMBER [auto-logicapp-nhse-ipls-uat-10];

CREATE USER [auto-logicapp-nhse-ipls-uat-11]
FROM
EXTERNAL
PROVIDER;

ALTER
ROLE db_datareader
ADD MEMBER [auto-logicapp-nhse-ipls-uat-11];

ALTER
ROLE db_datawriter
ADD MEMBER [auto-logicapp-nhse-ipls-uat-11];

CREATE USER [auto-logicapp-nhse-ipls-uat-12]
FROM
EXTERNAL
PROVIDER;

ALTER
ROLE db_datareader
ADD MEMBER [auto-logicapp-nhse-ipls-uat-12];

ALTER
ROLE db_datawriter
ADD MEMBER [auto-logicapp-nhse-ipls-uat-12];

CREATE USER [auto-logicapp-nhse-ipls-uat-13]
FROM
EXTERNAL
PROVIDER;

ALTER
ROLE db_datareader
ADD MEMBER [auto-logicapp-nhse-ipls-uat-13];

ALTER
ROLE db_datawriter
ADD MEMBER [auto-logicapp-nhse-ipls-uat-13];

CREATE USER [auto-logicapp-nhse-ipls-uat-14]
FROM
EXTERNAL
PROVIDER;

ALTER
ROLE db_datareader
ADD MEMBER [auto-logicapp-nhse-ipls-uat-14];

ALTER
ROLE db_datawriter
ADD MEMBER [auto-logicapp-nhse-ipls-uat-14];

CREATE USER [auto-logicapp-nhse-ipls-uat-15]
FROM
EXTERNAL
PROVIDER;

ALTER
ROLE db_datareader
ADD MEMBER [auto-logicapp-nhse-ipls-uat-15];

ALTER
ROLE db_datawriter
ADD MEMBER [auto-logicapp-nhse-ipls-uat-15];

CREATE USER [IPLSService@england.nhs.uk]
FROM
EXTERNAL
PROVIDER;

ALTER
ROLE db_datareader
ADD MEMBER [IPLSService@england.nhs.uk];

ALTER
ROLE db_datawriter
ADD MEMBER [IPLSService@england.nhs.uk];

SELECT DISTINCT [name] FROM sysusers WHERE issqlrole = 0 and name not in('dbo','guest','INFORMATION_SCHEMA','sys')
