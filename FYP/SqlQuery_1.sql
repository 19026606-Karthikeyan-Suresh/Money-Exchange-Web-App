--
-- Drop tables if they exist to make script re-runnable
--
DROP TABLE IF EXISTS Transactions;
DROP TABLE IF EXISTS Enquiries;
DROP TABLE IF EXISTS Accounts;
DROP TABLE IF EXISTS Currency;
DROP TABLE IF EXISTS ExchangeRates;
DROP TABLE IF EXISTS Stock;
--
-- Create tables
--

CREATE TABLE Accounts(
	account_id 	INT 		PRIMARY KEY,
	username 	VARCHAR(32) 	NOT NULL,
	password 	VARCHAR(32) 	NOT NULL,
	name 		VARCHAR(32) 	NOT NULL,
	role 		VARCHAR(32) 	NOT NULL,
	dob		DATE 		NOT NULL
);

INSERT INTO Accounts(account_id, username, password, name, role, dob) VALUES 
(01, 'john123', 'abc123456789', 'John', 'admin', '1967-06-01');

CREATE TABLE Enquiries(
	enquiry_id 	INT 	PRIMARY KEY,
	description VARCHAR (100) 	NOT NULL,
	enquiry_date DATE 		NOT NULL,
	status 	bit 	NOT NULL,
	account_id 	INT 		NULL,
	CONSTRAINT FK1 FOREIGN KEY(account_id) 
      REFERENCES Accounts(account_id) 
);

CREATE TABLE Currency(
	Currency_name  CHAR(3)  PRIMARY KEY
);

CREATE TABLE ExchangeRates( 
	Source_currency CHAR(3)      NOT NULL,
    	Target_currency CHAR(3)      NOT NULL,
    	Exchange_rate   DECIMAL(9,2) NOT NULL,
);

CREATE TABLE Transactions(
	Transaction_id 	      INT 	    PRIMARY KEY,
	Source_currency	      CHAR(3)       NOT NULL,
	Source_amount 	      DECIMAL(9,2)  NOT NULL,
	Converted_currency    CHAR(3)	    NOT NULL,
	Converted_amount      DECIMAL(9,2)  NOT NULL,
	Transaction_date      DATE 	    NOT NULL,
	CONSTRAINT FK2 FOREIGN KEY(Source_currency) 
      REFERENCES Currency(Currency_name),
	CONSTRAINT FK3 FOREIGN KEY(Converted_currency)
      REFERENCES Currency(Currency_name)
);

CREATE TABLE Stock(
	Currency_name	CHAR(3)		PRIMARY KEY,
	Currency_stock	DECIMAL(9,2)	NOT NULL,
	CONSTRAINT FK4 FOREIGN KEY(Currency_name)	
      REFERENCES Currency(Currency_name) 
);

