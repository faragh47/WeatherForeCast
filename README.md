# Weather Forecast API Client

## Overview

This project is a .NET 7 API client that fetches weather forecast data from the [Open-Meteo API](https://open-meteo.com) and stores it in a database. The application uses Docker to run services in a containerized environment, ensuring a consistent and reliable setup for development and production.

## Features

- Fetches weather forecast data from the Open-Meteo API.
- Implements retry and timeout policies using Polly.
- Stores weather data in an SQL Server database.
- Uses Docker Compose for easy container orchestration.

## Prerequisites

- [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- [Docker](https://www.docker.com/)
- [Git](https://git-scm.com/)

## How to Run Locally

### 1. Clone the Repository

```bash
git clone
cd WeatherForecast
```

### 2. Run

```bash
docker-compose up
```

