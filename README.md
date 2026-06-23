# Portfolio Analytics Performance Lab

A microservices-based system designed to benchmark and compare different performance strategies for portfolio analytics processing. The project simulates real-world financial portfolio computations under different execution models and visualizes performance results in a web-based UI dashboard.

---

## Overview

This system evaluates how different execution approaches impact the performance of portfolio analytics workloads.

It runs identical portfolio processing scenarios using different strategies and compares results such as execution time and system throughput, allowing direct analysis of trade-offs between simplicity, scalability, and performance.

---

## Key Idea

The project is designed to explore performance trade-offs in backend systems, such as:

- How multithreading improves processing speed vs sequential execution
- The impact of parallel processing on CPU-intensive workloads
- The overhead introduced by asynchronous messaging patterns
- The trade-offs between synchronous HTTP calls and asynchronous event-driven workflows

---

## Technologies

- **Backend:** .NET (Microservices architecture)
- **Communication:**
  - Synchronous: HTTP (REST APIs)
  - Asynchronous: RabbitMQ
- **Frontend:** Angular
- **Database:** SQL Server
- **Containerization:** Docker

---

## Purpose

This project was built as a portfolio piece to demonstrate skills in:

- Microservices design using .NET
- Performance-oriented backend engineering
- Concurrency and parallel processing techniques
- Event-driven architecture using RabbitMQ
- Building full-stack systems with Angular frontend
- Containerized application deployment with Docker

---

## UI Features

- Trigger portfolio processing scenarios from the UI
- Compare execution results across different runs
- Visualize performance differences between strategies
- Track and review past execution results

---

## Author

Built as a personal engineering project focused on backend architecture, distributed systems, and performance optimization.

