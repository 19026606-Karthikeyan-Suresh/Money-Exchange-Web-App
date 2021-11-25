--
-- Drop tables if they exist to make script re-runnable
--
DROP TABLE IF EXISTS Transactions;
DROP TABLE IF EXISTS Enquiries;
DROP TABLE IF EXISTS Accounts;
DROP TABLE IF EXISTS Currency;
DROP TABLE IF EXISTS ExchangeRates;
DROP TABLE IF EXISTS FAQ;
DROP TABLE IF EXISTS Stock;
--
-- Create tables
--

CREATE TABLE Accounts(
	username 		VARCHAR(32) 	PRIMARY KEY,
	password 		VARBINARY(32) 	NOT NULL,
	name 			VARCHAR(32) 	NOT NULL,
	role 			VARCHAR(32) 	NOT NULL,
	dob				DATE 			NOT NULL,
	deleted			BIT				NOT NULL,
    deleted_by		VARCHAR(32)		NULL
);
INSERT INTO Accounts(username, password, name, role, dob, deleted, deleted_by) VALUES 
('john123', HASHBYTES('SHA1', 'password1'), 'John Wick', 'admin', '1967-06-01', 0, null),
('jam123', HASHBYTES('SHA1', 'password2'), 'James Charles', 'staff', '1968-05-02', 0, null),
('char123', HASHBYTES('SHA1', 'password3'), 'Charlene Lim', 'staff', '1970-05-04', 0, null);

CREATE TABLE Enquiries(
	enquiry_id 				INT 			IDENTITY PRIMARY KEY,
	visitor_email_address	VARCHAR(100) 	NOT NULL,
	description 			VARCHAR(100) 	NOT NULL,
	enquiry_date 			DATE 			NOT NULL,
	status 					BIT 			NOT NULL,
	answered_by				VARCHAR(32)  	NULL,
	deleted					BIT				NOT NULL,
    deleted_by				VARCHAR(32)		NULL
    
	CONSTRAINT FKa1 FOREIGN KEY(answered_by) 
      REFERENCES Accounts(username), 
	CONSTRAINT FKa2 FOREIGN KEY(deleted_by) 
      REFERENCES Accounts(username) 
);
SET IDENTITY_INSERT Enquiries ON;
INSERT INTO Enquiries(enquiry_id, visitor_email_address, description, enquiry_date, status, answered_by, deleted, deleted_by) VALUES
(1, 'thunderblades48@gmail.com' ,'How much money can I convert in one transaction?', '2020-11-16', 1, 'jam123', 1, 'john123'),
(2, 'K.artixc@gmail.com' ,'Are the exchange rates updated regularly', '2020-11-18', 1, 'john123', 0, null),
(3, 'K.artixc@gmail.com' ,'How many currencies do you offer for conversion', '2021-01-15', 0, null, 0, null),
(4, 'karthikeyansuresh7@gmail.com' ,'What is the currency exchange rate between SGD and MMK', '2021-11-18', 0, null, 0, null);
SET IDENTITY_INSERT Enquiries OFF

CREATE TABLE FAQ(
	FAQ_ID			INT	 			IDENTITY PRIMARY KEY,
	Question 		VARCHAR(200)	NOT NULL,
	Answer			VARCHAR(200) 	NOT NULL,
    created_by		VARCHAR(32)  	NOT NULL,
	deleted			BIT				NOT NULL,
    deleted_by		VARCHAR(32)		NULL
    
	CONSTRAINT FKa3 FOREIGN KEY(created_by) 
      REFERENCES Accounts(username),
	CONSTRAINT FKa4 FOREIGN KEY(deleted_by) 
      REFERENCES Accounts(username) 
	);
    
    SET IDENTITY_INSERT FAQ ON;
    INSERT INTO FAQ(FAQ_ID, Question, Answer, created_by, deleted, deleted_by) VALUES
    (1, 'What is the maximum amount that we can exchange per transaction?', 'There is no fixed price!, but do note, if we do not have enough, we will take 3 working days to get back to you', 'john123', 0, null),
	(2, 'How many currencies do you have?', 'We hold a variety of currencies with their respective exchange rates. Most are from ASEAN countries only. We apologise for any inconvenience', 'john123', 1, 'john123'),
	(3, 'Do you have an exchange rate fee?', 'Yes! It is only 3% per transaction', 'john123', 0, null);
	 SET IDENTITY_INSERT FAQ OFF;
     
