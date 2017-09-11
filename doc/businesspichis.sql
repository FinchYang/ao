CREATE TABLE `businesspichis` (
  `identity` varchar(50) NOT NULL,
  `businesstype` smallint(2) NOT NULL DEFAULT '0',
  `pictype` smallint(2) NOT NULL DEFAULT '0',
  `uploaded` tinyint(1) NOT NULL DEFAULT '0',
  `time` datetime NOT NULL,
  `ordinal` int(11) NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`ordinal`),
  UNIQUE KEY `ordinal_UNIQUE` (`ordinal`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;
