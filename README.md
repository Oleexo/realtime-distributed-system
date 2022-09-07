# Realtime Distributed System

Goal, create a system with a maximum of stateless service based on queue and key-column database


## Pushers
Receive persistent connection from outside clients. (Grpc or Websocket)

## Orchestrator
Dispatch and manage queues for pushers. Cleanup dead queues if pusher go down without proper cleanup

## Distributor
Receive, store if necessary and dispatch message into pusher queue of connected users

## Technologies
- Grpc
- WebSocket
- Asp.NetCore Minimal API
- Docker
- Queue (Amazon Sqs, RabbitMq)
- Key-column database (DynamoDB, Cassandra)

## todo
- [ ] Get message from channel_id
- [ ] Edit a message
- [ ] Remove a message
- [ ] Handle snowflake id unique ID for each distributor