CREATE TABLE devices
(
  "DEV_ID" bigint NOT NULL,
  "DEV_name" character varying(50), -- Device name, like eth0 or wlx99feedcafe11 or some other identifier, like a reference number of an external system
  "DEV_desc" character varying(4000), -- A user defined description of the device
  "DEV_sys_id" bigint NOT NULL, -- The system the device belongs to, like a computer or a metering point
  CONSTRAINT "DEV_PK" PRIMARY KEY ("DEV_ID")
)
WITH (
  OIDS=FALSE
);

COMMENT ON COLUMN devices."DEV_name" IS 'Interface name, like eth0 or wlx99feedcafe11 or some other identifier, like a reference number of an external system';
COMMENT ON COLUMN devices."DEV_desc" IS 'A user defined description of the device';
COMMENT ON COLUMN devices."DEV_sys_id" IS 'The system the device belongs to, like a computer or a metering point'