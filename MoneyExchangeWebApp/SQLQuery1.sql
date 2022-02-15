--
-- Drop tables if they exist to make script re-runnable
--
DROP TABLE IF EXISTS Accounts;
DROP TABLE IF EXISTS Enquiries;
DROP TABLE IF EXISTS FAQ;
DROP TABLE IF EXISTS ExchangeRates;
DROP TABLE IF EXISTS CurrencyTrades;
DROP TABLE IF EXISTS Stock;
DROP TABLE IF EXISTS DepWithTransactions;
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
	DOB				DATE		    NOT NULL,
	Role 			VARCHAR(32) 	NOT NULL,
	DateCreated		DATETIME		NOT NULL,
	EditedBy		VARCHAR(200)	NULL,
	EditedDate		DATETIME		NULL,
	Deleted			BIT				NOT NULL,
	DeletedBy		VARCHAR(32)		NULL,
	DateDeleted		DATETIME		NULL
);

INSERT INTO Accounts(EmailAddress, Password, FirstName, LastName, Address, PhoneNumber, Gender, DOB, Role, DateCreated, EditedBy, EditedDate, Deleted, DeletedBy, DateDeleted) VALUES 
('k.artixc@gmail.com', HASHBYTES('SHA1', 'password3'),  'Karthikeyan', 'Suresh', 'Blk 35 Mandalay Road # 13–37 Mandalay Towers Singapore 308215','87687908','Male','2002-06-14','admin','2015-05-04',null,null, 0, null, null),
('john123@gmail.com', HASHBYTES('SHA1', 'password1'), 'John', 'gino', 'Blk 35 Mandalay Road # 13–37 Mandalay Towers Singapore 308215','87687908','Male','1999-01-12','staff', '2012-08-12',null,null,0, null, null),
('DummyUser1@myrp.edu.sg', HASHBYTES('SHA1', 'password5'), 'Jaden', ' Wai', '21C Tanjong Pagar International', '87689013', 'Male','1999-01-12', 'user','2022-01-03' ,null,null,0, null, null),
('DummyUser2@myrp.edu.sg', HASHBYTES('SHA1', 'password6'), 'Mark', ' Haedrig', '19D Tanjong Pagar International', '87680987', 'Male','2000-04-05', 'user', '2021-07-05' ,null,null,0, null, null),
('DummyUser3@myrp.edu.sg', HASHBYTES('SHA1', 'password7'), 'Amelia', ' Toh', 'Tanjong Pagar Bazar', '89761238', 'Female','2011-07-09', 'user', '2019-05-04' ,null,null,0, null, null),
('DummyUser4@myrp.edu.sg', HASHBYTES('SHA1', 'password8'), 'Amanda', ' Koh', 'Bedok South Park 1', '806184567', 'Unspecified','1999-07-08', 'user', '2018-02-04' ,null,null,0, null, null),
('DummyUser5@myrp.edu.sg', HASHBYTES('SHA1', 'password9'), 'Abdul', 'Rahman', 'Tampines North Blk 167D #167-042', '90718760', 'Male','2007-05-01', 'user', '2018-01-04' ,null,null,0, null, null);

CREATE TABLE Enquiries(
	EnquiryId 				INT 			IDENTITY PRIMARY KEY,
	EmailAddress			VARCHAR(200) 	NOT NULL,
	Subject					VARCHAR(200)	NOT NULL,
	Question	 			VARCHAR(200) 	NOT NULL,
	EnquiryDate 			DATETIME		NOT NULL,
	Status 					VARCHAR(20)		NOT NULL,
	Answer					VARCHAR(200)	NULL,
	AnsweredBy				VARCHAR(200)  	NULL,
	AnswerDate				DATETIME		NULL
);
INSERT INTO Enquiries(EmailAddress, Subject, Question, EnquiryDate, Status, Answer ,AnsweredBy, AnswerDate) VALUES
('thunderblades48@gmail.com', 'Transactions' ,'How much money can I convert in one transaction?', '2020-11-16', 'Replied', 'No Answer', 'jam123','2022-01-02'),
('k.artixc@gmail.com','Currency' ,'Are the exchange rates updated regularly?', '2020-11-18', 'Replied', 'Lazy to type answer','john123', '2022-01-02'),
('k.artixc@gmail.com','Currency' ,'How many currencies do you offer for conversion?', '2021-01-15', 'Pending', null, null, null),
('karthikeyansuresh7@gmail.com', 'Currency' ,'What is the currency exchange rate between SGD and MMK?', '2021-11-18', 'Pending', null, null, null),
('k.artixc@gmail.com','Wallet' ,'Can I hold different currencies in my wallet?', '2021-01-15', 'Pending', null, null, null);


CREATE TABLE FAQ(
	FaqId			INT	 			IDENTITY PRIMARY KEY,
	Question 		VARCHAR(200)	NOT NULL,
	Answer			VARCHAR(200) 	NOT NULL,
	);
 
    INSERT INTO FAQ(Question, Answer) VALUES
    ('What is the maximum amount that we can exchange per transaction?', 'There is no fixed price!, but do note if we do not have enough, we will take 3 working days to get back to you'),
	('How many currencies do you have?', 'We hold a variety of currencies with their respective exchange rates. Most are from ASEAN countries only. We apologise for any inconvenience'),
	('Do you have an exchange rate fee?', 'Yes! It is only 3% per transaction');

