-- This script ensures that necessary databases are created before other scripts run.

CREATE DATABASE IF NOT EXISTS mappingdata;
CREATE DATABASE IF NOT EXISTS data;
CREATE DATABASE IF NOT EXISTS playersmanager;

-- Add any additional database creation commands as needed
