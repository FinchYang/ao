use cars;

CREATE TABLE `carbusinesspic` (
  `identity` varchar(50) NOT NULL,
  `businesstype` int(11) NOT NULL DEFAULT '0',
  `pictype` smallint(2) NOT NULL DEFAULT '0',
  `uploaded` tinyint(1) NOT NULL DEFAULT '0',
  `time` datetime NOT NULL,
  PRIMARY KEY (`identity`,`businesstype`,`pictype`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

