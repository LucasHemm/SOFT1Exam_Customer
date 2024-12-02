# SOFT1Exam_Customer

## Table of Contents

- [SOFT1Exam\_Customer](#soft1exam_customer)
  - [Table of Contents](#table-of-contents)
  - [Introduction](#introduction)
  - [Build Status](#build-status)
  - [CI/CD Pipeline](#cicd-pipeline)
    - [Pipeline Steps](#pipeline-steps)
  - [Tech stack](#tech-stack)
  - [API Documentation](#api-documentation)
    - [REST](#rest)
  - [Docker Compose](#docker-compose)
    - [Overview](#overview)
    - [Dockerhub](#dockerhub)
    - [Services / Containers](#services--containers)

## Introduction

Welcome to the **SOFT1Exam_Customer** repository! This is one of 7 mircoservices for our first semester software PBA exam. This microservice is responsible for managing the customers and handeling their infomation. This microservice can be run independently, but is also part of a larger project called **MTOGO**. 

## Build Status
Check out the lastest build status here: ![CI/CD](https://github.com/LucasHemm/SOFT1Exam_Customer/actions/workflows/dotnet-tests.yml/badge.svg)

## CI/CD Pipeline

Our CI/CD pipeline utilizes GitHub Actions to automate the testing and deployment of the application. This uses our whole suite of tests. To initate the pipeline a pull request has to be made to merge the code. After the request has been made the pipeline will run the tests, and deploy the image of the application to dockerhub if all the tests pass.

The pipeline consists of the following steps:

### Pipeline Steps

1. **Checkout Repository**
2. **Setup .NET**
3. **Restore Dependencies**
4. **Build**
5. **Test**
6. **Log in to Docker Hub**
7. **Build Docker Image**
8. **Push Docker Image** 

## Tech stack
The tech stack for this microservice is as follows:
- **C#**: The main programming language for the application.
- **ASP NET Core 8.0**: The main framework for the application.
- **Microsoft SQL Server**: The database for the application.
- **Promehteus**: The library used for metrics.
- **Grafana**: The library used for visualizing the metrics.
- **Docker**: The containerization tool used to deploy the application.
- **Docker Compose**: The tool used to deploy the application locally.
- **GitHub Actions**: The CI/CD tool used to automate the testing and deployment of the application.
- **Swagger**: The library used to document the API.
- **xunit**: The library used for unit and integration testing.
- **Testcontainers**: The library used to create testcontainers for the integration tests.
- **Coverlet**: The library used to create code coverage reports.
## API Documentation
### REST

## API Documentation
### REST

| **Endpoint**                  | **Result**                                    | **Format**   |
|-------------------------------|-----------------------------------------------|--------------|
| `POST /api/CustomerApi`          | Creates a customer                            | JSON         |
| `PUT /api/CustomerApi`           | Updates a customer                            | JSON         |
| `GET /api/CustomerApi`           | Retrieves a list of all customers             | JSON         |
| `GET /api/CustomerApi/{id}`      | Retrieve a customer by ID                     | JSON         |



## Docker Compose

### Overview

To run this microservice, you can use Docker Compose to deploy the services locally. 

```yaml
docker-compose up --build
```
To access the Swagger UI and endpoints, navigate to the following URL:
```
http://localhost:8084/swagger/index.html
```

See performance metrics at:
```
http://localhost:8083/metrics
```
Or use the prometheus UI at:
```
http://localhost:9093
```
And the grafana UI at:
```
http://localhost:3003
```

Alternatively, you can run all the services from the MTOGO project by going to this repository and following the guide there.
```
https://github.com/LucasHemm/SOFTEXAM_Deployment
```

### Dockerhub
The docker-compose file uses the local dockerfile to build the image, and run the container. The image can also be found on Docker Hub at:
```
https://hub.docker.com/u/lucashemcph
```

### Services / Containers

- **App** / **Paymnetservicecontainer**: Runs the main application server.
- **DB** / **Database**: Runs the Microsoft SQL Server database.
- **Prometheus** / **Prometheus**: Runs the prometheus server.
- **Grafana** / **Grafana**: Runs the grafana server.
