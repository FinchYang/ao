use cars;
CREATE TABLE `carslog` (
  `ordinal` int(11) NOT NULL AUTO_INCREMENT,
  `ip` varchar(45) DEFAULT NULL,
  `content` varchar(4500) DEFAULT NULL,
  `method` varchar(45) DEFAULT NULL,
  `time` datetime NOT NULL,
  `businesstype` smallint(2) NOT NULL DEFAULT '0',
  PRIMARY KEY (`ordinal`),
  UNIQUE KEY `ordinal_UNIQUE` (`ordinal`)
) ENGINE=InnoDB AUTO_INCREMENT=57148 DEFAULT CHARSET=utf8;
