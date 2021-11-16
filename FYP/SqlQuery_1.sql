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
(01, 'john123', 'abc123456789', 'John', 'admin', '1967-06-01'),
(02, 'james321', 'abcd1234', 'James', 'staff', '1968-05-02'),
(03, 'charlene321', 'abcd12345', 'Charlene', 'staff', '1970-05-04');

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
INSERT INTO Currency(currency_name) VALUES
('SGD'), ('MMK'), ('CNY'), ('MYR');


CREATE TABLE ExchangeRates( 
		Source_currency CHAR(3)      NOT NULL,
    	Target_currency CHAR(3)      NOT NULL,
    	Exchange_rate   DECIMAL(9,2) NOT NULL,
	CONSTRAINT FK2 FOREIGN KEY(Source_currency) 
      REFERENCES Currency(Currency_name),
	CONSTRAINT FK3 FOREIGN KEY(Target_currency)
      REFERENCES Currency(Currency_name)
);
INSERT INTO ExchangeRates(Source_currency, Target_currency, Exchange_rate) VALUES
('SGD', 'MYR', 3.08), 
('SGD', 'CNY', 4.72), 
('SGD', 'MMK', 1307.11); 

CREATE TABLE Transactions(
	Transaction_id 	      INT 	    IDENTITY PRIMARY KEY,
	Source_currency	      CHAR(3)       NOT NULL,
	Source_amount 	      DECIMAL(9,2)  NOT NULL,
	Converted_currency    CHAR(3)	    NOT NULL,
	Converted_amount      DECIMAL(9,2)  NOT NULL,
	Exchange_rate		  DECIMAL(9,2)  NOT NULL,
	Transaction_date      DATE 			NOT NULL,
	CONSTRAINT FK4 FOREIGN KEY(Source_currency) 
      REFERENCES Currency(Currency_name),
	CONSTRAINT FK5 FOREIGN KEY(Converted_currency)
      REFERENCES Currency(Currency_name)
);

SET IDENTITY_INSERT Transactions ON;
INSERT INTO Transactions(Transaction_id, Source_currency, Source_amount, Converted_currency, Converted_amount, exchange_rate, Transaction_date) VALUES
(1, 'SGD', 10.00, 'MYR', 30.77, 3.08, '2021-11-16'),
(2, 'SGD', 1200000.00, 'CNY', 5662411.20, 4.72, '2021-11-16'), 
(3, 'SGD', 123.00, 'MMK', 160774.14, 1307.11, '2021-11-16');
SET IDENTITY_INSERT Transactions OFF

CREATE TABLE Stock(
	Stock_id		INT         IDENTITY PRIMARY KEY,
	Currency_name	CHAR(3)		NOT NULL,
	Currency_stock	DECIMAL(9,2)	NOT NULL,
	CONSTRAINT FK6 FOREIGN KEY(Currency_name)	
      REFERENCES Currency(Currency_name) 
);
SET IDENTITY_INSERT Stock ON;
INSERT INTO Stock(Stock_id, Currency_name, Currency_stock) VALUES
(1, 'SGD', 100000.00),
(2, 'MMK', 112345.00),
(3, 'CNY', 12345.00),
(4, 'MYR', 98760.00);
SET IDENTITY_INSERT Stock OFF;
