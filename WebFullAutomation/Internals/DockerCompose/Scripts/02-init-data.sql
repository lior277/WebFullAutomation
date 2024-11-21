-- MySQL dump 10.13  Distrib 8.3.0, for macos14.2 (arm64)
--
-- Host: legacy-sql.c4x04lsmqure.eu-west-1.rds.amazonaws.com    Database: data
-- ------------------------------------------------------
-- Server version	8.0.33

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Current Database: `data`
--

CREATE DATABASE /*!32312 IF NOT EXISTS*/ `data` /*!40100 DEFAULT CHARACTER SET utf8mb3 */ /*!80016 DEFAULT ENCRYPTION='N' */;

USE `data`;

--
-- Table structure for table `SettlementStates`
--

DROP TABLE IF EXISTS `SettlementStates`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `SettlementStates` (
  `Id` int NOT NULL,
  `Name` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `alerttypes`
--

DROP TABLE IF EXISTS `alerttypes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `alerttypes` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `asianmarketsuffix`
--

DROP TABLE IF EXISTS `asianmarketsuffix`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `asianmarketsuffix` (
  `Id` int NOT NULL,
  `Name` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `asianmarketsuffixmappings`
--

DROP TABLE IF EXISTS `asianmarketsuffixmappings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `asianmarketsuffixmappings` (
  `Id` int NOT NULL,
  `MainId` int NOT NULL,
  `ProviderId` int NOT NULL,
  `Name` varchar(20) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `MainId` (`MainId`) USING BTREE,
  KEY `ProviderId` (`ProviderId`) USING BTREE,
  CONSTRAINT `asianmarketsuffixmappings_ibfk_1` FOREIGN KEY (`MainId`) REFERENCES `asianmarketsuffix` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `asianmarketsuffixmappings_ibfk_2` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_asianmarketsuffixmappings_insert_trigger` AFTER INSERT ON `asianmarketsuffixmappings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('asianmarketsuffixmappings', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_asianmarketsuffixmappings_update_trigger` AFTER UPDATE ON `asianmarketsuffixmappings` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.mainid <=> new.mainid) || not(old.providerid <=> new.providerid) || not(old.name <=> new.name) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('asianmarketsuffixmappings', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_asianmarketsuffixmappings_delete_trigger` AFTER DELETE ON `asianmarketsuffixmappings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('asianmarketsuffixmappings', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `audlreplay`
--

DROP TABLE IF EXISTS `audlreplay`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `audlreplay` (
  `IsReplay` bit(1) DEFAULT NULL,
  `OriginalGameId` varchar(255) DEFAULT NULL,
  `ReplayGameId` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `automanualft`
--

DROP TABLE IF EXISTS `automanualft`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `automanualft` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FixtureId` int NOT NULL,
  `ClosedManually` bit(1) NOT NULL,
  `ClosingDate` datetime(6) DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP(6),
  `Status` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `automanual_ibfk_1` (`FixtureId`),
  CONSTRAINT `automanual_ibfk_1` FOREIGN KEY (`FixtureId`) REFERENCES `fixtures` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=523497 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `autorobotinstructions`
--

DROP TABLE IF EXISTS `autorobotinstructions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `autorobotinstructions` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Instructions` mediumtext NOT NULL,
  `LastUpdate` datetime NOT NULL,
  `LastAuthor` varchar(255) NOT NULL,
  `RobotName` varchar(64) NOT NULL,
  `Version` varchar(32) NOT NULL,
  `VersionComments` varchar(255) DEFAULT NULL,
  `Current` bit(1) NOT NULL DEFAULT b'1',
  PRIMARY KEY (`Id`),
  KEY `autorobotinstructions_name_idx` (`RobotName`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=49 DEFAULT CHARSET=utf8mb3 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `backgroundjobs`
--

DROP TABLE IF EXISTS `backgroundjobs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `backgroundjobs` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) DEFAULT NULL,
  `Description` varchar(100) DEFAULT NULL,
  `ExtraData` varchar(1024) DEFAULT NULL,
  `RunningInterval` int DEFAULT NULL,
  `IsActive` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `betnamemappings`
--

DROP TABLE IF EXISTS `betnamemappings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `betnamemappings` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `MainId` int DEFAULT NULL,
  `MarketFamilyId` int NOT NULL,
  `Name` varchar(255) NOT NULL,
  `IsActive` bit(1) NOT NULL DEFAULT b'1',
  `CreationDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`) USING BTREE,
  KEY `MainId` (`MainId`) USING BTREE,
  KEY `betnamemappings_ibfk_2` (`MarketFamilyId`) USING BTREE,
  CONSTRAINT `betnamemappings_ibfk_1` FOREIGN KEY (`MainId`) REFERENCES `mainbetnames` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `betnamemappings_ibfk_2` FOREIGN KEY (`MarketFamilyId`) REFERENCES `marketfamilies` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=523 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_betnamemappings_insert_trigger` AFTER INSERT ON `betnamemappings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('betnamemappings', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_betnamemappings_update_trigger` AFTER UPDATE ON `betnamemappings` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.mainid <=> new.mainid) || not(old.marketfamilyid <=> new.marketfamilyid) || not(old.name <=> new.name) || not(old.isactive <=> new.isactive) || not(old.creationdate <=> new.creationdate) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('betnamemappings', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_betnamemappings_delete_trigger` AFTER DELETE ON `betnamemappings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('betnamemappings', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `betstatuses`
--

DROP TABLE IF EXISTS `betstatuses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `betstatuses` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bettypes`
--

DROP TABLE IF EXISTS `bettypes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `bettypes` (
  `Id` int NOT NULL,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `breaktimedescriptions`
--

DROP TABLE IF EXISTS `breaktimedescriptions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `breaktimedescriptions` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `StatusDescriptionId` int NOT NULL,
  `SportId` int NOT NULL,
  `MandatoryScorePeriods` varchar(255) DEFAULT NULL,
  `Seconds` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `breaktimedescriptions_ibfk_1` (`StatusDescriptionId`),
  KEY `breaktimedescriptions_ibfk_2` (`SportId`),
  CONSTRAINT `breaktimedescriptions_ibfk_1` FOREIGN KEY (`StatusDescriptionId`) REFERENCES `statusdescriptions` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `breaktimedescriptions_ibfk_2` FOREIGN KEY (`SportId`) REFERENCES `sports` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_breaktimedescriptions_insert_trigger` AFTER INSERT ON `breaktimedescriptions` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('breaktimedescriptions', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_breaktimedescriptions_update_trigger` AFTER UPDATE ON `breaktimedescriptions` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.statusdescriptionid <=> new.statusdescriptionid) || not(old.sportid <=> new.sportid) || not(old.mandatoryscoreperiods <=> new.mandatoryscoreperiods) || not(old.seconds <=> new.seconds) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('breaktimedescriptions', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_breaktimedescriptions_delete_trigger` AFTER DELETE ON `breaktimedescriptions` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('breaktimedescriptions', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `cdc_dummy`
--

DROP TABLE IF EXISTS `cdc_dummy`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cdc_dummy` (
  `lastUpdated` timestamp NOT NULL,
  PRIMARY KEY (`lastUpdated`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `cities`
--

DROP TABLE IF EXISTS `cities`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cities` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  `StateId` int DEFAULT NULL,
  `CountryId` int NOT NULL,
  `IsActive` tinyint(1) NOT NULL DEFAULT '1',
  `CreationDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
  `LastUpdate` datetime(6) DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP(6),
  `Username` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Name_CountryId` (`Name`,`CountryId`) USING BTREE,
  KEY `State_IsActive` (`StateId`,`IsActive`) USING BTREE,
  KEY `State_CountryId_IsActive` (`StateId`,`CountryId`,`IsActive`) USING BTREE,
  KEY `CountryId_IsActive` (`CountryId`,`IsActive`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `citymappings`
--

DROP TABLE IF EXISTS `citymappings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `citymappings` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `MainId` int NOT NULL,
  `Name` varchar(255) NOT NULL,
  `IsActive` tinyint(1) NOT NULL DEFAULT '1',
  `CreationDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
  `LastUpdate` datetime(6) DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP(6),
  `Username` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `MainId_Name` (`MainId`,`Name`) USING BTREE,
  KEY `Name_IsActive` (`Name`,`IsActive`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `competitionmetadata`
--

DROP TABLE IF EXISTS `competitionmetadata`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `competitionmetadata` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  `TypeId` int NOT NULL,
  `CreationDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpdate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `IX_UQ` (`Name`,`TypeId`) USING BTREE,
  KEY `IX_ID` (`Id`) USING BTREE,
  KEY `IX_NAME` (`Name`) USING BTREE,
  KEY `IX_TYPE` (`TypeId`) USING BTREE,
  KEY `IX_CREATIONDATE` (`CreationDate`) USING BTREE,
  KEY `IX_LASTUPDATE` (`LastUpdate`) USING BTREE,
  KEY `IX_NAME_TYPE` (`Name`,`TypeId`) USING BTREE,
  KEY `IX_NAME_TYPE_LASTUPDATE` (`Name`,`TypeId`,`LastUpdate`) USING BTREE,
  CONSTRAINT `fk_competitionmetadata_typeid` FOREIGN KEY (`TypeId`) REFERENCES `competitionnodetypes` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=56748 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_competitionmetadata_insert_trigger` AFTER INSERT ON `competitionmetadata` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('competitionmetadata', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_competitionmetadata_update_trigger` AFTER UPDATE ON `competitionmetadata` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.name <=> new.name) || not(old.typeid <=> new.typeid) || not(old.creationdate <=> new.creationdate) || not(old.lastupdate <=> new.lastupdate) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('competitionmetadata', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `fixture_reflastupdate_competitionmetadata_update_trigger` AFTER UPDATE ON `competitionmetadata` FOR EACH ROW begin 
                        update fixtures
                        set referenceslastupdate=now()
                        where statusid in (1, 2, 5, 6, 8, 9)
                        and startdate >= now() - interval (select max(loadinterval) from sports) hour
                        and startdate >= now() - interval (select loadinterval from sports where id=sportid) hour
                        and id in
                        (
                            select fixtureid
                            from fixturecompetitions
                            where competitionid in
                            (
                                select id
                                from competitionnodes
                                where metadataid=new.id
                            )
                            order by fixtureid
                        )
                        order by id; end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_competitionmetadata_delete_trigger` AFTER DELETE ON `competitionmetadata` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('competitionmetadata', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `fixture_reflastupdate_competitionmetadata_delete_trigger` AFTER DELETE ON `competitionmetadata` FOR EACH ROW begin 
                        update fixtures
                        set referenceslastupdate=now()
                        where statusid in (1, 2, 5, 6, 8, 9)
                        and startdate >= now() - interval (select max(loadinterval) from sports) hour
                        and startdate >= now() - interval (select loadinterval from sports where id=sportid) hour
                        and id in
                        (
                            select fixtureid
                            from fixturecompetitions
                            where competitionid in
                            (
                                select id
                                from competitionnodes
                                where metadataid=old.id
                            )
                            order by fixtureid
                        )
                        order by id; end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `competitionmetadatamappings`
--

DROP TABLE IF EXISTS `competitionmetadatamappings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `competitionmetadatamappings` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `MainId` int NOT NULL,
  `Name` varchar(255) NOT NULL,
  `IsActive` bit(1) NOT NULL,
  `CreationDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpdate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `Name_MainId` (`MainId`,`Name`) USING BTREE,
  KEY `IX_ID` (`Id`) USING BTREE,
  KEY `IX_MAINID` (`MainId`) USING BTREE,
  KEY `IX_NAME` (`Name`) USING BTREE,
  KEY `IX_ISACTIVE` (`IsActive`) USING BTREE,
  KEY `IX_CREATIONDATE` (`CreationDate`) USING BTREE,
  KEY `IX_LASTUPDATE` (`LastUpdate`) USING BTREE,
  KEY `Name_IsActive` (`Name`,`IsActive`) USING BTREE,
  CONSTRAINT `fk_competitionmetadatamappings_mainid` FOREIGN KEY (`MainId`) REFERENCES `competitionmetadata` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=64610 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_competitionmetadatamappings_insert_trigger` AFTER INSERT ON `competitionmetadatamappings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('competitionmetadatamappings', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_competitionmetadatamappings_update_trigger` AFTER UPDATE ON `competitionmetadatamappings` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.mainid <=> new.mainid) || not(old.name <=> new.name) || not(old.isactive <=> new.isactive) || not(old.creationdate <=> new.creationdate) || not(old.lastupdate <=> new.lastupdate) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('competitionmetadatamappings', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_competitionmetadatamappings_delete_trigger` AFTER DELETE ON `competitionmetadatamappings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('competitionmetadatamappings', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `competitionnodes`
--

DROP TABLE IF EXISTS `competitionnodes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `competitionnodes` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ParentId` int DEFAULT NULL,
  `MetadataId` int NOT NULL,
  `LocationId` int NOT NULL,
  `CreationDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpdate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `SportId` int DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `IX_UQ` (`MetadataId`,`LocationId`,`SportId`) USING BTREE,
  KEY `IX_ID` (`Id`) USING BTREE,
  KEY `IX_PARENT` (`ParentId`) USING BTREE,
  KEY `IX_METADATA` (`MetadataId`) USING BTREE,
  KEY `IX_LOCATION` (`LocationId`) USING BTREE,
  KEY `IX_CREATIONDATE` (`CreationDate`) USING BTREE,
  KEY `IX_LASTUPDATE` (`LastUpdate`) USING BTREE,
  KEY `IX_METADATA_LASTUPDATE` (`MetadataId`,`LastUpdate`) USING BTREE,
  KEY `SportId` (`SportId`) USING BTREE,
  CONSTRAINT `competitionnodes_ibfk_1` FOREIGN KEY (`SportId`) REFERENCES `sports` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `fk_competitionnodes_location` FOREIGN KEY (`LocationId`) REFERENCES `locations` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `fk_competitionnodes_metadata` FOREIGN KEY (`MetadataId`) REFERENCES `competitionmetadata` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `fk_competitionnodes_parent` FOREIGN KEY (`ParentId`) REFERENCES `competitionnodes` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=65937 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_competitionnodes_insert_trigger` AFTER INSERT ON `competitionnodes` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('competitionnodes', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_competitionnodes_update_trigger` AFTER UPDATE ON `competitionnodes` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.parentid <=> new.parentid) || not(old.metadataid <=> new.metadataid) || not(old.locationid <=> new.locationid) || not(old.creationdate <=> new.creationdate) || not(old.lastupdate <=> new.lastupdate) || not(old.sportid <=> new.sportid) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('competitionnodes', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `fixture_reflastupdate_competitionnodes_update_trigger` AFTER UPDATE ON `competitionnodes` FOR EACH ROW begin 
                        update fixtures
                        set referenceslastupdate=now()
                        where statusid in (1, 2, 5, 6, 8, 9)
                        and startdate >= now() - interval (select loadinterval from sports where id=new.sportid) hour
                        and id in
                        (
                            select fixtureid
                            from fixturecompetitions
                            where competitionid=new.id
                            order by fixtureid
                        )
                        order by id; end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `competitionnodetypes`
--

DROP TABLE IF EXISTS `competitionnodetypes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `competitionnodetypes` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(25) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `IX_UQ` (`Name`) USING BTREE,
  KEY `IX_ID` (`Id`) USING BTREE,
  KEY `IX_NAME` (`Name`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `componentsettings`
--

DROP TABLE IF EXISTS `componentsettings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `componentsettings` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ComponentName` varchar(255) NOT NULL,
  `Key` varchar(255) DEFAULT NULL,
  `Value` varchar(5000) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `ComponentNameKeyUnique` (`ComponentName`,`Key`) USING BTREE,
  KEY `ComponentName` (`ComponentName`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=6474 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_componentsettings_insert_trigger` AFTER INSERT ON `componentsettings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('componentsettings', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_componentsettings_update_trigger` AFTER UPDATE ON `componentsettings` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.componentname <=> new.componentname) || not(old.key <=> new.key) || not(old.value <=> new.value) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('componentsettings', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_componentsettings_delete_trigger` AFTER DELETE ON `componentsettings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('componentsettings', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `coveragetype`
--

DROP TABLE IF EXISTS `coveragetype`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `coveragetype` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `customerips`
--

DROP TABLE IF EXISTS `customerips`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `customerips` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `CustomerId` int NOT NULL,
  `IPAddresses` varchar(2000) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Indx_CustomerIps_CustomerId` (`CustomerId`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=2560 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_customerips_insert_trigger` AFTER INSERT ON `customerips` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('customerips', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_customerips_update_trigger` AFTER UPDATE ON `customerips` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.customerid <=> new.customerid) || not(old.ipaddresses <=> new.ipaddresses) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('customerips', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_customerips_delete_trigger` AFTER DELETE ON `customerips` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('customerips', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `customerpackages`
--

DROP TABLE IF EXISTS `customerpackages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `customerpackages` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Guid` char(36) NOT NULL,
  `CustomerId` int NOT NULL,
  `ExpirationDate` datetime NOT NULL,
  `Description` varchar(255) DEFAULT NULL,
  `TotalCredits` int NOT NULL DEFAULT '0',
  `LastUpdate` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `ExternalProviderId` int DEFAULT NULL,
  `PushUrl` varchar(1250) DEFAULT NULL,
  `OddProviders` text NOT NULL,
  `Sports` text NOT NULL,
  `Locations` text NOT NULL,
  `Markets` text NOT NULL,
  `Leagues` text NOT NULL,
  `IsActive` bit(1) NOT NULL DEFAULT b'0',
  `RmqEnabled` bit(1) NOT NULL DEFAULT b'0',
  `GetAllPushEvents` bit(1) NOT NULL DEFAULT b'0',
  `HasCards` bit(1) NOT NULL DEFAULT b'0',
  `HasScores` bit(1) NOT NULL DEFAULT b'0',
  `HasScorers` bit(1) NOT NULL DEFAULT b'0',
  `HasStatistics` bit(1) NOT NULL DEFAULT b'0',
  `HasResulting` bit(1) NOT NULL DEFAULT b'0',
  `HasInPlayOdds` bit(1) NOT NULL DEFAULT b'0',
  `HasPreMatchOdds` bit(1) NOT NULL DEFAULT b'0',
  `HasBetSlip` bit(1) NOT NULL DEFAULT b'0',
  `HasAverage` bit(1) NOT NULL DEFAULT b'0',
  `HasStandings` bit(1) NOT NULL DEFAULT b'0',
  `HasTvChannels` bit(1) NOT NULL DEFAULT b'0',
  `HasProviderScores` bit(1) NOT NULL DEFAULT b'0',
  `HasItf` bit(1) NOT NULL DEFAULT b'0',
  `HasPushDownAlert` bit(1) NOT NULL DEFAULT b'0',
  `HasHistoricalStatistics` bit(1) NOT NULL DEFAULT b'0',
  `HasFastMarkets` bit(1) NOT NULL DEFAULT b'0',
  `HasHistoricalOdds` bit(1) NOT NULL DEFAULT b'0',
  `HasBetStop` bit(1) NOT NULL DEFAULT b'0',
  `HasOutrights` bit(1) NOT NULL DEFAULT b'1',
  `Username` varchar(255) NOT NULL,
  `Password` varchar(255) NOT NULL,
  `CreationDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `FormatType` int NOT NULL DEFAULT '1',
  `HasOutrightLeagues` bit(1) NOT NULL DEFAULT b'1',
  `HasPlayerMarkets` bit(1) NOT NULL DEFAULT b'0',
  `HasTennisStats` bit(1) NOT NULL DEFAULT b'0',
  `HasFootballStats` bit(1) NOT NULL DEFAULT b'0',
  `HasTips` bit(1) NOT NULL DEFAULT b'0',
  `HasPerformanceTest` bit(1) NOT NULL DEFAULT b'0',
  `MarketsDistributionMsgMultiply` int NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Guid` (`Guid`),
  KEY `CustomerId` (`CustomerId`)
) ENGINE=InnoDB AUTO_INCREMENT=3930 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_customerpackages_insert_trigger` AFTER INSERT ON `customerpackages` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('customerpackages', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_customerpackages_update_trigger` AFTER UPDATE ON `customerpackages` FOR EACH ROW begin
                                      if not(old.totalcredits <=> new.totalcredits) || not(old.externalproviderid <=> new.externalproviderid) || not(old.pushurl <=> new.pushurl) || not(old.oddproviders <=> new.oddproviders) || not(old.locations <=> new.locations) || not(old.markets <=> new.markets) || not(old.sports <=> new.sports) || not(old.leagues <=> new.leagues) || not(old.getallpushevents <=> new.getallpushevents) || not(old.hascards <=> new.hascards) || not(old.hasscores <=> new.hasscores) || not(old.hasscorers <=> new.hasscorers) || not(old.hasstatistics <=> new.hasstatistics) || not(old.hasinplayodds <=> new.hasinplayodds) || not(old.hasprematchodds <=> new.hasprematchodds) || not(old.hasbetslip <=> new.hasbetslip) || not(old.hasaverage <=> new.hasaverage) || not(old.hasstandings <=> new.hasstandings) || not(old.hastvchannels <=> new.hastvchannels) || not(old.hasproviderscores <=> new.hasproviderscores) || not(old.hasitf <=> new.hasitf) || not(old.haspushdownalert <=> new.haspushdownalert) || not(old.hashistoricalstatistics <=> new.hashistoricalstatistics) || not(old.hasfastmarkets <=> new.hasfastmarkets) || not(old.hasresulting <=> new.hasresulting) || not(old.formattype <=> new.formattype) || not(old.hashistoricalodds <=> new.hashistoricalodds) || not(old.hasbetstop <=> new.hasbetstop) || not(old.rmqenabled <=> new.rmqenabled) || not(old.hasoutrights <=> new.hasoutrights) || not(old.hasoutrightleagues <=> new.hasoutrightleagues) || not(old.hasplayermarkets <=> new.hasplayermarkets) || not(old.hastennisstats <=> new.hastennisstats) || not(old.hasfootballstats <=> new.hasfootballstats) || not(old.hastips <=> new.hastips) || not(old.id <=> new.id) || not(old.guid <=> new.guid) || not(old.description <=> new.description) || not(old.customerid <=> new.customerid) || not(old.expirationdate <=> new.expirationdate) || not(old.isactive <=> new.isactive) || not(old.creationdate <=> new.creationdate) || not(old.lastupdate <=> new.lastupdate) || not(old.username <=> new.username) || not(old.password <=> new.password) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('customerpackages', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `history_log_customerpackages` AFTER UPDATE ON `customerpackages` FOR EACH ROW begin
                                      if not(old.totalcredits <=> new.totalcredits) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'totalcredits',old.totalcredits,new.totalcredits,new.id);
                                        end if;
                                                if not(old.externalproviderid <=> new.externalproviderid) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'externalproviderid',old.externalproviderid,new.externalproviderid,new.id);
                                        end if;
                                                if not(old.pushurl <=> new.pushurl) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'pushurl',old.pushurl,new.pushurl,new.id);
                                        end if;
                                                if not(old.oddproviders <=> new.oddproviders) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'oddproviders',old.oddproviders,new.oddproviders,new.id);
                                        end if;
                                                if not(old.locations <=> new.locations) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'locations',old.locations,new.locations,new.id);
                                        end if;
                                                if not(old.markets <=> new.markets) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'markets',old.markets,new.markets,new.id);
                                        end if;
                                                if not(old.sports <=> new.sports) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'sports',old.sports,new.sports,new.id);
                                        end if;
                                                if not(old.leagues <=> new.leagues) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'leagues',old.leagues,new.leagues,new.id);
                                        end if;
                                                if not(old.getallpushevents <=> new.getallpushevents) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'getallpushevents',old.getallpushevents,new.getallpushevents,new.id);
                                        end if;
                                                if not(old.hascards <=> new.hascards) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'hascards',old.hascards,new.hascards,new.id);
                                        end if;
                                                if not(old.hasscores <=> new.hasscores) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'hasscores',old.hasscores,new.hasscores,new.id);
                                        end if;
                                                if not(old.hasscorers <=> new.hasscorers) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'hasscorers',old.hasscorers,new.hasscorers,new.id);
                                        end if;
                                                if not(old.hasstatistics <=> new.hasstatistics) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'hasstatistics',old.hasstatistics,new.hasstatistics,new.id);
                                        end if;
                                                if not(old.hasinplayodds <=> new.hasinplayodds) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'hasinplayodds',old.hasinplayodds,new.hasinplayodds,new.id);
                                        end if;
                                                if not(old.hasprematchodds <=> new.hasprematchodds) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'hasprematchodds',old.hasprematchodds,new.hasprematchodds,new.id);
                                        end if;
                                                if not(old.hasbetslip <=> new.hasbetslip) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'hasbetslip',old.hasbetslip,new.hasbetslip,new.id);
                                        end if;
                                                if not(old.hasaverage <=> new.hasaverage) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'hasaverage',old.hasaverage,new.hasaverage,new.id);
                                        end if;
                                                if not(old.hasstandings <=> new.hasstandings) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'hasstandings',old.hasstandings,new.hasstandings,new.id);
                                        end if;
                                                if not(old.hastvchannels <=> new.hastvchannels) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'hastvchannels',old.hastvchannels,new.hastvchannels,new.id);
                                        end if;
                                                if not(old.hasproviderscores <=> new.hasproviderscores) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'hasproviderscores',old.hasproviderscores,new.hasproviderscores,new.id);
                                        end if;
                                                if not(old.hasitf <=> new.hasitf) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'hasitf',old.hasitf,new.hasitf,new.id);
                                        end if;
                                                if not(old.haspushdownalert <=> new.haspushdownalert) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'haspushdownalert',old.haspushdownalert,new.haspushdownalert,new.id);
                                        end if;
                                                if not(old.hashistoricalstatistics <=> new.hashistoricalstatistics) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'hashistoricalstatistics',old.hashistoricalstatistics,new.hashistoricalstatistics,new.id);
                                        end if;
                                                if not(old.hasfastmarkets <=> new.hasfastmarkets) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'hasfastmarkets',old.hasfastmarkets,new.hasfastmarkets,new.id);
                                        end if;
                                                if not(old.hasresulting <=> new.hasresulting) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'hasresulting',old.hasresulting,new.hasresulting,new.id);
                                        end if;
                                                if not(old.formattype <=> new.formattype) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'formattype',old.formattype,new.formattype,new.id);
                                        end if;
                                                if not(old.hashistoricalodds <=> new.hashistoricalodds) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'hashistoricalodds',old.hashistoricalodds,new.hashistoricalodds,new.id);
                                        end if;
                                                if not(old.hasbetstop <=> new.hasbetstop) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'hasbetstop',old.hasbetstop,new.hasbetstop,new.id);
                                        end if;
                                                if not(old.rmqenabled <=> new.rmqenabled) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'rmqenabled',old.rmqenabled,new.rmqenabled,new.id);
                                        end if;
                                                if not(old.hasoutrights <=> new.hasoutrights) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'hasoutrights',old.hasoutrights,new.hasoutrights,new.id);
                                        end if;
                                                if not(old.hasoutrightleagues <=> new.hasoutrightleagues) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'hasoutrightleagues',old.hasoutrightleagues,new.hasoutrightleagues,new.id);
                                        end if;
                                                if not(old.hasplayermarkets <=> new.hasplayermarkets) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'hasplayermarkets',old.hasplayermarkets,new.hasplayermarkets,new.id);
                                        end if;
                                                if not(old.hastennisstats <=> new.hastennisstats) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'hastennisstats',old.hastennisstats,new.hastennisstats,new.id);
                                        end if;
                                                if not(old.hasfootballstats <=> new.hasfootballstats) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'hasfootballstats',old.hasfootballstats,new.hasfootballstats,new.id);
                                        end if;
                                                if not(old.hastips <=> new.hastips) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'hastips',old.hastips,new.hastips,new.id);
                                        end if;
                                                if not(old.guid <=> new.guid) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'guid',old.guid,new.guid,new.id);
                                        end if;
                                                if not(old.description <=> new.description) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'description',old.description,new.description,new.id);
                                        end if;
                                                if not(old.customerid <=> new.customerid) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'customerid',old.customerid,new.customerid,new.id);
                                        end if;
                                                if not(old.expirationdate <=> new.expirationdate) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'expirationdate',old.expirationdate,new.expirationdate,new.id);
                                        end if;
                                                if not(old.isactive <=> new.isactive) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'isactive',old.isactive,new.isactive,new.id);
                                        end if;
                                                if not(old.creationdate <=> new.creationdate) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'creationdate',old.creationdate,new.creationdate,new.id);
                                        end if;
                                                if not(old.username <=> new.username) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'username',old.username,new.username,new.id);
                                        end if;
                                                if not(old.password <=> new.password) 
                                            then
                                                insert data.customerpackageshistory(lastupdate,columnname,oldvalue,newvalue,changedid)
                                                values (now(),'password',old.password,new.password,new.id);
                                        end if;
                                                
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_customerpackages_delete_trigger` AFTER DELETE ON `customerpackages` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('customerpackages', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `customerpackages_new`
--

DROP TABLE IF EXISTS `customerpackages_new`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `customerpackages_new` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Guid` char(36) NOT NULL,
  `CustomerId` int NOT NULL,
  `ExpirationDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Description` varchar(255) DEFAULT NULL,
  `TotalCredits` int NOT NULL DEFAULT '0',
  `LastUpdate` datetime NOT NULL,
  `OddProviders` text NOT NULL,
  `Sports` text NOT NULL,
  `Locations` text NOT NULL,
  `Markets` text NOT NULL,
  `Leagues` text NOT NULL,
  `IsActive` bit(1) NOT NULL DEFAULT b'0',
  `Username` varchar(255) NOT NULL,
  `Password` varchar(255) NOT NULL,
  `CreationDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `FormatType` int NOT NULL DEFAULT '1',
  PRIMARY KEY (`Id`),
  KEY `customerpackagesnew_customerid` (`CustomerId`)
) ENGINE=InnoDB AUTO_INCREMENT=1199 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `customerpackagesettings`
--

DROP TABLE IF EXISTS `customerpackagesettings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `customerpackagesettings` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `CustomerPackagesId` int NOT NULL,
  `Key` int NOT NULL,
  `Value` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `customerpackagesettings_packageid_key_unq` (`CustomerPackagesId`,`Key`),
  KEY `customerpackagesettings_packageId_indx` (`CustomerPackagesId`) USING BTREE,
  KEY `customerpackagesettings_key_indx` (`Key`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=6373 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `customerpackagesettingtypes`
--

DROP TABLE IF EXISTS `customerpackagesettingtypes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `customerpackagesettingtypes` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Type` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `customerpackageshistory`
--

DROP TABLE IF EXISTS `customerpackageshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `customerpackageshistory` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `LastUpdate` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `ColumnName` varchar(255) NOT NULL,
  `NewValue` varchar(255) DEFAULT NULL,
  `OldValue` varchar(255) DEFAULT NULL,
  `ChangedId` int NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=615146 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `disabledfixtures`
--

DROP TABLE IF EXISTS `disabledfixtures`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `disabledfixtures` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `RobotId` int NOT NULL,
  `FixtureId` int NOT NULL,
  `UserId` int DEFAULT NULL,
  `DisabledFixtureType` int NOT NULL,
  `IsDisabled` tinyint(1) NOT NULL,
  `Comment` varchar(255) DEFAULT NULL,
  `LastUpdate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `UserName` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `ProviderId` (`RobotId`) USING BTREE,
  KEY `disabledfixtures_ibfk_2` (`DisabledFixtureType`) USING BTREE,
  KEY `FixtureId` (`FixtureId`) USING BTREE,
  CONSTRAINT `disabledfixtures_ibfk_1` FOREIGN KEY (`RobotId`) REFERENCES `robots` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `disabledfixtures_ibfk_2` FOREIGN KEY (`DisabledFixtureType`) REFERENCES `disabledfixturetypes` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=476 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_disabledfixtures_insert_trigger` AFTER INSERT ON `disabledfixtures` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('disabledfixtures', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_disabledfixtures_update_trigger` AFTER UPDATE ON `disabledfixtures` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.robotid <=> new.robotid) || not(old.userid <=> new.userid) || not(old.disabledfixturetype <=> new.disabledfixturetype) || not(old.isdisabled <=> new.isdisabled) || not(old.comment <=> new.comment) || not(old.lastupdate <=> new.lastupdate) || not(old.fixtureid <=> new.fixtureid) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('disabledfixtures', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_disabledfixtures_delete_trigger` AFTER DELETE ON `disabledfixtures` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('disabledfixtures', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `disabledfixturetypes`
--

DROP TABLE IF EXISTS `disabledfixturetypes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `disabledfixturetypes` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(25) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `duplicated_ids`
--

DROP TABLE IF EXISTS `duplicated_ids`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `duplicated_ids` (
  `Id` int NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `esportslivescoreproviderpriorities`
--

DROP TABLE IF EXISTS `esportslivescoreproviderpriorities`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `esportslivescoreproviderpriorities` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ProviderId` int NOT NULL,
  `SportId` int NOT NULL,
  `EndEventsAsSingleProvider` bit(1) NOT NULL DEFAULT b'1',
  `MainPriority` int NOT NULL,
  `ScoresPriority` int NOT NULL DEFAULT '-1',
  `ScorePlayersPriority` int NOT NULL DEFAULT '-1',
  `IncidentPlayersPriority` int NOT NULL DEFAULT '-1',
  `SecondsPriority` int NOT NULL DEFAULT '-1',
  `StatusDescriptionPriority` int NOT NULL DEFAULT '-1',
  `StatusPriority` int NOT NULL DEFAULT '-1',
  `LeagueIds` varchar(5000) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_LivescoreProviderPriorities_UQ` (`ProviderId`,`SportId`) USING BTREE,
  KEY `ProviderId` (`ProviderId`) USING BTREE,
  KEY `SportId` (`SportId`) USING BTREE,
  CONSTRAINT `esportslivescoreproviderpriorities_ibfk_1` FOREIGN KEY (`SportId`) REFERENCES `sports` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=221 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_esportslivescoreproviderpriorities_insert_trigger` AFTER INSERT ON `esportslivescoreproviderpriorities` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('esportslivescoreproviderpriorities', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_esportslivescoreproviderpriorities_update_trigger` AFTER UPDATE ON `esportslivescoreproviderpriorities` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.providerid <=> new.providerid) || not(old.sportid <=> new.sportid) || not(old.mainpriority <=> new.mainpriority) || not(old.scorespriority <=> new.scorespriority) || not(old.scoreplayerspriority <=> new.scoreplayerspriority) || not(old.secondspriority <=> new.secondspriority) || not(old.statusdescriptionpriority <=> new.statusdescriptionpriority) || not(old.statuspriority <=> new.statuspriority) || not(old.endeventsassingleprovider <=> new.endeventsassingleprovider) || not(old.incidentplayerspriority <=> new.incidentplayerspriority) || not(old.leagueids <=> new.leagueids) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('esportslivescoreproviderpriorities', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_esportslivescoreproviderpriorities_delete_trigger` AFTER DELETE ON `esportslivescoreproviderpriorities` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('esportslivescoreproviderpriorities', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `extradatacoverage`
--

DROP TABLE IF EXISTS `extradatacoverage`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `extradatacoverage` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `LeagueId` int NOT NULL,
  `ExtraDataType` varchar(255) NOT NULL,
  `ExtraDataValue` varchar(255) NOT NULL,
  `PeriodId` int DEFAULT NULL,
  `ProviderId` int NOT NULL,
  `LastUpdate` datetime NOT NULL,
  `CreationDate` datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `reference_unique` (`LeagueId`,`PeriodId`,`ProviderId`,`ExtraDataType`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=2259264825 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `extradatakeymappings`
--

DROP TABLE IF EXISTS `extradatakeymappings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `extradatakeymappings` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `MainId` int DEFAULT NULL,
  `Name` varchar(50) NOT NULL,
  `IsActive` tinyint unsigned NOT NULL DEFAULT '0',
  `CreationDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `Name_MainId` (`MainId`,`Name`) USING BTREE,
  KEY `extradatamappings_ibfk_1` (`MainId`) USING BTREE,
  KEY `Name_IsActive` (`Name`,`IsActive`) USING BTREE,
  CONSTRAINT `extradatakeymappings_ibfk_1` FOREIGN KEY (`MainId`) REFERENCES `extradatakeys` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=659483 DEFAULT CHARSET=utf8mb3 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_extradatakeymappings_insert_trigger` AFTER INSERT ON `extradatakeymappings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('extradatakeymappings', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_extradatakeymappings_update_trigger` AFTER UPDATE ON `extradatakeymappings` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.mainid <=> new.mainid) || not(old.name <=> new.name) || not(old.isactive <=> new.isactive) || not(old.creationdate <=> new.creationdate) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('extradatakeymappings', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_extradatakeymappings_delete_trigger` AFTER DELETE ON `extradatakeymappings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('extradatakeymappings', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `extradatakeys`
--

DROP TABLE IF EXISTS `extradatakeys`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `extradatakeys` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  `CreationDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `IsActive` bit(1) DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `Name` (`Name`) USING BTREE,
  KEY `IsActive` (`IsActive`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=786 DEFAULT CHARSET=utf8mb3 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_extradatakeys_insert_trigger` AFTER INSERT ON `extradatakeys` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('extradatakeys', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_extradatakeys_update_trigger` AFTER UPDATE ON `extradatakeys` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.name <=> new.name) || not(old.creationdate <=> new.creationdate) || not(old.isactive <=> new.isactive) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('extradatakeys', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_extradatakeys_delete_trigger` AFTER DELETE ON `extradatakeys` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('extradatakeys', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `fastmarketorders`
--

DROP TABLE IF EXISTS `fastmarketorders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `fastmarketorders` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FixtureId` int NOT NULL,
  `CustomerPackageId` int NOT NULL,
  `MarketId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_FastMarketOrders_Fixtures_idx` (`FixtureId`),
  KEY `FK_FastMarketOrders_Customer_idx` (`CustomerPackageId`),
  KEY `IX_FastMarketOrdersByCustomer` (`CustomerPackageId`,`FixtureId`,`MarketId`),
  CONSTRAINT `fastmarketorders_ibfk_1` FOREIGN KEY (`CustomerPackageId`) REFERENCES `customerpackages` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `FK_FastMarketOrders_Fixtures` FOREIGN KEY (`FixtureId`) REFERENCES `fixtures` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=70 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `fixturebets`
--

DROP TABLE IF EXISTS `fixturebets`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `fixturebets` (
  `Id` bigint NOT NULL,
  `FixtureMarketId` int NOT NULL,
  `ProviderBetId` varchar(500) NOT NULL,
  `BetStatusId` int NOT NULL,
  `SettlementId` int DEFAULT NULL,
  `Name` varchar(50) NOT NULL,
  `Line` varchar(10) DEFAULT NULL,
  `BaseLine` varchar(15) DEFAULT NULL,
  `StartPrice` double NOT NULL,
  `CurrentPrice` double NOT NULL,
  `PriceVolume` double DEFAULT NULL,
  `CurrentLayPrice` double DEFAULT NULL,
  `LayPriceVolume` double DEFAULT NULL,
  `CreationDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpdate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `BetStatusId` (`BetStatusId`) USING BTREE,
  KEY `FixtureMarketId` (`FixtureMarketId`) USING BTREE,
  CONSTRAINT `fixturebets_ibfk_1` FOREIGN KEY (`BetStatusId`) REFERENCES `betstatuses` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `fixturebets_ibfk_2` FOREIGN KEY (`FixtureMarketId`) REFERENCES `fixturemarkets` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `fixturecompetitions`
--

DROP TABLE IF EXISTS `fixturecompetitions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `fixturecompetitions` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FixtureId` int NOT NULL,
  `CompetitionId` int NOT NULL,
  `IsActive` bit(1) NOT NULL,
  `CreationDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpdate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `IsActive_Int` tinyint(1) GENERATED ALWAYS AS (cast(`IsActive` as unsigned)) VIRTUAL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_UQ` (`FixtureId`) USING BTREE,
  KEY `IX_COMPETITIONID` (`CompetitionId`) USING BTREE,
  KEY `IX_ISACTIVE` (`IsActive`) USING BTREE,
  KEY `IX_CREATIONDATE` (`CreationDate`) USING BTREE,
  KEY `IX_LASTUPDATE` (`LastUpdate`) USING BTREE,
  KEY `IX_FIXTUREID_COMPETITIONID` (`FixtureId`,`CompetitionId`) USING BTREE,
  KEY `IX_FIXTUREID_LASTUPDATE` (`FixtureId`,`LastUpdate`) USING BTREE,
  CONSTRAINT `fk_fixturecompetitions_competition` FOREIGN KEY (`CompetitionId`) REFERENCES `competitionnodes` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `fk_fixturecompetitions_fixtureid` FOREIGN KEY (`FixtureId`) REFERENCES `fixtures` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=1041765 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixturecompetitions_insert_trigger` AFTER INSERT ON `fixturecompetitions` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('fixturecompetitions', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `fixture_reflastupdate_fixturecompetitions_insert_trigger` AFTER INSERT ON `fixturecompetitions` FOR EACH ROW begin update fixtures set referenceslastupdate=now() where id=new.fixtureid; end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixturecompetitions_update_trigger` AFTER UPDATE ON `fixturecompetitions` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.competitionid <=> new.competitionid) || not(old.isactive <=> new.isactive) || not(old.creationdate <=> new.creationdate) || not(old.lastupdate <=> new.lastupdate) || not(old.fixtureid <=> new.fixtureid) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('fixturecompetitions', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `fixture_reflastupdate_fixturecompetitions_update_trigger` AFTER UPDATE ON `fixturecompetitions` FOR EACH ROW begin update fixtures set referenceslastupdate=now() where id=new.fixtureid; end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixturecompetitions_delete_trigger` AFTER DELETE ON `fixturecompetitions` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('fixturecompetitions', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `fixture_reflastupdate_fixturecompetitions_delete_trigger` AFTER DELETE ON `fixturecompetitions` FOR EACH ROW begin update fixtures set referenceslastupdate=now() where id=old.fixtureid; end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `fixtureenddate`
--

DROP TABLE IF EXISTS `fixtureenddate`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `fixtureenddate` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FixtureId` int NOT NULL,
  `EndDate` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `fixtureid_fk` (`FixtureId`),
  CONSTRAINT `fixtureid_fk` FOREIGN KEY (`FixtureId`) REFERENCES `fixtures` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=107 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `fixtureextradata`
--

DROP TABLE IF EXISTS `fixtureextradata`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `fixtureextradata` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  `Value` varchar(255) NOT NULL,
  `FixtureId` int NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `FixtureId_Name` (`Name`,`FixtureId`) USING BTREE,
  KEY `FixtureId` (`FixtureId`) USING BTREE,
  CONSTRAINT `fixtureextradata_ibfk_1` FOREIGN KEY (`FixtureId`) REFERENCES `fixtures` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=11090245 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixtureextradata_insert_trigger` AFTER INSERT ON `fixtureextradata` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('fixtureextradata', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `fixture_reflastupdate_fixtureextradata_insert_trigger` AFTER INSERT ON `fixtureextradata` FOR EACH ROW begin update fixtures set referenceslastupdate=now() where id=new.fixtureid; end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixtureextradata_update_trigger` AFTER UPDATE ON `fixtureextradata` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.name <=> new.name) || not(old.value <=> new.value) || not(old.fixtureid <=> new.fixtureid) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('fixtureextradata', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `fixture_reflastupdate_fixtureextradata_update_trigger` AFTER UPDATE ON `fixtureextradata` FOR EACH ROW begin update fixtures set referenceslastupdate=now() where id=new.fixtureid; end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixtureextradata_delete_trigger` AFTER DELETE ON `fixtureextradata` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('fixtureextradata', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `fixture_reflastupdate_fixtureextradata_delete_trigger` AFTER DELETE ON `fixtureextradata` FOR EACH ROW begin update fixtures set referenceslastupdate=now() where id=old.fixtureid; end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `fixturefieldconflicts`
--

DROP TABLE IF EXISTS `fixturefieldconflicts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `fixturefieldconflicts` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FixtureId` int NOT NULL,
  `Field` varchar(25) NOT NULL,
  `CurrentValue` varchar(50) NOT NULL,
  `ConflictingValues` varchar(255) NOT NULL,
  `LastUpdate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `IX_FixtureId` (`FixtureId`) USING BTREE,
  KEY `IX_FixtureId_Field` (`FixtureId`,`Field`) USING BTREE,
  CONSTRAINT `fixturefieldconflicts_ibfk_1` FOREIGN KEY (`FixtureId`) REFERENCES `fixtures` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixturefieldconflicts_insert_trigger` AFTER INSERT ON `fixturefieldconflicts` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('fixturefieldconflicts', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixturefieldconflicts_update_trigger` AFTER UPDATE ON `fixturefieldconflicts` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.fixtureid <=> new.fixtureid) || not(old.field <=> new.field) || not(old.currentvalue <=> new.currentvalue) || not(old.conflictingvalues <=> new.conflictingvalues) || not(old.lastupdate <=> new.lastupdate) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('fixturefieldconflicts', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixturefieldconflicts_delete_trigger` AFTER DELETE ON `fixturefieldconflicts` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('fixturefieldconflicts', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `fixturehistory`
--

DROP TABLE IF EXISTS `fixturehistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `fixturehistory` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ProviderId` int DEFAULT NULL,
  `FixtureId` int NOT NULL,
  `ChangedField` varchar(50) NOT NULL,
  `NewValue` varchar(25) NOT NULL,
  `OldValue` varchar(25) NOT NULL,
  `ChangedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `FixtureId` (`FixtureId`) USING BTREE,
  KEY `ProviderId` (`ProviderId`) USING BTREE,
  CONSTRAINT `fixturehistory_ibfk_1` FOREIGN KEY (`FixtureId`) REFERENCES `fixtures` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `fixturehistory_ibfk_2` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=29472446 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `fixtureidmapping`
--

DROP TABLE IF EXISTS `fixtureidmapping`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `fixtureidmapping` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FixtureId` int NOT NULL,
  `ProviderFixtureId` varchar(36) NOT NULL,
  `ProviderId` int NOT NULL,
  `IsSwapped` bit(1) DEFAULT NULL,
  `CreationDate` datetime(6) DEFAULT NULL,
  `RobotId` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_FixtureMapping_UQ_Combo` (`FixtureId`,`ProviderFixtureId`,`ProviderId`,`RobotId`),
  KEY `FixtureId` (`FixtureId`) USING BTREE,
  KEY `ProviderId` (`ProviderId`) USING BTREE,
  KEY `IX_FixtureMapping_ProviderFixture` (`ProviderFixtureId`,`ProviderId`) USING BTREE,
  CONSTRAINT `fixtureidmapping_ibfk_1` FOREIGN KEY (`FixtureId`) REFERENCES `fixtures` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT,
  CONSTRAINT `fixtureidmapping_ibfk_2` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=27189469 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixtureidmapping_insert_trigger` AFTER INSERT ON `fixtureidmapping` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('fixtureidmapping', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixtureidmapping_update_trigger` AFTER UPDATE ON `fixtureidmapping` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.providerfixtureid <=> new.providerfixtureid) || not(old.providerid <=> new.providerid) || not(old.isswapped <=> new.isswapped) || not(old.creationdate <=> new.creationdate) || not(old.robotid <=> new.robotid) || not(old.fixtureid <=> new.fixtureid) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('fixtureidmapping', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixtureidmapping_delete_trigger` AFTER DELETE ON `fixtureidmapping` FOR EACH ROW begin
   insert data.trackedchanges(tablename,operationtype,changedid) values ('fixtureidmapping', 3, old.id);
end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `fixtureidmapping_full`
--

DROP TABLE IF EXISTS `fixtureidmapping_full`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `fixtureidmapping_full` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FixtureId` int NOT NULL,
  `ProviderFixtureId` varchar(36) NOT NULL,
  `ProviderId` int NOT NULL,
  `IsSwapped` bit(1) DEFAULT NULL,
  `CreationDate` datetime(6) DEFAULT NULL,
  `RobotId` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FixtureId` (`FixtureId`) USING BTREE,
  KEY `ProviderId` (`ProviderId`) USING BTREE,
  KEY `IX_FixtureMapping_ProviderFixture` (`ProviderFixtureId`,`ProviderId`) USING BTREE,
  CONSTRAINT `fixtureidmapping_full_ibfk_1` FOREIGN KEY (`FixtureId`) REFERENCES `fixtures` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT,
  CONSTRAINT `fixtureidmapping_full_ibfk_2` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=6947466 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `fixtureindications`
--

DROP TABLE IF EXISTS `fixtureindications`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `fixtureindications` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `FixtureId` int unsigned NOT NULL,
  `WasInProgress` bit(1) NOT NULL,
  `CreationDate` datetime NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  KEY `FixtureId` (`FixtureId`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=2384476 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixtureindications_insert_trigger` AFTER INSERT ON `fixtureindications` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('fixtureindications', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixtureindications_update_trigger` AFTER UPDATE ON `fixtureindications` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.wasinprogress <=> new.wasinprogress) || not(old.creationdate <=> new.creationdate) || not(old.fixtureid <=> new.fixtureid) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('fixtureindications', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixtureindications_delete_trigger` AFTER DELETE ON `fixtureindications` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('fixtureindications', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `fixturelivescore`
--

DROP TABLE IF EXISTS `fixturelivescore`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `fixturelivescore` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FixtureId` int NOT NULL,
  `PeriodTypeId` int DEFAULT NULL,
  `HomeValue` varchar(50) DEFAULT NULL,
  `AwayValue` varchar(50) DEFAULT NULL,
  `Seconds` int NOT NULL,
  `StatusDescriptionId` int NOT NULL,
  `LastUpdate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `FixtureID` (`FixtureId`) USING BTREE,
  KEY `PeriodTypeID` (`PeriodTypeId`) USING BTREE,
  KEY `StatusDescriptionId` (`StatusDescriptionId`) USING BTREE,
  CONSTRAINT `fixturelivescore_ibfk_1` FOREIGN KEY (`FixtureId`) REFERENCES `fixtures` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `fixturelivescore_ibfk_2` FOREIGN KEY (`PeriodTypeId`) REFERENCES `periodtypes` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `fixturelivescore_ibfk_3` FOREIGN KEY (`StatusDescriptionId`) REFERENCES `statusdescriptions` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=632262 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `fixturemanagersettings`
--

DROP TABLE IF EXISTS `fixturemanagersettings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `fixturemanagersettings` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `SettingLevel` int NOT NULL,
  `SettingType` int NOT NULL,
  `Key` int DEFAULT NULL,
  `Value` varchar(255) NOT NULL,
  `Description` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_UQ` (`SettingLevel`,`SettingType`,`Key`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=2138 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixturemanagersettings_insert_trigger` AFTER INSERT ON `fixturemanagersettings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('fixturemanagersettings', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixturemanagersettings_update_trigger` AFTER UPDATE ON `fixturemanagersettings` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.settinglevel <=> new.settinglevel) || not(old.settingtype <=> new.settingtype) || not(old.key <=> new.key) || not(old.value <=> new.value) || not(old.description <=> new.description) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('fixturemanagersettings', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixturemanagersettings_delete_trigger` AFTER DELETE ON `fixturemanagersettings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('fixturemanagersettings', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `fixturemarkets`
--

DROP TABLE IF EXISTS `fixturemarkets`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `fixturemarkets` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FixtureId` int NOT NULL,
  `MarketId` int DEFAULT NULL,
  `ProviderId` int DEFAULT NULL,
  `RobotId` int DEFAULT NULL,
  `ProviderFixtureId` varchar(36) DEFAULT NULL,
  `ProviderLeagueId` varchar(25) DEFAULT NULL,
  `ProviderMarketId` varchar(25) DEFAULT NULL,
  `CreationDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpdate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `FixtureId` (`FixtureId`) USING BTREE,
  KEY `MarketId` (`MarketId`) USING BTREE,
  KEY `ProviderId` (`ProviderId`) USING BTREE,
  KEY `RobotId` (`RobotId`) USING BTREE,
  CONSTRAINT `fixturemarkets_ibfk_1` FOREIGN KEY (`FixtureId`) REFERENCES `fixtures` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `fixturemarkets_ibfk_2` FOREIGN KEY (`MarketId`) REFERENCES `markets` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `fixturemarkets_ibfk_3` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `fixturemarkets_ibfk_4` FOREIGN KEY (`RobotId`) REFERENCES `robots` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `fixtureorders`
--

DROP TABLE IF EXISTS `fixtureorders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `fixtureorders` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FixtureId` int NOT NULL,
  `CustomerId` int NOT NULL,
  `ProviderId` int NOT NULL,
  `PackageId` int DEFAULT NULL,
  `IsActive` bit(1) NOT NULL,
  `CreationDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpdate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `IsAutoAdded` bit(1) NOT NULL,
  `IsInPlay` bit(1) DEFAULT NULL,
  `IsFeedStopped` bit(1) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_FixtureOrder_UQ` (`FixtureId`,`CustomerId`,`ProviderId`,`PackageId`),
  KEY `FixtureId` (`FixtureId`) USING BTREE,
  KEY `ProviderId` (`ProviderId`) USING BTREE,
  KEY `Customer_orders` (`PackageId`,`IsActive`,`CreationDate`) USING BTREE,
  CONSTRAINT `fixtureorders_ibfk_1` FOREIGN KEY (`FixtureId`) REFERENCES `fixtures` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `fixtureorders_ibfk_2` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `fixtureorders_ibfk_3` FOREIGN KEY (`PackageId`) REFERENCES `customerpackages` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=24123300 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixtureorders_insert_trigger` AFTER INSERT ON `fixtureorders` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('fixtureorders', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixtureorders_update_trigger` AFTER UPDATE ON `fixtureorders` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.customerid <=> new.customerid) || not(old.providerid <=> new.providerid) || not(old.isactive <=> new.isactive) || not(old.creationdate <=> new.creationdate) || not(old.lastupdate <=> new.lastupdate) || not(old.isautoadded <=> new.isautoadded) || not(old.packageid <=> new.packageid) || not(old.isfeedstopped <=> new.isfeedstopped) || not(old.fixtureid <=> new.fixtureid) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('fixtureorders', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `fixtureparticipanthistory`
--

DROP TABLE IF EXISTS `fixtureparticipanthistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `fixtureparticipanthistory` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FixtureParticipantId` int NOT NULL,
  `ChangedField` varchar(50) NOT NULL,
  `OldValue` varchar(150) DEFAULT NULL,
  `NewValue` varchar(150) NOT NULL,
  `ChangedBy` varchar(50) NOT NULL,
  `ChangeDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `IX_FixtueParticipantId` (`FixtureParticipantId`) USING BTREE,
  KEY `IX_FixtureParticipant_Field` (`FixtureParticipantId`,`ChangedField`) USING BTREE,
  CONSTRAINT `fk_fixtureparticipanthistory_fixtureparticipantid` FOREIGN KEY (`FixtureParticipantId`) REFERENCES `fixtureparticipants` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=12525978 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `fixtureparticipantmappings`
--

DROP TABLE IF EXISTS `fixtureparticipantmappings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `fixtureparticipantmappings` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FixtureId` int DEFAULT NULL,
  `HomeName` varchar(255) DEFAULT NULL,
  `AwayName` varchar(255) DEFAULT NULL,
  `ProviderId` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FixtureId` (`FixtureId`) USING BTREE,
  KEY `ProviderId` (`ProviderId`) USING BTREE,
  CONSTRAINT `fixtureparticipantmappings_ibfk_1` FOREIGN KEY (`FixtureId`) REFERENCES `fixtures` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `fixtureparticipantmappings_ibfk_2` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixtureparticipantmappings_insert_trigger` AFTER INSERT ON `fixtureparticipantmappings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('fixtureparticipantmappings', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixtureparticipantmappings_update_trigger` AFTER UPDATE ON `fixtureparticipantmappings` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.homename <=> new.homename) || not(old.awayname <=> new.awayname) || not(old.providerid <=> new.providerid) || not(old.fixtureid <=> new.fixtureid) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('fixtureparticipantmappings', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixtureparticipantmappings_delete_trigger` AFTER DELETE ON `fixtureparticipantmappings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('fixtureparticipantmappings', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `fixtureparticipants`
--

DROP TABLE IF EXISTS `fixtureparticipants`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `fixtureparticipants` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FixtureId` int NOT NULL,
  `ParticipantId` int NOT NULL,
  `Position` varchar(11) DEFAULT NULL,
  `RotationId` int DEFAULT NULL,
  `IsActive` bit(1) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_UQ` (`FixtureId`,`ParticipantId`) USING BTREE,
  KEY `FixtureId` (`FixtureId`) USING BTREE,
  KEY `ParticipantId` (`ParticipantId`) USING BTREE,
  CONSTRAINT `fixtureparticipants_ibfk_1` FOREIGN KEY (`FixtureId`) REFERENCES `fixtures` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT,
  CONSTRAINT `fixtureparticipants_ibfk_2` FOREIGN KEY (`ParticipantId`) REFERENCES `participants` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=35293240 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixtureparticipants_insert_trigger` AFTER INSERT ON `fixtureparticipants` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('fixtureparticipants', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `fixture_reflastupdate_fixtureparticipants_insert_trigger` AFTER INSERT ON `fixtureparticipants` FOR EACH ROW begin update fixtures set referenceslastupdate=now() where id=new.fixtureid; end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixtureparticipants_update_trigger` AFTER UPDATE ON `fixtureparticipants` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.participantid <=> new.participantid) || not(old.position <=> new.position) || not(old.rotationid <=> new.rotationid) || not(old.isactive <=> new.isactive) || not(old.fixtureid <=> new.fixtureid) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('fixtureparticipants', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `fixture_reflastupdate_fixtureparticipants_update_trigger` AFTER UPDATE ON `fixtureparticipants` FOR EACH ROW begin update fixtures set referenceslastupdate=now() where id=new.fixtureid; end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixtureparticipants_delete_trigger` AFTER DELETE ON `fixtureparticipants` FOR EACH ROW begin
                                     insert data.trackedchanges(tablename,operationtype,changedid)
                                 values ('fixtureparticipants', 3, old.id);
end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `fixture_reflastupdate_fixtureparticipants_delete_trigger` AFTER DELETE ON `fixtureparticipants` FOR EACH ROW begin update fixtures set referenceslastupdate=now() where id=old.fixtureid; end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `fixtureperiods`
--

DROP TABLE IF EXISTS `fixtureperiods`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `fixtureperiods` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FixtureId` int DEFAULT NULL,
  `PeriodTypeId` int DEFAULT NULL,
  `ParentId` int DEFAULT NULL,
  `SequenceNumber` int DEFAULT NULL,
  `StartTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `HomeValue` varchar(50) NOT NULL,
  `AwayValue` varchar(50) NOT NULL,
  `IsConfirmed` tinyint(1) NOT NULL,
  `IsFinished` tinyint(1) DEFAULT NULL,
  `LastUpdate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `FixtureId` (`FixtureId`) USING BTREE,
  CONSTRAINT `fixtureperiods_ibfk_1` FOREIGN KEY (`FixtureId`) REFERENCES `fixtures` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=1959333 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `fixtures`
--

DROP TABLE IF EXISTS `fixtures`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `fixtures` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ProviderId` int NOT NULL,
  `LocationId` int NOT NULL,
  `LeagueId` int NOT NULL,
  `SportId` int NOT NULL,
  `StatusId` int NOT NULL,
  `StartDate` datetime(6) DEFAULT NULL,
  `LastUpdate` datetime(6) DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP(6),
  `CreationDate` datetime(6) NOT NULL,
  `SeasonId` int DEFAULT NULL,
  `IsOutright` bit(1) NOT NULL DEFAULT b'0',
  `Source` int DEFAULT '1',
  `Name` varchar(255) DEFAULT NULL,
  `ReferencesLastUpdate` datetime(6) DEFAULT NULL,
  `EndDate` datetime(6) DEFAULT NULL,
  `CityId` int DEFAULT NULL,
  `StateId` int DEFAULT NULL,
  `CountryId` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `ProviderId` (`ProviderId`) USING BTREE,
  KEY `LocationId` (`LocationId`) USING BTREE,
  KEY `LeagueId` (`LeagueId`) USING BTREE,
  KEY `SportId` (`SportId`) USING BTREE,
  KEY `StatusId` (`StatusId`) USING BTREE,
  KEY `idx_fixtures_LeagueId_StatusId` (`LeagueId`,`StatusId`),
  KEY `SportId_Status_StartDate` (`StartDate`,`StatusId`,`SportId`) USING BTREE,
  KEY `LastUpdate` (`LastUpdate`),
  KEY `Status_StartDate` (`StatusId`,`StartDate`) USING BTREE,
  KEY `IX_StartDate` (`StartDate`,`LocationId`,`LeagueId`,`SportId`,`StatusId`) USING BTREE,
  KEY `fixtures_SportId_IDX` (`SportId`,`StartDate`) USING BTREE,
  CONSTRAINT `fixtures_ibfk_1` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `fixtures_ibfk_2` FOREIGN KEY (`LocationId`) REFERENCES `locations` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `fixtures_ibfk_3` FOREIGN KEY (`LeagueId`) REFERENCES `leagues` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `fixtures_ibfk_4` FOREIGN KEY (`SportId`) REFERENCES `sports` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `fixtures_ibfk_5` FOREIGN KEY (`StatusId`) REFERENCES `fixturestatuses` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=23515182 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixtures_insert_trigger` AFTER INSERT ON `fixtures` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('fixtures', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixtures_update_trigger` AFTER UPDATE ON `fixtures` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.providerid <=> new.providerid) || not(old.locationid <=> new.locationid) || not(old.leagueid <=> new.leagueid) || not(old.sportid <=> new.sportid) || not(old.statusid <=> new.statusid) || not(old.startdate <=> new.startdate) || not(old.lastupdate <=> new.lastupdate) || not(old.creationdate <=> new.creationdate) || not(old.seasonid <=> new.seasonid) || not(old.isoutright <=> new.isoutright) || not(old.source <=> new.source) || not(old.name <=> new.name) || not(old.referenceslastupdate <=> new.referenceslastupdate) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('fixtures', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixtures_delete_trigger` AFTER DELETE ON `fixtures` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('fixtures', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `fixturestatistics`
--

DROP TABLE IF EXISTS `fixturestatistics`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `fixturestatistics` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FixtureId` int DEFAULT NULL,
  `PeriodTypeId` int DEFAULT NULL,
  `IncidentTypeId` int DEFAULT NULL,
  `HomeValue` varchar(10) DEFAULT NULL,
  `AwayValue` varchar(10) DEFAULT NULL,
  `LastUpdate` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FixtureID` (`FixtureId`) USING BTREE,
  KEY `IncidentTypeID` (`IncidentTypeId`) USING BTREE,
  KEY `PeriodTypeId` (`PeriodTypeId`) USING BTREE,
  CONSTRAINT `fixturestatistics_ibfk_1` FOREIGN KEY (`FixtureId`) REFERENCES `fixtures` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `fixturestatistics_ibfk_2` FOREIGN KEY (`FixtureId`) REFERENCES `fixtures` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `fixturestatistics_ibfk_3` FOREIGN KEY (`IncidentTypeId`) REFERENCES `incidenttypes` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `fixturestatistics_ibfk_4` FOREIGN KEY (`PeriodTypeId`) REFERENCES `periodtypes` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=4041703 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `fixturestatuscoverage`
--

DROP TABLE IF EXISTS `fixturestatuscoverage`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `fixturestatuscoverage` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `LeagueId` int NOT NULL,
  `StatusId` int NOT NULL,
  `ProviderId` int NOT NULL,
  `LastUpdate` datetime NOT NULL,
  `CreationDate` datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `reference_unique` (`LeagueId`,`StatusId`,`ProviderId`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=948128992 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `fixturestatuses`
--

DROP TABLE IF EXISTS `fixturestatuses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `fixturestatuses` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(25) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `fixturesubscriptions`
--

DROP TABLE IF EXISTS `fixturesubscriptions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `fixturesubscriptions` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `RobotId` int DEFAULT NULL,
  `SportId` int DEFAULT NULL,
  `LeagueId` int DEFAULT NULL,
  `LocationId` int DEFAULT NULL,
  `IsActive` tinyint(1) DEFAULT NULL,
  `AutoAddFixture` tinyint(1) DEFAULT NULL,
  `AutoAddParticipant` tinyint(1) DEFAULT NULL,
  `AutoAddLeague` tinyint(1) DEFAULT NULL,
  `IsOutrightOnly` tinyint(1) DEFAULT NULL,
  `AddAllFromSport` tinyint(1) DEFAULT NULL,
  `AddAllFromLocation` tinyint(1) DEFAULT NULL,
  `UserAdded` varchar(15) DEFAULT NULL,
  `CreationDate` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `RobotId` (`RobotId`) USING BTREE,
  KEY `SportId` (`SportId`) USING BTREE,
  KEY `LeagueId` (`LeagueId`) USING BTREE,
  KEY `LocationId` (`LocationId`) USING BTREE,
  CONSTRAINT `fixturesubscriptions_ibfk_1` FOREIGN KEY (`RobotId`) REFERENCES `robots` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `fixturesubscriptions_ibfk_2` FOREIGN KEY (`SportId`) REFERENCES `sports` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `fixturesubscriptions_ibfk_3` FOREIGN KEY (`LeagueId`) REFERENCES `leagues` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `fixturesubscriptions_ibfk_4` FOREIGN KEY (`LocationId`) REFERENCES `locations` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixturesubscriptions_insert_trigger` AFTER INSERT ON `fixturesubscriptions` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('fixturesubscriptions', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixturesubscriptions_update_trigger` AFTER UPDATE ON `fixturesubscriptions` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.robotid <=> new.robotid) || not(old.sportid <=> new.sportid) || not(old.leagueid <=> new.leagueid) || not(old.locationid <=> new.locationid) || not(old.isactive <=> new.isactive) || not(old.autoaddfixture <=> new.autoaddfixture) || not(old.autoaddleague <=> new.autoaddleague) || not(old.autoaddparticipant <=> new.autoaddparticipant) || not(old.isoutrightonly <=> new.isoutrightonly) || not(old.useradded <=> new.useradded) || not(old.creationdate <=> new.creationdate) || not(old.addallfromsport <=> new.addallfromsport) || not(old.addallfromlocation <=> new.addallfromlocation) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('fixturesubscriptions', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixturesubscriptions_delete_trigger` AFTER DELETE ON `fixturesubscriptions` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('fixturesubscriptions', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `fixturevenues`
--

DROP TABLE IF EXISTS `fixturevenues`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `fixturevenues` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FixtureId` int NOT NULL,
  `VenueId` int NOT NULL,
  `Capacity` int DEFAULT NULL,
  `Attendance` int DEFAULT NULL,
  `CourtSurfaceType` int DEFAULT NULL,
  `Environment` int DEFAULT NULL,
  `Assignment` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FixtureId` (`FixtureId`),
  KEY `VenueId` (`VenueId`),
  CONSTRAINT `fixturevenues_ibfk_1` FOREIGN KEY (`FixtureId`) REFERENCES `fixtures` (`Id`),
  CONSTRAINT `fixturevenues_ibfk_2` FOREIGN KEY (`VenueId`) REFERENCES `venues` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `formattypes`
--

DROP TABLE IF EXISTS `formattypes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `formattypes` (
  `Id` int NOT NULL,
  `Name` varchar(32) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `gamers`
--

DROP TABLE IF EXISTS `gamers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `gamers` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(128) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `Name` (`Name`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=9087 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_gamers_insert_trigger` AFTER INSERT ON `gamers` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('gamers', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_gamers_update_trigger` AFTER UPDATE ON `gamers` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.name <=> new.name) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('gamers', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_gamers_delete_trigger` AFTER DELETE ON `gamers` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('gamers', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `gb_test`
--

DROP TABLE IF EXISTS `gb_test`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `gb_test` (
  `id` int DEFAULT NULL,
  `f1` int DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `handledlivescorealerts`
--

DROP TABLE IF EXISTS `handledlivescorealerts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `handledlivescorealerts` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `SentAlertId` int NOT NULL,
  `HandlerId` int DEFAULT NULL,
  `StatusId` int DEFAULT NULL,
  `CurrentPeriod` int NOT NULL,
  `Time` int DEFAULT NULL,
  `Scores` longtext,
  `ReferenceUrl` text,
  `IsIgnored` tinyint(1) DEFAULT NULL,
  `HandlerIp` varchar(30) DEFAULT NULL,
  `CreationDate` datetime DEFAULT CURRENT_TIMESTAMP,
  `LastUpdate` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `HandlerName` varchar(256) DEFAULT NULL,
  `FixtureId` int DEFAULT NULL,
  `PreviousScores` longtext,
  `AlertFixtureId` int DEFAULT NULL,
  `AlertPreviousScores` longtext,
  PRIMARY KEY (`Id`),
  KEY `SentAlertId` (`SentAlertId`) USING BTREE,
  KEY `StatusId` (`StatusId`) USING BTREE,
  CONSTRAINT `handledlivescorealerts_ibfk_1` FOREIGN KEY (`StatusId`) REFERENCES `fixturestatuses` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=487 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_handledlivescorealerts_insert_trigger` AFTER INSERT ON `handledlivescorealerts` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('handledlivescorealerts', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_handledlivescorealerts_update_trigger` AFTER UPDATE ON `handledlivescorealerts` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.sentalertid <=> new.sentalertid) || not(old.handlerid <=> new.handlerid) || not(old.currentperiod <=> new.currentperiod) || not(old.statusid <=> new.statusid) || not(old.time <=> new.time) || not(old.scores <=> new.scores) || not(old.referenceurl <=> new.referenceurl) || not(old.isignored <=> new.isignored) || not(old.handlerip <=> new.handlerip) || not(old.creationdate <=> new.creationdate) || not(old.lastupdate <=> new.lastupdate) || not(old.handlername <=> new.handlername) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('handledlivescorealerts', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_handledlivescorealerts_delete_trigger` AFTER DELETE ON `handledlivescorealerts` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('handledlivescorealerts', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `incidentbuffer`
--

DROP TABLE IF EXISTS `incidentbuffer`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `incidentbuffer` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `IncidentId` int NOT NULL,
  `SportId` int NOT NULL,
  `BufferTime` int NOT NULL,
  `ConflictBufferTime` int NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  KEY `unique` (`IncidentId`) USING BTREE,
  KEY `sportsIdForeign` (`SportId`) USING BTREE,
  CONSTRAINT `incidentForeign` FOREIGN KEY (`IncidentId`) REFERENCES `incidenttypes` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `sportsIdForeign` FOREIGN KEY (`SportId`) REFERENCES `sports` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=210 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `incidentcategories`
--

DROP TABLE IF EXISTS `incidentcategories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `incidentcategories` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  `HasScoreLog` tinyint NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `incidentfamilies`
--

DROP TABLE IF EXISTS `incidentfamilies`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `incidentfamilies` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  `ContainsPlayer` bit(1) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `reference_unique` (`Name`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `incidentmappings`
--

DROP TABLE IF EXISTS `incidentmappings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `incidentmappings` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ProviderId` int DEFAULT NULL,
  `SportId` int DEFAULT NULL,
  `MainId` int DEFAULT NULL,
  `Name` varchar(100) DEFAULT NULL,
  `IsActive` tinyint(1) DEFAULT NULL,
  `CreationDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`) USING BTREE,
  KEY `incidentmappings_ibfk_1` (`SportId`) USING BTREE,
  KEY `incidentmappings_ibfk_2` (`ProviderId`) USING BTREE,
  KEY `incidentmappings_ibfk_3` (`MainId`),
  CONSTRAINT `incidentmappings_ibfk_1` FOREIGN KEY (`SportId`) REFERENCES `sports` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `incidentmappings_ibfk_2` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `incidentmappings_ibfk_3` FOREIGN KEY (`MainId`) REFERENCES `incidenttypes` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=4466 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_incidentmappings_insert_trigger` AFTER INSERT ON `incidentmappings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('incidentmappings', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_incidentmappings_update_trigger` AFTER UPDATE ON `incidentmappings` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.mainid <=> new.mainid) || not(old.name <=> new.name) || not(old.isactive <=> new.isactive) || not(old.creationdate <=> new.creationdate) || not(old.sportid <=> new.sportid) || not(old.providerid <=> new.providerid) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('incidentmappings', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_incidentmappings_delete_trigger` AFTER DELETE ON `incidentmappings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('incidentmappings', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `incidentslog`
--

DROP TABLE IF EXISTS `incidentslog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `incidentslog` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FixtureId` int NOT NULL,
  `PeriodTypeId` int NOT NULL,
  `IncidentTypeId` int NOT NULL,
  `PlayerId` int DEFAULT NULL,
  `SequenceNumber` int DEFAULT NULL,
  `Seconds` int NOT NULL,
  `HomeValue` varchar(20) NOT NULL,
  `AwayValue` varchar(20) NOT NULL,
  `ParticipantPosition` int NOT NULL,
  `IsCancelled` tinyint(1) DEFAULT '0',
  `LastUpdate` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `PlayerID` (`PlayerId`) USING BTREE,
  KEY `IncidentTypeID` (`IncidentTypeId`) USING BTREE,
  KEY `PeriodTypeID` (`PeriodTypeId`) USING BTREE,
  KEY `FixtureId` (`FixtureId`) USING BTREE,
  CONSTRAINT `incidentslog_ibfk_2` FOREIGN KEY (`IncidentTypeId`) REFERENCES `incidenttypes` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `incidentslog_ibfk_3` FOREIGN KEY (`PeriodTypeId`) REFERENCES `periodtypes` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `incidentslog_ibfk_4` FOREIGN KEY (`FixtureId`) REFERENCES `fixtures` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=8032370 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `incidentsmanagerextradata`
--

DROP TABLE IF EXISTS `incidentsmanagerextradata`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `incidentsmanagerextradata` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `SportId` int NOT NULL,
  `SportName` varchar(255) NOT NULL,
  `ProviderId` int NOT NULL,
  `ProviderName` varchar(255) NOT NULL,
  `ExtraData` varchar(255) NOT NULL,
  `SentToCustomers` bit(1) NOT NULL,
  `LastUpdate` datetime NOT NULL,
  `ExtraDataSample` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `reference_unique` (`SportId`,`ExtraData`,`ProviderId`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=409 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `incidentsmanagerfixturestate`
--

DROP TABLE IF EXISTS `incidentsmanagerfixturestate`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `incidentsmanagerfixturestate` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `SportId` int NOT NULL,
  `SportName` varchar(255) NOT NULL,
  `IncidentId` int NOT NULL,
  `IncidentName` varchar(255) NOT NULL,
  `ProviderId` int NOT NULL,
  `ProviderName` varchar(255) NOT NULL,
  `IncidentFamily` int NOT NULL,
  `SentToCustomers` bit(1) NOT NULL,
  `LastUpdate` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `reference_unique` (`SportId`,`IncidentId`,`ProviderId`,`IncidentFamily`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=3211 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `incidentsmanagerstatistics`
--

DROP TABLE IF EXISTS `incidentsmanagerstatistics`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `incidentsmanagerstatistics` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `SportId` int NOT NULL,
  `SportName` varchar(255) NOT NULL,
  `ProviderId` int NOT NULL,
  `ProviderName` varchar(255) NOT NULL,
  `IncidentId` int NOT NULL,
  `IncidentName` varchar(255) NOT NULL,
  `IncidentFamily` int NOT NULL,
  `SentToCustomers` bit(1) NOT NULL,
  `ContainsPlayerName` bit(1) NOT NULL,
  `PerPeriod` bit(1) NOT NULL,
  `Total` bit(1) NOT NULL,
  `FinishedStatus` bit(1) NOT NULL,
  `InProgressStatus` bit(1) NOT NULL,
  `LastUpdate` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `reference_unique` (`SportId`,`IncidentId`,`ProviderId`,`IncidentFamily`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=2284 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `incidentspersport`
--

DROP TABLE IF EXISTS `incidentspersport`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `incidentspersport` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `IncidentId` int NOT NULL,
  `SportId` int NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `reference_unique` (`SportId`,`IncidentId`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=285 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `incidenttypes`
--

DROP TABLE IF EXISTS `incidenttypes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `incidenttypes` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  `IsSettled` bit(1) DEFAULT b'0',
  `Status` int NOT NULL DEFAULT '1',
  `Description` varchar(255) DEFAULT NULL,
  `CategoryId` int DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  KEY `incidenttypes_status` (`Status`),
  KEY `incidenttypes_category` (`CategoryId`),
  CONSTRAINT `incidenttypes_category` FOREIGN KEY (`CategoryId`) REFERENCES `incidentcategories` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `incidenttypes_status` FOREIGN KEY (`Status`) REFERENCES `incidenttypestatuses` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=2385 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `incidenttypestatuses`
--

DROP TABLE IF EXISTS `incidenttypestatuses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `incidenttypestatuses` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `incidenttypetofamilies`
--

DROP TABLE IF EXISTS `incidenttypetofamilies`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `incidenttypetofamilies` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `IncidentTypeId` int NOT NULL,
  `IncidentFamilyId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `incident_type_ref` (`IncidentTypeId`),
  KEY `incident_family_ref` (`IncidentFamilyId`),
  CONSTRAINT `incident_family_ref` FOREIGN KEY (`IncidentFamilyId`) REFERENCES `incidentfamilies` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `incident_type_ref` FOREIGN KEY (`IncidentTypeId`) REFERENCES `incidenttypes` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=254 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `inplayfixtureschedule`
--

DROP TABLE IF EXISTS `inplayfixtureschedule`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `inplayfixtureschedule` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FixtureId` int NOT NULL,
  `ProviderId` int DEFAULT NULL,
  `IsWithOdds` bit(1) DEFAULT NULL,
  `IsWithLivescore` bit(1) DEFAULT NULL,
  `IsActive` bit(1) NOT NULL DEFAULT b'1',
  `CreationDate` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `reference_unique` (`ProviderId`,`FixtureId`),
  KEY `FixtureId` (`FixtureId`) USING BTREE,
  KEY `ProviderId` (`ProviderId`) USING BTREE,
  KEY `fixture_schedule` (`FixtureId`,`ProviderId`,`IsWithOdds`,`IsWithLivescore`,`IsActive`) USING BTREE,
  KEY `idx_inplayfixtureschedule_FixtureId_IsWithLivescore` (`FixtureId`,`IsWithLivescore`),
  KEY `idx_inplayfixtureschedule_FixtureId_IsWithOdds_IsActive` (`FixtureId`,`IsWithOdds`,`IsActive`),
  CONSTRAINT `inplayfixtureschedule_ibfk_1` FOREIGN KEY (`FixtureId`) REFERENCES `fixtures` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `inplayfixtureschedule_ibfk_2` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=471868614 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_inplayfixtureschedule_insert_trigger` AFTER INSERT ON `inplayfixtureschedule` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('inplayfixtureschedule', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_inplayfixtureschedule_update_trigger` AFTER UPDATE ON `inplayfixtureschedule` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.providerid <=> new.providerid) || not(old.iswithodds <=> new.iswithodds) || not(old.iswithlivescore <=> new.iswithlivescore) || not(old.isactive <=> new.isactive) || not(old.creationdate <=> new.creationdate) || not(old.fixtureid <=> new.fixtureid) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('inplayfixtureschedule', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_inplayfixtureschedule_delete_trigger` AFTER DELETE ON `inplayfixtureschedule` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('inplayfixtureschedule', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `inplaymarketsupport`
--

DROP TABLE IF EXISTS `inplaymarketsupport`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `inplaymarketsupport` (
  `Id` varchar(255) NOT NULL,
  `ProviderId` varchar(255) DEFAULT NULL,
  `MarketId` varchar(255) DEFAULT NULL,
  `LeagueId` varchar(255) DEFAULT NULL,
  `SportId` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `invalidmarkets`
--

DROP TABLE IF EXISTS `invalidmarkets`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `invalidmarkets` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ProviderId` int DEFAULT NULL,
  `FixtureId` int DEFAULT NULL,
  `MarketId` int DEFAULT NULL,
  `ProviderMarketName` varchar(100) DEFAULT NULL,
  `Reason` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `ProviderId` (`ProviderId`) USING BTREE,
  KEY `MarketId` (`MarketId`) USING BTREE,
  CONSTRAINT `invalidmarkets_ibfk_1` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `invalidmarkets_ibfk_2` FOREIGN KEY (`MarketId`) REFERENCES `markets` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=1887184 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `languages`
--

DROP TABLE IF EXISTS `languages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `languages` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  `Code` varchar(5) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=83 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `leagueextradata`
--

DROP TABLE IF EXISTS `leagueextradata`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `leagueextradata` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `LeagueId` int NOT NULL,
  `Key` varchar(255) NOT NULL,
  `Value` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `Id` (`Id`) USING BTREE,
  UNIQUE KEY `leagueextradata_index` (`LeagueId`,`Key`) USING BTREE,
  CONSTRAINT `leagueextradata_ibfk_1` FOREIGN KEY (`LeagueId`) REFERENCES `leagues` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=9935 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_leagueextradata_insert_trigger` AFTER INSERT ON `leagueextradata` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('leagueextradata', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_leagueextradata_update_trigger` AFTER UPDATE ON `leagueextradata` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.leagueid <=> new.leagueid) || not(old.key <=> new.key) || not(old.value <=> new.value) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('leagueextradata', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_leagueextradata_delete_trigger` AFTER DELETE ON `leagueextradata` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('leagueextradata', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `leagueformats`
--

DROP TABLE IF EXISTS `leagueformats`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `leagueformats` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  `SportId` int NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `SportId_Name` (`Name`,`SportId`) USING BTREE,
  KEY `SportId` (`SportId`) USING BTREE,
  CONSTRAINT `leagueformats_ibfk_1` FOREIGN KEY (`SportId`) REFERENCES `sports` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `leagueloadintervals`
--

DROP TABLE IF EXISTS `leagueloadintervals`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `leagueloadintervals` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `LeagueId` int NOT NULL,
  `LoadInterval` decimal(5,2) NOT NULL,
  `SingleParticipantLoadInterval` decimal(5,2) DEFAULT NULL,
  `WideInterval` decimal(5,2) DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `IX_UQ` (`LeagueId`) USING BTREE,
  CONSTRAINT `leagueloadintervals_ibfk_1` FOREIGN KEY (`LeagueId`) REFERENCES `leagues` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=100000 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `leaguemappings`
--

DROP TABLE IF EXISTS `leaguemappings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `leaguemappings` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `MainId` int DEFAULT NULL,
  `ProviderId` int DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `IsActive` bit(1) DEFAULT NULL,
  `CreationDate` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `MainId_Name` (`MainId`,`Name`) USING BTREE,
  KEY `LeagueMapping_ProviderId_FK` (`ProviderId`) USING BTREE,
  KEY `LeagueMapping_LeagueId_FK` (`MainId`) USING BTREE,
  KEY `Name_IsActive` (`Name`,`IsActive`) USING BTREE,
  KEY `Name` (`Name`) USING BTREE,
  CONSTRAINT `leaguemappings_ibfk_1` FOREIGN KEY (`MainId`) REFERENCES `leagues` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `leaguemappings_ibfk_2` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=415252 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_leaguemappings_insert_trigger` AFTER INSERT ON `leaguemappings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('leaguemappings', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_leaguemappings_update_trigger` AFTER UPDATE ON `leaguemappings` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.mainid <=> new.mainid) || not(old.name <=> new.name) || not(old.isactive <=> new.isactive) || not(old.creationdate <=> new.creationdate) || not(old.providerid <=> new.providerid) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('leaguemappings', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_leaguemappings_delete_trigger` AFTER DELETE ON `leaguemappings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('leaguemappings', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `leaguemerges`
--

DROP TABLE IF EXISTS `leaguemerges`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `leaguemerges` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FromId` int NOT NULL,
  `ToId` int NOT NULL,
  `HasHierarchy` bit(1) NOT NULL,
  `IsPending` bit(1) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `FromId_FK` (`FromId`) USING BTREE,
  KEY `ToId_FK` (`ToId`) USING BTREE,
  KEY `IsPending_FromId_Key` (`FromId`,`IsPending`) USING BTREE,
  KEY `IsPending_Key` (`IsPending`) USING BTREE,
  CONSTRAINT `FromId_FK` FOREIGN KEY (`FromId`) REFERENCES `leagues` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `ToId_FK` FOREIGN KEY (`ToId`) REFERENCES `leagues` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=8289 DEFAULT CHARSET=utf8mb3 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_leaguemerges_insert_trigger` AFTER INSERT ON `leaguemerges` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('leaguemerges', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_leaguemerges_update_trigger` AFTER UPDATE ON `leaguemerges` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.fromid <=> new.fromid) || not(old.toid <=> new.toid) || not(old.hashierarchy <=> new.hashierarchy) || not(old.ispending <=> new.ispending) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('leaguemerges', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_leaguemerges_delete_trigger` AFTER DELETE ON `leaguemerges` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('leaguemerges', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `leaguemovements`
--

DROP TABLE IF EXISTS `leaguemovements`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `leaguemovements` (
  `FromId` int NOT NULL,
  `ToId` int NOT NULL,
  `HasHierarchy` bit(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `leagueorders`
--

DROP TABLE IF EXISTS `leagueorders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `leagueorders` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `LeagueId` int NOT NULL,
  `CustomerId` int NOT NULL,
  `ProviderId` int NOT NULL,
  `PackageId` int DEFAULT NULL,
  `IsActive` bit(1) NOT NULL,
  `CreationDate` datetime(6) DEFAULT NULL,
  `LastUpdate` datetime(6) DEFAULT NULL,
  `IsInPlay` bit(1) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `LeagueId` (`LeagueId`) USING BTREE,
  KEY `ProviderId` (`ProviderId`) USING BTREE,
  KEY `IX_LeagueOrders` (`CustomerId`,`LeagueId`,`ProviderId`,`PackageId`,`IsActive`),
  KEY `PackageId` (`PackageId`),
  CONSTRAINT `leagueorders_ibfk_1` FOREIGN KEY (`LeagueId`) REFERENCES `leagues` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `leagueorders_ibfk_2` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `leagueorders_ibfk_3` FOREIGN KEY (`PackageId`) REFERENCES `customerpackages` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=725050 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_leagueorders_insert_trigger` AFTER INSERT ON `leagueorders` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('leagueorders', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_leagueorders_update_trigger` AFTER UPDATE ON `leagueorders` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.leagueid <=> new.leagueid) || not(old.customerid <=> new.customerid) || not(old.providerid <=> new.providerid) || not(old.isactive <=> new.isactive) || not(old.creationdate <=> new.creationdate) || not(old.lastupdate <=> new.lastupdate) || not(old.isinplay <=> new.isinplay) || not(old.packageid <=> new.packageid) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('leagueorders', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `leagueoverrides`
--

DROP TABLE IF EXISTS `leagueoverrides`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `leagueoverrides` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `LeagueId` int NOT NULL,
  `OverrideGender` bit(1) NOT NULL,
  `OverrideAgeCategory` bit(1) NOT NULL,
  `OverrideType` bit(1) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `LeagueId` (`LeagueId`) USING BTREE,
  CONSTRAINT `leagueoverrides_ibfk_1` FOREIGN KEY (`LeagueId`) REFERENCES `leagues` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=533 DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_leagueoverrides_insert_trigger` AFTER INSERT ON `leagueoverrides` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('leagueoverrides', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_leagueoverrides_update_trigger` AFTER UPDATE ON `leagueoverrides` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.leagueid <=> new.leagueid) || not(old.overridegender <=> new.overridegender) || not(old.overrideagecategory <=> new.overrideagecategory) || not(old.overridetype <=> new.overridetype) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('leagueoverrides', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_leagueoverrides_delete_trigger` AFTER DELETE ON `leagueoverrides` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('leagueoverrides', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `leagues`
--

DROP TABLE IF EXISTS `leagues`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `leagues` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  `LocationId` int NOT NULL,
  `SportId` int NOT NULL,
  `CreationDate` datetime(6) DEFAULT NULL,
  `IsActive` bit(1) DEFAULT NULL,
  `NumberOfPeriods` int DEFAULT NULL,
  `Priority` int DEFAULT NULL,
  `CoverageLevel` int NOT NULL DEFAULT '0',
  `TourId` int DEFAULT NULL,
  `Gender` tinyint unsigned DEFAULT NULL,
  `AgeCategory` tinyint unsigned DEFAULT NULL,
  `Type` tinyint unsigned DEFAULT NULL,
  `FormatId` int DEFAULT NULL,
  `IsActive_Int` tinyint(1) GENERATED ALWAYS AS (cast(`IsActive` as unsigned)) VIRTUAL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `Unique_Index` (`Name`,`LocationId`,`SportId`,`Gender`,`AgeCategory`,`Type`) USING BTREE,
  KEY `SportId` (`SportId`) USING BTREE,
  KEY `TourId` (`TourId`) USING BTREE,
  KEY `IsActive` (`IsActive`) USING BTREE,
  KEY `LeagueIsActive` (`Id`,`IsActive`),
  KEY `FormatId` (`FormatId`) USING BTREE,
  KEY `SportId_LocationId` (`SportId`,`LocationId`) USING BTREE,
  KEY `SportId_IsActive` (`SportId`,`IsActive`) USING BTREE,
  KEY `SportId_LocationId_IsActive` (`SportId`,`LocationId`,`IsActive`) USING BTREE,
  CONSTRAINT `leagues_ibfk_1` FOREIGN KEY (`SportId`) REFERENCES `sports` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `leagues_ibfk_2` FOREIGN KEY (`TourId`) REFERENCES `tours` (`Id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `leagues_ibfk_3` FOREIGN KEY (`FormatId`) REFERENCES `leagueformats` (`Id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=666896 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_leagues_insert_trigger` AFTER INSERT ON `leagues` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('leagues', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `fixture_reflastupdate_leagues_update_trigger` AFTER UPDATE ON `leagues` FOR EACH ROW begin                             update fixtures                            set referenceslastupdate=now()                            where statusid in (1, 2, 5, 6, 8, 9)                            and startdate >= now() - interval (select max(loadinterval) from sports) hour                            and startdate >= now() - interval (select loadinterval from sports where id=sportid) hour                            and leagueid=new.id                            order by id; end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'IGNORE_SPACE,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_leagues_update_trigger` AFTER UPDATE ON `leagues` FOR EACH ROW begin                                      if not(old.id <=> new.id) || not(old.name <=> new.name) || not(old.locationid <=> new.locationid) || not(old.sportid <=> new.sportid) || not(old.creationdate <=> new.creationdate) || not(old.isactive <=> new.isactive) || not(old.priority <=> new.priority) || not(old.numberofperiods <=> new.numberofperiods) || not(old.coveragelevel <=> new.coveragelevel) || not(old.tourid <=> new.tourid) || not(old.gender <=> new.gender) || not(old.agecategory <=> new.agecategory) || not(old.type <=> new.type) || not(old.formatid <=> new.formatid) then                                            insert data.trackedchanges(tablename,operationtype,changedid)                                            values ('leagues', 2, new.id);                                        end if;                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_leagues_delete_trigger` AFTER DELETE ON `leagues` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('leagues', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `fixture_reflastupdate_leagues_delete_trigger` AFTER DELETE ON `leagues` FOR EACH ROW begin 
                            update fixtures
                            set referenceslastupdate=now()
                            where statusid in (1, 2, 5, 6, 8, 9)
                            and startdate >= now() - interval (select max(loadinterval) from sports) hour
                            and startdate >= now() - interval (select loadinterval from sports where id=sportid) hour
                            and leagueid=old.id
                            order by id; end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `leaguesduplications`
--

DROP TABLE IF EXISTS `leaguesduplications`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `leaguesduplications` (
  `Id` int DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `LocationId` int DEFAULT NULL,
  `SportId` int DEFAULT NULL,
  `CreationDate` datetime DEFAULT NULL,
  `IsActive` bit(1) DEFAULT NULL,
  `NumberOfPeriods` int DEFAULT NULL,
  `Priority` int DEFAULT NULL,
  `CoverageLevel` int DEFAULT NULL,
  `TourId` int DEFAULT NULL,
  `Gender` tinyint DEFAULT NULL,
  `AgeCategory` tinyint DEFAULT NULL,
  `Type` tinyint DEFAULT NULL,
  `FormatId` int DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `leagueseasons`
--

DROP TABLE IF EXISTS `leagueseasons`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `leagueseasons` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `LeagueId` int NOT NULL,
  `SeasonId` int NOT NULL,
  `IsActive` bit(1) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `LeagueId,SeasonId` (`LeagueId`,`SeasonId`) USING BTREE,
  KEY `LeagueId` (`LeagueId`) USING BTREE,
  KEY `SeasonId` (`SeasonId`) USING BTREE,
  CONSTRAINT `leagueseasons_ibfk_1` FOREIGN KEY (`LeagueId`) REFERENCES `leagues` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `leagueseasons_ibfk_2` FOREIGN KEY (`SeasonId`) REFERENCES `seasons` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=686292 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_leagueseasons_insert_trigger` AFTER INSERT ON `leagueseasons` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('leagueseasons', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_leagueseasons_update_trigger` AFTER UPDATE ON `leagueseasons` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.leagueid <=> new.leagueid) || not(old.seasonid <=> new.seasonid) || not(old.isactive <=> new.isactive) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('leagueseasons', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_leagueseasons_delete_trigger` AFTER DELETE ON `leagueseasons` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('leagueseasons', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `leaguestandings`
--

DROP TABLE IF EXISTS `leaguestandings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `leaguestandings` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `LeagueId` int DEFAULT NULL,
  `ParticipantId` int DEFAULT NULL,
  `TypeId` int DEFAULT NULL,
  `Value` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `TypeId` (`TypeId`) USING BTREE,
  KEY `LeagueId` (`LeagueId`) USING BTREE,
  KEY `ParticipantId` (`ParticipantId`) USING BTREE,
  CONSTRAINT `leaguestandings_ibfk_1` FOREIGN KEY (`TypeId`) REFERENCES `leaguestandingtypes` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `leaguestandings_ibfk_2` FOREIGN KEY (`LeagueId`) REFERENCES `leagues` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `leaguestandings_ibfk_3` FOREIGN KEY (`ParticipantId`) REFERENCES `participants` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `leaguestandingtypes`
--

DROP TABLE IF EXISTS `leaguestandingtypes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `leaguestandingtypes` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `leaguestartdatedelays`
--

DROP TABLE IF EXISTS `leaguestartdatedelays`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `leaguestartdatedelays` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `LeagueId` int NOT NULL,
  `DelayInMinutes` int NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  KEY `Unique_Index` (`LeagueId`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_leaguestartdatedelays_insert_trigger` AFTER INSERT ON `leaguestartdatedelays` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('leaguestartdatedelays', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_leaguestartdatedelays_update_trigger` AFTER UPDATE ON `leaguestartdatedelays` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.leagueid <=> new.leagueid) || not(old.delayinminutes <=> new.delayinminutes) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('leaguestartdatedelays', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_leaguestartdatedelays_delete_trigger` AFTER DELETE ON `leaguestartdatedelays` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('leaguestartdatedelays', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `leagueversionmapping`
--

DROP TABLE IF EXISTS `leagueversionmapping`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `leagueversionmapping` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `V3Id` int NOT NULL,
  `V4Id` int NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `V3Id` (`V3Id`),
  UNIQUE KEY `V4Id` (`V4Id`)
) ENGINE=InnoDB AUTO_INCREMENT=52279 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `linetypes`
--

DROP TABLE IF EXISTS `linetypes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `linetypes` (
  `Id` int NOT NULL,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `lineup`
--

DROP TABLE IF EXISTS `lineup`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `lineup` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FixtureId` int NOT NULL,
  `SportId` int NOT NULL,
  `Position` varchar(3) NOT NULL,
  `ParticipantId` int NOT NULL,
  `PlayerId` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FixtureId` (`FixtureId`) USING BTREE,
  KEY `SportId` (`SportId`) USING BTREE,
  KEY `ParticipantId` (`ParticipantId`) USING BTREE,
  KEY `PlayerId` (`PlayerId`) USING BTREE,
  CONSTRAINT `lineup_ibfk_1` FOREIGN KEY (`FixtureId`) REFERENCES `fixtures` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `lineup_ibfk_2` FOREIGN KEY (`SportId`) REFERENCES `sports` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `lineup_ibfk_3` FOREIGN KEY (`ParticipantId`) REFERENCES `participants` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `lineup_ibfk_4` FOREIGN KEY (`PlayerId`) REFERENCES `players` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `livescoreextradata`
--

DROP TABLE IF EXISTS `livescoreextradata`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `livescoreextradata` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FixtureId` int DEFAULT NULL,
  `Name` varchar(50) DEFAULT NULL,
  `Value` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FixtureId` (`FixtureId`) USING BTREE,
  CONSTRAINT `livescoreextradata_ibfk_1` FOREIGN KEY (`FixtureId`) REFERENCES `fixtures` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=8114398 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `livescoreextradatatypes`
--

DROP TABLE IF EXISTS `livescoreextradatatypes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `livescoreextradatatypes` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(64) NOT NULL,
  `PossibleValues` varchar(255) NOT NULL,
  `IsConsistent` bit(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=53 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_livescoreextradatatypes_insert_trigger` AFTER INSERT ON `livescoreextradatatypes` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('livescoreextradatatypes', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_livescoreextradatatypes_update_trigger` AFTER UPDATE ON `livescoreextradatatypes` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.name <=> new.name) || not(old.possiblevalues <=> new.possiblevalues) || not(old.isconsistent <=> new.isconsistent) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('livescoreextradatatypes', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_livescoreextradatatypes_delete_trigger` AFTER DELETE ON `livescoreextradatatypes` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('livescoreextradatatypes', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `livescoreincidentpriorities`
--

DROP TABLE IF EXISTS `livescoreincidentpriorities`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `livescoreincidentpriorities` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ProviderPriorityId` int NOT NULL,
  `IncidentTypeId` int NOT NULL,
  `Priority` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `ProviderPriorityId` (`ProviderPriorityId`) USING BTREE,
  KEY `IncidentTypeId` (`IncidentTypeId`) USING BTREE,
  CONSTRAINT `livescoreincidentpriorities_ibfk_1` FOREIGN KEY (`ProviderPriorityId`) REFERENCES `livescoreproviderpriorities` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `livescoreincidentpriorities_ibfk_2` FOREIGN KEY (`IncidentTypeId`) REFERENCES `incidenttypes` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_livescoreincidentpriorities_insert_trigger` AFTER INSERT ON `livescoreincidentpriorities` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('livescoreincidentpriorities', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_livescoreincidentpriorities_update_trigger` AFTER UPDATE ON `livescoreincidentpriorities` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.providerpriorityid <=> new.providerpriorityid) || not(old.incidenttypeid <=> new.incidenttypeid) || not(old.priority <=> new.priority) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('livescoreincidentpriorities', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_livescoreincidentpriorities_delete_trigger` AFTER DELETE ON `livescoreincidentpriorities` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('livescoreincidentpriorities', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `livescoremanagersportsettings`
--

DROP TABLE IF EXISTS `livescoremanagersportsettings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `livescoremanagersportsettings` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `SportId` int NOT NULL,
  `KeepUpdatingAfterFtInMinutes` int DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `reference_unique` (`SportId`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_livescoremanagersportsettings_insert_trigger` AFTER INSERT ON `livescoremanagersportsettings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('livescoremanagersportsettings', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_livescoremanagersportsettings_update_trigger` AFTER UPDATE ON `livescoremanagersportsettings` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.sportid <=> new.sportid) || not(old.keepupdatingafterftinminutes <=> new.keepupdatingafterftinminutes) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('livescoremanagersportsettings', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_livescoremanagersportsettings_delete_trigger` AFTER DELETE ON `livescoremanagersportsettings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('livescoremanagersportsettings', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `livescoreproviderpriorities`
--

DROP TABLE IF EXISTS `livescoreproviderpriorities`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `livescoreproviderpriorities` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ProviderId` int NOT NULL,
  `SportId` int NOT NULL,
  `EndEventsAsSingleProvider` bit(1) NOT NULL DEFAULT b'1',
  `MainPriority` int NOT NULL,
  `ScoresPriority` int NOT NULL DEFAULT '-1',
  `ScorePlayersPriority` int NOT NULL DEFAULT '-1',
  `IncidentPlayersPriority` int NOT NULL DEFAULT '-1',
  `SecondsPriority` int NOT NULL DEFAULT '-1',
  `StatusDescriptionPriority` int NOT NULL DEFAULT '-1',
  `StatusPriority` int NOT NULL DEFAULT '-1',
  `LeagueIds` varchar(5000) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_LivescoreProviderPriorities_UQ` (`ProviderId`,`SportId`) USING BTREE,
  KEY `ProviderId` (`ProviderId`) USING BTREE,
  KEY `SportId` (`SportId`) USING BTREE,
  CONSTRAINT `livescoreproviderpriorities_ibfk_2` FOREIGN KEY (`SportId`) REFERENCES `sports` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=325 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_livescoreproviderpriorities_insert_trigger` AFTER INSERT ON `livescoreproviderpriorities` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('livescoreproviderpriorities', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_livescoreproviderpriorities_update_trigger` AFTER UPDATE ON `livescoreproviderpriorities` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.providerid <=> new.providerid) || not(old.sportid <=> new.sportid) || not(old.mainpriority <=> new.mainpriority) || not(old.scorespriority <=> new.scorespriority) || not(old.scoreplayerspriority <=> new.scoreplayerspriority) || not(old.secondspriority <=> new.secondspriority) || not(old.statusdescriptionpriority <=> new.statusdescriptionpriority) || not(old.statuspriority <=> new.statuspriority) || not(old.endeventsassingleprovider <=> new.endeventsassingleprovider) || not(old.incidentplayerspriority <=> new.incidentplayerspriority) || not(old.leagueids <=> new.leagueids) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('livescoreproviderpriorities', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_livescoreproviderpriorities_delete_trigger` AFTER DELETE ON `livescoreproviderpriorities` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('livescoreproviderpriorities', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `locationmappings`
--

DROP TABLE IF EXISTS `locationmappings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `locationmappings` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `MainId` int DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `IsActive` tinyint(1) DEFAULT '1',
  `CreationDate` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `Name_MainId` (`MainId`,`Name`) USING BTREE,
  KEY `MainId` (`MainId`) USING BTREE,
  KEY `Name_IsActive` (`Name`,`IsActive`) USING BTREE,
  KEY `Name` (`Name`) USING BTREE,
  CONSTRAINT `locationmappings_ibfk_1` FOREIGN KEY (`MainId`) REFERENCES `locations` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=8325 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_locationmappings_insert_trigger` AFTER INSERT ON `locationmappings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('locationmappings', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_locationmappings_update_trigger` AFTER UPDATE ON `locationmappings` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.mainid <=> new.mainid) || not(old.name <=> new.name) || not(old.isactive <=> new.isactive) || not(old.creationdate <=> new.creationdate) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('locationmappings', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_locationmappings_delete_trigger` AFTER DELETE ON `locationmappings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('locationmappings', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `locations`
--

DROP TABLE IF EXISTS `locations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `locations` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  `CountryCode` varchar(64) NOT NULL,
  `IOCCountryCode` varchar(6) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `Name` (`Name`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=260 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_locations_insert_trigger` AFTER INSERT ON `locations` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('locations', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_locations_update_trigger` AFTER UPDATE ON `locations` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.name <=> new.name) || not(old.countrycode <=> new.countrycode) || not(old.ioccountrycode <=> new.ioccountrycode) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('locations', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `fixture_reflastupdate_locations_update_trigger` AFTER UPDATE ON `locations` FOR EACH ROW begin 
                            update fixtures
                            set referenceslastupdate=now()
                            where statusid in (1, 2, 5, 6, 8, 9)
                            and startdate >= now() - interval (select max(loadinterval) from sports) hour
                            and startdate >= now() - interval (select loadinterval from sports where id=sportid) hour
                            and locationid=new.id
                            order by id; end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_locations_delete_trigger` AFTER DELETE ON `locations` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('locations', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `fixture_reflastupdate_locations_delete_trigger` AFTER DELETE ON `locations` FOR EACH ROW begin 
                            update fixtures
                            set referenceslastupdate=now()
                            where statusid in (1, 2, 5, 6, 8, 9)
                            and startdate >= now() - interval (select max(loadinterval) from sports) hour
                            and startdate >= now() - interval (select loadinterval from sports where id=sportid) hour
                            and locationid=old.id
                            order by id; end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `logleveltypes`
--

DROP TABLE IF EXISTS `logleveltypes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `logleveltypes` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `lsi_providers_performance`
--

DROP TABLE IF EXISTS `lsi_providers_performance`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `lsi_providers_performance` (
  `SportId` bigint DEFAULT NULL,
  `LeagueId` bigint DEFAULT NULL,
  `IncidentId` bigint DEFAULT NULL,
  `ProviderId` bigint DEFAULT NULL,
  `Number_of_Provider_League_Fixtures` bigint DEFAULT NULL,
  `Total_League_Fixtures` bigint DEFAULT NULL,
  `Provider_Percentage_of_League_Fixtures` double DEFAULT NULL,
  `Precision` double DEFAULT NULL,
  `Recall` double DEFAULT NULL,
  `latency` double DEFAULT NULL,
  `last_updated` timestamp NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `luminatirequestlog`
--

DROP TABLE IF EXISTS `luminatirequestlog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `luminatirequestlog` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Url` varchar(255) NOT NULL,
  `ResponseTime` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `Size` int NOT NULL,
  `Success` bit(1) NOT NULL,
  `Static` bit(1) NOT NULL DEFAULT b'0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=16727228 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `mainbetnames`
--

DROP TABLE IF EXISTS `mainbetnames`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mainbetnames` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  `OppositeId` int DEFAULT NULL,
  `IsActive` tinyint(1) NOT NULL DEFAULT '1',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=118 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_mainbetnames_insert_trigger` AFTER INSERT ON `mainbetnames` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('mainbetnames', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_mainbetnames_update_trigger` AFTER UPDATE ON `mainbetnames` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.name <=> new.name) || not(old.isactive <=> new.isactive) || not(old.oppositeid <=> new.oppositeid) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('mainbetnames', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_mainbetnames_delete_trigger` AFTER DELETE ON `mainbetnames` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('mainbetnames', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `mappedproviderleagues`
--

DROP TABLE IF EXISTS `mappedproviderleagues`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mappedproviderleagues` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ProviderId` int DEFAULT NULL,
  `LeagueId` int DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `IsActive` tinyint(1) DEFAULT NULL,
  `CreationDate` datetime(6) DEFAULT NULL,
  `VerifiedBy` varchar(15) DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  KEY `ProviderId` (`ProviderId`) USING BTREE,
  CONSTRAINT `mappedproviderleagues_ibfk_1` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `marketbetgroups`
--

DROP TABLE IF EXISTS `marketbetgroups`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `marketbetgroups` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `GroupId` int NOT NULL,
  `BetName` varchar(100) NOT NULL,
  `IsMainLine` tinyint(1) NOT NULL,
  `IsOptional` tinyint(1) DEFAULT NULL,
  `Order` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `GroupId` (`GroupId`)
) ENGINE=InnoDB AUTO_INCREMENT=481 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_marketbetgroups_insert_trigger` AFTER INSERT ON `marketbetgroups` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('marketbetgroups', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_marketbetgroups_update_trigger` AFTER UPDATE ON `marketbetgroups` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.groupid <=> new.groupid) || not(old.betname <=> new.betname) || not(old.ismainline <=> new.ismainline) || not(old.isoptional <=> new.isoptional) || not(old.order <=> new.order) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('marketbetgroups', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_marketbetgroups_delete_trigger` AFTER DELETE ON `marketbetgroups` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('marketbetgroups', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `marketbetstoppriorities`
--

DROP TABLE IF EXISTS `marketbetstoppriorities`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `marketbetstoppriorities` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `MarketId` int NOT NULL,
  `ProviderIds` varchar(255) NOT NULL,
  `Priority` int NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_marketbetstoppriorities_insert_trigger` AFTER INSERT ON `marketbetstoppriorities` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('marketbetstoppriorities', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_marketbetstoppriorities_update_trigger` AFTER UPDATE ON `marketbetstoppriorities` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.marketid <=> new.marketid) || not(old.providerids <=> new.providerids) || not(old.priority <=> new.priority) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('marketbetstoppriorities', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_marketbetstoppriorities_delete_trigger` AFTER DELETE ON `marketbetstoppriorities` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('marketbetstoppriorities', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `marketcoverage`
--

DROP TABLE IF EXISTS `marketcoverage`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `marketcoverage` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Type` int NOT NULL,
  `LeagueId` int NOT NULL,
  `MarketId` int NOT NULL,
  `ProviderId` int NOT NULL,
  `HasResulting` bit(1) NOT NULL,
  `LastUpdate` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `unq_LeagueId_ProviderId_MarketId_Type` (`LeagueId`,`Type`,`MarketId`,`ProviderId`) USING BTREE,
  KEY `idx_LeagueId` (`LeagueId`) USING BTREE,
  KEY `idx_ProviderId` (`ProviderId`) USING BTREE,
  KEY `idx_MarketId` (`MarketId`) USING BTREE,
  KEY `fk_Type` (`Type`) USING BTREE,
  KEY `idx_LeagueId_MarketId_Type` (`LeagueId`,`MarketId`,`Type`) USING BTREE,
  CONSTRAINT `fk_LeagueId` FOREIGN KEY (`LeagueId`) REFERENCES `leagues` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `fk_MarketId` FOREIGN KEY (`MarketId`) REFERENCES `markets` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `fk_ProviderId` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `fk_Type` FOREIGN KEY (`Type`) REFERENCES `coveragetype` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=384659879 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_marketcoverage_insert_trigger` AFTER INSERT ON `marketcoverage` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('marketcoverage', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_marketcoverage_update_trigger` AFTER UPDATE ON `marketcoverage` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.leagueid <=> new.leagueid) || not(old.type <=> new.type) || not(old.marketid <=> new.marketid) || not(old.providerid <=> new.providerid) || not(old.hasresulting <=> new.hasresulting) || not(old.lastupdate <=> new.lastupdate) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('marketcoverage', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_marketcoverage_delete_trigger` AFTER DELETE ON `marketcoverage` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('marketcoverage', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `marketfamilies`
--

DROP TABLE IF EXISTS `marketfamilies`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `marketfamilies` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  `MarketLogicGroupId` int NOT NULL,
  `MarketBetGroupId` int DEFAULT NULL,
  `MarketPropertiesId` int NOT NULL,
  `AutoCompleteMissingBets` tinyint(1) DEFAULT '0',
  `MarketTypeId` int DEFAULT NULL,
  `MarketTypeDescriptionId` int DEFAULT NULL,
  `BetTypeId` int DEFAULT NULL,
  `LineTypeId` int DEFAULT NULL,
  `MarginType` int DEFAULT '100',
  PRIMARY KEY (`Id`),
  KEY `MarketLogicGroupId` (`MarketLogicGroupId`) USING BTREE,
  KEY `MarketBetGroupId` (`MarketBetGroupId`) USING BTREE,
  KEY `MarketPropertiesId` (`MarketPropertiesId`) USING BTREE,
  CONSTRAINT `marketfamilies_ibfk_1` FOREIGN KEY (`MarketLogicGroupId`) REFERENCES `marketlogicgroups` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `marketfamilies_ibfk_3` FOREIGN KEY (`MarketPropertiesId`) REFERENCES `marketproperties` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `marketfamilies_MarketGroupId` FOREIGN KEY (`MarketBetGroupId`) REFERENCES `marketbetgroups` (`GroupId`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=274 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_marketfamilies_insert_trigger` AFTER INSERT ON `marketfamilies` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('marketfamilies', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_marketfamilies_update_trigger` AFTER UPDATE ON `marketfamilies` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.name <=> new.name) || not(old.marketlogicgroupid <=> new.marketlogicgroupid) || not(old.marketbetgroupid <=> new.marketbetgroupid) || not(old.marketpropertiesid <=> new.marketpropertiesid) || not(old.autocompletemissingbets <=> new.autocompletemissingbets) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('marketfamilies', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_marketfamilies_delete_trigger` AFTER DELETE ON `marketfamilies` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('marketfamilies', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `marketlogicgroups`
--

DROP TABLE IF EXISTS `marketlogicgroups`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `marketlogicgroups` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=51 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `marketmappings`
--

DROP TABLE IF EXISTS `marketmappings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `marketmappings` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `MainId` int DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `IsActive` tinyint(1) DEFAULT NULL,
  `CreationDate` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `Name` (`Name`) USING BTREE,
  KEY `MainId` (`MainId`) USING BTREE,
  CONSTRAINT `marketmappings_ibfk_1` FOREIGN KEY (`MainId`) REFERENCES `markets` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=4249 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_marketmappings_insert_trigger` AFTER INSERT ON `marketmappings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('marketmappings', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_marketmappings_update_trigger` AFTER UPDATE ON `marketmappings` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.mainid <=> new.mainid) || not(old.name <=> new.name) || not(old.isactive <=> new.isactive) || not(old.creationdate <=> new.creationdate) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('marketmappings', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_marketmappings_delete_trigger` AFTER DELETE ON `marketmappings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('marketmappings', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `marketproperties`
--

DROP TABLE IF EXISTS `marketproperties`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `marketproperties` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `SwapEnabled` tinyint(1) NOT NULL DEFAULT '0',
  `LineRegexFormat` varchar(255) DEFAULT NULL,
  `IsWithLine` tinyint(1) DEFAULT NULL,
  `IsSpecialMarket` tinyint(1) NOT NULL DEFAULT '0',
  `BetRegexFormat` varchar(255) DEFAULT NULL,
  `LineTypes` varchar(40) DEFAULT NULL,
  `IsOutright` bit(1) NOT NULL DEFAULT b'0',
  `MarketType` int NOT NULL,
  `IsOutright_Int` tinyint(1) GENERATED ALWAYS AS (cast(`IsOutright` as unsigned)) VIRTUAL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `IX_UQ` (`SwapEnabled`,`LineRegexFormat`,`IsWithLine`,`IsSpecialMarket`,`BetRegexFormat`,`LineTypes`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=105 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_marketproperties_insert_trigger` AFTER INSERT ON `marketproperties` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('marketproperties', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_marketproperties_update_trigger` AFTER UPDATE ON `marketproperties` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.swapenabled <=> new.swapenabled) || not(old.lineregexformat <=> new.lineregexformat) || not(old.iswithline <=> new.iswithline) || not(old.isspecialmarket <=> new.isspecialmarket) || not(old.betregexformat <=> new.betregexformat) || not(old.isoutright <=> new.isoutright) || not(old.markettype <=> new.markettype) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('marketproperties', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_marketproperties_delete_trigger` AFTER DELETE ON `marketproperties` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('marketproperties', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `markets`
--

DROP TABLE IF EXISTS `markets`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `markets` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(500) NOT NULL,
  `IsActive` tinyint(1) NOT NULL DEFAULT '1',
  `ActivePeriod` int DEFAULT NULL,
  `SettlementPeriod` int DEFAULT NULL,
  `MarketFamilyId` int NOT NULL,
  `CoverageLevel` int NOT NULL DEFAULT '0',
  `OppositeId` int DEFAULT NULL,
  `IsPostMatchSettled` bit(1) DEFAULT b'0',
  `IsInPlaySettled` bit(1) DEFAULT b'0',
  `IncidentTypeId` int DEFAULT NULL,
  `Stage` int NOT NULL DEFAULT '1',
  `Description` varchar(254) DEFAULT NULL,
  `Period` varchar(2) NOT NULL,
  `IsPostMatchSettled_Int` tinyint(1) GENERATED ALWAYS AS (cast(`IsPostMatchSettled` as unsigned)) VIRTUAL,
  `IsInPlaySettled_Int` tinyint(1) GENERATED ALWAYS AS (cast(`IsInPlaySettled` as unsigned)) VIRTUAL,
  `DynamicBets` tinyint(1) DEFAULT NULL,
  `PartialOutcomes` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Name` (`Name`) USING BTREE,
  KEY `markets_ibfk_1` (`MarketFamilyId`),
  KEY `oppsiteId` (`OppositeId`),
  CONSTRAINT `markets_ibfk_1` FOREIGN KEY (`MarketFamilyId`) REFERENCES `marketfamilies` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `oppsiteId` FOREIGN KEY (`OppositeId`) REFERENCES `markets` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=3142 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_markets_insert_trigger` AFTER INSERT ON `markets` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('markets', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_markets_update_trigger` AFTER UPDATE ON `markets` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.name <=> new.name) || not(old.isactive <=> new.isactive) || not(old.activeperiod <=> new.activeperiod) || not(old.settlementperiod <=> new.settlementperiod) || not(old.marketfamilyid <=> new.marketfamilyid) || not(old.coveragelevel <=> new.coveragelevel) || not(old.oppositeid <=> new.oppositeid) || not(old.incidenttypeid <=> new.incidenttypeid) || not(old.stage <=> new.stage) || not(old.description <=> new.description) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('markets', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_markets_delete_trigger` AFTER DELETE ON `markets` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('markets', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `marketsalepriority`
--

DROP TABLE IF EXISTS `marketsalepriority`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `marketsalepriority` (
  `Id` int NOT NULL,
  `MarketId` int NOT NULL,
  `SportId` int NOT NULL,
  `SalesPriority` bit(1) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Unq_SportId_MarketId_Sales` (`MarketId`,`SportId`,`SalesPriority`) USING BTREE,
  KEY `MarketsPerSportSportsId` (`SportId`),
  CONSTRAINT `MarketsPerSportMarketsId` FOREIGN KEY (`MarketId`) REFERENCES `markets` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `MarketsPerSportSportsId` FOREIGN KEY (`SportId`) REFERENCES `sports` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `marketsettlementcoverage`
--

DROP TABLE IF EXISTS `marketsettlementcoverage`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `marketsettlementcoverage` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `MarketId` int DEFAULT NULL,
  `PreMatch` varchar(1) CHARACTER SET utf8mb3 COLLATE utf8mb3_bin DEFAULT NULL,
  `InPlay` varchar(1) CHARACTER SET utf8mb3 COLLATE utf8mb3_bin DEFAULT NULL,
  `SettlementsInPlay` bit(1) DEFAULT NULL,
  `SettlementsPostMatch` bit(1) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UniqueFields` (`MarketId`,`PreMatch`,`InPlay`,`SettlementsInPlay`,`SettlementsPostMatch`) USING BTREE,
  CONSTRAINT `MarketIdField` FOREIGN KEY (`MarketId`) REFERENCES `markets` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=1594 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `marketsindication`
--

DROP TABLE IF EXISTS `marketsindication`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `marketsindication` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `MarketId` int NOT NULL,
  `MarketName` varchar(500) DEFAULT NULL,
  `ScorePostMatch` binary(1) DEFAULT NULL,
  `ScoreInPlay` binary(1) DEFAULT NULL,
  `ScorePeriod` binary(1) DEFAULT NULL,
  `ScorelogPostMatch` binary(1) DEFAULT NULL,
  `ScorelogInplay` binary(1) DEFAULT NULL,
  `ScorelogPeriod` binary(1) DEFAULT NULL,
  `IncidentPostMatch` binary(1) DEFAULT NULL,
  `IncidentInPlay` binary(1) DEFAULT NULL,
  `IncidentPeriod` binary(1) DEFAULT NULL,
  `PlayerMarketsPostMatch` binary(1) DEFAULT NULL,
  `PlayerMarketsInPlay` binary(1) DEFAULT NULL,
  `OutrightSports` binary(1) DEFAULT NULL,
  `OutrightMarkets` binary(1) DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `UniqueFields` (`MarketId`,`MarketName`) USING BTREE,
  KEY `MarketName` (`MarketName`) USING BTREE,
  CONSTRAINT `MarketId` FOREIGN KEY (`MarketId`) REFERENCES `markets` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=1421 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `marketsmetadata`
--

DROP TABLE IF EXISTS `marketsmetadata`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `marketsmetadata` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `MarketId` int DEFAULT NULL,
  `SportId` int DEFAULT NULL,
  `WithLine` bit(1) DEFAULT NULL,
  `SalesPriority` bit(1) DEFAULT b'0',
  `Outright` bit(1) DEFAULT b'0',
  `Explanation` varchar(254) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Unq_SportId_MarketId` (`MarketId`,`SportId`,`WithLine`,`Explanation`,`Outright`) USING BTREE,
  KEY `sportforeignkey` (`SportId`),
  CONSTRAINT `foreignKey` FOREIGN KEY (`MarketId`) REFERENCES `markets` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `sportforeignkey` FOREIGN KEY (`SportId`) REFERENCES `sports` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=12571 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `marketspersports`
--

DROP TABLE IF EXISTS `marketspersports`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `marketspersports` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `MarketId` int NOT NULL,
  `SportId` int NOT NULL,
  `SettlementStateId` int NOT NULL DEFAULT '1',
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `Unq_SportId_MarketId` (`SportId`,`MarketId`) USING BTREE,
  KEY `marketspersports_ibfk_1` (`MarketId`) USING BTREE,
  KEY `marketspersports_ibfk_2` (`SportId`) USING BTREE,
  CONSTRAINT `marketspersports_ibfk_1` FOREIGN KEY (`MarketId`) REFERENCES `markets` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `marketspersports_ibfk_2` FOREIGN KEY (`SportId`) REFERENCES `sports` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=6586 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_marketspersports_insert_trigger` AFTER INSERT ON `marketspersports` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('marketspersports', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_marketspersports_update_trigger` AFTER UPDATE ON `marketspersports` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.marketid <=> new.marketid) || not(old.sportid <=> new.sportid) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('marketspersports', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_marketspersports_delete_trigger` AFTER DELETE ON `marketspersports` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('marketspersports', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `marketsuptime`
--

DROP TABLE IF EXISTS `marketsuptime`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `marketsuptime` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FixtureId` int DEFAULT NULL,
  `ProviderId` int NOT NULL,
  `ProviderFixtureId` varchar(255) DEFAULT NULL,
  `MarketId` int NOT NULL,
  `OpenSeconds` float(53,3) NOT NULL,
  `Uptime` float(53,3) NOT NULL,
  `UptimeInTotal` float(53,3) NOT NULL,
  `SuspendedSeconds` float(53,3) NOT NULL,
  `TotalTime` float(53,3) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FixtureID_ProviderID_MarketID` (`FixtureId`,`ProviderId`,`MarketId`),
  KEY `marketsuptime_ibfk_2` (`ProviderId`),
  KEY `marketsuptime_ibfk_3` (`MarketId`),
  CONSTRAINT `marketsuptime_ibfk_1` FOREIGN KEY (`FixtureId`) REFERENCES `fixtures` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `marketsuptime_ibfk_2` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `marketsuptime_ibfk_3` FOREIGN KEY (`MarketId`) REFERENCES `markets` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=26494792 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `marketsurl`
--

DROP TABLE IF EXISTS `marketsurl`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `marketsurl` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FixtureId` int NOT NULL,
  `MarketId` int NOT NULL,
  `Url` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `marketswithouthandlers`
--

DROP TABLE IF EXISTS `marketswithouthandlers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `marketswithouthandlers` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ProviderId` int DEFAULT NULL,
  `MarketId` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `ProviderId` (`ProviderId`) USING BTREE,
  KEY `MarketId` (`MarketId`) USING BTREE,
  CONSTRAINT `marketswithouthandlers_ibfk_1` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `marketswithouthandlers_ibfk_2` FOREIGN KEY (`MarketId`) REFERENCES `markets` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=469 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `markettypes`
--

DROP TABLE IF EXISTS `markettypes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `markettypes` (
  `Id` int NOT NULL,
  `Name` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `markettypesdescription`
--

DROP TABLE IF EXISTS `markettypesdescription`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `markettypesdescription` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  `MarketTypeId` int DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `marketversionmapping`
--

DROP TABLE IF EXISTS `marketversionmapping`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `marketversionmapping` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `V3Id` int NOT NULL,
  `V4Id` int NOT NULL,
  `LastUpdate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_marketversionmapping_insert_trigger` AFTER INSERT ON `marketversionmapping` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('marketversionmapping', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_marketversionmapping_update_trigger` AFTER UPDATE ON `marketversionmapping` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.v3id <=> new.v3id) || not(old.v4id <=> new.v4id) || not(old.lastupdate <=> new.lastupdate) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('marketversionmapping', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_marketversionmapping_delete_trigger` AFTER DELETE ON `marketversionmapping` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('marketversionmapping', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `migrations_log`
--

DROP TABLE IF EXISTS `migrations_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `migrations_log` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  `Timestamp` datetime NOT NULL,
  `ExecutedAt` datetime NOT NULL,
  `Index` varchar(255) NOT NULL,
  `Retries` tinyint NOT NULL,
  `Executed` tinyint NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `notificationtemplates`
--

DROP TABLE IF EXISTS `notificationtemplates`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `notificationtemplates` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  `Template` text NOT NULL,
  `Method` int NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `NotificationTemplateUniqueIndx` (`Name`,`Method`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_notificationtemplates_insert_trigger` AFTER INSERT ON `notificationtemplates` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('notificationtemplates', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_notificationtemplates_update_trigger` AFTER UPDATE ON `notificationtemplates` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.name <=> new.name) || not(old.template <=> new.template) || not(old.method <=> new.method) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('notificationtemplates', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_notificationtemplates_delete_trigger` AFTER DELETE ON `notificationtemplates` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('notificationtemplates', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `notmappedaliases`
--

DROP TABLE IF EXISTS `notmappedaliases`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `notmappedaliases` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Type` varchar(255) DEFAULT NULL,
  `SportId` int NOT NULL,
  `Alias` varchar(255) DEFAULT NULL,
  `ClosestMatch` varchar(255) DEFAULT NULL,
  `ClosestMatchId` int DEFAULT NULL,
  `Probability` decimal(11,3) DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `UniqueRow` (`Type`,`SportId`,`Alias`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `notmappedfixtures`
--

DROP TABLE IF EXISTS `notmappedfixtures`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `notmappedfixtures` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ProviderId` int DEFAULT NULL,
  `ProviderFixtureId` varchar(36) NOT NULL,
  `SportId` int NOT NULL,
  `LeagueName` varchar(45) DEFAULT NULL,
  `LocationName` varchar(45) DEFAULT NULL,
  `StartDate` datetime(6) DEFAULT NULL,
  `HomeParticipantName` varchar(45) NOT NULL,
  `AwayParticipantName` varchar(45) NOT NULL,
  `SuggestedFixtureId` int DEFAULT NULL,
  `SimilarityPercentage` double DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `ProviderId` (`ProviderId`) USING BTREE,
  CONSTRAINT `notmappedfixtures_ibfk_1` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=146513 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `notmappedincidents`
--

DROP TABLE IF EXISTS `notmappedincidents`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `notmappedincidents` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `ProviderId` int DEFAULT NULL,
  `SportId` int DEFAULT NULL,
  `Name` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `ProviderId` (`ProviderId`) USING BTREE,
  KEY `notmappedincidents_ibfk_3` (`SportId`),
  CONSTRAINT `notmappedincidents_ibfk_1` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `notmappedincidents_ibfk_3` FOREIGN KEY (`SportId`) REFERENCES `sports` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=5024 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `notmappedleagues`
--

DROP TABLE IF EXISTS `notmappedleagues`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `notmappedleagues` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ProviderId` int DEFAULT NULL,
  `LeagueName` varchar(50) DEFAULT NULL,
  `LocationName` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `ProviderId` (`ProviderId`) USING BTREE,
  CONSTRAINT `notmappedleagues_ibfk_1` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=1563438 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `notmappedlocations`
--

DROP TABLE IF EXISTS `notmappedlocations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `notmappedlocations` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ProviderId` int DEFAULT NULL,
  `LocationName` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `ProviderId` (`ProviderId`) USING BTREE,
  CONSTRAINT `notmappedlocations_ibfk_1` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=21743 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `notmappedmarkets`
--

DROP TABLE IF EXISTS `notmappedmarkets`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `notmappedmarkets` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `RobotId` int DEFAULT NULL,
  `MarketName` varchar(256) DEFAULT NULL,
  `BetNames` varchar(255) DEFAULT NULL,
  `Lines` varchar(255) DEFAULT NULL,
  `SportId` int DEFAULT NULL,
  `LastUpdate` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `notmappedmarkets_ibfk_2` (`SportId`) USING BTREE,
  KEY `IX_robotId` (`RobotId`) USING BTREE,
  KEY `notmappedmarkets_RobotId_IDX` (`RobotId`,`SportId`,`MarketName`) USING BTREE,
  CONSTRAINT `notmappedmarkets_ibfk_1` FOREIGN KEY (`RobotId`) REFERENCES `robots` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `notmappedmarkets_ibfk_2` FOREIGN KEY (`SportId`) REFERENCES `sports` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=8321120 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_notmappedmarkets_insert_trigger` AFTER INSERT ON `notmappedmarkets` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('notmappedmarkets', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_notmappedmarkets_update_trigger` AFTER UPDATE ON `notmappedmarkets` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.robotid <=> new.robotid) || not(old.marketname <=> new.marketname) || not(old.betnames <=> new.betnames) || not(old.lines <=> new.lines) || not(old.sportid <=> new.sportid) || not(old.lastupdate <=> new.lastupdate) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('notmappedmarkets', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_notmappedmarkets_delete_trigger` AFTER DELETE ON `notmappedmarkets` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('notmappedmarkets', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `notmappedmarkets_old`
--

DROP TABLE IF EXISTS `notmappedmarkets_old`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `notmappedmarkets_old` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ProviderId` int DEFAULT NULL,
  `MarketName` varchar(256) DEFAULT NULL,
  `BetNames` varchar(255) DEFAULT NULL,
  `Lines` varchar(255) DEFAULT NULL,
  `SportId` int DEFAULT NULL,
  `LastUpdate` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `ProviderId` (`ProviderId`) USING BTREE,
  KEY `notmappedmarkets_ibfk_2` (`SportId`),
  CONSTRAINT `notmappedmarkets_old_ibfk_1` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `notmappedmarkets_old_ibfk_2` FOREIGN KEY (`SportId`) REFERENCES `sports` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=69753 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `notmappedperiods`
--

DROP TABLE IF EXISTS `notmappedperiods`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `notmappedperiods` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `ProviderId` int DEFAULT NULL,
  `SportId` int DEFAULT NULL,
  `Name` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `ProviderId` (`ProviderId`) USING BTREE,
  KEY `notmappedincidents_ibfk_2` (`SportId`),
  CONSTRAINT `notmappedincidents_ibfk_2` FOREIGN KEY (`SportId`) REFERENCES `sports` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `notmappedperiods_ibfk_1` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=3922 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `notmappedsports`
--

DROP TABLE IF EXISTS `notmappedsports`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `notmappedsports` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ProviderId` int DEFAULT NULL,
  `Name` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `ProviderId` (`ProviderId`) USING BTREE,
  CONSTRAINT `notmappedsports_ibfk_1` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=32827 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `notmappedstatusdescriptions`
--

DROP TABLE IF EXISTS `notmappedstatusdescriptions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `notmappedstatusdescriptions` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ProviderId` int DEFAULT NULL,
  `Name` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `ProviderId` (`ProviderId`) USING BTREE,
  CONSTRAINT `notmappedstatusdescriptions_ibfk_1` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=172 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `outrightleaguerelations`
--

DROP TABLE IF EXISTS `outrightleaguerelations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `outrightleaguerelations` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `OutrightLeagueFixtureId` int NOT NULL,
  `StandardFixtureId` int NOT NULL,
  `IsConflicted` tinyint unsigned NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `Keys` (`OutrightLeagueFixtureId`,`StandardFixtureId`) USING BTREE,
  KEY `outrightleaguelinks_fk2` (`StandardFixtureId`),
  KEY `outrightleaguelinks_fk1` (`OutrightLeagueFixtureId`) USING BTREE,
  CONSTRAINT `outrightleaguelinks_fk1` FOREIGN KEY (`OutrightLeagueFixtureId`) REFERENCES `fixtures` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `outrightleaguelinks_fk2` FOREIGN KEY (`StandardFixtureId`) REFERENCES `fixtures` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=683245 DEFAULT CHARSET=utf8mb3 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_outrightleaguerelations_insert_trigger` AFTER INSERT ON `outrightleaguerelations` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('outrightleaguerelations', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_outrightleaguerelations_update_trigger` AFTER UPDATE ON `outrightleaguerelations` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.outrightleaguefixtureid <=> new.outrightleaguefixtureid) || not(old.standardfixtureid <=> new.standardfixtureid) || not(old.isconflicted <=> new.isconflicted) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('outrightleaguerelations', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_outrightleaguerelations_delete_trigger` AFTER DELETE ON `outrightleaguerelations` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('outrightleaguerelations', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `outrightscoreconflicts`
--

DROP TABLE IF EXISTS `outrightscoreconflicts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `outrightscoreconflicts` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FixtureId` int NOT NULL,
  `ConflictType` int NOT NULL,
  `IsActive` bit(1) NOT NULL,
  `ReferenceId` int DEFAULT NULL,
  `ProviderData` varchar(500) NOT NULL,
  `LastUpdate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `fk_outrightscoreconflicts_fixtureid` (`FixtureId`),
  CONSTRAINT `fk_outrightscoreconflicts_fixtureid` FOREIGN KEY (`FixtureId`) REFERENCES `fixtures` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=29889 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `participantdynamicextradata`
--

DROP TABLE IF EXISTS `participantdynamicextradata`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `participantdynamicextradata` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  `Value` varchar(255) NOT NULL,
  `ParticipantId` int NOT NULL,
  `FixtureId` int NOT NULL,
  `CreationDate` datetime(6) DEFAULT NULL,
  `LastUpdate` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `ParticipantDynamicEd_Name_ParticipantId_FixtureId` (`Name`,`ParticipantId`,`FixtureId`) USING BTREE,
  KEY `ParticipantDynamicED_ParticipantID` (`ParticipantId`) USING BTREE,
  KEY `ParticipantDynamicED_FixtureID` (`FixtureId`) USING BTREE,
  KEY `ParticipantDynamicED_FixtureID_ParticipantId` (`ParticipantId`,`FixtureId`) USING BTREE,
  CONSTRAINT `participantdynamicextradata_ibfk_1` FOREIGN KEY (`ParticipantId`) REFERENCES `participants` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `participantdynamicextradata_ibfk_2` FOREIGN KEY (`FixtureId`) REFERENCES `fixtures` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=117128530 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_participantdynamicextradata_insert_trigger` AFTER INSERT ON `participantdynamicextradata` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('participantdynamicextradata', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `fixture_reflastupdate_participantdynamicextradata_insert_trigger` AFTER INSERT ON `participantdynamicextradata` FOR EACH ROW begin update fixtures set referenceslastupdate=now() where id=new.fixtureid; end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_participantdynamicextradata_update_trigger` AFTER UPDATE ON `participantdynamicextradata` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.name <=> new.name) || not(old.value <=> new.value) || not(old.participantid <=> new.participantid) || not(old.creationdate <=> new.creationdate) || not(old.lastupdate <=> new.lastupdate) || not(old.fixtureid <=> new.fixtureid) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('participantdynamicextradata', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `fixture_reflastupdate_participantdynamicextradata_update_trigger` AFTER UPDATE ON `participantdynamicextradata` FOR EACH ROW begin update fixtures set referenceslastupdate=now() where id=new.fixtureid; end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_participantdynamicextradata_delete_trigger` AFTER DELETE ON `participantdynamicextradata` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('participantdynamicextradata', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `fixture_reflastupdate_participantdynamicextradata_delete_trigger` AFTER DELETE ON `participantdynamicextradata` FOR EACH ROW begin update fixtures set referenceslastupdate=now() where id=old.fixtureid; end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `participanthistory`
--

DROP TABLE IF EXISTS `participanthistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `participanthistory` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ParticipantId` int NOT NULL,
  `ChangedField` varchar(50) NOT NULL,
  `OldValue` varchar(50) DEFAULT NULL,
  `NewValue` varchar(50) NOT NULL,
  `ChangedBy` varchar(50) NOT NULL,
  `ChangeDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `IX_ParticipantId` (`ParticipantId`) USING BTREE,
  KEY `IX_ParticipantId_ChangedField` (`ParticipantId`,`ChangedField`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=4118255 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `participantmappingchangelogs`
--

DROP TABLE IF EXISTS `participantmappingchangelogs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `participantmappingchangelogs` (
  `id` int NOT NULL AUTO_INCREMENT,
  `User` varchar(255) NOT NULL,
  `FieldName` varchar(255) NOT NULL,
  `FieldNewValue` varchar(255) NOT NULL,
  `LastUpdate` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `ParticipantMappingId` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `ParticipantMappingId` (`ParticipantMappingId`) USING BTREE,
  CONSTRAINT `participantmappingchangelogs_ibfk_1` FOREIGN KEY (`ParticipantMappingId`) REFERENCES `participantmappings` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=475162 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `participantmappings`
--

DROP TABLE IF EXISTS `participantmappings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `participantmappings` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `MainId` int NOT NULL,
  `Name` varchar(255) NOT NULL,
  `IsActive` bit(1) NOT NULL,
  `CreationDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `MainId_Name` (`MainId`,`Name`) USING BTREE,
  KEY `MainID` (`MainId`) USING BTREE,
  KEY `idx_participantmappings_Name` (`Name`) USING BTREE,
  KEY `Name_IsActive` (`Name`,`IsActive`) USING BTREE,
  CONSTRAINT `participantmappings_ibfk_1` FOREIGN KEY (`MainId`) REFERENCES `participants` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=4055314 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_participantmappings_insert_trigger` AFTER INSERT ON `participantmappings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('participantmappings', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_participantmappings_update_trigger` AFTER UPDATE ON `participantmappings` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.mainid <=> new.mainid) || not(old.name <=> new.name) || not(old.isactive <=> new.isactive) || not(old.creationdate <=> new.creationdate) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('participantmappings', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_participantmappings_delete_trigger` AFTER DELETE ON `participantmappings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('participantmappings', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `participantmergegroups`
--

DROP TABLE IF EXISTS `participantmergegroups`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `participantmergegroups` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `GroupId` int NOT NULL,
  `ParticipantIds` text,
  `IsActive` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=1140 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `participantmovements`
--

DROP TABLE IF EXISTS `participantmovements`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `participantmovements` (
  `FromId` int NOT NULL,
  `ToId` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `participants`
--

DROP TABLE IF EXISTS `participants`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `participants` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `SportId` int NOT NULL,
  `LocationId` int NOT NULL,
  `Name` varchar(100) NOT NULL,
  `IsActive` tinyint(1) NOT NULL,
  `Gender` tinyint unsigned DEFAULT NULL,
  `AgeCategory` tinyint unsigned DEFAULT NULL,
  `Type` tinyint unsigned DEFAULT NULL,
  `BaseType` tinyint unsigned DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `Unique_Index` (`Name`,`LocationId`,`SportId`,`Gender`,`AgeCategory`,`Type`) USING BTREE,
  KEY `SportId` (`SportId`) USING BTREE,
  KEY `LocationId` (`LocationId`) USING BTREE,
  CONSTRAINT `participants_ibfk_1` FOREIGN KEY (`SportId`) REFERENCES `sports` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `participants_ibfk_2` FOREIGN KEY (`LocationId`) REFERENCES `locations` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=53365537 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_participants_insert_trigger` AFTER INSERT ON `participants` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('participants', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_participants_update_trigger` AFTER UPDATE ON `participants` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.sportid <=> new.sportid) || not(old.locationid <=> new.locationid) || not(binary old.name <=> binary new.name) || not(old.isactive <=> new.isactive) || not(old.gender <=> new.gender) || not(old.agecategory <=> new.agecategory) || not(old.type <=> new.type) || not(old.basetype <=> new.basetype) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('participants', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `fixture_reflastupdate_participants_update_trigger` AFTER UPDATE ON `participants` FOR EACH ROW begin 
                        update fixtures
                        set referenceslastupdate=now()
                        where statusid in (1, 2, 5, 6, 8, 9)
                        and startdate >= now() - interval (select loadinterval from sports where id=new.sportid) hour
                        and id in
                        (
                            select fixtureid
                            from fixtureparticipants
                            where participantid=new.id
                            order by fixtureid
                        )
                        order by id; end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_participants_delete_trigger` AFTER DELETE ON `participants` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('participants', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `fixture_reflastupdate_participants_delete_trigger` AFTER DELETE ON `participants` FOR EACH ROW begin 
                        update fixtures
                        set referenceslastupdate=now()
                        where statusid in (1, 2, 5, 6, 8, 9)
                        and startdate >= now() - interval (select loadinterval from sports where id=old.sportid) hour
                        and id in
                        (
                            select fixtureid
                            from fixtureparticipants
                            where participantid=old.id
                            order by fixtureid
                        )
                        order by id; end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `participants_copy`
--

DROP TABLE IF EXISTS `participants_copy`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `participants_copy` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `SportId` int NOT NULL,
  `LocationId` int NOT NULL,
  `Name` varchar(100) NOT NULL,
  `IsActive` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `SportId` (`SportId`) USING BTREE,
  KEY `LocationId` (`LocationId`) USING BTREE,
  CONSTRAINT `participants_copy_ibfk_1` FOREIGN KEY (`SportId`) REFERENCES `sports` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `participants_copy_ibfk_2` FOREIGN KEY (`LocationId`) REFERENCES `locations` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=52299948 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_participants_insert_trigger_copy` AFTER INSERT ON `participants_copy` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('participants', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_participants_update_trigger_copy` AFTER UPDATE ON `participants_copy` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.sportid <=> new.sportid) || not(old.locationid <=> new.locationid) || not(old.name <=> new.name) || not(old.isactive <=> new.isactive) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('participants', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_participants_delete_trigger_copy` AFTER DELETE ON `participants_copy` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('participants', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `participantsduplications`
--

DROP TABLE IF EXISTS `participantsduplications`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `participantsduplications` (
  `Id` int DEFAULT NULL,
  `SportId` int DEFAULT NULL,
  `LocationId` int DEFAULT NULL,
  `Name` varchar(100) DEFAULT NULL,
  `IsActive` tinyint DEFAULT NULL,
  `Gender` tinyint DEFAULT NULL,
  `AgeCategory` tinyint DEFAULT NULL,
  `Type` tinyint DEFAULT NULL,
  `BaseType` tinyint DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `participantstaticextradata`
--

DROP TABLE IF EXISTS `participantstaticextradata`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `participantstaticextradata` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ParticipantId` int NOT NULL,
  `Name` varchar(50) NOT NULL,
  `Value` varchar(255) NOT NULL,
  `CreationDate` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `ParticipantId_Name` (`ParticipantId`,`Name`) USING BTREE,
  KEY `ParticipantId` (`ParticipantId`) USING BTREE,
  CONSTRAINT `participantstaticextradata_ibfk_1` FOREIGN KEY (`ParticipantId`) REFERENCES `participants` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=2603861 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_participantstaticextradata_insert_trigger` AFTER INSERT ON `participantstaticextradata` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('participantstaticextradata', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `fixture_reflastupdate_participantstaticextradata_insert_trigger` AFTER INSERT ON `participantstaticextradata` FOR EACH ROW begin 
                        update fixtures
                        set referenceslastupdate=now()
                        where statusid in (1, 2, 5, 6, 8, 9)
                        and startdate >= now() - interval (select max(loadinterval) from sports) hour
                        and startdate >= now() - interval (select loadinterval from sports where id=sportid) hour
                        and id in
                        (
                            select fixtureid
                            from fixtureparticipants
                            where participantid=new.participantid
                            order by fixtureid
                        )
                        order by id; end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_participantstaticextradata_update_trigger` AFTER UPDATE ON `participantstaticextradata` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.participantid <=> new.participantid) || not(old.name <=> new.name) || not(old.value <=> new.value) || not(old.creationdate <=> new.creationdate) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('participantstaticextradata', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `fixture_reflastupdate_participantstaticextradata_update_trigger` AFTER UPDATE ON `participantstaticextradata` FOR EACH ROW begin 
                        update fixtures
                        set referenceslastupdate=now()
                        where statusid in (1, 2, 5, 6, 8, 9)
                        and startdate >= now() - interval (select max(loadinterval) from sports) hour
                        and startdate >= now() - interval (select loadinterval from sports where id=sportid) hour
                        and id in
                        (
                            select fixtureid
                            from fixtureparticipants
                            where participantid=new.participantid
                            order by fixtureid
                        )
                        order by id; end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_participantstaticextradata_delete_trigger` AFTER DELETE ON `participantstaticextradata` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('participantstaticextradata', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `fixture_reflastupdate_participantstaticextradata_delete_trigger` AFTER DELETE ON `participantstaticextradata` FOR EACH ROW begin 
                        update fixtures
                        set referenceslastupdate=now()
                        where statusid in (1, 2, 5, 6, 8, 9)
                        and startdate >= now() - interval (select max(loadinterval) from sports) hour
                        and startdate >= now() - interval (select loadinterval from sports where id=sportid) hour
                        and id in
                        (
                            select fixtureid
                            from fixtureparticipants
                            where participantid=old.participantid
                            order by fixtureid
                        )
                        order by id; end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `participantswithoutdiacritics`
--

DROP TABLE IF EXISTS `participantswithoutdiacritics`;

CREATE TABLE `participantswithoutdiacritics` (
                                                 `Id` INT NOT NULL AUTO_INCREMENT,
                                                 `Name` VARCHAR(255) NOT NULL,
                                                 `ParticipantId` INT NOT NULL,
                                                 `SportId` INT NOT NULL,
                                                 `LocationId` INT NOT NULL,
                                                 PRIMARY KEY (`Id`),
                                                 UNIQUE KEY `Idx_Name_Sport_Location` (`Name`, `SportId`, `LocationId`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=2094 DEFAULT CHARSET=utf8mb4;
--
-- Table structure for table `participantversionmapping`
--

DROP TABLE IF EXISTS `participantversionmapping`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `participantversionmapping` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `V3Id` int NOT NULL,
  `V4Id` int NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=88685 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `pendingfixturereasontype`
--

DROP TABLE IF EXISTS `pendingfixturereasontype`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pendingfixturereasontype` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `pendingfixtures`
--

DROP TABLE IF EXISTS `pendingfixtures`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pendingfixtures` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `SportID` int NOT NULL,
  `LeagueID` int NOT NULL,
  `HomeID` int DEFAULT NULL,
  `AwayID` int DEFAULT NULL,
  `HomeName` varchar(255) DEFAULT NULL,
  `AwayName` varchar(255) DEFAULT NULL,
  `LeagueName` varchar(255) NOT NULL,
  `LocationID` int NOT NULL,
  `StartDate` datetime(6) DEFAULT NULL,
  `StatusID` int NOT NULL,
  `ProviderEventID` int NOT NULL,
  `ProviderID` int NOT NULL,
  `Type` varchar(50) NOT NULL,
  `AutoAddParticipant` tinyint(1) NOT NULL,
  `HomeTeamCount` int NOT NULL,
  `AwayTeamCount` int NOT NULL,
  `LastUpdate` datetime(6) DEFAULT NULL,
  `AddedBy` varchar(20) NOT NULL,
  `AddedDate` datetime(6) DEFAULT NULL,
  `IsAdded` tinyint(1) NOT NULL,
  `IsIgnored` tinyint(1) NOT NULL,
  `PendingFixtureReasonTypeId` int DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `SportID` (`SportID`) USING BTREE,
  KEY `LeagueID` (`LeagueID`) USING BTREE,
  KEY `HomeID` (`HomeID`) USING BTREE,
  KEY `AwayID` (`AwayID`) USING BTREE,
  KEY `LocationID` (`LocationID`) USING BTREE,
  KEY `StatusID` (`StatusID`) USING BTREE,
  KEY `ProviderID` (`ProviderID`) USING BTREE,
  KEY `PendingFixtureReasonTypeId` (`PendingFixtureReasonTypeId`) USING BTREE,
  CONSTRAINT `pendingfixtures_ibfk6` FOREIGN KEY (`PendingFixtureReasonTypeId`) REFERENCES `pendingfixturereasontype` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `pendingfixtures_ibfk_1` FOREIGN KEY (`SportID`) REFERENCES `sports` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `pendingfixtures_ibfk_2` FOREIGN KEY (`LeagueID`) REFERENCES `leagues` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `pendingfixtures_ibfk_3` FOREIGN KEY (`LocationID`) REFERENCES `locations` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `pendingfixtures_ibfk_4` FOREIGN KEY (`StatusID`) REFERENCES `fixturestatuses` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `pendingfixtures_ibfk_5` FOREIGN KEY (`ProviderID`) REFERENCES `providers` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `pendingfixtures_new`
--

DROP TABLE IF EXISTS `pendingfixtures_new`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pendingfixtures_new` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `RobotId` int NOT NULL,
  `ProviderFixtureId` varchar(36) NOT NULL,
  `Metadata` mediumtext,
  `PotentialFixtures` text,
  `LastUpdate` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `IsActive` bit(1) NOT NULL,
  `ProviderName` varchar(35) NOT NULL,
  `StartDate` datetime NOT NULL,
  `IsOutright` bit(1) NOT NULL,
  `FixtureKey` varchar(255) DEFAULT NULL,
  `Ignored` bit(1) NOT NULL DEFAULT b'0',
  `Description` varchar(255) DEFAULT NULL,
  `CreationDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `ActionTaken` varchar(255) DEFAULT NULL,
  `StatusId` tinyint unsigned NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UQ_IX` (`FixtureKey`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=297506 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_pendingfixtures_new_insert_trigger` AFTER INSERT ON `pendingfixtures_new` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('pendingfixtures_new', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_pendingfixtures_new_update_trigger` AFTER UPDATE ON `pendingfixtures_new` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.robotid <=> new.robotid) || not(old.providername <=> new.providername) || not(old.providerfixtureid <=> new.providerfixtureid) || not(old.metadata <=> new.metadata) || not(old.potentialfixtures <=> new.potentialfixtures) || not(old.fixturekey <=> new.fixturekey) || not(old.lastupdate <=> new.lastupdate) || not(old.isactive <=> new.isactive) || not(old.startdate <=> new.startdate) || not(old.isoutright <=> new.isoutright) || not(old.ignored <=> new.ignored) || not(old.description <=> new.description) || not(old.creationdate <=> new.creationdate) || not(old.actiontaken <=> new.actiontaken) || not(old.statusid <=> new.statusid) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('pendingfixtures_new', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_pendingfixtures_new_delete_trigger` AFTER DELETE ON `pendingfixtures_new` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('pendingfixtures_new', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `pendingleaguemovements`
--

DROP TABLE IF EXISTS `pendingleaguemovements`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pendingleaguemovements` (
  `Id` int NOT NULL,
  `ToId` int NOT NULL,
  `HasHierarchy` bit(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_pendingleaguemovements_insert_trigger` AFTER INSERT ON `pendingleaguemovements` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('pendingleaguemovements', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_pendingleaguemovements_update_trigger` AFTER UPDATE ON `pendingleaguemovements` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.toid <=> new.toid) || not(old.hashierarchy <=> new.hashierarchy) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('pendingleaguemovements', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_pendingleaguemovements_delete_trigger` AFTER DELETE ON `pendingleaguemovements` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('pendingleaguemovements', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `periodmappings`
--

DROP TABLE IF EXISTS `periodmappings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `periodmappings` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ProviderId` int NOT NULL,
  `SportId` int NOT NULL,
  `MainId` int NOT NULL,
  `Name` varchar(100) NOT NULL,
  `IsActive` tinyint(1) NOT NULL DEFAULT '1',
  `CreationDate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`) USING BTREE,
  KEY `periodmappings_bfk1` (`MainId`) USING BTREE,
  KEY `ProviderId` (`ProviderId`) USING BTREE,
  KEY `SportId` (`SportId`) USING BTREE,
  CONSTRAINT `periodmappings_ibfk_1` FOREIGN KEY (`MainId`) REFERENCES `periodtypes` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `periodmappings_ibfk_2` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `periodmappings_ibfk_3` FOREIGN KEY (`SportId`) REFERENCES `sports` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=2773 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_periodmappings_insert_trigger` AFTER INSERT ON `periodmappings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('periodmappings', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_periodmappings_update_trigger` AFTER UPDATE ON `periodmappings` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.providerid <=> new.providerid) || not(old.sportid <=> new.sportid) || not(old.mainid <=> new.mainid) || not(old.name <=> new.name) || not(old.isactive <=> new.isactive) || not(old.creationdate <=> new.creationdate) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('periodmappings', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_periodmappings_delete_trigger` AFTER DELETE ON `periodmappings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('periodmappings', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `periodtypes`
--

DROP TABLE IF EXISTS `periodtypes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `periodtypes` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `SportId` int NOT NULL,
  `Name` varchar(50) NOT NULL,
  `DisplayId` int DEFAULT NULL,
  `MandatoryScorePeriods` varchar(2000) DEFAULT '',
  `IsAfterFt` tinyint(1) DEFAULT NULL,
  `Time` int DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `IX_PeriodTypes_UQ` (`SportId`,`Name`,`DisplayId`) USING BTREE,
  KEY `SportId` (`SportId`) USING BTREE,
  KEY `DisplayId` (`DisplayId`),
  CONSTRAINT `periodtypes_ibfk_1` FOREIGN KEY (`SportId`) REFERENCES `sports` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=326 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_periodtypes_insert_trigger` AFTER INSERT ON `periodtypes` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('periodtypes', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_periodtypes_update_trigger` AFTER UPDATE ON `periodtypes` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.sportid <=> new.sportid) || not(old.name <=> new.name) || not(old.displayid <=> new.displayid) || not(old.mandatoryscoreperiods <=> new.mandatoryscoreperiods) || not(old.isafterft <=> new.isafterft) || not(old.time <=> new.time) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('periodtypes', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_periodtypes_delete_trigger` AFTER DELETE ON `periodtypes` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('periodtypes', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `periodtypescoverage`
--

DROP TABLE IF EXISTS `periodtypescoverage`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `periodtypescoverage` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `LeagueId` int NOT NULL,
  `PeriodId` int NOT NULL,
  `ProviderId` int NOT NULL,
  `LastUpdate` datetime NOT NULL,
  `CreationDate` datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `reference_unique` (`LeagueId`,`PeriodId`,`ProviderId`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=2113696843 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `players`
--

DROP TABLE IF EXISTS `players`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `players` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FirstName` varchar(50) NOT NULL,
  `LastName` varchar(50) NOT NULL,
  `LocationId` int NOT NULL,
  `ParticipantId` int NOT NULL,
  `Height` int NOT NULL,
  `Weight` int NOT NULL,
  `PositionId` int NOT NULL,
  `ShirtNumber` int NOT NULL,
  `DateOfBirth` datetime(6) DEFAULT NULL,
  `Nationality` int NOT NULL,
  `PlaceOfBirth` int NOT NULL,
  `ProviderId` int NOT NULL,
  `ProviderPlayerId` int NOT NULL,
  `SportID` int DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_players_insert_trigger` AFTER INSERT ON `players` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('players', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_players_update_trigger` AFTER UPDATE ON `players` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.firstname <=> new.firstname) || not(old.lastname <=> new.lastname) || not(old.locationid <=> new.locationid) || not(old.participantid <=> new.participantid) || not(old.height <=> new.height) || not(old.weight <=> new.weight) || not(old.positionid <=> new.positionid) || not(old.shirtnumber <=> new.shirtnumber) || not(old.dateofbirth <=> new.dateofbirth) || not(old.nationality <=> new.nationality) || not(old.placeofbirth <=> new.placeofbirth) || not(old.providerid <=> new.providerid) || not(old.providerplayerid <=> new.providerplayerid) || not(old.sportid <=> new.sportid) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('players', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_players_delete_trigger` AFTER DELETE ON `players` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('players', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `playerstatisticscoverage`
--

DROP TABLE IF EXISTS `playerstatisticscoverage`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `playerstatisticscoverage` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `LeagueId` int NOT NULL,
  `PlayerStatisticId` int NOT NULL,
  `StatusId` int NOT NULL,
  `ProviderId` int NOT NULL,
  `LastUpdate` datetime NOT NULL,
  `CreationDate` datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `reference_unique` (`LeagueId`,`PlayerStatisticId`,`StatusId`,`ProviderId`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=1105229451 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `prioritysuggestions`
--

DROP TABLE IF EXISTS `prioritysuggestions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `prioritysuggestions` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Level` varchar(255) NOT NULL COMMENT 'sport/ league/ location',
  `LevelEntityId` int NOT NULL COMMENT 'sport id/ league id/ location id',
  `SportId` int NOT NULL,
  `IncidentId` int NOT NULL,
  `ProviderId` int NOT NULL,
  `Rank` int NOT NULL DEFAULT '-1',
  `UserRank` int DEFAULT NULL,
  `Precision` float(255,5) NOT NULL,
  `Latency` float(255,5) NOT NULL,
  `Recall` float(255,5) NOT NULL,
  `FixturesCount` int NOT NULL,
  `FixtureStatusId` int NOT NULL,
  `InsertDate` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `reference_unique` (`Level`,`LevelEntityId`,`SportId`,`IncidentId`,`ProviderId`,`FixtureStatusId`,`InsertDate`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=587845 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_prioritysuggestions_insert_trigger` AFTER INSERT ON `prioritysuggestions` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('prioritysuggestions', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_prioritysuggestions_update_trigger` AFTER UPDATE ON `prioritysuggestions` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.level <=> new.level) || not(old.levelentityid <=> new.levelentityid) || not(old.sportid <=> new.sportid) || not(old.incidentid <=> new.incidentid) || not(old.providerid <=> new.providerid) || not(old.rank <=> new.rank) || not(old.userrank <=> new.userrank) || not(old.precision <=> new.precision) || not(old.latency <=> new.latency) || not(old.recall <=> new.recall) || not(old.fixturescount <=> new.fixturescount) || not(old.fixturestatusid <=> new.fixturestatusid) || not(old.insertdate <=> new.insertdate) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('prioritysuggestions', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_prioritysuggestions_delete_trigger` AFTER DELETE ON `prioritysuggestions` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('prioritysuggestions', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `providerfixturecoverage`
--

DROP TABLE IF EXISTS `providerfixturecoverage`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `providerfixturecoverage` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Type` varchar(11) NOT NULL,
  `ProviderId` int NOT NULL,
  `FixtureId` int NOT NULL,
  `CreationDate` datetime NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `UniqueKeys` (`Type`,`ProviderId`,`FixtureId`) USING BTREE,
  KEY `ProviderForeignKey` (`ProviderId`) USING BTREE,
  KEY `FixtureIdForeignkey` (`FixtureId`) USING BTREE,
  CONSTRAINT `FixtureIdForeignkey` FOREIGN KEY (`FixtureId`) REFERENCES `fixtures` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `ProviderForeignKey` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=57627156 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_providerfixturecoverage_insert_trigger` AFTER INSERT ON `providerfixturecoverage` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('providerfixturecoverage', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_providerfixturecoverage_update_trigger` AFTER UPDATE ON `providerfixturecoverage` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.providerid <=> new.providerid) || not(old.type <=> new.type) || not(old.creationdate <=> new.creationdate) || not(old.fixtureid <=> new.fixtureid) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('providerfixturecoverage', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_providerfixturecoverage_delete_trigger` AFTER DELETE ON `providerfixturecoverage` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('providerfixturecoverage', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `providerfixturemetadata`
--

DROP TABLE IF EXISTS `providerfixturemetadata`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `providerfixturemetadata` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ProviderId` int NOT NULL,
  `ProviderFixtureId` varchar(255) NOT NULL,
  `LeagueName` varchar(255) NOT NULL,
  `SportId` int NOT NULL,
  `LocationName` varchar(255) NOT NULL,
  `StartDate` datetime(6) NOT NULL ON UPDATE CURRENT_TIMESTAMP(6),
  `HomeParticipantName` varchar(255) NOT NULL,
  `AwayParticipantName` varchar(255) NOT NULL,
  `HomeAfterTrackingName` varchar(255) DEFAULT NULL,
  `AwayAfterTrackingName` varchar(255) DEFAULT NULL,
  `RobotId` int NOT NULL,
  `LSportsId` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `reference_unique` (`ProviderFixtureId`,`RobotId`),
  KEY `ProviderId_FK` (`ProviderId`),
  KEY `RobotId_FK` (`RobotId`),
  KEY `SportsId_Fk` (`SportId`),
  KEY `IX_ProviderFixture` (`RobotId`,`ProviderFixtureId`),
  CONSTRAINT `ProviderId_FK` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `RobotId_FK` FOREIGN KEY (`RobotId`) REFERENCES `robots` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `SportsId_Fk` FOREIGN KEY (`SportId`) REFERENCES `sports` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=113647117 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `providermappings`
--

DROP TABLE IF EXISTS `providermappings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `providermappings` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `MainId` int NOT NULL,
  `Name` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `IsActive` tinyint(1) NOT NULL DEFAULT '1',
  `CreationDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpdate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `UserName` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL DEFAULT '',
  PRIMARY KEY (`Id`),
  KEY `providermappings_FK` (`MainId`),
  CONSTRAINT `providermappings_FK` FOREIGN KEY (`MainId`) REFERENCES `providers` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=513 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `providermarketmappings`
--

DROP TABLE IF EXISTS `providermarketmappings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `providermarketmappings` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `MarketId` int NOT NULL,
  `SportId` int NOT NULL,
  `LeagueId` int NOT NULL DEFAULT '-1',
  `Name` varchar(255) NOT NULL,
  `IsActive` bit(1) NOT NULL,
  `RobotId` int NOT NULL,
  `IsActive_Int` tinyint(1) GENERATED ALWAYS AS (cast(`IsActive` as unsigned)) VIRTUAL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `Unq_RobotId_SportId_LeagueId_Name` (`RobotId`,`SportId`,`LeagueId`,`Name`) USING BTREE,
  KEY `MarketID` (`MarketId`) USING BTREE,
  KEY `Name` (`Name`) USING BTREE,
  KEY `SportID` (`SportId`) USING BTREE,
  KEY `RobotID` (`RobotId`) USING BTREE,
  KEY `LeagueId` (`LeagueId`) USING BTREE,
  CONSTRAINT `providermarketmappings_ibfk_1` FOREIGN KEY (`RobotId`) REFERENCES `robots` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `providermarketmappings_ibfk_2` FOREIGN KEY (`MarketId`) REFERENCES `markets` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `providermarketmappings_ibfk_3` FOREIGN KEY (`SportId`) REFERENCES `sports` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `providermarketmappings_ibfk_4` FOREIGN KEY (`LeagueId`) REFERENCES `leagues` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=139422 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_providermarketmappings_insert_trigger` AFTER INSERT ON `providermarketmappings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('providermarketmappings', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_providermarketmappings_update_trigger` AFTER UPDATE ON `providermarketmappings` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.robotid <=> new.robotid) || not(old.marketid <=> new.marketid) || not(old.sportid <=> new.sportid) || not(old.name <=> new.name) || not(old.isactive <=> new.isactive) || not(old.leagueid <=> new.leagueid) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('providermarketmappings', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_providermarketmappings_delete_trigger` AFTER DELETE ON `providermarketmappings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('providermarketmappings', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `providermarketscoverage`
--

DROP TABLE IF EXISTS `providermarketscoverage`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `providermarketscoverage` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Type` varchar(11) NOT NULL,
  `MarketId` int NOT NULL,
  `ProviderId` int NOT NULL,
  `LeagueId` int NOT NULL,
  `LastUpdate` datetime NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `UniqueIndexes` (`MarketId`,`ProviderId`,`Type`,`LeagueId`) USING BTREE,
  KEY `LeagueIdForeignKey` (`LeagueId`) USING BTREE,
  KEY `ProviderIdForeignKey` (`ProviderId`) USING BTREE,
  KEY `MarketIdForeignKey` (`MarketId`) USING BTREE,
  CONSTRAINT `MarketIdForeignKey` FOREIGN KEY (`MarketId`) REFERENCES `markets` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `ProviderIdForeignKey` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=4339807 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `providers`
--

DROP TABLE IF EXISTS `providers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `providers` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  `IsActive` tinyint(1) NOT NULL DEFAULT '1',
  `IsExchange` tinyint(1) NOT NULL DEFAULT '0',
  `IsPrematch` bit(1) NOT NULL DEFAULT b'0',
  `IsInPlay` bit(1) NOT NULL DEFAULT b'0',
  `ParentId` int DEFAULT NULL,
  `IsPrematch_Int` tinyint(1) GENERATED ALWAYS AS (cast(`IsPrematch` as unsigned)) VIRTUAL,
  `IsInPlay_Int` tinyint(1) GENERATED ALWAYS AS (cast(`IsInPlay` as unsigned)) VIRTUAL,
  PRIMARY KEY (`Id`) USING BTREE,
  KEY `ParentId` (`ParentId`) USING BTREE,
  CONSTRAINT `providers_ibfk_1` FOREIGN KEY (`ParentId`) REFERENCES `providers` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=10013 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_providers_insert_trigger` AFTER INSERT ON `providers` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('providers', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_providers_update_trigger` AFTER UPDATE ON `providers` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.name <=> new.name) || not(old.isactive <=> new.isactive) || not(old.isexchange <=> new.isexchange) || not(old.parentid <=> new.parentid) || not(old.isprematch <=> new.isprematch) || not(old.isinplay <=> new.isinplay) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('providers', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_providers_delete_trigger` AFTER DELETE ON `providers` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('providers', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `providerstartdates`
--

DROP TABLE IF EXISTS `providerstartdates`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `providerstartdates` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ProviderId` int NOT NULL,
  `StartDate` datetime(6) DEFAULT NULL,
  `FixtureId` int NOT NULL,
  `CreationTime` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_Fixtures_Id_ProvidrDates_FixtureId` (`FixtureId`) USING BTREE,
  KEY `FK_Provider_Id_ProviderStartDates_ProviderId` (`ProviderId`) USING BTREE,
  KEY `Indx_ProviderStartDates_ProviderId_FixtureId` (`ProviderId`,`FixtureId`) USING BTREE,
  CONSTRAINT `providerstartdates_ibfk_1` FOREIGN KEY (`FixtureId`) REFERENCES `fixtures` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `providerstartdates_ibfk_2` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `proxies`
--

DROP TABLE IF EXISTS `proxies`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `proxies` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Ip` varchar(50) DEFAULT NULL,
  `Port` int NOT NULL,
  `LocationId` int DEFAULT NULL,
  `IsActive` tinyint(1) DEFAULT NULL,
  `Timezone` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Proxies_UQ` (`Ip`,`Port`) USING BTREE,
  KEY `LocationId` (`LocationId`) USING BTREE,
  CONSTRAINT `proxies_ibfk_1` FOREIGN KEY (`LocationId`) REFERENCES `locations` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=62116 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_proxies_insert_trigger` AFTER INSERT ON `proxies` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('proxies', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_proxies_update_trigger` AFTER UPDATE ON `proxies` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.ip <=> new.ip) || not(old.port <=> new.port) || not(old.locationid <=> new.locationid) || not(old.isactive <=> new.isactive) || not(old.timezone <=> new.timezone) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('proxies', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_proxies_delete_trigger` AFTER DELETE ON `proxies` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('proxies', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `proxies_backup`
--

DROP TABLE IF EXISTS `proxies_backup`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `proxies_backup` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Ip` varchar(50) DEFAULT NULL,
  `Port` int NOT NULL,
  `LocationId` int DEFAULT NULL,
  `IsActive` tinyint(1) DEFAULT NULL,
  `Timezone` int DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=61897 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `resultingmarketsettings`
--

DROP TABLE IF EXISTS `resultingmarketsettings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `resultingmarketsettings` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `SportId` int NOT NULL,
  `MarketId` int NOT NULL,
  `ShouldFilter` bit(1) NOT NULL DEFAULT b'0',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `ref_unique` (`SportId`,`MarketId`),
  KEY `market_id` (`MarketId`),
  CONSTRAINT `market_id` FOREIGN KEY (`MarketId`) REFERENCES `markets` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `sport_id` FOREIGN KEY (`SportId`) REFERENCES `sports` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=116 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `robotadaptermappings`
--

DROP TABLE IF EXISTS `robotadaptermappings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `robotadaptermappings` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `RobotIdV3` int NOT NULL,
  `RobotIdV4` int NOT NULL,
  `IsV4ToV3Enabled` bit(1) NOT NULL DEFAULT b'0',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `RobotIdV4` (`RobotIdV4`) USING BTREE,
  UNIQUE KEY `RobotIdV3` (`RobotIdV3`)
) ENGINE=InnoDB AUTO_INCREMENT=63 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_robotadaptermappings_insert_trigger` AFTER INSERT ON `robotadaptermappings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('robotadaptermappings', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_robotadaptermappings_update_trigger` AFTER UPDATE ON `robotadaptermappings` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.robotidv3 <=> new.robotidv3) || not(old.robotidv4 <=> new.robotidv4) || not(old.isv4tov3enabled <=> new.isv4tov3enabled) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('robotadaptermappings', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_robotadaptermappings_delete_trigger` AFTER DELETE ON `robotadaptermappings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('robotadaptermappings', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `robotdownloadsettingslog`
--

DROP TABLE IF EXISTS `robotdownloadsettingslog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `robotdownloadsettingslog` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `RobotId` int NOT NULL,
  `Settings` varchar(2048) NOT NULL,
  `SettingsName` varchar(255) NOT NULL,
  `Cookies` varchar(512) DEFAULT NULL,
  `Date` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `User` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `RobotDownloadSettingsLog_robotId_Name` (`RobotId`,`SettingsName`) USING BTREE,
  KEY `RobotDownloadSettingsLog_robotId` (`RobotId`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=2092 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `robots`
--

DROP TABLE IF EXISTS `robots`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `robots` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ProviderId` int NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Timezone` int DEFAULT '0',
  `Interval` int DEFAULT '30000',
  `Threads` int DEFAULT '100',
  `FixtureIdMappingEnabled` tinyint(1) DEFAULT NULL,
  `FixturesEnabled` tinyint(1) DEFAULT NULL,
  `LeagueMappingEnabled` tinyint(1) DEFAULT NULL,
  `SendToService` tinyint(1) DEFAULT NULL,
  `SendToPush` tinyint(1) DEFAULT NULL,
  `IsActive` tinyint(1) DEFAULT '1',
  `SatelliteProviders` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `RobotName` (`Name`) USING BTREE,
  KEY `ProviderId` (`ProviderId`) USING BTREE,
  CONSTRAINT `robots_ibfk_1` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=1392 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_robots_insert_trigger` AFTER INSERT ON `robots` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('robots', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_robots_update_trigger` AFTER UPDATE ON `robots` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.providerid <=> new.providerid) || not(old.name <=> new.name) || not(old.timezone <=> new.timezone) || not(old.interval <=> new.interval) || not(old.threads <=> new.threads) || not(old.fixtureidmappingenabled <=> new.fixtureidmappingenabled) || not(old.fixturesenabled <=> new.fixturesenabled) || not(old.leaguemappingenabled <=> new.leaguemappingenabled) || not(old.sendtoservice <=> new.sendtoservice) || not(old.sendtopush <=> new.sendtopush) || not(old.isactive <=> new.isactive) || not(old.satelliteproviders <=> new.satelliteproviders) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('robots', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_robots_delete_trigger` AFTER DELETE ON `robots` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('robots', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `robotsegmentstatus`
--

DROP TABLE IF EXISTS `robotsegmentstatus`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `robotsegmentstatus` (
  `Id` int NOT NULL,
  `SegmentStatus` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `robotsmonitoring`
--

DROP TABLE IF EXISTS `robotsmonitoring`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `robotsmonitoring` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ProviderId` int NOT NULL,
  `RobotId` int NOT NULL,
  `ComponentOption` varchar(255) DEFAULT NULL,
  `StatusId` int NOT NULL,
  `KeepAliveAmount` int NOT NULL,
  `ThresholdInSeconds` int NOT NULL DEFAULT '300',
  `LastUpdate` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `RobotSegmentStatus` int DEFAULT '2',
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `robot` (`RobotId`,`ComponentOption`) USING BTREE,
  KEY `RobotSegmentStatus` (`RobotSegmentStatus`),
  CONSTRAINT `RobotId` FOREIGN KEY (`RobotId`) REFERENCES `robots` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `robotsmonitoring_ibfk_1` FOREIGN KEY (`RobotSegmentStatus`) REFERENCES `robotsegmentstatus` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=7265 DEFAULT CHARSET=utf8mb3 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `robotsupportedsports`
--

DROP TABLE IF EXISTS `robotsupportedsports`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `robotsupportedsports` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `RobotId` int NOT NULL,
  `SportId` int NOT NULL,
  `IsActive` bit(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `SportId_fk` (`SportId`),
  KEY `RobotsId_fk` (`RobotId`),
  CONSTRAINT `RobotsId_fk` FOREIGN KEY (`RobotId`) REFERENCES `robots` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `SportId_fk` FOREIGN KEY (`SportId`) REFERENCES `sports` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=559 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `scoretypescoverage`
--

DROP TABLE IF EXISTS `scoretypescoverage`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `scoretypescoverage` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `LeagueId` int NOT NULL,
  `ScoreTypeId` int NOT NULL,
  `StatusId` int NOT NULL,
  `PeriodId` int DEFAULT NULL,
  `ProviderId` int NOT NULL,
  `ContainsPlayerName` bit(1) NOT NULL,
  `LastUpdate` datetime NOT NULL,
  `CreationDate` datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `reference_unique` (`LeagueId`,`ScoreTypeId`,`StatusId`,`PeriodId`,`ProviderId`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=14344152853 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `scraping_targets`
--

DROP TABLE IF EXISTS `scraping_targets`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `scraping_targets` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `url` varchar(300) NOT NULL DEFAULT '',
  `bad_strings` tinytext,
  `query` text,
  `extra_scan` int DEFAULT NULL,
  `scanners` tinytext,
  `required_str` varchar(100) DEFAULT NULL,
  `limit` int DEFAULT '3000',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Temporary view structure for view `sd_fixture_start_date_history`
--

DROP TABLE IF EXISTS `sd_fixture_start_date_history`;
/*!50001 DROP VIEW IF EXISTS `sd_fixture_start_date_history`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `sd_fixture_start_date_history` AS SELECT 
 1 AS `FixtureId`,
 1 AS `StartDate`,
 1 AS `SportId`,
 1 AS `SportName`,
 1 AS `league_type`,
 1 AS `CreationDate`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `sd_robots_accuracy`
--

DROP TABLE IF EXISTS `sd_robots_accuracy`;
/*!50001 DROP VIEW IF EXISTS `sd_robots_accuracy`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `sd_robots_accuracy` AS SELECT 
 1 AS `RobotId`,
 1 AS `SportId`,
 1 AS `accuracy`,
 1 AS `NumberOfFixtures`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `seasonmappings`
--

DROP TABLE IF EXISTS `seasonmappings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `seasonmappings` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `MainId` int DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `IsActive` tinyint(1) DEFAULT '1',
  `CreationDate` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `MainId_Name` (`MainId`,`Name`) USING BTREE,
  KEY `MainId` (`MainId`) USING BTREE,
  KEY `Name_IsActive` (`Name`,`IsActive`) USING BTREE,
  KEY `Name` (`Name`) USING BTREE,
  CONSTRAINT `seasonmappings_ibfk_1` FOREIGN KEY (`MainId`) REFERENCES `seasons` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=104 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_seasonmappings_insert_trigger` AFTER INSERT ON `seasonmappings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('seasonmappings', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_seasonmappings_update_trigger` AFTER UPDATE ON `seasonmappings` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.mainid <=> new.mainid) || not(old.name <=> new.name) || not(old.isactive <=> new.isactive) || not(old.creationdate <=> new.creationdate) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('seasonmappings', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_seasonmappings_delete_trigger` AFTER DELETE ON `seasonmappings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('seasonmappings', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `seasons`
--

DROP TABLE IF EXISTS `seasons`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `seasons` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(128) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `iu_uq` (`Name`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=2646 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_seasons_insert_trigger` AFTER INSERT ON `seasons` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('seasons', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_seasons_update_trigger` AFTER UPDATE ON `seasons` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.name <=> new.name) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('seasons', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `fixture_reflastupdate_seasons_update_trigger` AFTER UPDATE ON `seasons` FOR EACH ROW begin 
                            update fixtures
                            set referenceslastupdate=now()
                            where statusid in (1, 2, 5, 6, 8, 9)
                            and startdate >= now() - interval (select max(loadinterval) from sports) hour
                            and startdate >= now() - interval (select loadinterval from sports where id=sportid) hour
                            and seasonid=new.id
                            order by id; end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_seasons_delete_trigger` AFTER DELETE ON `seasons` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('seasons', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `fixture_reflastupdate_seasons_delete_trigger` AFTER DELETE ON `seasons` FOR EACH ROW begin 
                            update fixtures
                            set referenceslastupdate=now()
                            where statusid in (1, 2, 5, 6, 8, 9)
                            and startdate >= now() - interval (select max(loadinterval) from sports) hour
                            and startdate >= now() - interval (select loadinterval from sports where id=sportid) hour
                            and seasonid=old.id
                            order by id; end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `sentlivescorealerts`
--

DROP TABLE IF EXISTS `sentlivescorealerts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sentlivescorealerts` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `AlertTypeId` int NOT NULL,
  `HandledAlertId` int DEFAULT NULL,
  `FixtureId` int NOT NULL,
  `Statuses` varchar(255) DEFAULT NULL,
  `CurrentPeriod` varchar(255) DEFAULT NULL,
  `Scores` longtext,
  `Comment` varchar(255) DEFAULT NULL,
  `CreationDate` datetime(6) DEFAULT NULL,
  `SqlCreationDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
  PRIMARY KEY (`Id`),
  UNIQUE KEY `sentlivescorealerts_UN` (`FixtureId`,`AlertTypeId`),
  KEY `AlertTypeId` (`AlertTypeId`) USING BTREE,
  KEY `FixtureId` (`FixtureId`) USING BTREE,
  KEY `Handled_FixtureId` (`FixtureId`,`HandledAlertId`) USING BTREE,
  KEY `HandledAlertId` (`HandledAlertId`),
  KEY `sentlivescorealerts_CreationDate_IDX` (`CreationDate`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=186244371 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `settlementevent`
--

DROP TABLE IF EXISTS `settlementevent`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `settlementevent` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `IncidentId` int NOT NULL,
  `PeriodId` int NOT NULL,
  `MarketId` int NOT NULL,
  `LogicId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IncidentId` (`IncidentId`),
  KEY `MarketId` (`MarketId`),
  KEY `PeriodId` (`PeriodId`),
  CONSTRAINT `settlementevent_ibfk_1` FOREIGN KEY (`IncidentId`) REFERENCES `incidenttypes` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `settlementevent_ibfk_2` FOREIGN KEY (`MarketId`) REFERENCES `markets` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `settlementevent_ibfk_3` FOREIGN KEY (`PeriodId`) REFERENCES `periodtypes` (`DisplayId`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `settlementscoverage`
--

DROP TABLE IF EXISTS `settlementscoverage`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `settlementscoverage` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `MarketId` int NOT NULL,
  `SettlementType` varchar(255) NOT NULL,
  `LeagueId` int NOT NULL,
  `LastUpdate` datetime NOT NULL,
  `MarketSource` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `UniqueIndexes` (`MarketId`,`SettlementType`,`LeagueId`,`MarketSource`) USING BTREE,
  KEY `LeagueId_foreignKey` (`LeagueId`) USING BTREE,
  CONSTRAINT `LeagueId_foreignKey` FOREIGN KEY (`LeagueId`) REFERENCES `leagues` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `MarketId_foreignKey` FOREIGN KEY (`MarketId`) REFERENCES `markets` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=686 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `settlementtypes`
--

DROP TABLE IF EXISTS `settlementtypes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `settlementtypes` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(20) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `specialmarketorders`
--

DROP TABLE IF EXISTS `specialmarketorders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `specialmarketorders` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FixtureId` int DEFAULT NULL,
  `CustomerId` int DEFAULT NULL,
  `MarketId` int NOT NULL,
  `CreationDate` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FixtureId` (`FixtureId`) USING BTREE,
  KEY `MarketId` (`MarketId`) USING BTREE,
  CONSTRAINT `specialmarketorders_ibfk_1` FOREIGN KEY (`FixtureId`) REFERENCES `fixtures` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `specialmarketorders_ibfk_2` FOREIGN KEY (`MarketId`) REFERENCES `fixturemarkets` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `sportintervals`
--

DROP TABLE IF EXISTS `sportintervals`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sportintervals` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `SportId` int unsigned NOT NULL,
  `WideInterval` decimal(5,2) NOT NULL DEFAULT '6.00',
  `SingleParticipantInterval` decimal(5,2) NOT NULL DEFAULT '6.00',
  `DoubleHeaderInterval` decimal(5,2) NOT NULL DEFAULT '6.00',
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `sportintervals_index1` (`SportId`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=71 DEFAULT CHARSET=utf8mb3 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_sportintervals_insert_trigger` AFTER INSERT ON `sportintervals` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('sportintervals', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_sportintervals_update_trigger` AFTER UPDATE ON `sportintervals` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.sportid <=> new.sportid) || not(old.wideinterval <=> new.wideinterval) || not(old.singleparticipantinterval <=> new.singleparticipantinterval) || not(old.doubleheaderinterval <=> new.doubleheaderinterval) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('sportintervals', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_sportintervals_delete_trigger` AFTER DELETE ON `sportintervals` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('sportintervals', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `sportmappings`
--

DROP TABLE IF EXISTS `sportmappings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sportmappings` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `MainId` int DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `IsActive` tinyint(1) DEFAULT NULL,
  `CreationDate` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `MainId_Name` (`MainId`,`Name`) USING BTREE,
  KEY `MainId` (`MainId`) USING BTREE,
  KEY `Name_IsActive` (`Name`,`IsActive`) USING BTREE,
  CONSTRAINT `sportmappings_ibfk_1` FOREIGN KEY (`MainId`) REFERENCES `sports` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=1991 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_sportmappings_insert_trigger` AFTER INSERT ON `sportmappings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('sportmappings', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_sportmappings_update_trigger` AFTER UPDATE ON `sportmappings` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.mainid <=> new.mainid) || not(old.name <=> new.name) || not(old.isactive <=> new.isactive) || not(old.creationdate <=> new.creationdate) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('sportmappings', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_sportmappings_delete_trigger` AFTER DELETE ON `sportmappings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('sportmappings', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `sports`
--

DROP TABLE IF EXISTS `sports`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sports` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(20) NOT NULL,
  `LoadInterval` int NOT NULL DEFAULT '6',
  `AverageMatchLength` int DEFAULT NULL,
  `IsRacing` bit(1) DEFAULT NULL,
  `IsOutrightSport` bit(1) DEFAULT NULL,
  `IsRacing_Int` tinyint(1) GENERATED ALWAYS AS (cast(`IsRacing` as unsigned)) VIRTUAL,
  `IsOutrightSport_Int` tinyint(1) GENERATED ALWAYS AS (cast(`IsOutrightSport` as unsigned)) VIRTUAL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `Name` (`Name`) USING BTREE,
  KEY `sports_index1` (`LoadInterval`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=7587547 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_sports_insert_trigger` AFTER INSERT ON `sports` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('sports', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `fixture_reflastupdate_sports_update_trigger` AFTER UPDATE ON `sports` FOR EACH ROW begin 
                            update fixtures
                            set referenceslastupdate=now()
                            where statusid in (1, 2, 5, 6, 8, 9)
                            and startdate >= now() - interval (select max(loadinterval) from sports) hour
                            and startdate >= now() - interval (select loadinterval from sports where id=sportid) hour
                            and sportid=new.id
                            order by id; end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_sports_update_trigger` AFTER UPDATE ON `sports` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.name <=> new.name) || not(old.loadinterval <=> new.loadinterval) || not(old.averagematchlength <=> new.averagematchlength) || not(old.isracing <=> new.isracing) || not(old.isoutrightsport <=> new.isoutrightsport) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('sports', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_sports_delete_trigger` AFTER DELETE ON `sports` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('sports', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `fixture_reflastupdate_sports_delete_trigger` AFTER DELETE ON `sports` FOR EACH ROW begin 
                            update fixtures
                            set referenceslastupdate=now()
                            where statusid in (1, 2, 5, 6, 8, 9)
                            and startdate >= now() - interval (select max(loadinterval) from sports) hour
                            and startdate >= now() - interval (select loadinterval from sports where id=sportid) hour
                            and sportid=old.id
                            order by id; end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `standings`
--

DROP TABLE IF EXISTS `standings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `standings` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `LeagueId` int NOT NULL,
  `ParticipantId` int NOT NULL,
  `Position` varchar(3) NOT NULL,
  `HomePlayed` int NOT NULL,
  `HomeWon` int NOT NULL,
  `HomeLost` int NOT NULL,
  `HomeScored` int NOT NULL,
  `HomeConceded` int NOT NULL,
  `HomeDifference` int NOT NULL,
  `HomePoints` int NOT NULL,
  `HomeDraw` int NOT NULL,
  `AwayPlayed` int NOT NULL,
  `AwayWon` int NOT NULL,
  `AwayLost` int NOT NULL,
  `AwayScored` int NOT NULL,
  `AwayConceded` int NOT NULL,
  `AwayDifference` int NOT NULL,
  `AwayPoints` int NOT NULL,
  `AwayDraw` int NOT NULL,
  `TotalPlayed` int NOT NULL,
  `TotalWon` int NOT NULL,
  `TotalLost` int NOT NULL,
  `TotalScored` int DEFAULT NULL,
  `TotalConceded` int NOT NULL,
  `TotalDifference` int NOT NULL,
  `TotalPoints` int NOT NULL,
  `TotalDraw` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `LeagueId` (`LeagueId`) USING BTREE,
  KEY `ParticipantId` (`ParticipantId`) USING BTREE,
  CONSTRAINT `standings_ibfk_1` FOREIGN KEY (`LeagueId`) REFERENCES `leagues` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `standings_ibfk_2` FOREIGN KEY (`ParticipantId`) REFERENCES `participants` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `standingspointformula`
--

DROP TABLE IF EXISTS `standingspointformula`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `standingspointformula` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `SportId` int NOT NULL,
  `LocationId` int DEFAULT NULL,
  `LeagueId` int DEFAULT NULL,
  `LeagueNameContains` varchar(30) DEFAULT NULL,
  `Formula` varchar(100) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `SportId` (`SportId`) USING BTREE,
  KEY `LocationId` (`LocationId`) USING BTREE,
  KEY `LeagueId` (`LeagueId`) USING BTREE,
  CONSTRAINT `standingspointformula_ibfk_1` FOREIGN KEY (`SportId`) REFERENCES `sports` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `standingspointformula_ibfk_2` FOREIGN KEY (`LocationId`) REFERENCES `locations` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `standingspointformula_ibfk_3` FOREIGN KEY (`LeagueId`) REFERENCES `leagues` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `startdatedelays`
--

DROP TABLE IF EXISTS `startdatedelays`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `startdatedelays` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `SportId` int NOT NULL,
  `LocationId` int DEFAULT NULL,
  `TournamentId` int NOT NULL,
  `DelayInSeconds` int NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id` (`Id`) USING BTREE,
  KEY `Sport` (`SportId`) USING BTREE,
  KEY `Location` (`LocationId`) USING BTREE,
  CONSTRAINT `location` FOREIGN KEY (`LocationId`) REFERENCES `locations` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `sport` FOREIGN KEY (`SportId`) REFERENCES `sports` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=552 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_startdatedelays_insert_trigger` AFTER INSERT ON `startdatedelays` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('startdatedelays', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_startdatedelays_update_trigger` AFTER UPDATE ON `startdatedelays` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.locationid <=> new.locationid) || not(old.sportid <=> new.sportid) || not(old.tournamentid <=> new.tournamentid) || not(old.delayinseconds <=> new.delayinseconds) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('startdatedelays', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_startdatedelays_delete_trigger` AFTER DELETE ON `startdatedelays` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('startdatedelays', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `statemappings`
--

DROP TABLE IF EXISTS `statemappings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `statemappings` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `MainId` int NOT NULL,
  `Name` varchar(255) NOT NULL,
  `IsActive` tinyint(1) NOT NULL DEFAULT '1',
  `CreationDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
  `LastUpdate` datetime(6) DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP(6),
  `Username` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `MainId_Name` (`MainId`,`Name`) USING BTREE,
  KEY `Name_IsActive` (`Name`,`IsActive`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `states`
--

DROP TABLE IF EXISTS `states`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `states` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  `CountryId` int NOT NULL,
  `IsActive` tinyint(1) NOT NULL DEFAULT '1',
  `CreationDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
  `LastUpdate` datetime(6) DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP(6),
  `Username` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Name_CountryId` (`Name`,`CountryId`) USING BTREE,
  KEY `IsActive_CountryId` (`IsActive`,`CountryId`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `statisticaverages`
--

DROP TABLE IF EXISTS `statisticaverages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `statisticaverages` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `LeagueId` int DEFAULT NULL,
  `LocationId` int DEFAULT NULL,
  `IncidentTypeId` int DEFAULT NULL,
  `StatisticAverageTypeId` int DEFAULT NULL,
  `Average` double DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `LeagueId` (`LeagueId`) USING BTREE,
  KEY `LocationId` (`LocationId`) USING BTREE,
  KEY `IncidentTypeId` (`IncidentTypeId`) USING BTREE,
  CONSTRAINT `statisticaverages_ibfk_1` FOREIGN KEY (`LeagueId`) REFERENCES `leagues` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `statisticaverages_ibfk_2` FOREIGN KEY (`LocationId`) REFERENCES `locations` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `statisticaverages_ibfk_3` FOREIGN KEY (`IncidentTypeId`) REFERENCES `incidenttypes` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `statisticscoverage`
--

DROP TABLE IF EXISTS `statisticscoverage`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `statisticscoverage` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `LeagueId` int NOT NULL,
  `StatisticId` int NOT NULL,
  `StatusId` int NOT NULL,
  `PeriodId` int DEFAULT NULL,
  `ProviderId` int NOT NULL,
  `ContainsIncidentLog` bit(1) NOT NULL,
  `ContainsPlayerName` bit(1) NOT NULL,
  `LastUpdate` datetime NOT NULL,
  `CreationDate` datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `reference_unique` (`LeagueId`,`StatisticId`,`StatusId`,`PeriodId`,`ProviderId`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=6316666343 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `statusdescriptionmappings`
--

DROP TABLE IF EXISTS `statusdescriptionmappings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `statusdescriptionmappings` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `MainId` int DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `IsActive` tinyint(1) DEFAULT NULL,
  `CreationDate` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`) USING BTREE,
  KEY `MainId` (`MainId`) USING BTREE,
  CONSTRAINT `statusdescriptionmappings_ibfk_1` FOREIGN KEY (`MainId`) REFERENCES `statusdescriptions` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=48 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_statusdescriptionmappings_insert_trigger` AFTER INSERT ON `statusdescriptionmappings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('statusdescriptionmappings', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_statusdescriptionmappings_update_trigger` AFTER UPDATE ON `statusdescriptionmappings` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.mainid <=> new.mainid) || not(old.name <=> new.name) || not(old.isactive <=> new.isactive) || not(old.creationdate <=> new.creationdate) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('statusdescriptionmappings', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_statusdescriptionmappings_delete_trigger` AFTER DELETE ON `statusdescriptionmappings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('statusdescriptionmappings', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `statusdescriptions`
--

DROP TABLE IF EXISTS `statusdescriptions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `statusdescriptions` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(20) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `streamingschedule`
--

DROP TABLE IF EXISTS `streamingschedule`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `streamingschedule` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `IsActive` bit(1) NOT NULL,
  `FixtureId` int NOT NULL,
  `ProviderId` int NOT NULL,
  `Channel` varchar(512) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `StreamingSchedule_FixtureId_Provider` (`ProviderId`,`FixtureId`) USING BTREE,
  KEY `StreamingSchedule_FixtureId` (`FixtureId`),
  KEY `StreamingSchedule_FixtureId_Provider_IsActive` (`ProviderId`,`FixtureId`,`IsActive`),
  KEY `StreamingSchedule_FixtureId_IsActive` (`IsActive`,`FixtureId`)
) ENGINE=InnoDB AUTO_INCREMENT=81 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_streamingschedule_insert_trigger` AFTER INSERT ON `streamingschedule` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('streamingschedule', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_streamingschedule_update_trigger` AFTER UPDATE ON `streamingschedule` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.isactive <=> new.isactive) || not(old.providerid <=> new.providerid) || not(old.channel <=> new.channel) || not(old.fixtureid <=> new.fixtureid) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('streamingschedule', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_streamingschedule_delete_trigger` AFTER DELETE ON `streamingschedule` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('streamingschedule', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `subleaguemappings`
--

DROP TABLE IF EXISTS `subleaguemappings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `subleaguemappings` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `MainId` int DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `IsActive` bit(1) DEFAULT NULL,
  `CreationDate` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `MainId_Name` (`MainId`,`Name`) USING BTREE,
  KEY `SubLeagueMappings_MainId_FK` (`MainId`) USING BTREE,
  CONSTRAINT `subleaguemappings_ibfk_1` FOREIGN KEY (`MainId`) REFERENCES `subleagues` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=2356 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_subleaguemappings_insert_trigger` AFTER INSERT ON `subleaguemappings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('subleaguemappings', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_subleaguemappings_update_trigger` AFTER UPDATE ON `subleaguemappings` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.mainid <=> new.mainid) || not(old.name <=> new.name) || not(old.isactive <=> new.isactive) || not(old.creationdate <=> new.creationdate) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('subleaguemappings', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_subleaguemappings_delete_trigger` AFTER DELETE ON `subleaguemappings` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('subleaguemappings', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `subleagues`
--

DROP TABLE IF EXISTS `subleagues`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `subleagues` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  `LeagueId` int NOT NULL,
  `CreationDate` datetime(6) DEFAULT NULL,
  `IsActive` bit(1) DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `Nasme_LeagueId` (`Name`,`LeagueId`) USING BTREE,
  KEY `LeagueId` (`LeagueId`) USING BTREE,
  CONSTRAINT `subleagues_ibfk_1` FOREIGN KEY (`LeagueId`) REFERENCES `leagues` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=2356 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_subleagues_insert_trigger` AFTER INSERT ON `subleagues` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('subleagues', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_subleagues_update_trigger` AFTER UPDATE ON `subleagues` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.name <=> new.name) || not(old.leagueid <=> new.leagueid) || not(old.creationdate <=> new.creationdate) || not(old.isactive <=> new.isactive) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('subleagues', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_subleagues_delete_trigger` AFTER DELETE ON `subleagues` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('subleagues', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `suspensiondelays`
--

DROP TABLE IF EXISTS `suspensiondelays`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `suspensiondelays` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `SportId` int NOT NULL,
  `LeagueId` int NOT NULL DEFAULT '-1',
  `DelayInSeconds` int NOT NULL,
  `NoSuspension` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id` (`Id`),
  UNIQUE KEY `unique_sportId_leagueId` (`SportId`,`LeagueId`),
  KEY `idx_sportId_leagueId` (`SportId`,`LeagueId`),
  KEY `fk_league` (`LeagueId`)
) ENGINE=InnoDB AUTO_INCREMENT=80 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `suspensionmanagerconfiguration`
--

DROP TABLE IF EXISTS `suspensionmanagerconfiguration`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `suspensionmanagerconfiguration` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `configName` varchar(255) NOT NULL,
  `configValue` int NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `temptracking`
--

DROP TABLE IF EXISTS `temptracking`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `temptracking` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ServerName` varchar(255) NOT NULL,
  `ProcessId` int NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tmp_ids`
--

DROP TABLE IF EXISTS `tmp_ids`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tmp_ids` (
  `id` int DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tours`
--

DROP TABLE IF EXISTS `tours`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tours` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  `SportId` int NOT NULL,
  `ServiceLevel` int DEFAULT NULL,
  `IsActive` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `SportId_Name` (`Name`,`SportId`) USING BTREE,
  KEY `SportId` (`SportId`),
  CONSTRAINT `tours_ibfk_1` FOREIGN KEY (`SportId`) REFERENCES `sports` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=393 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `trackedchanges`
--

DROP TABLE IF EXISTS `trackedchanges`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `trackedchanges` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `TableName` varchar(50) DEFAULT NULL,
  `OperationType` int DEFAULT NULL,
  `ChangedId` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=159735415 DEFAULT CHARSET=utf8mb3 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `trackedchanges_v2`
--

DROP TABLE IF EXISTS `trackedchanges_v2`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `trackedchanges_v2` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `TableName` varchar(50) DEFAULT NULL,
  `OperationType` int DEFAULT NULL,
  `ChangedId` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `translations`
--

DROP TABLE IF EXISTS `translations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `translations` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `LanguageId` int DEFAULT NULL,
  `TableId` int DEFAULT NULL,
  `OriginalId` int DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  KEY `LanguageId` (`LanguageId`) USING BTREE,
  KEY `translations_ibfk_2` (`TableId`) USING BTREE,
  CONSTRAINT `translations_ibfk_1` FOREIGN KEY (`LanguageId`) REFERENCES `languages` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `translations_ibfk_2` FOREIGN KEY (`TableId`) REFERENCES `translationtables` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=1004790 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_translations_insert_trigger` AFTER INSERT ON `translations` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('translations', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_translations_update_trigger` AFTER UPDATE ON `translations` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.languageid <=> new.languageid) || not(old.tableid <=> new.tableid) || not(old.originalid <=> new.originalid) || not(old.name <=> new.name) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('translations', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_translations_delete_trigger` AFTER DELETE ON `translations` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('translations', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `translationtables`
--

DROP TABLE IF EXISTS `translationtables`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `translationtables` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tvchannels`
--

DROP TABLE IF EXISTS `tvchannels`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tvchannels` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  `LocationId` int DEFAULT NULL,
  `LanguageId` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `LanguageId` (`LanguageId`) USING BTREE,
  CONSTRAINT `tvchannels_ibfk_1` FOREIGN KEY (`LanguageId`) REFERENCES `languages` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tvfixtures`
--

DROP TABLE IF EXISTS `tvfixtures`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tvfixtures` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FixtureId` int DEFAULT NULL,
  `ChannelId` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `ChannelId` (`ChannelId`) USING BTREE,
  KEY `FixtureId` (`FixtureId`) USING BTREE,
  CONSTRAINT `tvfixtures_ibfk_1` FOREIGN KEY (`ChannelId`) REFERENCES `tvchannels` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `tvfixtures_ibfk_2` FOREIGN KEY (`FixtureId`) REFERENCES `fixtures` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `unmappedextradatakeys`
--

DROP TABLE IF EXISTS `unmappedextradatakeys`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `unmappedextradatakeys` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `Name` (`Name`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=539 DEFAULT CHARSET=utf8mb3 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_unmappedextradatakeys_insert_trigger` AFTER INSERT ON `unmappedextradatakeys` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('unmappedextradatakeys', 1, new.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_unmappedextradatakeys_update_trigger` AFTER UPDATE ON `unmappedextradatakeys` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.name <=> new.name) then
                                            insert data.trackedchanges(tablename,operationtype,changedid)
                                            values ('unmappedextradatakeys', 2, new.id);
                                        end if;
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_unmappedextradatakeys_delete_trigger` AFTER DELETE ON `unmappedextradatakeys` FOR EACH ROW begin
                                      insert data.trackedchanges(tablename,operationtype,changedid)
                                        values ('unmappedextradatakeys', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `uploadlog`
--

DROP TABLE IF EXISTS `uploadlog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `uploadlog` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `AssemblyName` varchar(255) NOT NULL,
  `Version` varchar(255) NOT NULL,
  `ChangeLog` text,
  `ServerName` varchar(255) NOT NULL,
  `UploadingMachine` varchar(255) NOT NULL,
  `UploadingUser` varchar(255) NOT NULL,
  `Time` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `Destination` int NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5924 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `venuemappings`
--

DROP TABLE IF EXISTS `venuemappings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `venuemappings` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `MainId` int NOT NULL,
  `Name` varchar(255) NOT NULL,
  `IsActive` tinyint(1) NOT NULL DEFAULT '1',
  `CreationDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
  `LastUpdate` datetime(6) DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP(6),
  `Username` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `MainId_Name` (`MainId`,`Name`) USING BTREE,
  KEY `Name_IsActive` (`Name`,`IsActive`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `venues`
--

DROP TABLE IF EXISTS `venues`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `venues` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  `CityId` int NOT NULL,
  `IsActive` tinyint(1) NOT NULL DEFAULT '1',
  `CreationDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
  `LastUpdate` datetime(6) DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP(6),
  `Username` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `CityId` (`CityId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `versionrepository`
--

DROP TABLE IF EXISTS `versionrepository`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `versionrepository` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `AssemblyName` varchar(255) NOT NULL,
  `Environment` int NOT NULL,
  `Date` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `Version` varchar(255) NOT NULL,
  `Path` varchar(255) NOT NULL,
  `UploadedBy` varchar(255) NOT NULL,
  `ChangeLog` text NOT NULL,
  `HasAssembly` bit(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=8131 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Current Database: `data`
--

USE `data`;

--
-- Final view structure for view `sd_fixture_start_date_history`
--

/*!50001 DROP VIEW IF EXISTS `sd_fixture_start_date_history`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb3 */;
/*!50001 SET character_set_results     = utf8mb3 */;
/*!50001 SET collation_connection      = utf8mb3_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`lsports_admin`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `sd_fixture_start_date_history` AS select `f`.`Id` AS `FixtureId`,`f`.`StartDate` AS `StartDate`,`f`.`SportId` AS `SportId`,(case when (`l`.`Type` = 5) then concat(`s`.`Name`,'_esports') else `s`.`Name` end) AS `SportName`,`l`.`Type` AS `league_type`,`f`.`CreationDate` AS `CreationDate` from ((`fixtures` `f` join `sports` `s` on((`s`.`Id` = `f`.`SportId`))) join `leagues` `l` on((`l`.`Id` = `f`.`LeagueId`))) where ((`f`.`StartDate` between (sysdate() - interval 3 month) and sysdate()) and `f`.`Id` in (select `fh`.`FixtureId` from `fixturehistory` `fh` where ((`fh`.`ProviderId` = 75) and (`fh`.`ChangedField` = 'StartDate'))) is false) union select `f`.`Id` AS `FixtureId`,str_to_date(`fh`.`NewValue`,'%m/%d/%Y %T') AS `StartDate`,`f`.`SportId` AS `SportId`,(case when (`l`.`Type` = 5) then concat(`s`.`Name`,'_esports') else `s`.`Name` end) AS `SportName`,`l`.`Type` AS `league_type`,`fh`.`ChangedDate` AS `CreationDate` from (((`fixtures` `f` join `sports` `s` on((`s`.`Id` = `f`.`SportId`))) join `leagues` `l` on((`l`.`Id` = `f`.`LeagueId`))) join `fixturehistory` `fh` on((`f`.`Id` = `fh`.`FixtureId`))) where ((`f`.`StartDate` between (sysdate() - interval 3 month) and sysdate()) and (`fh`.`ProviderId` = 75) and (`fh`.`ChangedField` = 'StartDate')) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `sd_robots_accuracy`
--

/*!50001 DROP VIEW IF EXISTS `sd_robots_accuracy`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb3 */;
/*!50001 SET character_set_results     = utf8mb3 */;
/*!50001 SET collation_connection      = utf8mb3_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`lsports_admin`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `sd_robots_accuracy` AS select `temp`.`RobotId` AS `RobotId`,`temp`.`SportId` AS `SportId`,avg(`temp`.`is_correct`) AS `accuracy`,count(0) AS `NumberOfFixtures` from (select `s`.`RobotId` AS `RobotId`,(case when (timestampdiff(MINUTE,`fh`.`ChangedDate`,`f`.`StartDate`) between -(10) and 10) then 1 else 0 end) AS `is_correct`,`l`.`Id` AS `LeagueId`,`f`.`SportId` AS `SportId` from (((`fixtures` `f` join `fixturehistory` `fh` on((`fh`.`FixtureId` = `f`.`Id`))) join `leagues` `l` on((`f`.`LeagueId` = `l`.`Id`))) join `mappingdata`.`suggestioncontexthistory` `s` on((`s`.`FixtureId` = `f`.`Id`))) where ((`fh`.`NewValue` = 'InProgress') and ((`fh`.`OldValue` = 'NSY') or (`fh`.`OldValue` = 'AboutToStart')) and (`fh`.`ChangedDate` is not null) and (`s`.`FixtureId` is not null) and (`s`.`CreationDate` < `f`.`StartDate`) and (`l`.`Type` <> 5) and (`fh`.`ChangedDate` between (sysdate() - interval 1 month) and sysdate()))) `temp` group by `temp`.`RobotId`,`temp`.`SportId` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-11-11 13:27:53
