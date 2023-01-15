# .NET API Gateway

This project is a short demo for myself to see if I can create an [API Gateway](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/architect-microservice-container-applications/direct-client-to-microservice-communication-versus-the-api-gateway-pattern) to retrieve requests as REST, but processes them through [RabbitMQ](https://www.rabbitmq.com/) with [RebusMQ](https://github.com/rebus-org/Rebus).


## To launch it

Start RabbitMQ with the following command from the root-project, to deploy a container to handle the message queue:
```powershell
docker compose up -d
```

Compile the shared project `SharedTypes` to get the types send between the services.

Compile and run both the `API Gateway` and `Worker1`.
When both are up and running, the `API Gateway`'s project will have launched Swagger. 
Trigger the only method in it, and they should send a message back and forth.

## Architecture

To embrace the microservice mindset, everything will be built in small services, but with a single entrypoint to the backend.

The single entrypoint will be the `API Gateway`, which is a .NET 7 Web api, set to run as a container.
All calls to the system comes through it as REST (for now - change to GraphQL at some point).

Because the `API Gateway` should remain small and rather stupid, it will handle the incoming requests by redistribute them to several workers through RabbitMQ.
Each of the workers (like `Worker1`), will listen for specific events they can handle, do what should be done, and sending the required messages back through RabbitMQ.