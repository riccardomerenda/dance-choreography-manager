#!/bin/bash
set -e

# Set password method to scram-sha-256 (default for PostgreSQL 17)
psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
    ALTER SYSTEM SET password_encryption = 'scram-sha-256';
    ALTER SYSTEM SET listen_addresses = '*';
EOSQL

# Update pg_hba.conf to use scram-sha-256 instead of md5
cat > /var/lib/postgresql/data/pg_hba.conf <<-EOT
# TYPE  DATABASE        USER            ADDRESS                 METHOD
local   all             postgres                                scram-sha-256
local   all             all                                     scram-sha-256
host    all             all             127.0.0.1/32            scram-sha-256
host    all             all             ::1/128                 scram-sha-256
host    all             all             0.0.0.0/0               scram-sha-256
host    all             all             ::/0                    scram-sha-256
EOT

# Reload PostgreSQL to apply changes
pg_ctl reload -D /var/lib/postgresql/data