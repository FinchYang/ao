use cars;
CREATE TABLE `carbusinesshis` (
  `identity` varchar(50) NOT NULL,
  `businesstype` smallint(2) NOT NULL DEFAULT '0',
  `completed` tinyint(1) NOT NULL DEFAULT '0',
  `time` datetime NOT NULL,
  `postaddr` varchar(145) DEFAULT NULL,
  `acceptingplace` varchar(145) DEFAULT NULL,
  `quasiDrivingLicense` varchar(45) DEFAULT NULL,
  `ordinal` int(11) NOT NULL AUTO_INCREMENT,
  `reason` varchar(545) DEFAULT NULL,
    `status` smallint(2) NOT NULL DEFAULT '0',
  `waittime` datetime NOT NULL DEFAULT '2000-01-01 00:00:00',
  `processtime` datetime NOT NULL DEFAULT '2000-01-01 00:00:00',
  `finishtime` datetime NOT NULL DEFAULT '2000-01-01 00:00:00',
  `integrated` tinyint(1) DEFAULT '0',
  
  `exporttime` datetime NOT NULL DEFAULT '2000-01-01 00:00:00',
  `province` varchar(145) DEFAULT NULL,
  `county` varchar(145) DEFAULT NULL,
  `city` varchar(145) DEFAULT NULL,
  `cartype` smallint(2) NOT NULL DEFAULT '0',
  `scrapplace` smallint(2) NOT NULL DEFAULT '0',
  `platetype` smallint(2) NOT NULL DEFAULT '0',
  `platenumber1` varchar(45) DEFAULT NULL,
  `platenumber2` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`ordinal`),
  UNIQUE KEY `ordinal_UNIQUE` (`ordinal`)
) ENGINE=InnoDB AUTO_INCREMENT=287 DEFAULT CHARSET=utf8;

