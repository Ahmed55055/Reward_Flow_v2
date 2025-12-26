#!/bin/bash

# Integration Test Runner for RewardFlow
# This script sets up the test environment and runs integration tests

set -e

echo "Starting RewardFlow Integration Tests..."

# Check if Docker is running
if ! docker info > /dev/null 2>&1; then
    echo "Error: Docker is not running. Please start Docker and try again."
    exit 1
fi

# Navigate to the integration tests directory
cd "$(dirname "$0")"

# Clean up any existing test containers
echo "Cleaning up existing test containers..."
docker-compose -f docker-compose.test.yml down -v --remove-orphans 2>/dev/null || true

# Build the project
echo "Building the project..."
dotnet build ../RewardFlow.API/RewardFlow.API.csproj
dotnet build RewardFlow.IntegrationTests.csproj

if [ $? -ne 0 ]; then
    echo "Error: Failed to build the project."
    exit 1
fi

# Run the tests
echo "Running integration tests..."
dotnet test RewardFlow.IntegrationTests.csproj --logger "console;verbosity=detailed" --collect:"XPlat Code Coverage"

TEST_RESULT=$?

if [ $TEST_RESULT -eq 0 ]; then
    echo "✅ All integration tests passed successfully!"
else
    echo "❌ Some integration tests failed."
fi

# Optional: Clean up test containers after tests
read -p "Do you want to clean up test containers? (y/n): " -n 1 -r
echo
if [[ $REPLY =~ ^[Yy]$ ]]; then
    echo "Cleaning up test containers..."
    docker-compose -f docker-compose.test.yml down -v --remove-orphans
    echo "Cleanup completed."
fi

echo "Integration test run completed."
exit $TEST_RESULT