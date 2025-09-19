#!/bin/bash
curl -X POST http://localhost:8080/api/check-all >> /home/sanya/hakaton/cron.log 2>&1
