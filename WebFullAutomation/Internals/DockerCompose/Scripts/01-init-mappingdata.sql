-- MySQL dump 10.13  Distrib 8.3.0, for macos14.2 (arm64)
--
-- Host: legacy-sql.c4x04lsmqure.eu-west-1.rds.amazonaws.com    Database: mappingdata
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
SET @MYSQLDUMP_TEMP_LOG_BIN = @@SESSION.SQL_LOG_BIN;
SET @@SESSION.SQL_LOG_BIN= 0;

--
-- GTID state at the beginning of the backup 
--


--
-- Current Database: `mappingdata`
--

/*!40000 DROP DATABASE IF EXISTS `mappingdata`*/;

CREATE DATABASE /*!32312 IF NOT EXISTS*/ `mappingdata` /*!40100 DEFAULT CHARACTER SET utf8mb3 */ /*!80016 DEFAULT ENCRYPTION='N' */;

USE `mappingdata`;

--
-- Table structure for table `blacklistedmetadata`
--

DROP TABLE IF EXISTS `blacklistedmetadata`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `blacklistedmetadata` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `Type` tinyint unsigned NOT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `MatchType` tinyint unsigned NOT NULL,
  `ActionType` tinyint unsigned NOT NULL,
  `Username` varchar(45) NOT NULL DEFAULT '',
  `Reason` varchar(255) DEFAULT NULL,
  `SportId` int DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `blacklistedmetadata_unique` (`Type`,`Name`,`MatchType`,`SportId`) USING BTREE,
  KEY `blacklistedmetadata_name_index` (`Name`) USING BTREE,
  KEY `blacklistedmetadata_matchtype_index` (`MatchType`) USING BTREE,
  KEY `blacklistedmetadata_matchtype_name_index` (`Name`,`MatchType`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=75880 DEFAULT CHARSET=utf8mb3;
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_blacklistedmetadata_insert_trigger` AFTER INSERT ON `blacklistedmetadata` FOR EACH ROW begin
                                      insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                        values ('blacklistedmetadata', 1, new.id);
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_blacklistedmetadata_update_trigger` AFTER UPDATE ON `blacklistedmetadata` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.type <=> new.type) || not(old.name <=> new.name) || not(old.matchtype <=> new.matchtype) || not(old.actiontype <=> new.actiontype) || not(old.username <=> new.username) || not(old.reason <=> new.reason) || not(old.sportid <=> new.sportid) then
                                            insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                            values ('blacklistedmetadata', 2, new.id);
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_blacklistedmetadata_delete_trigger` AFTER DELETE ON `blacklistedmetadata` FOR EACH ROW begin
                                      insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                        values ('blacklistedmetadata', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `fixtureconflicts`
--

DROP TABLE IF EXISTS `fixtureconflicts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `fixtureconflicts` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `FixtureId` int NOT NULL,
  `SelectedValue` varchar(100) DEFAULT '',
  `Username` varchar(45) DEFAULT '',
  `Type` tinyint unsigned NOT NULL,
  `Status` tinyint unsigned NOT NULL,
  `ExpirationTime` datetime(6) NOT NULL,
  `DisableAutomaticUpdates` tinyint unsigned NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  KEY `FixtureId_Status_Index` (`FixtureId`,`Status`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=5080729 DEFAULT CHARSET=utf8mb3 ROW_FORMAT=DYNAMIC;
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixtureconflicts_insert_trigger` AFTER INSERT ON `fixtureconflicts` FOR EACH ROW begin
                                      insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                        values ('fixtureconflicts', 1, new.id);
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixtureconflicts_update_trigger` AFTER UPDATE ON `fixtureconflicts` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.fixtureid <=> new.fixtureid) || not(old.selectedvalue <=> new.selectedvalue) || not(old.username <=> new.username) || not(old.type <=> new.type) || not(old.status <=> new.status) || not(old.expirationtime <=> new.expirationtime) || not(old.disableautomaticupdates <=> new.disableautomaticupdates) then
                                            insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                            values ('fixtureconflicts', 2, new.id);
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixtureconflicts_delete_trigger` AFTER DELETE ON `fixtureconflicts` FOR EACH ROW begin
                                      insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                        values ('fixtureconflicts', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `fixtureconflictsextradata`
--

DROP TABLE IF EXISTS `fixtureconflictsextradata`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `fixtureconflictsextradata` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `ConflictId` int unsigned NOT NULL,
  `RobotId` int NOT NULL,
  `ProviderId` int NOT NULL,
  `Value` varchar(100) NOT NULL DEFAULT '',
  `LastHeartbeat` datetime(6) NOT NULL,
  `CreationDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
  PRIMARY KEY (`Id`) USING BTREE,
  KEY `conflicts_ed_fk1` (`ConflictId`) USING BTREE,
  KEY `LastHeartBeat_Index` (`LastHeartbeat`) USING BTREE,
  CONSTRAINT `conflicts_ed_fk1` FOREIGN KEY (`ConflictId`) REFERENCES `fixtureconflicts` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=34350601 DEFAULT CHARSET=utf8mb3 ROW_FORMAT=DYNAMIC;
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixtureconflictsextradata_insert_trigger` AFTER INSERT ON `fixtureconflictsextradata` FOR EACH ROW begin
                                      insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                        values ('fixtureconflictsextradata', 1, new.id);
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixtureconflictsextradata_update_trigger` AFTER UPDATE ON `fixtureconflictsextradata` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.conflictid <=> new.conflictid) || not(old.robotid <=> new.robotid) || not(old.providerid <=> new.providerid) || not(old.value <=> new.value) || not(old.lastheartbeat <=> new.lastheartbeat) || not(old.creationdate <=> new.creationdate) then
                                            insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                            values ('fixtureconflictsextradata', 2, new.id);
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_fixtureconflictsextradata_delete_trigger` AFTER DELETE ON `fixtureconflictsextradata` FOR EACH ROW begin
                                      insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                        values ('fixtureconflictsextradata', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `mappingconfigurations`
--

DROP TABLE IF EXISTS `mappingconfigurations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mappingconfigurations` (
  `Id` smallint unsigned NOT NULL AUTO_INCREMENT,
  `SettingLevel` int NOT NULL,
  `SettingType` int NOT NULL,
  `SettingLevelId` int DEFAULT NULL,
  `SettingTypeValue` varchar(255) NOT NULL,
  `IsOutright` tinyint unsigned NOT NULL,
  `LocationId` int DEFAULT NULL,
  `CreationDate` datetime(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3),
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `unique_index` (`SettingLevel`,`SettingType`,`SettingLevelId`,`IsOutright`,`LocationId`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=53173 DEFAULT CHARSET=utf8mb3 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_mappingconfigurations_insert_trigger` AFTER INSERT ON `mappingconfigurations` FOR EACH ROW begin
                                      insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                        values ('mappingconfigurations', 1, new.id);
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
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_mappingconfigurations_update_trigger` AFTER UPDATE ON `mappingconfigurations` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.settinglevel <=> new.settinglevel) || not(old.settinglevelid <=> new.settinglevelid) || not(old.settingtype <=> new.settingtype) || not(old.settingtypevalue <=> new.settingtypevalue) || not(old.isoutright <=> new.isoutright) || not(old.locationid <=> new.locationid) || not(old.creationdate <=> new.creationdate) then
                                            insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                            values ('mappingconfigurations', 2, new.id);
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
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_mappingconfigurations_delete_trigger` AFTER DELETE ON `mappingconfigurations` FOR EACH ROW begin
                                      insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                        values ('mappingconfigurations', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `participantmappingupdates`
--

DROP TABLE IF EXISTS `participantmappingupdates`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `participantmappingupdates` (
  `Id` int NOT NULL,
  `LastUpdate` datetime(3) NOT NULL,
  `UpdatedBy` varchar(255) NOT NULL,
  `IsVerified` bit(1) NOT NULL DEFAULT b'0',
  PRIMARY KEY (`Id`),
  KEY `LastUpdate` (`LastUpdate`,`IsVerified`),
  CONSTRAINT `participantmappingupdates_ibfk_1` FOREIGN KEY (`Id`) REFERENCES `data`.`participantmappings` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `participantmarkets`
--

DROP TABLE IF EXISTS `participantmarkets`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `participantmarkets` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `MarketId` int NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  KEY `fk1` (`MarketId`) USING BTREE,
  CONSTRAINT `fk1` FOREIGN KEY (`MarketId`) REFERENCES `data`.`markets` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=57 DEFAULT CHARSET=utf8mb3 ROW_FORMAT=DYNAMIC;
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_participantmarkets_insert_trigger` AFTER INSERT ON `participantmarkets` FOR EACH ROW begin
                                      insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                        values ('participantmarkets', 1, new.id);
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_participantmarkets_update_trigger` AFTER UPDATE ON `participantmarkets` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.marketid <=> new.marketid) then
                                            insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                            values ('participantmarkets', 2, new.id);
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_participantmarkets_delete_trigger` AFTER DELETE ON `participantmarkets` FOR EACH ROW begin
                                      insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                        values ('participantmarkets', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `providerdatatypes`
--

DROP TABLE IF EXISTS `providerdatatypes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `providerdatatypes` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `idx_name` (`Name`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `reports_fixturetimetomarket`
--

DROP TABLE IF EXISTS `reports_fixturetimetomarket`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reports_fixturetimetomarket` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `SportId` int NOT NULL,
  `IsEsports` tinyint unsigned NOT NULL,
  `CreationDate` datetime(6) NOT NULL,
  `FixtureCreationDate` datetime(6) NOT NULL,
  `EventArrivalDate` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  KEY `CreationDate_Index` (`CreationDate`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=2180118 DEFAULT CHARSET=utf8mb3;
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_reports_fixturetimetomarket_insert_trigger` AFTER INSERT ON `reports_fixturetimetomarket` FOR EACH ROW begin
                                      insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                        values ('reports_fixturetimetomarket', 1, new.id);
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_reports_fixturetimetomarket_update_trigger` AFTER UPDATE ON `reports_fixturetimetomarket` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.sportid <=> new.sportid) || not(old.isesports <=> new.isesports) || not(old.creationdate <=> new.creationdate) || not(old.fixturecreationdate <=> new.fixturecreationdate) || not(old.eventarrivaldate <=> new.eventarrivaldate) then
                                            insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                            values ('reports_fixturetimetomarket', 2, new.id);
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_reports_fixturetimetomarket_delete_trigger` AFTER DELETE ON `reports_fixturetimetomarket` FOR EACH ROW begin
                                      insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                        values ('reports_fixturetimetomarket', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `reports_ghostfixtures`
--

DROP TABLE IF EXISTS `reports_ghostfixtures`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reports_ghostfixtures` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `SportId` int NOT NULL,
  `IsEsports` tinyint unsigned NOT NULL,
  `Type` tinyint unsigned NOT NULL,
  `CreationDate` datetime(6) NOT NULL,
  `FixtureId` int NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  KEY `CreationDate_Index` (`CreationDate`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=469143 DEFAULT CHARSET=utf8mb3;
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_reports_ghostfixtures_insert_trigger` AFTER INSERT ON `reports_ghostfixtures` FOR EACH ROW begin
                                      insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                        values ('reports_ghostfixtures', 1, new.id);
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_reports_ghostfixtures_update_trigger` AFTER UPDATE ON `reports_ghostfixtures` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.sportid <=> new.sportid) || not(old.isesports <=> new.isesports) || not(old.type <=> new.type) || not(old.creationdate <=> new.creationdate) || not(old.fixtureid <=> new.fixtureid) then
                                            insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                            values ('reports_ghostfixtures', 2, new.id);
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_reports_ghostfixtures_delete_trigger` AFTER DELETE ON `reports_ghostfixtures` FOR EACH ROW begin
                                      insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                        values ('reports_ghostfixtures', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `reports_manualfixtureresolution`
--

DROP TABLE IF EXISTS `reports_manualfixtureresolution`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reports_manualfixtureresolution` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `SportId` int NOT NULL,
  `IsEsports` tinyint unsigned NOT NULL,
  `Type` tinyint unsigned NOT NULL,
  `CreationDate` datetime(6) NOT NULL,
  `FixtureId` int DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  KEY `CreationDate_Index` (`CreationDate`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=113523 DEFAULT CHARSET=utf8mb3;
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_reports_manualfixtureresolution_insert_trigger` AFTER INSERT ON `reports_manualfixtureresolution` FOR EACH ROW begin
                                      insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                        values ('reports_manualfixtureresolution', 1, new.id);
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_reports_manualfixtureresolution_update_trigger` AFTER UPDATE ON `reports_manualfixtureresolution` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.sportid <=> new.sportid) || not(old.isesports <=> new.isesports) || not(old.type <=> new.type) || not(old.creationdate <=> new.creationdate) || not(old.fixtureid <=> new.fixtureid) then
                                            insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                            values ('reports_manualfixtureresolution', 2, new.id);
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_reports_manualfixtureresolution_delete_trigger` AFTER DELETE ON `reports_manualfixtureresolution` FOR EACH ROW begin
                                      insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                        values ('reports_manualfixtureresolution', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `reports_newvsmanualmappings`
--

DROP TABLE IF EXISTS `reports_newvsmanualmappings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reports_newvsmanualmappings` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `SportId` int NOT NULL,
  `IsEsports` tinyint unsigned NOT NULL,
  `GroupId` int unsigned NOT NULL,
  `CreationDate` datetime(6) NOT NULL,
  `Type` tinyint unsigned NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  KEY `CreationDate_Index` (`CreationDate`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=4392753 DEFAULT CHARSET=utf8mb3;
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_reports_newvsmanualmappings_insert_trigger` AFTER INSERT ON `reports_newvsmanualmappings` FOR EACH ROW begin
                                      insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                        values ('reports_newvsmanualmappings', 1, new.id);
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_reports_newvsmanualmappings_update_trigger` AFTER UPDATE ON `reports_newvsmanualmappings` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.sportid <=> new.sportid) || not(old.isesports <=> new.isesports) || not(old.groupid <=> new.groupid) || not(old.type <=> new.type) || not(old.creationdate <=> new.creationdate) then
                                            insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                            values ('reports_newvsmanualmappings', 2, new.id);
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_reports_newvsmanualmappings_delete_trigger` AFTER DELETE ON `reports_newvsmanualmappings` FOR EACH ROW begin
                                      insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                        values ('reports_newvsmanualmappings', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `reports_potentialalerts`
--

DROP TABLE IF EXISTS `reports_potentialalerts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reports_potentialalerts` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `SportId` int NOT NULL,
  `IsEsports` tinyint unsigned NOT NULL,
  `GroupId` int unsigned NOT NULL,
  `CreationDate` datetime(6) NOT NULL,
  `Alert` varchar(50) NOT NULL,
  `CreationMethod` tinyint unsigned NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  KEY `CreationDate_Index` (`CreationDate`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=1697666 DEFAULT CHARSET=utf8mb3;
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_reports_potentialalerts_insert_trigger` AFTER INSERT ON `reports_potentialalerts` FOR EACH ROW begin
                                      insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                        values ('reports_potentialalerts', 1, new.id);
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_reports_potentialalerts_update_trigger` AFTER UPDATE ON `reports_potentialalerts` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.sportid <=> new.sportid) || not(old.isesports <=> new.isesports) || not(old.groupid <=> new.groupid) || not(old.alert <=> new.alert) || not(old.creationdate <=> new.creationdate) then
                                            insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                            values ('reports_potentialalerts', 2, new.id);
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_reports_potentialalerts_delete_trigger` AFTER DELETE ON `reports_potentialalerts` FOR EACH ROW begin
                                      insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                        values ('reports_potentialalerts', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `reports_providersaccuracy`
--

DROP TABLE IF EXISTS `reports_providersaccuracy`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reports_providersaccuracy` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `RobotId` int NOT NULL,
  `ProviderId` int NOT NULL,
  `SportId` int NOT NULL,
  `IsEsports` tinyint unsigned NOT NULL,
  `FixtureId` int NOT NULL,
  `RobotStartDate` datetime(6) NOT NULL,
  `FixtureStartDate` datetime(6) NOT NULL,
  `IsCorrect` tinyint unsigned NOT NULL,
  `Type` tinyint unsigned NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  KEY `FixtureStartDate_Index` (`FixtureStartDate`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=24709072 DEFAULT CHARSET=utf8mb3;
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_reports_providersaccuracy_insert_trigger` AFTER INSERT ON `reports_providersaccuracy` FOR EACH ROW begin
                                      insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                        values ('reports_providersaccuracy', 1, new.id);
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_reports_providersaccuracy_update_trigger` AFTER UPDATE ON `reports_providersaccuracy` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.robotid <=> new.robotid) || not(old.providerid <=> new.providerid) || not(old.sportid <=> new.sportid) || not(old.isesports <=> new.isesports) || not(old.fixtureid <=> new.fixtureid) || not(old.robotstartdate <=> new.robotstartdate) || not(old.fixturestartdate <=> new.fixturestartdate) || not(old.iscorrect <=> new.iscorrect) || not(old.type <=> new.type) then
                                            insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                            values ('reports_providersaccuracy', 2, new.id);
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_reports_providersaccuracy_delete_trigger` AFTER DELETE ON `reports_providersaccuracy` FOR EACH ROW begin
                                      insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                        values ('reports_providersaccuracy', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `reports_startdatemanualupdates`
--

DROP TABLE IF EXISTS `reports_startdatemanualupdates`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reports_startdatemanualupdates` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `SportId` int NOT NULL,
  `IsEsports` tinyint unsigned NOT NULL,
  `FixtureId` int NOT NULL,
  `PreviousDate` datetime(6) DEFAULT NULL,
  `NewDate` datetime(6) NOT NULL,
  `CreationDate` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  KEY `CreationDate_Index` (`CreationDate`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=46 DEFAULT CHARSET=utf8mb3;
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_reports_startdatemanualupdates_insert_trigger` AFTER INSERT ON `reports_startdatemanualupdates` FOR EACH ROW begin
                                      insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                        values ('reports_startdatemanualupdates', 1, new.id);
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_reports_startdatemanualupdates_update_trigger` AFTER UPDATE ON `reports_startdatemanualupdates` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.sportid <=> new.sportid) || not(old.isesports <=> new.isesports) || not(old.fixtureid <=> new.fixtureid) || not(old.previousdate <=> new.previousdate) || not(old.newdate <=> new.newdate) || not(old.creationdate <=> new.creationdate) then
                                            insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                            values ('reports_startdatemanualupdates', 2, new.id);
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
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_reports_startdatemanualupdates_delete_trigger` AFTER DELETE ON `reports_startdatemanualupdates` FOR EACH ROW begin
                                      insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                        values ('reports_startdatemanualupdates', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `robotconfigurations`
--

DROP TABLE IF EXISTS `robotconfigurations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `robotconfigurations` (
  `Id` smallint unsigned NOT NULL AUTO_INCREMENT,
  `ProviderId` int unsigned NOT NULL,
  `RobotId` int unsigned DEFAULT NULL,
  `SettingLevel` int NOT NULL,
  `SettingType` int NOT NULL,
  `SettingLevelId` int DEFAULT NULL,
  `SettingTypeValue` varchar(255) NOT NULL,
  `IsOutright` tinyint unsigned NOT NULL,
  `LocationId` int DEFAULT NULL,
  `CreationDate` datetime(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3),
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `robotconfigurations_unique_index` (`ProviderId`,`RobotId`,`SettingLevel`,`SettingType`,`SettingLevelId`,`IsOutright`,`LocationId`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=11514 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_robotconfigurations_insert_trigger` AFTER INSERT ON `robotconfigurations` FOR EACH ROW begin
                                      insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                        values ('robotconfigurations', 1, new.id);
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
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_robotconfigurations_update_trigger` AFTER UPDATE ON `robotconfigurations` FOR EACH ROW begin
                                      if not(old.id <=> new.id) || not(old.providerid <=> new.providerid) || not(old.robotid <=> new.robotid) || not(old.settinglevel <=> new.settinglevel) || not(old.settinglevelid <=> new.settinglevelid) || not(old.settingtype <=> new.settingtype) || not(old.settingtypevalue <=> new.settingtypevalue) || not(old.isoutright <=> new.isoutright) || not(old.locationid <=> new.locationid) then
                                            insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                            values ('robotconfigurations', 2, new.id);
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
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`lsports_admin`@`%`*/ /*!50003 TRIGGER `trackedchanges_robotconfigurations_delete_trigger` AFTER DELETE ON `robotconfigurations` FOR EACH ROW begin
                                      insert mappingdata.trackedchanges(tablename,operationtype,changedid)
                                        values ('robotconfigurations', 3, old.id);
                                  end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `suggestioncontextextradata`
--

DROP TABLE IF EXISTS `suggestioncontextextradata`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `suggestioncontextextradata` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `ContextId` int unsigned NOT NULL,
  `Key` varchar(50) NOT NULL,
  `Value` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `suggestioncontextextradata_key1` (`ContextId`,`Key`) USING BTREE,
  KEY `suggestioncontextextradata_fk1` (`ContextId`) USING BTREE,
  CONSTRAINT `suggestioncontextextradata_fk1` FOREIGN KEY (`ContextId`) REFERENCES `suggestioncontexts` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=3379032 DEFAULT CHARSET=utf8mb3 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `suggestioncontextfixtures`
--

DROP TABLE IF EXISTS `suggestioncontextfixtures`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `suggestioncontextfixtures` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `ContextId` int unsigned NOT NULL,
  `FixtureId` int NOT NULL,
  `Confidence` tinyint unsigned NOT NULL,
  `Reason` tinyint unsigned NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `suggestioncontextfixtures_key1` (`ContextId`,`FixtureId`,`Reason`) USING BTREE,
  KEY `suggestioncontextfixtures_fk1` (`ContextId`) USING BTREE,
  KEY `FixtureId` (`FixtureId`) USING BTREE,
  KEY `Reason` (`Reason`) USING BTREE,
  CONSTRAINT `suggestioncontextfixtures_fk1` FOREIGN KEY (`ContextId`) REFERENCES `suggestioncontexts` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=119960809 DEFAULT CHARSET=utf8mb3 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `suggestioncontextgroups`
--

DROP TABLE IF EXISTS `suggestioncontextgroups`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `suggestioncontextgroups` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `Status` tinyint unsigned NOT NULL,
  `IgnoreStatus` tinyint NOT NULL,
  `Approved` bit(1) NOT NULL,
  `FixtureId` int DEFAULT NULL,
  `SportId` int NOT NULL,
  `IsOutright` tinyint unsigned NOT NULL,
  `ExpirationTime` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=26389353 DEFAULT CHARSET=utf8mb3 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `suggestioncontextgroups_archive`
--

DROP TABLE IF EXISTS `suggestioncontextgroups_archive`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `suggestioncontextgroups_archive` (
  `Id` int unsigned NOT NULL,
  `FixtureId` int DEFAULT NULL,
  `Json` mediumtext NOT NULL,
  `CreationDate` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  KEY `CreationDate_Index` (`CreationDate`) USING BTREE,
  KEY `FixtureId_Index` (`FixtureId`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `suggestioncontexthistory`
--

DROP TABLE IF EXISTS `suggestioncontexthistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `suggestioncontexthistory` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `GroupId` int unsigned NOT NULL,
  `ContextId` int unsigned NOT NULL,
  `RobotId` int NOT NULL,
  `ProviderId` int NOT NULL,
  `ProviderFixtureId` varchar(36) NOT NULL,
  `SportId` int NOT NULL,
  `LeagueId` int DEFAULT NULL,
  `StartDate` datetime(6) NOT NULL,
  `CreationDate` datetime(6) NOT NULL,
  `FixtureId` int DEFAULT NULL,
  `LeagueName` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  KEY `CreationDate_Index` (`CreationDate`) USING BTREE,
  KEY `GroupId_Index` (`GroupId`) USING BTREE,
  KEY `FixtureId_Index` (`FixtureId`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=100030967 DEFAULT CHARSET=utf8mb3 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `suggestioncontextmarkets`
--

DROP TABLE IF EXISTS `suggestioncontextmarkets`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `suggestioncontextmarkets` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `ContextId` int unsigned NOT NULL,
  `MarketId` int DEFAULT NULL,
  `MarketSourceName` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `suggestioncontextmarkets_key1` (`ContextId`,`MarketId`,`MarketSourceName`) USING BTREE,
  KEY `suggestioncontextmarkets_fk1` (`ContextId`) USING BTREE,
  CONSTRAINT `suggestioncontextmarkets_fk1` FOREIGN KEY (`ContextId`) REFERENCES `suggestioncontexts` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `suggestioncontextmetadata`
--

DROP TABLE IF EXISTS `suggestioncontextmetadata`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `suggestioncontextmetadata` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `HolderId` int unsigned NOT NULL,
  `MainId` int NOT NULL,
  `Confidence` tinyint unsigned NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `suggestioncontextmetadata_index1` (`HolderId`,`MainId`) USING BTREE,
  KEY `suggestioncontextmetadata_fk1` (`HolderId`) USING BTREE,
  KEY `MainId` (`MainId`) USING BTREE,
  CONSTRAINT `suggestioncontextmetadata_fk1` FOREIGN KEY (`HolderId`) REFERENCES `suggestioncontextmetadataholders` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=726487093 DEFAULT CHARSET=utf8mb3 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `suggestioncontextmetadataholderextradata`
--

DROP TABLE IF EXISTS `suggestioncontextmetadataholderextradata`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `suggestioncontextmetadataholderextradata` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `HolderId` int unsigned NOT NULL,
  `Key` varchar(50) NOT NULL,
  `Value` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `suggestioncontextproviderparticipantextradata_HolderId_Key` (`HolderId`,`Key`) USING BTREE,
  KEY `suggestioncontextproviderparticipantextradata_fk1` (`HolderId`) USING BTREE,
  CONSTRAINT `suggestioncontextproviderparticipantextradata_fk1` FOREIGN KEY (`HolderId`) REFERENCES `suggestioncontextmetadataholders` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=407324954 DEFAULT CHARSET=utf8mb3 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `suggestioncontextmetadataholders`
--

DROP TABLE IF EXISTS `suggestioncontextmetadataholders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `suggestioncontextmetadataholders` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `ContextId` int unsigned NOT NULL,
  `SourceName` varchar(255) DEFAULT NULL,
  `Type` tinyint unsigned NOT NULL,
  `Identifier` varchar(10) NOT NULL,
  `IsErased` tinyint unsigned NOT NULL,
  `EraseReason` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `ContextId_Type_Identifier` (`ContextId`,`Type`,`Identifier`) USING BTREE,
  KEY `suggestioncontextmetadataholders_fk1` (`ContextId`) USING BTREE,
  KEY `suggestioncontextmetadataholders_name_index` (`SourceName`) USING BTREE,
  KEY `suggestioncontextmetadataholders_iserased_index` (`IsErased`) USING BTREE,
  KEY `Type_IsErased_SourceName` (`Type`,`IsErased`,`SourceName`) USING BTREE,
  KEY `Type` (`Type`) USING BTREE,
  KEY `ContextId_SourceName_Type_Identifier` (`ContextId`,`SourceName`,`Type`,`Identifier`) USING BTREE,
  KEY `type_sourcename_index` (`Type`,`SourceName`),
  CONSTRAINT `suggestioncontextmetadataholders_fk1` FOREIGN KEY (`ContextId`) REFERENCES `suggestioncontexts` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=769392652 DEFAULT CHARSET=utf8mb3 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `suggestioncontexts`
--

DROP TABLE IF EXISTS `suggestioncontexts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `suggestioncontexts` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `GroupId` int unsigned NOT NULL,
  `RobotId` int NOT NULL,
  `ProviderId` int NOT NULL,
  `ProviderFixtureId` varchar(64) NOT NULL,
  `SportId` int NOT NULL,
  `StartDate` datetime(6) NOT NULL,
  `CreationDate` datetime(6) NOT NULL,
  `Priority` tinyint unsigned NOT NULL,
  `Status` tinyint unsigned NOT NULL,
  `FullHashCode` varchar(255) DEFAULT NULL,
  `SlimHashCode` varchar(255) DEFAULT NULL,
  `IsOutright` tinyint unsigned NOT NULL,
  `State` tinyint unsigned NOT NULL,
  `DataSourceIdentifier` varchar(16) DEFAULT NULL,
  `Category` tinyint unsigned NOT NULL,
  `ProviderDataType` int DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `suggestioncontexts_providerfixtureid_robotid` (`RobotId`,`ProviderFixtureId`) USING BTREE,
  UNIQUE KEY `suggestioncontexts_hashcode` (`FullHashCode`) USING BTREE,
  KEY `suggestioncontexts_fk1` (`GroupId`) USING BTREE,
  KEY `suggestioncontexts_creationdate` (`CreationDate`) USING BTREE,
  KEY `suggestioncontexts_state` (`State`) USING BTREE,
  KEY `SportId` (`SportId`) USING BTREE,
  KEY `SportId_IsOutright_StartDate` (`SportId`,`StartDate`,`IsOutright`) USING BTREE,
  KEY `IsOutright` (`IsOutright`) USING BTREE,
  KEY `ProviderId` (`ProviderId`) USING BTREE,
  KEY `RobotId` (`RobotId`) USING BTREE,
  CONSTRAINT `suggestioncontexts_fk1` FOREIGN KEY (`GroupId`) REFERENCES `suggestioncontextgroups` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=104780943 DEFAULT CHARSET=utf8mb3 ROW_FORMAT=DYNAMIC;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `suggestioncontextuserselectedmetadata`
--

DROP TABLE IF EXISTS `suggestioncontextuserselectedmetadata`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `suggestioncontextuserselectedmetadata` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `HolderId` int unsigned NOT NULL,
  `MainId` int NOT NULL,
  `Reason` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `suggestioncontextuserselectedmetadata_fk1` (`HolderId`) USING BTREE,
  CONSTRAINT `suggestioncontextuserselectedmetadata_fk1` FOREIGN KEY (`HolderId`) REFERENCES `suggestioncontextmetadataholders` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=4219 DEFAULT CHARSET=utf8mb3 ROW_FORMAT=DYNAMIC;
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
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=179089202 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `user_filters`
--

DROP TABLE IF EXISTS `user_filters`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_filters` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `User` varchar(45) NOT NULL DEFAULT '',
  `CustomFilters` text,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `User` (`User`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `verifiedmappings`
--

DROP TABLE IF EXISTS `verifiedmappings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `verifiedmappings` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `ProviderId` int NOT NULL,
  `ProviderFixtureId` varchar(36) NOT NULL,
  `StartDate` datetime(6) NOT NULL,
  `SportId` int NOT NULL,
  `LeagueName` varchar(255) DEFAULT NULL,
  `LocationName` varchar(255) DEFAULT NULL,
  `OpposingParticipant` varchar(255) DEFAULT NULL,
  `ProviderParticipant` varchar(255) DEFAULT NULL,
  `MainId` int NOT NULL,
  `IsCorrect` tinyint unsigned NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=1680 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `verifiedselections`
--

DROP TABLE IF EXISTS `verifiedselections`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `verifiedselections` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `LeagueId` int DEFAULT NULL,
  `ProviderParticipant` varchar(255) DEFAULT NULL,
  `MainId` int NOT NULL,
  `Status` tinyint unsigned NOT NULL,
  `ExactCount` smallint unsigned NOT NULL,
  `SuggestionCount` smallint unsigned NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `verifiedselections_index` (`LeagueId`,`ProviderParticipant`,`MainId`,`Status`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=305502 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping routines for database 'mappingdata'
--
/*!50003 DROP PROCEDURE IF EXISTS `explain_statement` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`lsports_admin`@`%` PROCEDURE `explain_statement`(IN query TEXT)
BEGIN
    SET @explain := CONCAT('EXPLAIN FORMAT=json ', query);
    PREPARE stmt FROM @explain;
    EXECUTE stmt;
    DEALLOCATE PREPARE stmt;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
SET @@SESSION.SQL_LOG_BIN = @MYSQLDUMP_TEMP_LOG_BIN;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-11-11 12:39:52
