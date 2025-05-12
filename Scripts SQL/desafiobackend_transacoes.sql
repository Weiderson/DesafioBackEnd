CREATE DATABASE  IF NOT EXISTS `desafiobackend2` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `desafiobackend2`;
-- MySQL dump 10.13  Distrib 8.0.42, for Win64 (x86_64)
--
-- Host: localhost    Database: desafiobackend2
-- ------------------------------------------------------
-- Server version	8.0.42

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

--
-- Position to start replication or point-in-time recovery from
--

-- CHANGE MASTER TO MASTER_LOG_FILE='DESKTOP-ESNAMLC-bin.000023', MASTER_LOG_POS=157;

--
-- Table structure for table `transacoes`
--

DROP TABLE IF EXISTS `transacoes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `transacoes` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Descricao` longtext NOT NULL,
  `Tipo` longtext NOT NULL,
  `Valor` decimal(18,2) NOT NULL,
  `DtInclusao` datetime(6) NOT NULL,
  `DtExclusao` datetime(6) DEFAULT NULL,
  `ContaId` int NOT NULL,
  `ContaId_Destino` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Transacoes_ContaId` (`ContaId`),
  CONSTRAINT `FK_Transacoes_Contas_ContaId` FOREIGN KEY (`ContaId`) REFERENCES `contas` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `transacoes`
--
-- ORDER BY:  `Id`

LOCK TABLES `transacoes` WRITE;
/*!40000 ALTER TABLE `transacoes` DISABLE KEYS */;
INSERT INTO `transacoes` (`Id`, `Descricao`, `Tipo`, `Valor`, `DtInclusao`, `DtExclusao`, `ContaId`, `ContaId_Destino`) VALUES (1,'DEPÓSITO C/C','Deposito',1000.00,'2025-05-12 10:31:00.382569',NULL,1,NULL),(2,'SAQUE C/C','Saque',150.00,'2025-05-12 10:31:00.409833',NULL,1,NULL),(3,'DEPÓSITO C/C','Deposito',1500.00,'2025-05-12 10:31:00.410013',NULL,2,NULL),(4,'SAQUE C/C','Saque',800.00,'2025-05-12 10:31:00.410042',NULL,2,NULL);
/*!40000 ALTER TABLE `transacoes` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-05-12 10:39:50
