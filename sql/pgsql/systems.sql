CREATE TABLE systems
(
  sys_id serial PRIMARY KEY,
  sys_name character varying(100) NOT NULL,
  sys_desc character varying(4000)
)
WITH (
  OIDS=FALSE
);

COMMENT ON COLUMN systems.sys_name is 'System name, like a hostname or a metering point code';
COMMENT ON COLUMN systems.sys_desc is 'A human readable description of the system; can be NULL';