CREATE TABLE ExchangeRates( 
		BaseCurrency 	VARCHAR(5)      NOT NULL,
		QuoteCurrency 	VARCHAR(5)      NOT NULL,
    	ExchangeRate   	Float 			NOT NULL
);

CREATE TABLE CurrencyTrades(
	TransactionId 	      INT 	    	IDENTITY PRIMARY KEY,
	BaseCurrency	      VARCHAR(5)    NOT NULL,
	BaseAmount	 	      float		    NOT NULL,
	QuoteCurrency		  VARCHAR(5)	NOT NULL,
	QuoteAmount			  float		    NOT NULL,
	ExchangeRate		  float		    NOT NULL,
	TransactionDate		  DATETIME		NOT NULL,
	DoneBy				  VARCHAR(200)  NOT NULL,
	EditedBy			  VARCHAR(200)  NULL,
	EditedDate			  DATETIME		NULL,
	Deleted				  BIT			NOT NULL,
    DeletedBy			  VARCHAR(32) 	NULL,
	DeletedDate			  DATETIME		NULL
);

INSERT INTO CurrencyTrades(BaseCurrency, BaseAmount, QuoteCurrency, QuoteAmount, ExchangeRate, TransactionDate, DoneBy, EditedBy, EditedDate, Deleted, DeletedBy, DeletedDate) VALUES

('JPY', 120.00, 'MMK', 156853.20, 1307.11, '2021-02-08','k.artixc@gmail.com', null, null, 0, null, null),
('JPY', 120.00, 'MMK', 156853.20, 1307.11, '2021-02-08','k.artixc@gmail.com', null, null, 0, null, null),
('JPY', 120.00, 'MMK', 156853.20, 1307.11, '2021-02-08','k.artixc@gmail.com', null, null, 0, null, null),
('JPY', 120.00, 'MMK', 156853.20, 1307.11, '2021-02-08','k.artixc@gmail.com', null, null, 0, null, null),
('JPY', 120.00, 'MMK', 156853.20, 1307.11, '2021-02-08','k.artixc@gmail.com', null, null, 0, null, null),
('JPY', 120.00, 'MMK', 156853.20, 1307.11, '2021-02-08','k.artixc@gmail.com', null, null, 0, null, null),
('JPY', 120.00, 'MMK', 156853.20, 1307.11, '2021-02-08','k.artixc@gmail.com', null, null, 0, null, null),
('JPY', 120.00, 'MMK', 156853.20, 1307.11, '2021-01-25','k.artixc@gmail.com', null, null, 0, null, null),
('JPY', 120.00, 'MMK', 156853.20, 1307.11, '2021-01-25','k.artixc@gmail.com', null, null, 0, null, null);

CREATE TABLE Stock(
	StockId		INT         	IDENTITY PRIMARY KEY,
	ISO			VARCHAR(3)		NOT NULL,
	Amount		Float			NOT NULL,

);
INSERT INTO Stock(ISO, Amount) VALUES
('SGD', 1000000),
('MMK', 112345),
('CNY', 12345),
('MYR', 98760),
('JPY', 1200000);

CREATE TABLE DepWithTransactions(
	TransactionId	INT				IDENTITY PRIMARY KEY,
	StockId			INT				NOT NULL,
	ISO				VARCHAR(3)		NOT NULL,
	DepOrWith		VARCHAR(30)		NOT NULL,
	Amount			Float			NOT NULL,
	TransactionDate	DATETIME		NOT NULL,
	Deleted			BIT				NOT NULl,
	DeletedDate		DATETIME		NULL,
);

INSERT INTO DepWithTransactions(StockId, ISO, DepOrWith, Amount, TransactionDate, Deleted, DeletedDate) VALUES
(1, 'SGD', 'Deposit', 1000.00, '2021-01-14', 0, null),
(1, 'SGD', 'Deposit', 2000.00, '2022-01-03', 0, null),
(1, 'SGD', 'Withdrawal', 3000.00, '2021-04-12', 0, null),
(1, 'SGD', 'Deposit', 4000.00, '2021-03-14', 0, null),
(1, 'SGD', 'Deposit', 1000.00, '2021-02-14', 0, null),
(1, 'SGD', 'Deposit', 1000.00, '2021-01-14', 0, null),
(1, 'SGD', 'Withdrawal', 2000.00, '2021-01-14', 0, null),
(1, 'SGD', 'Deposit', 4000.00, '2021-01-14', 0, null),
(1, 'SGD', 'Withdrawal', 2000.00, '2021-01-14', 0, null),
(2, 'MMK', 'Deposit', 34.00, '2021-01-15', 0, null),
(3, 'CNY', 'Withdrawal', 4000.00, '2021-01-16', 0, null),
(4, 'MYR', 'Deposit', 30000.00, '2021-01-17', 0, null);
