CREATE TABLE `aouser` (
  `identity` varchar(50) NOT NULL,
  `name` varchar(45) DEFAULT NULL,
  `phone` varchar(45) DEFAULT NULL,
  `photofile` varchar(145) DEFAULT NULL,
  `verificationcode` varchar(45) DEFAULT NULL,
  `newphone` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`identity`),
  UNIQUE KEY `identity_UNIQUE` (`identity`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

