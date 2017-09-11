CREATE TABLE `businesshis` (
  `identity` varchar(50) NOT NULL,
  `businesstype` smallint(2) NOT NULL DEFAULT '0',
  `completed` tinyint(1) NOT NULL DEFAULT '0',
  `time` datetime NOT NULL,
  `postaddr` varchar(145) DEFAULT NULL,
  `acceptingplace` varchar(145) DEFAULT NULL,
  `quasiDrivingLicense` varchar(45) DEFAULT NULL,
  `ordinal` int(11) NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`ordinal`),
  UNIQUE KEY `ordinal_UNIQUE` (`ordinal`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
