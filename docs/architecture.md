# Dance Choreography Manager - Architecture Overview

## Microservices Architecture

The Dance Choreography Manager is built using a microservices architecture with .NET Aspire, which provides a streamlined way to build cloud-ready, distributed applications.

### Service Boundaries

Each microservice represents a bounded context within the domain:

1. **Authentication Service**: Handles user identity, authentication, and authorization.
2. **Dancer Management Service**: Manages dancer profiles, attributes, and grouping.
3. **Course Management Service**: Handles dance classes, schedules, and attendance.
4. **Choreography Service**: Manages choreography content, music synchronization, and versions.
5. **Formation Management Service**: Controls spatial arrangements, positioning, and transitions.
6. **AI Assistant Service**: Provides intelligent recommendations using ML.NET.

### Communication Patterns

The architecture implements two primary communication patterns:

1. **Synchronous Communication**: 
   - REST APIs for direct client-to-service interactions through the API Gateway
   - gRPC for efficient service-to-service communication when immediate responses are required

2. **Asynchronous Communication**: 
   - Event-driven messaging via RabbitMQ for inter-service communication
   - Pub/Sub pattern for event propagation across the system

## Data Management

Each service owns its data and maintains its own database schema:

- PostgreSQL databases with independent schemas per service
- Entity Framework Core for ORM capabilities
- Database migrations for schema evolution
- CQRS pattern for optimized read/write operations in appropriate services

## Development Environment

### Prerequisites

- .NET 8.0 SDK
- Docker and Docker Compose
- Node.js and npm
- PostgreSQL (via Docker)
- Visual Studio 2022 or Cursor

### Local Development Setup

1. Clone the repository
2. Run `docker-compose up -d` to start required infrastructure (PostgreSQL, RabbitMQ)
3. Build and run the .NET Aspire application using `dotnet run --project src/AppHost`
4. Start the frontend using `npm run dev` from the `src/Frontend` directory

## Deployment Strategy

The application is containerized using Docker and can be deployed to:

- Kubernetes cluster
- Azure Container Apps
- AWS ECS
- Docker Swarm

CI/CD pipelines automate the build, test, and deployment processes.