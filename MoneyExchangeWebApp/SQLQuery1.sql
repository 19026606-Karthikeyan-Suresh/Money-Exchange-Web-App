--
-- Drop tables if they exist to make script re-runnable
--
DROP TABLE IF EXISTS Transactions;
DROP TABLE IF EXISTS Enquiries;
DROP TABLE IF EXISTS Accounts;
DROP TABLE IF EXISTS ExchangeRates;
DROP TABLE IF EXISTS FAQ;
DROP TABLE IF EXISTS Stock;
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
	EditedBy		VARCHAR(200)	NULL,
	EditedDate		DATE			NULL,
	Deleted			BIT				NOT NULL,
	DeletedBy		VARCHAR(32)		NULL,
	DateDeleted		DATE			NULL
);

INSERT INTO Accounts(EmailAddress, Password, FirstName, LastName, Address, PhoneNumber, Gender, DOB, Role, DateCreated, EditedBy, EditedDate, Deleted, DeletedBy, DateDeleted) VALUES 
('john123@gmail.com', HASHBYTES('SHA1', 'password1'), 'John', 'gino', 'Blk 35 Mandalay Road # 13–37 Mandalay Towers Singapore 308215','87687908','Male','1999-01-12','staff', '2012-08-12',null,null,0, null, null),
('kaiwen4399@gmail.com', HASHBYTES('SHA1', 'password2'), 'Kaiwen', 'Huang', 'Blk 35 Mandalay Road # 13–37 Mandalay Towers Singapore 308215','87687908','Male','2000-01-15', 'admin','2013-05-02',null,null, 0, null, null),
('k.artixc@gmail.com', HASHBYTES('SHA1', 'password3'), 'Karthikeyan', 'Suresh', 'Blk 35 Mandalay Road # 13–37 Mandalay Towers Singapore 308215','87687908','Male','2002-06-14','admin','2015-05-04',null,null, 0, null, null),
('Tengyik1763@gmail.com', HASHBYTES('SHA1', 'password3'), 'Yik', 'Teng', 'Blk 35 Mandalay Road # 13–37 Mandalay Towers Singapore 308215','87687908','Male','2002-01-19', 'admin','2019-05-04',null,null,0 , null, null),
('19007578@myrp.edu.sg', HASHBYTES('SHA1', 'password4'), 'Jasper', 'Mak Jun Wai', 'Blk 35 Mandalay Road # 13–37 Mandalay Towers Singapore 308215','87687908','Male','1999-01-12','admin', '2019-05-04' ,null,null,0 , null, null),
('DummyUser1@myrp.edu.sg', HASHBYTES('SHA1', 'password5'), 'Jaden', ' Wai', '21C Tanjong Pagar International', '87689013', 'Male','1999-01-12', 'user','2022-01-03' ,null,null,0, null, null),
('DummyUser2@myrp.edu.sg', HASHBYTES('SHA1', 'password6'), 'Mark', ' Haedrig', '19D Tanjong Pagar International', '87680987', 'Male','2000-04-05', 'user', '2021-07-05' ,null,null,0, null, null),
('DummyUser3@myrp.edu.sg', HASHBYTES('SHA1', 'password7'), 'Amelia', ' Toh', 'Tanjong Pagar Bazar', '89761238', 'Female','2011-07-09', 'user', '2019-05-04' ,null,null,0, null, null),
('DummyUser4@myrp.edu.sg', HASHBYTES('SHA1', 'password8'), 'Amanda', ' Koh', 'Bedok South Park 1', '806184567', 'Unspecified','1999-07-08', 'user', '2018-02-04' ,null,null,0, null, null),
('DummyUser5@myrp.edu.sg', HASHBYTES('SHA1', 'password9'), 'Abdul', 'Rahman', 'Tampines North Blk 167D #167-042', '90718760', 'Male','2007-05-01', 'user', '2018-01-04' ,null,null,0, null, null);

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
	FaqId			INT	 			IDENTITY PRIMARY KEY,
	Question 		VARCHAR(200)	NOT NULL,
	Answer			VARCHAR(200) 	NOT NULL,
    CreatedBy		VARCHAR(32)  	NOT NULL,
	Deleted			BIT				NOT NULL,
    DeletedBy		VARCHAR(32)		NULL
	);
 
    INSERT INTO FAQ(Question, Answer, CreatedBy, Deleted, DeletedBy) VALUES
    ('What is the maximum amount that we can exchange per transaction?', 'There is no fixed price!, but do note if we do not have enough, we will take 3 working days to get back to you', 'John Wick', 0, null),
	('How many currencies do you have?', 'We hold a variety of currencies with their respective exchange rates. Most are from ASEAN countries only. We apologise for any inconvenience', 'John Wick', 1, 'John Wick'),
	('Do you have an exchange rate fee?', 'Yes! It is only 3% per transaction', 'John Wick', 0, null);

CREATE TABLE ExchangeRates( 
		BaseCurrency 	VARCHAR(5)      NOT NULL,
		QuoteCurrency 	VARCHAR(5)      NOT NULL,
    	ExchangeRate   	Float 			NOT NULL
);

CREATE TABLE Transactions(
	TransactionId 	      INT 	    	IDENTITY PRIMARY KEY,
	BaseCurrency	      VARCHAR(5)    NOT NULL,
	BaseAmount	 	      DECIMAL(9,2)  NOT NULL,
	QuoteCurrency		  VARCHAR(5)	NOT NULL,
	QuoteAmount			  DECIMAL(9,2)  NOT NULL,
	ExchangeRate		  DECIMAL(9,2)  NOT NULL,
	TransactionDate		  DATE 			NOT NULL,
	DoneBy				  VARCHAR(200)  NOT NULL,
	EditedBy			  VARCHAR(200)  NULL,
	EditedDate			  DATE			NULL,
	Deleted				  BIT			NOT NULL,
    DeletedBy			  VARCHAR(32) 	NULL,
	DeletedDate			  DATE			NULL
);

INSERT INTO Transactions(BaseCurrency, BaseAmount, QuoteCurrency, QuoteAmount, ExchangeRate, TransactionDate, DoneBy, Deleted, DeletedBy, DeletedDate) VALUES
('SGD', 10.00, 'MYR', 30.77, 3.08, '2021-11-16','john123@gmail.com', null, null, 0, null, null),
('SGD', 1200000.00, 'CNY', 5662411.20, 4.72, '2021-11-15','k.artixc@gmail.com' , null, null, 0, null, null), 
('SGD', 123.00, 'MMK', 160774.14, 1307.11, '2021-11-14','k.artixc@gmail.com', null, null, 0, null, null),
('SGD', 120.00, 'MMK', 156853.20, 1307.11, '2021-01-14','k.artixc@gmail.com', null, null, 1, 'Ng Yik Teng', '2022-01-05');

CREATE TABLE Stock(
	StockId		INT         	IDENTITY PRIMARY KEY,
	AccountId	INT				NOT NULL,
	ISO			VARCHAR(3)		NOT NULL,
	Amount		Float			NOT NULL,

);
INSERT INTO Stock(AccountId, ISO, Amount) VALUES
(1,'SGD', 100000.00),
(1,'MMK', 112345.00),
(1,'CNY', 12345.00),
(1,'MYR', 98760.00),
(2,'SGD', 1000000.00);