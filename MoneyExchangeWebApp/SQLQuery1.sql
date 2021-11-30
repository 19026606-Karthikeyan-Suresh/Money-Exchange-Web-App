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
	Account_id		INT				IDENTITY PRIMARY KEY,
	Username 		VARCHAR(32) 	NOT NULL,
	Password 		VARBINARY(200) 	NOT NULL,
	Name 			VARCHAR(32) 	NOT NULL,
	Role 			VARCHAR(32) 	NOT NULL,
	Date_created	DATE			NOT NULL,
	Deleted			BIT				NOT NULL,
    Deleted_by		VARCHAR(32)		NULL
);
SET IDENTITY_INSERT Accounts ON;
INSERT INTO Accounts(Account_id, Username, Password, Name, Role, Date_created, Deleted, Deleted_by) VALUES 
(01,'john123', HASHBYTES('SHA1', 'password1'), 'John Wick', 'admin', '2012-08-12',0, null),
(02,'jam123', HASHBYTES('SHA1', 'password2'), 'James Charles', 'staff', '2013-05-02', 0, null),
(03,'char123', HASHBYTES('SHA1', 'password3'), 'Charlene Lim', 'staff', '2015-05-04', 0, null),
(04,'Kanye123', HASHBYTES('SHA1', 'password3'), 'Kanye West', 'staff', '2019-05-04', 1, 'John Wick');
SET IDENTITY_INSERT Accounts OFF;

CREATE TABLE Enquiries(
	Enquiry_id 				INT 			IDENTITY PRIMARY KEY,
	Visitor_email_address	VARCHAR(100) 	NOT NULL,
	Description 			VARCHAR(100) 	NOT NULL,
	Enquiry_date 			DATE 			NOT NULL,
	Status 					BIT 			NOT NULL,
	Answered_by				VARCHAR(32)  	NULL,
	Deleted					BIT				NOT NULL,
    Deleted_by				VARCHAR(32)		NULL
);
SET IDENTITY_INSERT Enquiries ON;
INSERT INTO Enquiries(Enquiry_id, Visitor_email_address, Description, Enquiry_date, Status, Answered_by, Deleted, Deleted_by) VALUES
(01, 'thunderblades48@gmail.com' ,'How much money can I convert in one transaction?', '2020-11-16', 1, 'jam123', 1, 'john123'),
(02, 'K.artixc@gmail.com' ,'Are the exchange rates updated regularly', '2020-11-18', 1, 'john123', 0, null),
(03, 'K.artixc@gmail.com' ,'How many currencies do you offer for conversion', '2021-01-15', 0, null, 0, null),
(04, 'karthikeyansuresh7@gmail.com' ,'What is the currency exchange rate between SGD and MMK', '2021-11-18', 0, null, 0, null);
SET IDENTITY_INSERT Enquiries OFF;

CREATE TABLE FAQ(
	FAQ_ID			INT	 			IDENTITY PRIMARY KEY,
	Question 		VARCHAR(200)	NOT NULL,
	Answer			VARCHAR(200) 	NOT NULL,
    Created_by		VARCHAR(32)  	NOT NULL,
	Deleted			BIT				NOT NULL,
    Deleted_by		VARCHAR(32)		NULL
	);
    
    SET IDENTITY_INSERT FAQ ON;
    INSERT INTO FAQ(FAQ_ID, Question, Answer, Created_by, Deleted, Deleted_by) VALUES
    (01, 'What is the maximum amount that we can exchange per transaction?', 'There is no fixed price!, but do note if we do not have enough, we will take 3 working days to get back to you', 'John Wick', 0, null),
	(02, 'How many currencies do you have?', 'We hold a variety of currencies with their respective exchange rates. Most are from ASEAN countries only. We apologise for any inconvenience', 'John Wick', 1, 'John Wick'),
	(03, 'Do you have an exchange rate fee?', 'Yes! It is only 3% per transaction', 'John Wick', 0, null);
	 SET IDENTITY_INSERT FAQ OFF;
     
