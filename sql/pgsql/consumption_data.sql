CREATE TABLE consumption_data
(
  cd_id serial PRIMARY KEY,
  cd_dev_id bigint NOT NULL REFERENCES devices(dev_id),
  cd_timestamp timestamp with time zone NOT NULL,
  cd_rx bigint,
  cd_tx bigint,
  cd_refreshed timestamp with time zone
)
WITH (
  OIDS=FALSE
);

COMMENT ON COLUMN consumption_data.cd_dev_id IS 'Reference to the device the consumption belongs to or the device that measured the consumption';
COMMENT ON COLUMN consumption_data.cd_timestamp IS 'Date and time when the consumption occured';
COMMENT ON COLUMN consumption_data.cd_rx IS 'Any received amount that was measured, for example bytes, kWh, cubic meters';
COMMENT ON COLUMN consumption_data.cd_tx IS 'Any sent amount that was measured, for example bytes, kWh, cubic meters';
COMMENT ON COLUMN consumption_data.cd_refreshed IS 'Date and time when this record was updated';