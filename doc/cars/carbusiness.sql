use cars;

CREATE TABLE `carbusiness` (
  `identity` varchar(50) NOT NULL,
  `businesstype` smallint(2) NOT NULL DEFAULT '0',
  `completed` tinyint(1) NOT NULL DEFAULT '0',
  `time` datetime NOT NULL,
  `postaddr` varchar(450) DEFAULT NULL,
  `acceptingplace` varchar(145) DEFAULT NULL,
  `quasiDrivingLicense` varchar(45) DEFAULT NULL,
  `status` smallint(2) NOT NULL DEFAULT '0',
  `waittime` datetime NOT NULL DEFAULT '2000-01-01 00:00:00',
  `processtime` datetime NOT NULL DEFAULT '2000-01-01 00:00:00',
  `finishtime` datetime NOT NULL DEFAULT '2000-01-01 00:00:00',
  `integrated` tinyint(1) DEFAULT '0',
  `reason` varchar(450) DEFAULT NULL,
  `losttime` datetime NOT NULL DEFAULT '2000-01-01 00:00:00',
  `exporttime` datetime NOT NULL DEFAULT '2000-01-01 00:00:00',
  `abroadorservice` tinyint(1) NOT NULL DEFAULT '0' COMMENT '0--服兵役，1-出国',
  `province` varchar(145) DEFAULT NULL,
  `county` varchar(145) DEFAULT NULL,
  `city` varchar(145) DEFAULT NULL,
  PRIMARY KEY (`identity`,`businesstype`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
