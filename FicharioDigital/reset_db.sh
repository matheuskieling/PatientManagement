#!/bin/bash
# Stop and remove the container
docker-compose down

# Remove the volume
docker volume rm fichariodigital_pgdata

# Start the containers again
docker-compose up -d