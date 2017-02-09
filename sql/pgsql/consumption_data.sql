CREATE TABLE consumption_data
(
  cd_id bigint NOT NULL,
  cd_dev_id bigint NOT NULL,
  cd_timestamp timestamp with time zone NOT NULL,
  cd_rx bigint,
  cd_tx bigint,
  cd_refreshed timestamp with time zone,
  CONSTRAINT cd_pk PRIMARY KEY (cd_id)
)
WITH (
  OIDS=FALSE
);