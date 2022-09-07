# Realtime Distributed System

**Goal:** Create a system with a maximum of stateless service based on queue and key-column database. This is a Proof Of Concept and no intend to by used in production

## Services
### Pushers
Receive persistent connection from outside clients. (Grpc or Websocket)

### Orchestrator
Dispatch and manage queues for pushers. Cleanup dead queues if pusher go down without proper cleanup

### Distributor
Receive, store if necessary and dispatch message into pusher queue of connected users

## Vocabularies

**Tag:** A tag is a string who represent the kind or a category of Letter.
Global is a builtin tag who represent Letter with information about the system or send to a user regardless the tag.
The client can send any type of tag with the message or event. The tag is used to dispatch to recipients listening the tag.
By default, all clients listen the Global tag.

**Message:** A message contains information as text send by client, store and dispatch to recipients

**Event:** An event contains information as text send by client and dispatch to recipients

**Letter:** A letter is a wrapper around a Message or an event with recipients  

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