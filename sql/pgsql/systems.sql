CREATE TABLE systems
(
  sys_id bigint NOT NULL,
  sys_name character varying(100) NOT NULL,
  sys_desc character varying(4000),
  CONSTRAINT sys_pk PRIMARY KEY (sys_id)
)
WITH (
  OIDS=FALSE
);