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

DROP PROCEDURE IF EXISTS AddOrUpdateNamedTable;
DELIMITER$$
-- 1表示新增列,2表示修改列类型,3表示删除列
CREATE PROCEDURE AddOrUpdateNamedTable(tbName VARCHAR(50))
BEGIN
	
    
    --  检查表是否存在
    --  检查表是否存在
    SET @createTbSql = CONCAT('CREATE TABLE IF NOT EXISTS `' , tbName , '` (`Id` int auto_increment PRIMARY KEY NOT NULL) DEFAULT CHARSET=utf8 ENGINE=INNODB');
		PREPARE tmpSql FROM @createTbSql;
    EXECUTE tmpSql;
		

    --  id 
    -- CALL AddOrUpdateColumn (tbName,'Id','char(40) NOT NULL COMMENT ''系统唯一编号''','PRIMARY KEY');
    --  createTime 
    CALL AddOrUpdateColumn (tbName,'Name','varchar(255) NOT NULL COMMENT ''名称''','');
    --  createUserId 
    CALL AddOrUpdateColumn (tbName,'Description','varchar(800) NULL COMMENT ''描述''','');
    --  createUserName 
    
END$$
DELIMITER ;


CALL AddOrUpdateNamedTable('simple_func');

CALL AddOrUpdateColumn('simple_func','code','varchar(800) NULL COMMENT''路径编号''','');

CALL AddOrUpdateColumn('simple_func','url','varchar(800) NULL COMMENT''功能/菜单的访问地址''','');

CALL AddOrUpdateColumn('simple_func','icon','varchar(60) NULL COMMENT''功能/菜单的访问地址''','');

CALL AddOrUpdateColumn('simple_func','parentId','char(40) NULL COMMENT''上级菜单/页面''','');

CALL AddOrUpdateColumn('simple_func','type','int NOT NULL DEFAULT 0 COMMENT''功能类型:0=url,1=page,2=主菜单,3=快捷菜单''','');

CALL AddOrUpdateColumn('simple_func','isRelativeWithParent','int NOT NULL DEFAULT 0 COMMENT ''是否与上级func关联，如果上级func被选中，该func也一并会被选中''','');