CREATE TABLE Currency (
    Currency_name 	VARCHAR(5) 		PRIMARY KEY,
    Country 		VARCHAR(100) 	NOT NULL,
    created_by		VARCHAR(32)  	NULL,
    deleted 		BIT 			NOT NULL

	CONSTRAINT FKa5 FOREIGN KEY(created_by) 
      REFERENCES Accounts(username) 
);
INSERT INTO Currency(currency_name, Country, created_by, deleted) VALUES
('SGD', 'Singapore', 'john123', 0), ('MMK', 'Myanmar', 'john123', 0), ('CNY', 'China', 'john123', 0), ('MYR', 'Malaysia', 'john123', 0);


CREATE TABLE ExchangeRates( 
		Source_currency 	VARCHAR(5)      NOT NULL,
    	Target_currency 	VARCHAR(5)      NOT NULL,
    	Exchange_rate   	DECIMAL(9,2) 	NOT NULL
	CONSTRAINT FK1 FOREIGN KEY(Source_currency) 
      REFERENCES Currency(Currency_name),
	CONSTRAINT FK2 FOREIGN KEY(Target_currency)
      REFERENCES Currency(Currency_name)
);
INSERT INTO ExchangeRates(Source_currency, Target_currency, Exchange_rate) VALUES
('SGD', 'MYR', 3.08), 
('SGD', 'CNY', 4.72), 
('SGD', 'MMK', 1307.11); 

CREATE TABLE Transactions(
	Transaction_id 	      INT 	    	IDENTITY PRIMARY KEY,
	Source_currency	      VARCHAR(5)    NOT NULL,
	Source_amount 	      DECIMAL(9,2)  NOT NULL,
	Converted_currency    VARCHAR(5)	NOT NULL,
	Converted_amount      DECIMAL(9,2)  NOT NULL,
	Exchange_rate		  DECIMAL(9,2)  NOT NULL,
	Transaction_date      DATE 			NOT NULL,
    created_by			  VARCHAR(32)	NOT NULL,
	deleted				  BIT			NOT NULL,
    deleted_by			  VARCHAR(32) 	NULL
    
	CONSTRAINT FK3 FOREIGN KEY(Source_currency) 
      REFERENCES Currency(Currency_name),
	CONSTRAINT FK4 FOREIGN KEY(Converted_currency)
      REFERENCES Currency(Currency_name),
	CONSTRAINT FKa6 FOREIGN KEY(created_by) 
      REFERENCES Accounts(username),
	CONSTRAINT FKa7 FOREIGN KEY(deleted_by) 
      REFERENCES Accounts(username) 
);

SET IDENTITY_INSERT Transactions ON;
INSERT INTO Transactions(Transaction_id, Source_currency, Source_amount, Converted_currency, Converted_amount, exchange_rate, Transaction_date, created_by, deleted, deleted_by) VALUES
(1, 'SGD', 10.00, 'MYR', 30.77, 3.08, '2021-11-16', 'john123', 0, null),
(2, 'SGD', 1200000.00, 'CNY', 5662411.20, 4.72, '2021-11-15', 'jam123' ,0, null), 
(3, 'SGD', 123.00, 'MMK', 160774.14, 1307.11, '2021-11-14', 'char123', 0, null);
SET IDENTITY_INSERT Transactions OFF;

CREATE TABLE Stock(
	Stock_id		INT         	IDENTITY PRIMARY KEY,
	Currency_name	VARCHAR(5)		NOT NULL,
	Currency_stock	DECIMAL(9,2)	NOT NULL,
	Average_Rate	DECIMAL(9,2)	NOT NULL,
	deleted			BIT				NOT NULL,
    deleted_by		VARCHAR(32)		NULL
	CONSTRAINT FK5 FOREIGN KEY(Currency_name)	
      REFERENCES Currency(Currency_name), 
	CONSTRAINT FKa8 FOREIGN KEY(deleted_by) 
      REFERENCES Accounts(username) 
);
SET IDENTITY_INSERT Stock ON;
INSERT INTO Stock(Stock_id, Currency_name, Currency_stock, Average_Rate, deleted, deleted_by) VALUES
(1, 'SGD', 100000.00, 1.00, 0, null),
(2, 'MMK', 112345.00, 4.72, 0, null),
(3, 'CNY', 12345.00, 1307.11, 0, null),
(4, 'MYR', 98760.00, 3.08, 0, null);
SET IDENTITY_INSERT Stock OFF;