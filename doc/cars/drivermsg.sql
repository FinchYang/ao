CREATE TABLE `drivermsg` (
  `ordinal` int(11) NOT NULL,
  `content` varchar(450) DEFAULT NULL,
  `busiflag` tinyint(1) NOT NULL DEFAULT '0',
  `sendflag` tinyint(1) NOT NULL DEFAULT '0',
  `timestamp` datetime NOT NULL,
  `busitype` smallint(2) NOT NULL DEFAULT '0',
  `phone` varchar(45) NOT NULL,
  `identity` varchar(45) DEFAULT NULL,
  `name` varchar(45) DEFAULT NULL,
  `count` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`ordinal`),
  UNIQUE KEY `ordinal_UNIQUE` (`ordinal`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
