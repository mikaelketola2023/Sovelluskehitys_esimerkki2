CREATE TABLE tuotteet (id INTEGER IDENTITY(1,1) PRIMARY KEY, nimi TEXT, hinta INTEGER);

CREATE TABLE asiakkaat (id INTEGER IDENTITY (1,1) PRIMARY KEY, nimi VARCHAR(50), puhelinnumero VARCHAR(50));

CREATE TABLE tilaukset (id INTEGER IDENTITY (1,1) PRIMARY KEY, asiakas_id INTEGER REFERENCES asiakkaat ON DELETE CASCADE, tuote_id INTEGER REFERENCES tuotteet ON DELETE CASCADE, toimitettu BIT DEFAULT 0);

INSERT INTO tuotteet (nimi, hinta) VALUES ('HHL+ 128', 250);

INSERT INTO tilaukset (asiakas_id, tuote_id) VALUES (1, 1);

SELECT * FROM tuotteet;

SELECT * FROM asiakkaat;

SELECT ti.id AS id, a.nimi AS asiakas, tu.nimi AS tuote, ti.toimitettu AS toimitettu  FROM tilaukset ti, asiakkaat a, tuotteet tu WHERE a.id=ti.asiakas_id AND tu.id=ti.tuote_id

CREATE TABLE tilaustiedot (id INTEGER IDENTITY (1,1) PRIMARY KEY, tilaus_id INTEGER REFERENCES tilaukset(id) ON DELETE CASCADE, paivays DATE, kuvaus TEXT

INSERT INTO tilaustiedot (tilaus_id, paivays, kuvaus) VALUES (1, '2023-01-01', 'Tilaus toimitettu onnistuneesti.');

SELECT
    ti.id AS id, a.nimi AS asiakas, tu.nimi AS tuote, ti.toimitettu AS toimitettu, td.paivays AS tilaus_paivays, td.kuvaus AS tilaus_kuvaus

FROM tilaukset ti JOIN asiakkaat a ON a.id = ti.asiakas_id JOIN tuotteet tu ON tu.id = ti.tuote_id JOIN tilaustiedot td ON td.tilaus_id = ti.id;