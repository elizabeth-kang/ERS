DROP TABLE ers.Users;

DROP TABLE ers.Tickets;

DROP SCHEMA ers;



CREATE SCHEMA ers;

CREATE TABLE ers.Users (
	user_ID INT IDENTITY,
	username VARCHAR(50) UNIQUE NOT NULL,
	password VARCHAR(50) NOT NULL,
	role VARCHAR(8) NOT NULL CHECK (role IN ('Manager', 'Employee')),
	PRIMARY KEY (user_ID)
);

CREATE TABLE ers.Tickets (
	ticket_ID INT IDENTITY,
	author_fk INT NOT NULL FOREIGN KEY REFERENCES ers.Users(user_ID),
	resolver_fk INT FOREIGN KEY REFERENCES ers.Users(user_ID),
	description VARCHAR(127) NOT NULL,
	status VARCHAR(8) NOT NULL CHECK (status IN ('Pending', 'Approved', 'Denied')),
	amount DECIMAL NOT NULL,
	PRIMARY KEY (ticket_ID)
);


INSERT INTO ers.Users (username, password, role) VALUES ('ManagerUser1', 'ManagerPass1', 'Manager');
INSERT INTO ers.Users (username, password, role) VALUES ('ManagerUser2', 'ManagerPass2', 'Manager');

INSERT INTO ers.Users (username, password, role) VALUES ('EmployeeUser1', 'EmployeePass1', 'Employee');
INSERT INTO ers.Users (username, password, role) VALUES ('EmployeeUser2', 'EmployeePass2', 'Employee');

INSERT INTO ers.Tickets (author_fk, description, status, amount) VALUES (3,'Dog', 'Pending', 1231);
INSERT INTO ers.Tickets (author_fk, resolver_fk, description, status, amount) VALUES (4, 2, 'Testing2', 'Pending', 534);
INSERT INTO ers.Tickets (author_fk, resolver_fk, description, status, amount) VALUES (3, 1, 'Testing3', 'Pending', 798);
INSERT INTO ers.Tickets (author_fk, resolver_fk, description, status, amount) VALUES (4, 1, 'Testing4', 'Pending', 678);
INSERT INTO ers.Tickets (author_fk, resolver_fk, description, status, amount) VALUES (1, 2, 'Testing5', 'Pending', 123);
INSERT INTO ers.Tickets (author_fk, resolver_fk, description, status, amount) VALUES (2, 1, 'Testing6', 'Pending', 2321);


UPDATE ers.Tickets SET status = 'Approved' WHERE ticket_ID = 2;
UPDATE ers.Tickets SET status = 'Approved' WHERE ticket_ID = 3;
UPDATE ers.Tickets SET status = 'Denied' WHERE ticket_ID = 4;



SELECT * FROM ers.Users;
SELECT * FROM ers.Tickets;

