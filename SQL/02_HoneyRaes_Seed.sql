\c HoneyRaes

INSERT INTO Customer (Id, Name, Address) OVERRIDING SYSTEM VALUE  VALUES (1, 'Bob', '123 Main St');
INSERT INTO Customer (Id, Name, Address) OVERRIDING SYSTEM VALUE  VALUES (2, 'Jim', '124 Main St');
INSERT INTO Customer (Id, Name, Address) OVERRIDING SYSTEM VALUE  VALUES (3, 'Thorton', '125 Main St');

INSERT INTO Employee (Id, Name, Specialty) OVERRIDING SYSTEM VALUE  VALUES (1, 'Tim', 'Accounting');
INSERT INTO Employee (Id, Name, Specialty) OVERRIDING SYSTEM VALUE  VALUES (2, 'Timmy', 'Nothing');

INSERT INTO ServiceTicket (Id, CustomerId, EmployeeId, Description, Emergency, DateCompleted) OVERRIDING SYSTEM VALUE VALUES (1, 1, NULL, 'Pushy Guest', true, '2025-04-03 14:30:00');
INSERT INTO ServiceTicket (Id, CustomerId, EmployeeId, Description, Emergency, DateCompleted) OVERRIDING SYSTEM VALUE VALUES (2, 2, 2, 'Nice Guest', false, NULL);
INSERT INTO ServiceTicket (Id, CustomerId, EmployeeId, Description, Emergency, DateCompleted) OVERRIDING SYSTEM VALUE VALUES (3, 1, 2, 'Nice Guest', false, CURRENT_TIMESTAMP);
INSERT INTO ServiceTicket (Id, CustomerId, EmployeeId, Description, Emergency, DateCompleted) OVERRIDING SYSTEM VALUE VALUES (4, 3, 1, 'Nice Guest', false, CURRENT_TIMESTAMP);
INSERT INTO ServiceTicket (Id, CustomerId, EmployeeId, Description, Emergency, DateCompleted) OVERRIDING SYSTEM VALUE VALUES (5, 1, 1, 'Nice Guest', true, CURRENT_TIMESTAMP);
