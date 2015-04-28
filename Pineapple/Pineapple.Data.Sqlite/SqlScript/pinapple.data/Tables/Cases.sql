Create table if not exists Cases
(
    CaseId		    INTEGER primary key AUTOINCREMENT,
    Title	        TEXT,
    SubTitle        TEXT,
    CreateDate      TEXT,
    Description     TEXT,
    TimeInMs        INTEGER,
	DisplayOrder	INTEGER
)