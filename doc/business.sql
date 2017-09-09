CREATE TABLE `business` (
  `identity` varchar(50) NOT NULL,
  `businesstype` smallint(2) NOT NULL DEFAULT '0',
  `completed` tinyint(1) NOT NULL DEFAULT '0',
  `time` datetime NOT NULL,
  `postaddr` varchar(145) DEFAULT NULL,
  `acceptingplace` varchar(145) DEFAULT NULL,
  `quasiDrivingLicense` varchar(45) DEFAULT NULL,
  `status` smallint(2) DEFAULT '0',
  `waittime` datetime NOT NULL DEFAULT '2000-01-01 00:00:00',
  `processtime` datetime NOT NULL DEFAULT '2000-01-01 00:00:00',
  `finishtime` datetime NOT NULL DEFAULT '2000-01-01 00:00:00',
  PRIMARY KEY (`identity`,`businesstype`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;