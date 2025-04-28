# Saga Pattern Examples

This project is an implementation of different **Saga Pattern** approaches used to ensure data consistency in distributed systems.  
It includes example implementations of two different approaches: **Orchestration-based Saga** and **Choreography-based Saga**.

## Technologies Used

- .NET 8
- MassTransit
- RabbitMQ (Cloud AMQP compatible)
- SQL Server
- Entity Framework Core

## Features

- Example of **Orchestration-based Saga**
- Example of **Choreography-based Saga**
- Support for both Cloud-based and Local RabbitMQ
- Saga State Machine management with MassTransit
- Persistence of Saga states on SQL Server
- Data access using Entity Framework Core
