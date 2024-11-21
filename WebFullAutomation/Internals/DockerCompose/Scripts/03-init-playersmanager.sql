-- MySQL dump 10.13  Distrib 8.3.0, for macos14.2 (arm64)
--
-- Host: legacy-sql.c4x04lsmqure.eu-west-1.rds.amazonaws.com    Database: playersmanager
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
-- Current Database: `playersmanager`
--

CREATE DATABASE /*!32312 IF NOT EXISTS*/ `playersmanager` /*!40100 DEFAULT CHARACTER SET latin1 */ /*!80016 DEFAULT ENCRYPTION='N' */;

USE `playersmanager`;

--
-- Table structure for table `cdc_dummy`
--

DROP TABLE IF EXISTS `cdc_dummy`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cdc_dummy` (
  `lastUpdated` timestamp NOT NULL,
  PRIMARY KEY (`lastUpdated`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `fixtureplayers`
--

DROP TABLE IF EXISTS `fixtureplayers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `fixtureplayers` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FixtureId` int NOT NULL,
  `PlayerId` int NOT NULL,
  `PositionId` int DEFAULT NULL,
  `StateId` int DEFAULT NULL,
  `ShirtNumber` int DEFAULT NULL,
  `IsCaptain` tinyint(1) DEFAULT NULL,
  `IsActive` tinyint NOT NULL,
  `CreationDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpdate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `fixtureplayers_key` (`FixtureId`,`PlayerId`) USING BTREE,
  KEY `fixtureplayers_index` (`FixtureId`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `player_creation_suggestions`
--

DROP TABLE IF EXISTS `player_creation_suggestions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `player_creation_suggestions` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `PlayerId` int NOT NULL,
  `SuggestedName` varchar(100) CHARACTER SET utf8mb3 DEFAULT NULL,
  `SuggestedTeam` int DEFAULT NULL,
  `SuggestedNationality` int DEFAULT NULL,
  `SuggestedBirthDate` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=5631 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `player_mapping_times`
--

DROP TABLE IF EXISTS `player_mapping_times`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `player_mapping_times` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FixtureId` int NOT NULL,
  `SportId` int NOT NULL,
  `PlayerName` varchar(255) CHARACTER SET utf8mb3 NOT NULL,
  `Time` decimal(20,3) unsigned NOT NULL,
  `CreationDate` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=47095 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `playercontextgroups`
--

DROP TABLE IF EXISTS `playercontextgroups`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `playercontextgroups` (
  `FixtureId` int NOT NULL,
  `MappedCount` smallint unsigned NOT NULL,
  `TotalCount` smallint unsigned NOT NULL,
  `Ignored` tinyint(1) NOT NULL,
  `HasMarkets` tinyint unsigned NOT NULL,
  PRIMARY KEY (`FixtureId`) USING BTREE,
  CONSTRAINT `playercontextgroups_fk1` FOREIGN KEY (`FixtureId`) REFERENCES `data`.`fixtures` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `playermappings`
--

DROP TABLE IF EXISTS `playermappings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `playermappings` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `MainId` int NOT NULL,
  `Name` varchar(255) CHARACTER SET utf8mb3 NOT NULL,
  `IsActive` bit(1) NOT NULL,
  `CreationDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpdate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `Username` varchar(45) CHARACTER SET utf8mb3 NOT NULL DEFAULT '',
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `playermappings_key` (`MainId`,`Name`) USING BTREE,
  KEY `playermappings_fk1` (`MainId`) USING BTREE,
  KEY `playermappings_index1` (`Name`,`IsActive`) USING BTREE,
  KEY `playermappings_index2` (`MainId`,`Name`,`IsActive`) USING BTREE,
  CONSTRAINT `playermappings_fk1` FOREIGN KEY (`MainId`) REFERENCES `players` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=589847 DEFAULT CHARSET=latin1;
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_playermappings_insert_trigger` AFTER INSERT ON `playermappings` FOR EACH ROW begin
                                      insert playersmanager.trackedchanges(tablename,operationtype,changedid)
                                        values ('playermappings', 1, new.id);
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_playermappings_update_trigger` AFTER UPDATE ON `playermappings` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.mainid <=> new.mainid) || not(old.name <=> new.name) || not(old.isactive <=> new.isactive) || not(old.creationdate <=> new.creationdate) || not(old.lastupdate <=> new.lastupdate) || not(old.username <=> new.username) then
                                            insert playersmanager.trackedchanges(tablename,operationtype,changedid)
                                            values ('playermappings', 2, new.id);
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_playermappings_delete_trigger` AFTER DELETE ON `playermappings` FOR EACH ROW begin
                                      insert playersmanager.trackedchanges(tablename,operationtype,changedid)
                                        values ('playermappings', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `playerpositionmappings`
--

DROP TABLE IF EXISTS `playerpositionmappings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `playerpositionmappings` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `MainId` int NOT NULL,
  `Name` varchar(255) NOT NULL,
  `IsActive` bit(1) NOT NULL,
  `CreationDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpdate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `Username` varchar(45) NOT NULL DEFAULT '',
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `playerpositionmappings_key` (`MainId`,`Name`) USING BTREE,
  KEY `playerpositionmappings_fk1` (`MainId`) USING BTREE,
  KEY `playerpositionmappings_index1` (`Name`,`IsActive`) USING BTREE,
  KEY `playerpositionmappings_index2` (`MainId`,`Name`,`IsActive`) USING BTREE,
  CONSTRAINT `playerpositionmappings_fk1` FOREIGN KEY (`MainId`) REFERENCES `playerpositions` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `playerpositions`
--

DROP TABLE IF EXISTS `playerpositions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `playerpositions` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  `IsActive` tinyint(1) NOT NULL,
  `CreationDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpdate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `Username` varchar(45) NOT NULL DEFAULT '',
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `playerposition_key` (`Name`) USING BTREE,
  KEY `playerpositions_index` (`Name`,`IsActive`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `players`
--

DROP TABLE IF EXISTS `players`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `players` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) CHARACTER SET utf8mb3 NOT NULL,
  `SportId` int DEFAULT NULL,
  `TeamId` int DEFAULT NULL,
  `NationalityId` int DEFAULT NULL,
  `IsActive` tinyint(1) NOT NULL,
  `BirthDate` datetime DEFAULT '0000-00-00 00:00:00',
  `CreationDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpdate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `Username` varchar(45) CHARACTER SET utf8mb3 NOT NULL DEFAULT '',
  `Type` tinyint unsigned NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `players_key` (`Name`,`TeamId`) USING BTREE,
  KEY `players_fk1` (`TeamId`) USING BTREE,
  KEY `players_fk2` (`NationalityId`) USING BTREE,
  KEY `players_fk3` (`SportId`) USING BTREE,
  KEY `players_index1` (`TeamId`,`IsActive`) USING BTREE,
  KEY `players_index2` (`Name`,`IsActive`) USING BTREE,
  CONSTRAINT `players_fk1` FOREIGN KEY (`TeamId`) REFERENCES `data`.`participants` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `players_fk2` FOREIGN KEY (`NationalityId`) REFERENCES `data`.`locations` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `players_fk3` FOREIGN KEY (`SportId`) REFERENCES `data`.`sports` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=635215 DEFAULT CHARSET=latin1;
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_players_insert_trigger` AFTER INSERT ON `players` FOR EACH ROW begin
                                      insert playersmanager.trackedchanges(tablename,operationtype,changedid)
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
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_players_update_trigger` AFTER UPDATE ON `players` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.name <=> new.name) || not(old.teamid <=> new.teamid) || not(old.nationalityid <=> new.nationalityid) || not(old.isactive <=> new.isactive) || not(old.birthdate <=> new.birthdate) || not(old.creationdate <=> new.creationdate) || not(old.lastupdate <=> new.lastupdate) || not(old.username <=> new.username) || not(old.type <=> new.type) || not(old.sportid <=> new.sportid) then
                                            insert playersmanager.trackedchanges(tablename,operationtype,changedid)
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
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_players_delete_trigger` AFTER DELETE ON `players` FOR EACH ROW begin
                                      insert playersmanager.trackedchanges(tablename,operationtype,changedid)
                                        values ('players', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `playerstatemappings`
--

DROP TABLE IF EXISTS `playerstatemappings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `playerstatemappings` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `MainId` int NOT NULL,
  `Name` varchar(255) NOT NULL,
  `IsActive` bit(1) NOT NULL,
  `CreationDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpdate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `Username` varchar(45) NOT NULL DEFAULT '',
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `playerstatesmappings_key` (`MainId`,`Name`) USING BTREE,
  KEY `playerstatesmappings_fk1` (`MainId`) USING BTREE,
  KEY `playerstatesmappings_index1` (`Name`,`IsActive`) USING BTREE,
  KEY `playerstatesmappings_index2` (`MainId`,`Name`,`IsActive`) USING BTREE,
  CONSTRAINT `playerstatesmappings_fk1` FOREIGN KEY (`MainId`) REFERENCES `playerstates` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `playerstates`
--

DROP TABLE IF EXISTS `playerstates`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `playerstates` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  `IsActive` tinyint(1) NOT NULL,
  `CreationDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpdate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `Username` varchar(45) NOT NULL DEFAULT '',
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `playerstates_key` (`Name`) USING BTREE,
  KEY `playerstates_index` (`Name`,`IsActive`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
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
) ENGINE=InnoDB AUTO_INCREMENT=417702 DEFAULT CHARSET=utf8mb3 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-11-11 13:28:38
