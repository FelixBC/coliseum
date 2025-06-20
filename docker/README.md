# Docker Setup for Coliseum Android App

## Prerequisites

1. Docker and Docker Compose installed
2. Git installed

## Building and Running the App

1. Navigate to the docker directory:
   ```bash
   cd docker
   ```

2. Build and run the containers:
   ```bash
   docker-compose up --build
   ```

3. The app will be available at:
   - Android app: http://localhost:8081
   - API: http://localhost:5000

## Development Mode

The docker-compose.yml is configured for development with:
- Hot reload enabled
- NuGet packages cached
- Debug port exposed
- Watch mode enabled

## Production Build

For production builds, modify the docker-compose.yml to use the Release configuration instead of Debug.

## Troubleshooting

1. If you encounter permission issues, run:
   ```bash
   sudo chown -R $USER:$USER .
   ```

2. If the build fails, clean the Docker cache:
   ```bash
   docker-compose down
   docker system prune -a
   ```
