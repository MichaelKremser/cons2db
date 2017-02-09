CREATE TABLE devices
(
  dev_id serial PRIMARY KEY,
  dev_name character varying(50),
  dev_desc character varying(4000),
  dev_sys_id bigint NOT NULL REFERENCES systems(sys_id)
)
WITH (
  OIDS=FALSE
);

COMMENT ON COLUMN devices.dev_name IS 'Device name, like eth0 or wlx99feedcafe11 or some other identifier, like a reference number of an external system';
COMMENT ON COLUMN devices.dev_desc IS 'A user defined description of the device';
COMMENT ON COLUMN devices.dev_sys_id IS 'The system the device belongs to, like a computer or a metering point';