CREATE TABLE Currency (
    Currency_name 	VARCHAR(5) 		PRIMARY KEY,
    Country 		VARCHAR(100) 	NOT NULL,
	Average_rate	DECIMAL(9,4)	NOT NULL,
    Created_by		VARCHAR(32)  	NULL,
    Deleted 		BIT 			NOT NULL,
	Deleted_by		VARCHAR(32)		NULL
);
INSERT INTO Currency(Currency_name, Country, Created_by, Deleted, Deleted_by) VALUES
('SGD', 'Singapore',1.0 ,'John Wick', 0, null),
('MMK', 'Myanmar', 1,315.5, 'John Wick', 0, null),
('CNY', 'China', 4.6614, 'John Wick', 0, null),
('MYR', 'Malaysia', 0.33, 'John Wick', 0, null);


CREATE TABLE ExchangeRates( 
		Source_currency 	VARCHAR(5)      NOT NULL,
    	Target_currency 	VARCHAR(5)      NOT NULL,
    	Exchange_rate   	DECIMAL(9,4) 	NOT NULL
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
	Source_amount 	      DECIMAL(9,4)  NOT NULL,
	Converted_currency    VARCHAR(5)	NOT NULL,
	Converted_amount      DECIMAL(9,4)  NOT NULL,
	Exchange_rate		  DECIMAL(9,4)  NOT NULL,
	Transaction_date      DATE 			NOT NULL,
    Created_by			  VARCHAR(32)	NOT NULL,
	Deleted				  BIT			NOT NULL,
    Deleted_by			  VARCHAR(32) 	NULL
    
	CONSTRAINT FK3 FOREIGN KEY(Source_currency) 
      REFERENCES Currency(Currency_name),
	CONSTRAINT FK4 FOREIGN KEY(Converted_currency)
	  REFERENCES Currency(Currency_name)
);

SET IDENTITY_INSERT Transactions ON;
INSERT INTO Transactions(Transaction_id, Source_currency, Source_amount, Converted_currency, Converted_amount, Exchange_rate, Transaction_date, Created_by, Deleted, Deleted_by) VALUES
(01, 'SGD', 10.00, 'MYR', 30.77, 3.08, '2021-11-16', 'John Wick', 0, null),
(02, 'SGD', 1200000.00, 'CNY', 5662411.20, 4.72, '2021-11-15', 'James Charles' ,0, null), 
(03, 'SGD', 123.00, 'MMK', 160774.14, 1307.11, '2021-11-14', 'Charlene Lim', 0, null),
(04, 'SGD', 120.00, 'MMK', 156853.20, 1307.11, '2021-01-14', 'Charlene Lim', 1, 'Kanye West');
SET IDENTITY_INSERT Transactions OFF;

CREATE TABLE Stock(
	Stock_id		INT         	IDENTITY PRIMARY KEY,
	Stock_name		VARCHAR(5)		NOT NULL,
	Stock_amount	DECIMAL(9,4)	NOT NULL,
	Average_rate	DECIMAL(9,4)	NOT NULL,
	Deleted			BIT				NOT NULL,
    Deleted_by		VARCHAR(32)		NULL
	CONSTRAINT FK5 FOREIGN KEY(Stock_name) 
      REFERENCES Currency(Currency_name)
);
SET IDENTITY_INSERT Stock ON;
INSERT INTO Stock(Stock_id, Stock_name, Stock_amount, Average_rate, Deleted, Deleted_by) VALUES
(01, 'SGD', 100000.00, 1.0 ,0, null),
(02, 'MMK', 112345.00, 1315.5, null),
(03, 'CNY', 12345.00, 0, 4.6614 ,null),
(04, 'MYR', 98760.00, 0, 0.33,null);
SET IDENTITY_INSERT Stock OFF;