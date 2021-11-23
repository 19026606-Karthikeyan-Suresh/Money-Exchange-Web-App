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
	account_id 	INT 		IDENTITY PRIMARY KEY,
	username 	VARCHAR(32) 	NOT NULL,
	password 	VARBINARY(32) 	NOT NULL,
	name 		VARCHAR(32) 	NOT NULL,
	role 		VARCHAR(32) 	NOT NULL,
	dob		DATE 		NOT NULL
);
SET IDENTITY_INSERT Accounts ON;
INSERT INTO Accounts(account_id, username, password, name, role, dob) VALUES 
(01, 'john', HASHBYTES('SHA1', 'password1'), 'John', 'admin', '1967-06-01'),
(02, 'jam', HASHBYTES('SHA1', 'password2'), 'James', 'staff', '1968-05-02'),
(03, 'char', HASHBYTES('SHA1', 'password3'), 'Charlene', 'staff', '1970-05-04');
SET IDENTITY_INSERT Accounts OFF;

CREATE TABLE Enquiries(
	enquiry_id 	INT 	PRIMARY KEY,
	description VARCHAR (100) 	NOT NULL,
	enquiry_date DATE 		NOT NULL,
	status 	bit 	NOT NULL,
	account_id 	INT 		NULL,
	CONSTRAINT FKa1 FOREIGN KEY(account_id) 
      REFERENCES Accounts(account_id) 
);

CREATE TABLE Currency(
	Currency_name  VARCHAR(5)  PRIMARY KEY,
	Country VARCHAR(100) NOT NULL
);
INSERT INTO Currency(currency_name, Country) VALUES
('SGD', 'Singapore'), ('MMK', 'Myanmar'), ('CNY', 'China'), ('MYR', 'Malaysia');


CREATE TABLE ExchangeRates( 
		Source_currency VARCHAR(5)      NOT NULL,
    	Target_currency VARCHAR(5)      NOT NULL,
    	Exchange_rate   DECIMAL(9,2) NOT NULL,
	CONSTRAINT FKa2 FOREIGN KEY(Source_currency) 
      REFERENCES Currency(Currency_name),
	CONSTRAINT FKa3 FOREIGN KEY(Target_currency)
      REFERENCES Currency(Currency_name)
);
INSERT INTO ExchangeRates(Source_currency, Target_currency, Exchange_rate) VALUES
('SGD', 'MYR', 3.08), 
('SGD', 'CNY', 4.72), 
('SGD', 'MMK', 1307.11); 

CREATE TABLE Transactions(
	Transaction_id 	      INT 	    IDENTITY PRIMARY KEY,
	Source_currency	      VARCHAR(5)       NOT NULL,
	Source_amount 	      DECIMAL(9,2)  NOT NULL,
	Converted_currency    VARCHAR(5)	    NOT NULL,
	Converted_amount      DECIMAL(9,2)  NOT NULL,
	Exchange_rate		  DECIMAL(9,2)  NOT NULL,
	Transaction_date      DATE 			NOT NULL,
	CONSTRAINT FKa4 FOREIGN KEY(Source_currency) 
      REFERENCES Currency(Currency_name),
	CONSTRAINT FKa5 FOREIGN KEY(Converted_currency)
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
	Currency_name	VARCHAR(5)		NOT NULL,
	Currency_stock	DECIMAL(9,2)	NOT NULL,
	Average_Rate	DECIMAL(9,2)	NOT NULL,
	CONSTRAINT FKa6 FOREIGN KEY(Currency_name)	
      REFERENCES Currency(Currency_name) 
);
SET IDENTITY_INSERT Stock ON;
INSERT INTO Stock(Stock_id, Currency_name, Currency_stock, Average_Rate) VALUES
(1, 'SGD', 100000.00, 1.00),
(2, 'MMK', 112345.00, 4.72),
(3, 'CNY', 12345.00, 1307.11),
(4, 'MYR', 98760.00, 3.08);
SET IDENTITY_INSERT Stock OFF;
