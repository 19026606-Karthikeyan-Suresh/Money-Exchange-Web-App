--
-- Drop tables if they exist to make script re-runnable
--
DROP TABLE IF EXISTS Transactions;
DROP TABLE IF EXISTS Enquiries;
DROP TABLE IF EXISTS Accounts;
DROP TABLE IF EXISTS ExchangeRates;
DROP TABLE IF EXISTS FAQ;
DROP TABLE IF EXISTS Wallet;
--
-- Create tables
--

CREATE TABLE Accounts(
	AccountId		INT				IDENTITY PRIMARY KEY,
	EmailAddress 	VARCHAR(32) 	NOT NULL,
	Password 		VARBINARY(200) 	NOT NULL,
	FirstName 		VARCHAR(32)		NOT NULL,
	LastName		VARCHAR(32)		NOT NULL,
	Address			VARCHAR(200)	NOT NULL,
	PhoneNumber		INT				NOT NULL,
	Gender			VARCHAR(20)		NOT NULL,
	DOB				DATE			NOT NULL,
	Role 			VARCHAR(32) 	NOT NULL,
	DateCreated		DATE			NOT NULL,
	Deleted			BIT				NOT NULL,
	DeletedBy		VARCHAR(32)		NULL,
	DateDeleted		DATE			NULL
);

INSERT INTO Accounts(EmailAddress, Password, FirstName, LastName, Address, PhoneNumber, Gender, DOB, Role, DateCreated, Deleted, DeletedBy, DateDeleted) VALUES 
('john123@gmail.com', HASHBYTES('SHA1', 'password1'), 'John', 'gino', 'Blk 35 Mandalay Road # 13–37 Mandalay Towers Singapore 308215','87687908','Male','1999-01-12','staff', '2012-08-12',0, null, null),
('kaiwen4399@gmail.com', HASHBYTES('SHA1', 'password2'), 'Kaiwen', 'Huang', 'Blk 35 Mandalay Road # 13–37 Mandalay Towers Singapore 308215','87687908','Male','2000-01-15', 'admin', '2013-05-02', 0, null, null),
('k.artixc@gmail.com', HASHBYTES('SHA1', 'password3'), 'Karthikeyan', 'Suresh', 'Blk 35 Mandalay Road # 13–37 Mandalay Towers Singapore 308215','87687908','Male','2002-06-14','admin', '2015-05-04', 0, null, null),
('Tengyik1763@gmail.com', HASHBYTES('SHA1', 'password3'), 'Yik', 'Teng', 'Blk 35 Mandalay Road # 13–37 Mandalay Towers Singapore 308215','87687908','Male','2002-01-19', 'admin', '2019-05-04', 0, null, null),
('19007578@myrp.edu.sg', HASHBYTES('SHA1', 'password4'), 'Jasper', 'Mak Jun Wai', 'Blk 35 Mandalay Road # 13–37 Mandalay Towers Singapore 308215','87687908','Male','1999-01-12', 'admin', '2019-05-04' ,0, null, null);

CREATE TABLE Enquiries(
	EnquiryId 				INT 			IDENTITY PRIMARY KEY,
	EmailAddress			VARCHAR(100) 	NOT NULL,
	Subject					VARCHAR(100)	NOT NULL,
	Question	 			VARCHAR(100) 	NOT NULL,
	EnquiryDate 			DATE 			NOT NULL,
	Status 					VARCHAR(20)		NOT NULL,
	Answer					VARCHAR(100)	NULL,
	AnsweredBy				VARCHAR(32)  	NULL,
	AnswerDate				DATE			NULL
);
INSERT INTO Enquiries(EmailAddress, Subject, Question, EnquiryDate, Status, Answer ,AnsweredBy, AnswerDate) VALUES
('thunderblades48@gmail.com', 'Transactions' ,'How much money can I convert in one transaction?', '2020-11-16', 'Replied', 'No Answer', 'jam123','2022-01-02'),
('k.artixc@gmail.com','Currency' ,'Are the exchange rates updated regularly?', '2020-11-18', 'Replied', 'Lazy to type answer','john123', '2022-01-02'),
('k.artixc@gmail.com','Currency' ,'How many currencies do you offer for conversion?', '2021-01-15', 'Pending', null, null, null),
('karthikeyansuresh7@gmail.com', 'Currency' ,'What is the currency exchange rate between SGD and MMK?', '2021-11-18', 'Pending', null, null, null),
('k.artixc@gmail.com','Wallet' ,'Can I hold different currencies in my wallet?', '2021-01-15', 'Pending', null, null, null),
('kaiwen4399@gmail.com','Other','Sending Test', '2022-01-03', 'Pending', null, null, null);


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

CREATE TABLE ExchangeRates( 
		Source_currency 	VARCHAR(5)      NOT NULL,
    	Target_currency 	VARCHAR(5)      NOT NULL,
    	Exchange_rate   	Float 	NOT NULL
);

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

CREATE TABLE Wallet(
	WalletId	INT         	IDENTITY PRIMARY KEY,
	Email		VARCHAR(200)	NOT NULL,
	ISO			VARCHAR(3)		NOT NULL,
	Amount		Float			NOT NULL,

);
INSERT INTO Wallet(Email, ISO, Amount) VALUES
('k.artixc@gmail.com','SGD', 100000.00),
('k.artixc@gmail.com','MMK', 112345.00),
('k.artixc@gmail.com','CNY', 12345.00),
('k.artixc@gmail.com','MYR', 98760.00),
('Tengyik1763@gmail.com','SGD', 1000000.00);