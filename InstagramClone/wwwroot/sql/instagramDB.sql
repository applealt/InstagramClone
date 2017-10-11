CREATE DATABASE InstagramDB
GO

USE InstagramDB
GO

CREATE TABLE Users (
	ID INT IDENTITY PRIMARY KEY,
	displayName VARCHAR(50) NOT NULL,
	userName VARCHAR(50) NOT NULL,
	email VARCHAR(50) NOT NULL,
	[password] VARCHAR(200) NOT NULL,
	imageProfile VARCHAR(50) ,
	firstName VARCHAR(50) NOT NULL,
	lastName VARCHAR(50) NOT NULL,
	postsCount INT DEFAULT 0,
	followersCount INT DEFAULT 0,
	followingCount INT DEFAULT 0,
	createdDate DATETIME DEFAULT GETDATE()
)

GO

CREATE TABLE Posts (
	ID INT IDENTITY(1,1) PRIMARY KEY,
	[user] INT NOT NULL FOREIGN KEY REFERENCES Users(ID) ON DELETE NO ACTION,
	[image] VARCHAR(100) NOT NULL,
	[status] VARCHAR(50),
	hashTag VARCHAR(50),
	likeCount INT NOT NULL DEFAULT 0,
	feedbackCount INT NOT NULL DEFAULT 0,
	createdDate DATETIME NOT NULL
)

GO

CREATE TABLE Likes (
	ID INT IDENTITY PRIMARY KEY,
	[user] INT NOT NULL FOREIGN KEY REFERENCES Users(ID) ON DELETE NO ACTION,
	post INT NOT NULL FOREIGN KEY REFERENCES Posts(ID) ON DELETE NO ACTION,
	likeDate DATETIME DEFAULT GETDATE()
)

GO

CREATE TABLE Follows (
	ID INT IDENTITY PRIMARY KEY,
	followingUser INT FOREIGN KEY REFERENCES Users(ID) ON DELETE NO ACTION,
	followedUser INT FOREIGN KEY REFERENCES Users(ID) ON DELETE NO ACTION,
	followDate DATETIME DEFAULT GETDATE()
)

GO

CREATE TABLE PasswordRecoveries (
	ID INT IDENTITY PRIMARY KEY,
	[user] INT NOT NULL FOREIGN KEY REFERENCES Users(ID) ON DELETE NO ACTION,
	[password] varchar(200) NOT NULL,
	verifyID varchar(200) NOT NULL,
	expirationDate DATETIME DEFAULT GETDATE()
)

GO

INSERT INTO Users(displayName, userName,email, [password], imageProfile,firstName, lastName, followersCount, postsCount, createdDate) VALUES ('Daniel Dang','danieldang','hieutrantvvn2006@gmail.com','123456','instagram.jpg','Daniel','Dang', 0, 8, GETDATE())
INSERT INTO Users(displayName, userName,email, [password], imageProfile,firstName, lastName, followersCount, postsCount, createdDate) VALUES ('Selena Gomez','selenagomez','recievewebdesign@gmail.com','123456','selenagomez.jpg','Selena', 'Gomez', 15 ,8, GETDATE())
INSERT INTO Users(displayName, userName,email, [password], imageProfile,firstName, lastName, followersCount, postsCount, createdDate) VALUES ('Ariana Grande','arianagrande','arianagrande@gmail.com','123456','arianagrande.jpg','Ariana', 'Grande',100,8, GETDATE())
INSERT INTO Users(displayName, userName,email, [password], imageProfile,firstName, lastName, followersCount, postsCount, createdDate) VALUES ('Cristiano Ronaldo','cristiano','cristiano@gmail.com','123456','cristiano.jpg','Cristiano','Ronaldo', 150, 8, GETDATE())
INSERT INTO Users(displayName, userName,email, [password], imageProfile,firstName, lastName, followersCount, postsCount, createdDate) VALUES ('Justin Bieber','justinbieber','justinbieber@gmail.com','123456','justinbieber.jpg','Justin','Bieber',125, 8, GETDATE())

INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (1, 'instagram1.jpg', 'Eating ice cream, in the park', '#icecream ', 480, 2387, GETDATE())
INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (1, 'instagram2.jpg', 'It was suthe dance', '#sunset ', 636, 2300, GETDATE())
INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (1, 'instagram3.jpg', 'We\u2019re swinging into atia', '#TheWeekOnInstagram ', 801, 2195, GETDATE())
INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (1, 'instagram4.jpg', 'Magic happens in the kitchen', '#magicn', 11, 4570, GETDATE())
INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (1, 'instagram5.jpg', 'Moving to  was an inspiration', '#Johannesburg ', 641, 2401, GETDATE())
INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (1, 'instagram6.jpg', 'After spending  ', '#MadeRight', 530, 6185, GETDATE())
INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (1, 'instagram7.jpg', 'We\u2019re excited to ', '#KindComments', 806, 4290, GETDATE())
INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (1, 'instagram8.jpg', 'This confident little ', '#Ruby ', 19, 9182, GETDATE())

INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (2, 'selenagomez1.jpg', 'Also so stoked I got', '#sneakpeak ', 41, 338, GETDATE())
INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (2, 'selenagomez2.jpg', '@instylemagazine', '#instylemagazine', 45, 285, GETDATE())
INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (2, 'selenagomez3.jpg', 'Ahhh!thank you Instyle!!', '#Coach', 26, 127, GETDATE())
INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (2, 'selenagomez4.jpg', 'Fetish Video directed by ', '#petrafcollins ', 27, 201, GETDATE())
INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (2, 'selenagomez5.jpg', 'Fetish Video directed by ', '#petrafcollins', 35, 138, GETDATE())
INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (2, 'selenagomez6.jpg', 'My people', '#mypeople', 56, 671, GETDATE())
INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (2, 'selenagomez7.jpg', 'My Petra', '#mypetra', 28, 105, GETDATE())
INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (2, 'selenagomez8.jpg', 'Thank you for all of my bday love', '#thankyou ', 68, 985, GETDATE())

INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (3, 'arianagrande1.jpg', 'To resurrect mo', '#moonlightbae', 22, 498, GETDATE())
INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (3, 'arianagrande2.jpg', 'ciao', '#ciao', 34, 812, GETDATE())
INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (3, 'arianagrande3.jpg', 'World tour', '#worldtour', 11, 6892, GETDATE())
INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (3, 'arianagrande4.jpg', 'Moonlight', '#moonlight', 19, 213, GETDATE())
INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (3, 'arianagrande5.jpg', 'Trans rights are human rights', '#transrights ', 13, 226, GETDATE())
INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (3, 'arianagrande6.jpg', 'Sunday August 27th', '#sunday', 22, 171, GETDATE())
INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (3, 'arianagrande7.jpg', 'Thank you thank you ', '#thankyou ', 11, 163, GETDATE())
INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (3, 'arianagrande8.jpg', 'Beautiful', '#beautiful', 46, 942, GETDATE())

INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (4, 'cristiano1.jpg', 'Great to be back', '#great', 21, 131, GETDATE())
INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (4, 'cristiano2.jpg', 'I only choose a  me', '#CLEARMen', 21, 128, GETDATE())
INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (4, 'cristiano3.jpg', 'A lot can happen', '#Syria ', 31, 26, GETDATE())
INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (4, 'cristiano4.jpg', 'Just do it!', '#adidas', 28, 157, GETDATE())
INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (4, 'cristiano5.jpg', 'Hi little guy', '#hi', 42, 217, GETDATE())
INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (4, 'cristiano6.jpg', 'O que incomoda ', '#incomoda ', 27, 189, GETDATE())
INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (4, 'cristiano7.jpg', 'Hard work and dedication', '#hardwork ', 35, 148, GETDATE())
INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (4, 'cristiano8.jpg', 'Treadmill workout', '#treadmill', 29, 144, GETDATE())

INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (5, 'justinbieber1.jpg', 'Going out', '#going out', 18, 236, GETDATE())
INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (5, 'justinbieber2.jpg', 'First day of school', '#firstday #school', 29, 486, GETDATE())
INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (5, 'justinbieber3.jpg', 'Vintage look', '#vintage', 21, 176, GETDATE())
INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (5, 'justinbieber4.jpg', '@CHADCVEACH', '#ZOE', 16, 168, GETDATE())
INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (5, 'justinbieber5.jpg', 'I am so grateful for this ', '#grateful ', 17, 979, GETDATE())
INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (5, 'justinbieber6.jpg', 'YOU WILL CHANGE THE WORLD', '#changetheworld ', 11, 133, GETDATE())
INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (5, 'justinbieber7.jpg', '@johnmayer is a boss on every level', '#johnmayer', 100, 7505, GETDATE())
INSERT INTO Posts([user], [image], [status], hashTag, likeCount, feedbackCount, createdDate) VALUES (5, 'justinbieber8.jpg', 'This guy @coreyharper', '#legend ', 1000, 5756, GETDATE())

GO