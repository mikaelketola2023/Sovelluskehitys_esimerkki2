CREATE TABLE tuotteet (id INTEGER IDENTITY(1,1) PRIMARY KEY, nimi TEXT, hinta INTEGER);

INSERT INTO tuotteet (nimi, hinta) VALUES ('maito', 2);

SELECT * FROM tuotteet;