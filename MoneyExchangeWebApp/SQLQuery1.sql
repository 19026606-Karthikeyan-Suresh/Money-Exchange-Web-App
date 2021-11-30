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

INSERT INTO Accounts(Username, Password, Name, Role, Date_created, Deleted, Deleted_by) VALUES 
('john123', HASHBYTES('SHA1', 'password1'), 'John Wick', 'admin', '2012-08-12',0, null),
('jam123', HASHBYTES('SHA1', 'password2'), 'James Charles', 'staff', '2013-05-02', 0, null),
('char123', HASHBYTES('SHA1', 'password3'), 'Charlene Lim', 'staff', '2015-05-04', 0, null),
('Kanye123', HASHBYTES('SHA1', 'password3'), 'Kanye West', 'staff', '2019-05-04', 1, 'John Wick');

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
INSERT INTO Enquiries(Visitor_email_address, Description, Enquiry_date, Status, Answered_by, Deleted, Deleted_by) VALUES
('thunderblades48@gmail.com' ,'How much money can I convert in one transaction?', '2020-11-16', 1, 'jam123', 1, 'john123'),
('K.artixc@gmail.com' ,'Are the exchange rates updated regularly', '2020-11-18', 1, 'john123', 0, null),
('K.artixc@gmail.com' ,'How many currencies do you offer for conversion', '2021-01-15', 0, null, 0, null),
('karthikeyansuresh7@gmail.com' ,'What is the currency exchange rate between SGD and MMK', '2021-11-18', 0, null, 0, null);


CREATE TABLE FAQ(
	FAQ_ID			INT	 			IDENTITY PRIMARY KEY,
	Question 		VARCHAR(200)	NOT NULL,
	Answer			VARCHAR(200) 	NOT NULL,
    Created_by		VARCHAR(32)  	NOT NULL,
	Deleted			BIT				NOT NULL,
    Deleted_by		VARCHAR(32)		NULL
	);
 
    INSERT INTO FAQ(Question, Answer, Created_by, Deleted, Deleted_by) VALUES
    ('What is the maximum amount that we can exchange per transaction?', 'There is no fixed price!, but do note if we do not have enough, we will take 3 working days to get back to you', 'John Wick', 0, null),
	('How many currencies do you have?', 'We hold a variety of currencies with their respective exchange rates. Most are from ASEAN countries only. We apologise for any inconvenience', 'John Wick', 1, 'John Wick'),
	('Do you have an exchange rate fee?', 'Yes! It is only 3% per transaction', 'John Wick', 0, null);

     
CREATE TABLE Currency(
	Currency_id		INT				IDENTITY PRIMARY KEY,
    Currency_name 	VARCHAR(5) 		NOT NULL,
    Country 		VARCHAR(100) 	NOT NULL,
	Average_rate	DECIMAL(9,2)	NOT NULL,
    Created_by		VARCHAR(32)  	NOT NULL,
	Created_date	DATE			NOT NULL,
    Deleted 		BIT 			NOT NULL,
	Deleted_by		VARCHAR(32)		NULL
);
INSERT INTO Currency(Currency_name, Country, Average_rate, Created_by, Created_date, Deleted, Deleted_by) VALUES
('SGD', 'Singapore', 1.0 , 'John Wick', '2012-08-12', 0, null),
('MMK', 'Myanmar', 1315.5, 'John Wick', '2012-08-12', 0, null),
('CNY', 'China', 4.66, 'John Wick', '2012-08-12', 0, null),
('MYR', 'Malaysia', 0.33, 'John Wick', '2012-08-12',0, null);

CREATE TABLE ExchangeRates( 
		Source_currency 	VARCHAR(5)      NOT NULL,
    	Target_currency 	VARCHAR(5)      NOT NULL,
    	Exchange_rate   	DECIMAL(9,2) 	NOT NULL
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
    Created_by			  VARCHAR(32)	NOT NULL,
	Deleted				  BIT			NOT NULL,
    Deleted_by			  VARCHAR(32) 	NULL
);

INSERT INTO Transactions(Source_currency, Source_amount, Converted_currency, Converted_amount, Exchange_rate, Transaction_date, Created_by, Deleted, Deleted_by) VALUES
('SGD', 10.00, 'MYR', 30.77, 3.08, '2021-11-16', 'John Wick', 0, null),
('SGD', 1200000.00, 'CNY', 5662411.20, 4.72, '2021-11-15', 'James Charles' ,0, null), 
('SGD', 123.00, 'MMK', 160774.14, 1307.11, '2021-11-14', 'Charlene Lim', 0, null),
('SGD', 120.00, 'MMK', 156853.20, 1307.11, '2021-01-14', 'Charlene Lim', 1, 'Kanye West');

CREATE TABLE Stock(
	Stock_id		INT         	IDENTITY PRIMARY KEY,
	Stock_name		VARCHAR(5)		NOT NULL,
	Stock_amount	DECIMAL(9,2)	NOT NULL,
	Average_rate	DECIMAL(9,2)	NOT NULL
);
INSERT INTO Stock(Stock_name, Stock_amount, Average_rate) VALUES
('SGD', 100000.00, 1.0),
('MMK', 112345.00, 1315.5),
('CNY', 12345.00, 4.66),
('MYR', 98760.00, 0.33);