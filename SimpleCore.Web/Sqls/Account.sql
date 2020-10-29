--  定义修改字段的存储过程
DROP PROCEDURE IF EXISTS AddOrUpdateColumn;
DELIMITER$$

CREATE PROCEDURE AddOrUpdateColumn(TableName VARCHAR(50),ColumnName VARCHAR(50),SqlStr VARCHAR(4000), KeyStr VARCHAR(200))
BEGIN
    DECLARE Rows1 INT;
    SET Rows1=0;
    SELECT COUNT(*) INTO Rows1  FROM INFORMATION_SCHEMA.Columns
    WHERE table_schema= DATABASE() AND table_name=TableName AND column_name=ColumnName;
    -- 新增列
    IF (Rows1<=0) THEN
        SET SqlStr := CONCAT( 'ALTER TABLE `',TableName,'` ADD COLUMN `',ColumnName,'` ',SqlStr,KeyStr);
    -- 修改列类型
    ELSEIF (Rows1>0)  THEN
        SET SqlStr := CONCAT('ALTER TABLE `',TableName,'` MODIFY  `',ColumnName,'` ',SqlStr);
    -- 删除列
    -- ELSEIF (CType=3 AND Rows1>0) THEN
    -- SET SqlStr := CONCAT('ALTER TABLE  `',TableName,'` DROP COLUMN  `',ColumnName);
    -- ELSE  SET SqlStr :='';
    END IF;
    -- 执行命令
    IF (SqlStr<>'') THEN 
    SET @SQL1 = SqlStr;
    PREPARE stmt1 FROM @SQL1;
    EXECUTE stmt1;
    END IF;
END$$
DELIMITER;

DROP PROCEDURE IF EXISTS AddOrUpdateRecordTable;
DELIMITER$$
-- 1表示新增列,2表示修改列类型,3表示删除列
CREATE PROCEDURE AddOrUpdateRecordTable(tbName VARCHAR(50))
BEGIN
	
    
    --  检查表是否存在
    --  检查表是否存在
    SET @createTbSql = CONCAT('CREATE TABLE IF NOT EXISTS `' , tbName , '` (`Id` char(16) PRIMARY KEY NOT NULL) DEFAULT CHARSET=utf8 ENGINE=INNODB');
		PREPARE tmpSql FROM @createTbSql;
    EXECUTE tmpSql;
		

    --  id 
    CALL AddOrUpdateColumn (tbName,'Id','char(40) NOT NULL COMMENT ''系统唯一编号''','PRIMARY KEY');
    --  createTime 
    CALL AddOrUpdateColumn (tbName,'CreateTime','datetime NOT NULL COMMENT ''新建该账号的时间''','');
    --  createUserId 
    CALL AddOrUpdateColumn (tbName,'CreateByUserId','char(40) NOT NULL COMMENT ''新建者Id''','');
    --  createUserName 
    CALL AddOrUpdateColumn (tbName,'CreateByUserName','varchar(200) NOT NULL COMMENT ''新建者名称''','');
    --  modifyTime 
    CALL AddOrUpdateColumn (tbName,'UpdateTime','datetime NOT NULL COMMENT ''修改该账号的时间''','');
    --  modifyUserId 
    CALL AddOrUpdateColumn (tbName,'UpdateByUserId','char(40) NOT NULL COMMENT ''新建者Id''','');
    --  modifyUserName 
    CALL AddOrUpdateColumn (tbName,'UpdateByUserName','varchar(200) NOT NULL COMMENT ''修改者名称''','');

END$$
DELIMITER ;


CALL AddOrUpdateRecordTable('simple_account');

CALL AddOrUpdateColumn('simple_account','Username','VARCHAR(200) NOT NULL COMMENT''登录名''','');

CALL AddOrUpdateColumn('simple_account','Password','VARCHAR(200) NOT NULL COMMENT''密码''','');

CALL AddOrUpdateColumn('simple_account','Email','VARCHAR(200) NOT NULL COMMENT''电子邮件''','');

CALL AddOrUpdateColumn('simple_account','Mobile','VARCHAR(200) NOT NULL COMMENT''移动号码''','');
