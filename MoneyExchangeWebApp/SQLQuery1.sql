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
DROP TABLE IF EXISTS Employees;
--
-- Create tables
--

CREATE TABLE Accounts(
	account_id		INT				IDENTITY PRIMARY KEY,
	username 		VARCHAR(32) 	NOT NULL,
	password 		VARBINARY(32) 	NOT NULL,
	name 			VARCHAR(32) 	NOT NULL,
	role 			VARCHAR(32) 	NOT NULL,
	date_created	DATE			NOT NULL,
	deleted			BIT				NOT NULL,
    deleted_by		VARCHAR(32)		NULL
);
SET IDENTITY_INSERT Accounts ON;
INSERT INTO Accounts(account_id, username, password, name, role, deleted, deleted_by) VALUES 
(01,'john123', HASHBYTES('SHA1', 'password1'), 'John Wick', 'admin', '2012-08-12',0, null),
(02,'jam123', HASHBYTES('SHA1', 'password2'), 'James Charles', 'staff', '2013-05-02', 0, null),
(03,'char123', HASHBYTES('SHA1', 'password3'), 'Charlene Lim', 'staff', '2015-05-04', 0, null),
(04,'Kanye123', HASHBYTES('SHA1', 'password3'), 'Kanye West', 'staff', '2019-05-04', 1, 'John Wick');
SET IDENTITY_INSERT Accounts OFF;

CREATE TABLE Employees(
	Employee_id		INT				IDENTITY PRIMARY KEY,
	name 			VARCHAR(32) 	NOT NULL,
	gender			VARCHAR(32)		NOT NULL,
	dob				DATE 			NOT NULL,
	deleted			BIT				NOT NULL,
    deleted_by		VARCHAR(32)		NULL
	);

SET IDENTITY_INSERT Employees ON;
INSERT INTO Employees(Employee_id, name, dob, gender, deleted, deleted_by) VALUES 
(01,'John Wick', '1967-06-01', 'Male' ,0, null),
(02, 'James Charles','1968-05-02','Unspecified', 0, null),
(03,'Charlene Lim', '1970-05-04','Female', 0, null),
(04,'Kanye West','1970-05-04','Male' ,1, 'John Wick');
SET IDENTITY_INSERT Employees OFF;

CREATE TABLE Enquiries(
	enquiry_id 				INT 			IDENTITY PRIMARY KEY,
	visitor_email_address	VARCHAR(100) 	NOT NULL,
	description 			VARCHAR(100) 	NOT NULL,
	enquiry_date 			DATE 			NOT NULL,
	status 					BIT 			NOT NULL,
	answered_by				VARCHAR(32)  	NULL,
	deleted					BIT				NOT NULL,
    deleted_by				VARCHAR(32)		NULL
);
SET IDENTITY_INSERT Enquiries ON;
INSERT INTO Enquiries(enquiry_id, visitor_email_address, description, enquiry_date, status, answered_by, deleted, deleted_by) VALUES
(01, 'thunderblades48@gmail.com' ,'How much money can I convert in one transaction?', '2020-11-16', 1, 'jam123', 1, 'john123'),
(02, 'K.artixc@gmail.com' ,'Are the exchange rates updated regularly', '2020-11-18', 1, 'john123', 0, null),
(03, 'K.artixc@gmail.com' ,'How many currencies do you offer for conversion', '2021-01-15', 0, null, 0, null),
(04, 'karthikeyansuresh7@gmail.com' ,'What is the currency exchange rate between SGD and MMK', '2021-11-18', 0, null, 0, null);
SET IDENTITY_INSERT Enquiries OFF;

CREATE TABLE FAQ(
	FAQ_ID			INT	 			IDENTITY PRIMARY KEY,
	Question 		VARCHAR(200)	NOT NULL,
	Answer			VARCHAR(200) 	NOT NULL,
    created_by		VARCHAR(32)  	NOT NULL,
	deleted			BIT				NOT NULL,
    deleted_by		VARCHAR(32)		NULL
	);
    
    SET IDENTITY_INSERT FAQ ON;
    INSERT INTO FAQ(FAQ_ID, Question, Answer, created_by, deleted, deleted_by) VALUES
    (01, 'What is the maximum amount that we can exchange per transaction?', 'There is no fixed price!, but do note if we do not have enough, we will take 3 working days to get back to you', 'John Wick', 0, null),
	(02, 'How many currencies do you have?', 'We hold a variety of currencies with their respective exchange rates. Most are from ASEAN countries only. We apologise for any inconvenience', 'John Wick', 1, 'John Wick'),
	(03, 'Do you have an exchange rate fee?', 'Yes! It is only 3% per transaction', 'John Wick', 0, null);
	 SET IDENTITY_INSERT FAQ OFF;
     
CREATE TABLE Currency (
    Currency_name 	VARCHAR(5) 		PRIMARY KEY,
    Country 		VARCHAR(100) 	NOT NULL,
    created_by		VARCHAR(32)  	NULL,
    deleted 		BIT 			NOT NULL
);
INSERT INTO Currency(currency_name, Country, created_by, deleted) VALUES
('SGD', 'Singapore', 'John Wick', 0), ('MMK', 'Myanmar', 'John Wick', 0), ('CNY', 'China', 'John Wick', 0), ('MYR', 'Malaysia', 'John Wick', 0);


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
	  REFERENCES Currency(Currency_name)
);

SET IDENTITY_INSERT Transactions ON;
INSERT INTO Transactions(Transaction_id, Source_currency, Source_amount, Converted_currency, Converted_amount, exchange_rate, Transaction_date, created_by, deleted, deleted_by) VALUES
(01, 'SGD', 10.00, 'MYR', 30.77, 3.08, '2021-11-16', 'John Wick', 0, null),
(02, 'SGD', 1200000.00, 'CNY', 5662411.20, 4.72, '2021-11-15', 'James Charles' ,0, null), 
(03, 'SGD', 123.00, 'MMK', 160774.14, 1307.11, '2021-11-14', 'Charlene Lim', 0, null),
(04, 'SGD', 120.00, 'MMK', 156853.20, 1307.11, '2021-01-14', 'Charlene Lim', 1, 'Kanye West');
SET IDENTITY_INSERT Transactions OFF;

CREATE TABLE Stock(
	Stock_id		INT         	IDENTITY PRIMARY KEY,
	Currency_name	VARCHAR(5)		NOT NULL,
	Currency_stock	DECIMAL(9,2)	NOT NULL,
	Average_Rate	DECIMAL(9,2)	NOT NULL,
	deleted			BIT				NOT NULL,
    deleted_by		VARCHAR(32)		NULL
);
SET IDENTITY_INSERT Stock ON;
INSERT INTO Stock(Stock_id, Currency_name, Currency_stock, Average_Rate, deleted, deleted_by) VALUES
(01, 'SGD', 100000.00, 1.00, 0, null),
(02, 'MMK', 112345.00, 4.72, 0, null),
(03, 'CNY', 12345.00, 1307.11, 0, null),
(04, 'MYR', 98760.00, 3.08, 0, null);
SET IDENTITY_INSERT Stock OFF;