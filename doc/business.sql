CREATE TABLE `business` (
  `identity` varchar(50) NOT NULL,
  `businesstype` smallint(2) NOT NULL DEFAULT '0',
  `completed` tinyint(1) NOT NULL DEFAULT '0',
  `time` datetime NOT NULL,
  PRIMARY KEY (`identity`,`businesstype`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
;