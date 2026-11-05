-- MySQL dump 10.13  Distrib 8.0.28, for Win64 (x86_64)
--
-- Host: localhost    Database: hciprojekat
-- ------------------------------------------------------
-- Server version	8.0.28

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

CREATE SCHEMA IF NOT EXISTS `hciprojekat` DEFAULT CHARACTER SET utf8 ;
USE `hciprojekat` ;

--
-- Table structure for table `dobavljac`
--

DROP TABLE IF EXISTS `dobavljac`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `dobavljac` (
  `JIB` varchar(13) NOT NULL,
  `Naziv` varchar(45) NOT NULL,
  `Adresa` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`JIB`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `dobavljac`
--

LOCK TABLES `dobavljac` WRITE;
/*!40000 ALTER TABLE `dobavljac` DISABLE KEYS */;
INSERT INTO `dobavljac` VALUES ('132465','Dobavljac',''),('1598745269874','PetWorld',''),('2154966687787','Pseći svijet',''),('7412563214785','Cat&Dog','Vase Pelagića 7');
/*!40000 ALTER TABLE `dobavljac` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ljubimac`
--

DROP TABLE IF EXISTS `ljubimac`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ljubimac` (
  `PROIZVOD_Sifra` int NOT NULL,
  `VrstaZivotinje` varchar(45) NOT NULL,
  `Uzrast` decimal(3,0) NOT NULL,
  `Tezina` decimal(5,2) DEFAULT NULL,
  `Rasa` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`PROIZVOD_Sifra`),
  CONSTRAINT `fk_LJUBIMAC_PROIZVOD1` FOREIGN KEY (`PROIZVOD_Sifra`) REFERENCES `proizvod` (`Sifra`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ljubimac`
--

LOCK TABLES `ljubimac` WRITE;
/*!40000 ALTER TABLE `ljubimac` DISABLE KEYS */;
INSERT INTO `ljubimac` VALUES (134655,'Riba',1,0.01,''),(178952,'Ptica',4,3.00,''),(314785,'Kornjača',2,0.30,''),(457896,'Riba',1,0.01,''),(458791,'Hrčak',3,0.80,''),(554444,'Hrčak',2,0.00,'');
/*!40000 ALTER TABLE `ljubimac` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `nabavka`
--

DROP TABLE IF EXISTS `nabavka`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `nabavka` (
  `SifraNabavke` int NOT NULL AUTO_INCREMENT,
  `DatumKreiranja` datetime NOT NULL,
  `ZAPOSLENI_JMB` varchar(14) NOT NULL,
  `DOBAVLJAC_JIB` varchar(13) NOT NULL,
  `UkupnaCijena` decimal(6,2) NOT NULL,
  PRIMARY KEY (`SifraNabavke`),
  KEY `fk_NABAVKA_ZAPOSLENI1_idx` (`ZAPOSLENI_JMB`),
  KEY `fk_NABAVKA_DOBAVLJAC1_idx` (`DOBAVLJAC_JIB`),
  CONSTRAINT `fk_NABAVKA_DOBAVLJAC1` FOREIGN KEY (`DOBAVLJAC_JIB`) REFERENCES `dobavljac` (`JIB`),
  CONSTRAINT `fk_NABAVKA_ZAPOSLENI1` FOREIGN KEY (`ZAPOSLENI_JMB`) REFERENCES `zaposleni` (`JMB`)
) ENGINE=InnoDB AUTO_INCREMENT=921563783 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `nabavka`
--

LOCK TABLES `nabavka` WRITE;
/*!40000 ALTER TABLE `nabavka` DISABLE KEYS */;
INSERT INTO `nabavka` VALUES (426348320,'2024-11-22 02:12:22','1407987112233','1598745269874',703.00),(921563782,'2024-11-22 02:11:06','1407987112233','2154966687787',274.90);
/*!40000 ALTER TABLE `nabavka` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ostalo`
--

DROP TABLE IF EXISTS `ostalo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ostalo` (
  `PROIZVOD_Sifra` int NOT NULL,
  `Vrsta` varchar(30) NOT NULL,
  `DodatniOpis` varchar(150) DEFAULT NULL,
  PRIMARY KEY (`PROIZVOD_Sifra`),
  CONSTRAINT `fk_DODATAK_PROIZVOD1` FOREIGN KEY (`PROIZVOD_Sifra`) REFERENCES `proizvod` (`Sifra`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ostalo`
--

LOCK TABLES `ostalo` WRITE;
/*!40000 ALTER TABLE `ostalo` DISABLE KEYS */;
INSERT INTO `ostalo` VALUES (147852,'Ogrlica',''),(151214,'igračka','Šarena loptica za pse'),(161922,'Hrana','10 kg'),(171819,'Hrana','10 kg '),(514252,'Povodac','Smeđa boja, print šapice'),(552647,'Hrana','200g'),(741562,'Akvarijum',''),(859741,'Higijena','Šampon za suho pranje'),(987458,'Higijena','');
/*!40000 ALTER TABLE `ostalo` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `petshop`
--

DROP TABLE IF EXISTS `petshop`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `petshop` (
  `Naziv` varchar(50) NOT NULL,
  `Adresa` varchar(100) NOT NULL,
  `Email` varchar(45) NOT NULL,
  `BrojTelefona` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`Naziv`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `petshop`
--

LOCK TABLES `petshop` WRITE;
/*!40000 ALTER TABLE `petshop` DISABLE KEYS */;
INSERT INTO `petshop` VALUES ('Šapica','Jovana Dučića 48','sapicapet@gmail.com','+38766222000');
/*!40000 ALTER TABLE `petshop` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `proizvod`
--

DROP TABLE IF EXISTS `proizvod`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `proizvod` (
  `Sifra` int NOT NULL,
  `Naziv` varchar(100) NOT NULL,
  `Stanje` int NOT NULL,
  `Cijena` decimal(5,2) NOT NULL,
  `Opis` varchar(100) DEFAULT NULL,
  `Tip` varchar(20) NOT NULL,
  `Postoji` int NOT NULL,
  PRIMARY KEY (`Sifra`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `proizvod`
--

LOCK TABLES `proizvod` WRITE;
/*!40000 ALTER TABLE `proizvod` DISABLE KEYS */;
INSERT INTO `proizvod` VALUES (134655,'Šarena ribica',0,32.00,'','Ljubimac',1),(147852,'Ogrlica za pse',17,5.20,'Boja: plava, zelena, crna','Ostalo',1),(151214,'Loptica',19,3.00,'','Ostalo',1),(161922,'Junior food',15,27.80,'Ukus piletine i govedine','Ostalo',1),(171819,'Hrana za mačke',27,30.50,'Ukus piletine','Ostalo',1),(178952,'Papagaj',2,154.50,'Ženka, plavo-zelena ','Ljubimac',1),(314785,'Kornjača',3,42.00,'Muška kornjača','Ljubimac',1),(457896,'Zlatna ribica',16,24.95,'','Ljubimac',1),(458791,'Hrčak Rile',1,38.90,'','Ljubimac',1),(514252,'Povodac',18,3.50,'Povodac za pse','Ostalo',1),(552647,'Premium cat',70,4.70,'Kesice za mačke, okus piletina','Ostalo',1),(554444,'Hrčak Rale',1,45.00,'Muški hrčak','Ljubimac',1),(741562,'Rounded akvarijum',6,60.00,'Akvarijum za male ribice','Ostalo',1),(859741,'Wash&',40,6.95,'Šampon za pse','Ostalo',1),(987458,'Clean&Nice',16,13.45,'Šampon za pse','Ostalo',1);
/*!40000 ALTER TABLE `proizvod` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `proizvod_nabavka`
--

DROP TABLE IF EXISTS `proizvod_nabavka`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `proizvod_nabavka` (
  `PROIZVOD_Sifra` int NOT NULL,
  `NABAVKA_SifraNabavke` int NOT NULL,
  `NabavnaCijena` decimal(5,2) NOT NULL,
  `Kolicina` int NOT NULL,
  PRIMARY KEY (`PROIZVOD_Sifra`,`NABAVKA_SifraNabavke`),
  KEY `fk_PROIZVOD_has_NABAVKA_NABAVKA1_idx` (`NABAVKA_SifraNabavke`),
  KEY `fk_PROIZVOD_has_NABAVKA_PROIZVOD1_idx` (`PROIZVOD_Sifra`),
  CONSTRAINT `fk_PROIZVOD_has_NABAVKA_NABAVKA1` FOREIGN KEY (`NABAVKA_SifraNabavke`) REFERENCES `nabavka` (`SifraNabavke`),
  CONSTRAINT `fk_PROIZVOD_has_NABAVKA_PROIZVOD1` FOREIGN KEY (`PROIZVOD_Sifra`) REFERENCES `proizvod` (`Sifra`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `proizvod_nabavka`
--

LOCK TABLES `proizvod_nabavka` WRITE;
/*!40000 ALTER TABLE `proizvod_nabavka` DISABLE KEYS */;
INSERT INTO `proizvod_nabavka` VALUES (134655,426348320,32.00,1),(147852,921563782,5.20,7),(151214,426348320,3.00,10),(151214,921563782,3.00,10),(161922,921563782,27.80,3),(171819,426348320,30.50,15),(514252,426348320,3.50,14),(859741,921563782,6.95,18),(987458,426348320,13.45,10);
/*!40000 ALTER TABLE `proizvod_nabavka` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `racun`
--

DROP TABLE IF EXISTS `racun`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `racun` (
  `Sifra` varchar(10) NOT NULL,
  `DatumKreiranja` datetime NOT NULL,
  `UkupnaCijena` decimal(6,2) NOT NULL,
  `ZAPOSLENI_JMB` varchar(13) NOT NULL,
  PRIMARY KEY (`Sifra`),
  KEY `fk_RACUN_ZAPOSLENI1_idx` (`ZAPOSLENI_JMB`),
  CONSTRAINT `fk_RACUN_ZAPOSLENI1` FOREIGN KEY (`ZAPOSLENI_JMB`) REFERENCES `zaposleni` (`JMB`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `racun`
--

LOCK TABLES `racun` WRITE;
/*!40000 ALTER TABLE `racun` DISABLE KEYS */;
INSERT INTO `racun` VALUES ('e14o7vlods','2024-11-22 02:13:14',99.00,'1716090602135');
/*!40000 ALTER TABLE `racun` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `stavka`
--

DROP TABLE IF EXISTS `stavka`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `stavka` (
  `PROIZVOD_Sifra` int NOT NULL,
  `RACUN_Sifra` varchar(10) NOT NULL,
  `NazivProizvoda` varchar(100) NOT NULL,
  `KolicinaProizvoda` int NOT NULL,
  `ProdajnaCijena` decimal(5,2) DEFAULT NULL,
  PRIMARY KEY (`PROIZVOD_Sifra`,`RACUN_Sifra`),
  KEY `fk_PROIZVOD_has_RACUN_RACUN1_idx` (`RACUN_Sifra`),
  KEY `fk_PROIZVOD_has_RACUN_PROIZVOD1_idx` (`PROIZVOD_Sifra`),
  CONSTRAINT `fk_PROIZVOD_has_RACUN_PROIZVOD1` FOREIGN KEY (`PROIZVOD_Sifra`) REFERENCES `proizvod` (`Sifra`),
  CONSTRAINT `fk_PROIZVOD_has_RACUN_RACUN1` FOREIGN KEY (`RACUN_Sifra`) REFERENCES `racun` (`Sifra`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `stavka`
--

LOCK TABLES `stavka` WRITE;
/*!40000 ALTER TABLE `stavka` DISABLE KEYS */;
INSERT INTO `stavka` VALUES (134655,'e14o7vlods','Šarena ribica',3,32.00),(151214,'e14o7vlods','Loptica',1,3.00);
/*!40000 ALTER TABLE `stavka` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `zaposleni`
--

DROP TABLE IF EXISTS `zaposleni`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `zaposleni` (
  `JMB` varchar(13) NOT NULL,
  `Ime` varchar(45) NOT NULL,
  `Prezime` varchar(45) NOT NULL,
  `Admin` int NOT NULL,
  `Status` int NOT NULL,
  `KorisnickoIme` varchar(20) NOT NULL,
  `Lozinka` varchar(520) NOT NULL,
  `Tema` varchar(30) DEFAULT 'ZelenaTema',
  PRIMARY KEY (`JMB`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `zaposleni`
--

LOCK TABLES `zaposleni` WRITE;
/*!40000 ALTER TABLE `zaposleni` DISABLE KEYS */;
INSERT INTO `zaposleni` VALUES ('0203003611445','Ivana','Babić',0,1,'ivana','feee3235626c079da26ff0ccbacba430f809cdf5a8dc7358aee52f05ae2e6ed0','ZelenaTema'),('0708977010203','Milovan','Bibić',0,0,'milovan','c6d5d2426b365c13c22185528d69214cf258cd278e58bb42aff0f2afe9331caf','ZelenaTema'),('1002998201236','Petar','Petrović',1,0,'petar','9d6245d7fb961b620b28c91d22e1a585413743388dc1833e678129f1751d94ae','ZelenaTema'),('1203999104045','Marko','Marković',1,0,'marko','4bbbb08b4c8c478c7d8d2843bb14c3e2a6015de5bd7d0128cc2b26f36cc44ca3','ZelenaTema'),('1204997101010','Aleksandar','Babić',0,0,'aco','f49f3b8e2f439c94016950f0ad625b25ffd75a860cd507a1b3c567c070899ae6','ZelenaTema'),('1204998303030','Nemanja','Ivić',0,1,'nemanja123','4dc3464d30e59cbc1a95472f71df0baf15dae8087951b733296d723f9256347f','ZelenaTema'),('1208997203010','Sara','Kekić',0,1,'sara','3a6d64c24cf80b69ccda37650406467e8266667b50cfd0b984beb3651b129ed7','ZelenaTema'),('1306993201030','Siniša','Lakić',0,1,'sinisa','1ca98a0f9dfc48ee47313b24b58e23bb5c24ac38a8c6d49be054aa35f052d327','ZelenaTema'),('1308976451245','Dejan','Petrović',0,1,'deki','0325569bffe9a43fbecff9152dd8dbb6391bf04a4fa905aadb289176987ee3bc','ZelenaTema'),('1308988554213','Igor','Petrić',1,0,'admin1','25f43b1486ad95a1398e3eeb3d83bc4010015fcc9bedb35b432e00298d5021f7','ZelenaTema'),('1405968123457','Slavko','Popović',0,1,'slavko','0f2a6ac4a00cdf20a7357b7961d2563433eb432d9bd17f6d67443428a7e9beab','ZelenaTema'),('1407987112233','Ana','Anić',1,1,'admin','8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918','PlavaTema'),('1710991050181','Jovana','Ilić ',1,1,'jovana','44b660850cb906161098c854da222febb4ef17a138c0656bb029f491beccd974','ZelenaTema'),('1710996321321','Ivona','Ilić',0,1,'ivona','feee3235626c079da26ff0ccbacba430f809cdf5a8dc7358aee52f05ae2e6ed0','ZelenaTema'),('1710999105018','Mitar','Mitić',0,1,'mitar','59a43d94b3735510edb2615e1a49916f418ae7301cb1987ede2f9867575d9cba','ZelenaTema'),('1716090602135','Sara','Sarić',0,1,'user','04f8996da763b7a969b1028ee3007569eaf3a635486ddab211d512c85b9df8fb','LjubicastaTema'),('2007998506030','Aljoša','Anić',1,1,'aljosa','6eb7e55dc356a99505a4c111dd31597f228ac74e1e3e393662dcac06b6702e2d','ZelenaTema'),('2302999101010','Ivana','Jokić',0,0,'ivanica','feee3235626c079da26ff0ccbacba430f809cdf5a8dc7358aee52f05ae2e6ed0','ZelenaTema');
/*!40000 ALTER TABLE `zaposleni` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'hciprojekat'
--
/*!50003 DROP PROCEDURE IF EXISTS `DodajNabavku` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `DodajNabavku`(
    IN pSifra INT,
    IN pDatumKreiranja DATETIME,
    IN pJMB VARCHAR(13),
    IN pJIB VARCHAR(13),
    IN pUkupnaCijena DECIMAL(6,2)
)
BEGIN
    INSERT INTO nabavka (SifraNabavke, DatumKreiranja, ZAPOSLENI_JMB, DOBAVLJAC_JIB, UkupnaCijena)
    VALUES (pSifra, pDatumKreiranja, pJMB, pJIB, pUkupnaCijena);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `DodajProizvodLjubimac` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `DodajProizvodLjubimac`(
	IN pSifra INT,
    IN pNaziv VARCHAR(100),
    IN pStanje INT,
    IN pCijena DECIMAL(5, 2),
    IN pOpis VARCHAR(100),
    IN pVrstaZivotinje VARCHAR(45),
    IN pRasa VARCHAR(45),
    IN pTezina DECIMAL(5, 2),
    IN pUzrast DECIMAL(3, 0)
)
BEGIN 
	INSERT INTO proizvod (Sifra, Naziv, Stanje, Cijena, Opis, Tip, Postoji)
    VALUES (pSifra, pNaziv, pStanje, pCijena, pOpis, "Ljubimac", 1);
    
    INSERT INTO ljubimac (PROIZVOD_Sifra, VrstaZivotinje, Uzrast, Tezina, Rasa)
    VALUES (pSifra, pVrstaZivotinje, pUzrast, pTezina, pRasa);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `DodajProizvodOstalo` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `DodajProizvodOstalo`(
	IN pSifra INT,
    IN pNaziv VARCHAR(100),
    IN pStanje INT,
    IN pCijena DECIMAL(5, 2),
    IN pOpis VARCHAR(100),
    IN pVrsta VARCHAR(30),
    IN pDodatniOpis VARCHAR(150)
)
BEGIN 
	INSERT INTO proizvod (Sifra, Naziv, Stanje, Cijena, Opis, Tip, Postoji)
    VALUES (pSifra, pNaziv, pStanje, pCijena, pOpis, "Ostalo", 1);
    
    INSERT INTO ostalo (PROIZVOD_Sifra, Vrsta, DodatniOpis)
    VALUES (pSifra, pVrsta, pDodatniOpis);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `DodajRacun` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `DodajRacun`(
    IN pSifra VARCHAR(10),
    IN pDatumKreiranja DATETIME,
    IN pUkupnaCijena DECIMAL(6,2),
    IN pZaposleniJMB VARCHAR(13)
)
BEGIN
    INSERT INTO racun (Sifra, DatumKreiranja, UkupnaCijena, ZAPOSLENI_JMB)
    VALUES (pSifra, pDatumKreiranja, pUkupnaCijena, pZaposleniJMB);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `DodajRadnika` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `DodajRadnika`(
	IN pJMB VARCHAR(13),
    IN pIme VARCHAR(45),
    IN pPrezime VARCHAR(45),
    IN pAdmin INT,
    IN pStatus INT,
    IN pKorisnickoIme VARCHAR(20),
    IN pLozinka VARCHAR(520)
)
BEGIN
	INSERT INTO zaposleni (JMB, Ime, Prezime, Admin, Status, KorisnickoIme, Lozinka)
    VALUES (pJMB, pIme, pPrezime, pAdmin, pStatus, pKorisnickoIme, pLozinka);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `IzmijeniLjubimac` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `IzmijeniLjubimac`(
	IN p_sifra INT,
    IN p_naziv VARCHAR(100),
    IN p_stanje INT,
    IN p_cijena DECIMAL(5, 2),
    IN p_opis VARCHAR(100),
    IN p_vrsta_zivotinje VARCHAR(45),
    IN p_uzrast DECIMAL(3, 0),
    IN p_tezina DECIMAL(5, 2),
    IN p_rasa VARCHAR(45)
)
BEGIN 
	UPDATE proizvod
    SET 
        Naziv = p_naziv,
        Stanje = p_stanje,
        Cijena = p_cijena,
        Opis = p_opis
    WHERE Sifra = p_sifra;
    UPDATE ljubimac
    SET 
        VrstaZivotinje = p_vrsta_zivotinje,
        Uzrast = p_uzrast,
        Tezina = p_tezina,
        Rasa = p_rasa
    WHERE PROIZVOD_Sifra = p_sifra;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `IzmijeniOstalo` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `IzmijeniOstalo`(
    IN p_sifra INT,
    IN p_naziv VARCHAR(100),
    IN p_stanje INT,
    IN p_cijena DECIMAL(5, 2),
    IN p_opis VARCHAR(100),
    IN p_vrsta VARCHAR(30),
    IN p_dodatni_opis VARCHAR(150)
)
BEGIN
    UPDATE proizvod
    SET 
        Naziv = p_naziv,
        Stanje = p_stanje,
        Cijena = p_cijena,
        Opis = p_opis
    WHERE Sifra = p_sifra;
    UPDATE ostalo
    SET 
        Vrsta = p_vrsta,
        DodatniOpis = p_dodatni_opis
    WHERE PROIZVOD_Sifra = p_sifra;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `IzmijeniRadnika` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `IzmijeniRadnika`(
    IN p_JMB VARCHAR(13),
    IN p_ime VARCHAR(45),
    IN p_prezime VARCHAR(45),
    IN p_admin INT,
    IN p_status INT
)
BEGIN
    UPDATE zaposleni
    SET 
        Ime = p_ime,
        Prezime = p_prezime,
        Admin = p_admin,
        Status = p_status
    WHERE JMB = p_JMB;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `PretraziNabavku` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `PretraziNabavku`(IN searchTerm VARCHAR(255))
BEGIN
    SELECT SifraNabavke, DatumKreiranja, ZAPOSLENI_JMB, z.Ime, z.Prezime, DOBAVLJAC_JIB, d.Naziv, UkupnaCijena  
	FROM nabavka 
	INNER JOIN zaposleni AS z ON ZAPOSLENI_JMB = z.JMB 
    INNER JOIN dobavljac AS d ON DOBAVLJAC_JIB = d.JIB
    WHERE (SifraNabavke LIKE CONCAT('%', searchTerm, '%') OR DatumKreiranja LIKE CONCAT('%', searchTerm, '%')
    OR ZAPOSLENI_JMB LIKE CONCAT('%', searchTerm, '%') OR z.Ime LIKE CONCAT('%', searchTerm, '%') 
    OR z.Prezime LIKE CONCAT('%', searchTerm, '%') OR d.Naziv LIKE CONCAT('%', searchTerm, '%')
    OR DOBAVLJAC_JIB LIKE CONCAT('%', searchTerm, '%'));
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `PretraziProizvod` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `PretraziProizvod`(IN searchTerm VARCHAR(255))
BEGIN
    SELECT Sifra, Naziv, Cijena, Stanje
    FROM proizvod
    WHERE (Sifra LIKE CONCAT('%', searchTerm, '%') OR Naziv LIKE CONCAT('%', searchTerm, '%'));
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `PretraziProizvodNabavka` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `PretraziProizvodNabavka`(IN searchTerm VARCHAR(255), sifra INT)
BEGIN
SELECT PROIZVOD_Sifra, p.Naziv, NabavnaCijena, Kolicina  
	FROM proizvod_nabavka INNER JOIN proizvod AS p ON PROIZVOD_Sifra = p.Sifra
    WHERE NABAVKA_SifraNabavke = sifra AND (PROIZVOD_Sifra LIKE CONCAT('%', searchTerm, '%') OR p.Naziv LIKE CONCAT('%', searchTerm, '%'));
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `PretraziRacun` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_AUTO_VALUE_ON_ZERO' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `PretraziRacun`(IN searchTerm VARCHAR(255))
BEGIN
    SELECT Sifra, DatumKreiranja, ZAPOSLENI_JMB, z.Ime, z.Prezime, UkupnaCijena 
	FROM racun 
	INNER JOIN zaposleni AS z ON ZAPOSLENI_JMB = z.JMB 
    WHERE (Sifra LIKE CONCAT('%', searchTerm, '%') OR DatumKreiranja LIKE CONCAT('%', searchTerm, '%')
    OR ZAPOSLENI_JMB LIKE CONCAT('%', searchTerm, '%') OR z.Ime LIKE CONCAT('%', searchTerm, '%') 
    OR z.Prezime LIKE CONCAT('%', searchTerm, '%'));
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `PretraziRadnika` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `PretraziRadnika`(IN searchTerm VARCHAR(255))
BEGIN
SELECT JMB, Ime, Prezime, Admin, Status, KorisnickoIme
    FROM zaposleni
    WHERE (JMB LIKE CONCAT('%', searchTerm, '%') OR Ime LIKE CONCAT('%', searchTerm, '%') OR Prezime LIKE CONCAT('%', searchTerm, '%'))
    ORDER BY Status=1 DESC;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `PretraziStavka` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_AUTO_VALUE_ON_ZERO' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `PretraziStavka`(IN searchTerm VARCHAR(255), sifra VARCHAR(10))
BEGIN
SELECT PROIZVOD_Sifra, p.Naziv, ProdajnaCijena, KolicinaProizvoda  
	FROM stavka INNER JOIN proizvod AS p ON PROIZVOD_Sifra = p.Sifra
    WHERE RACUN_Sifra = sifra AND (PROIZVOD_Sifra LIKE CONCAT('%', searchTerm, '%') OR p.Naziv LIKE CONCAT('%', searchTerm, '%'));
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-11-22 13:27:02
