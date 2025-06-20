#!/bin/bash

# Update system
sudo apt-get update
sudo apt-get upgrade -y

# Install Docker
sudo apt-get install -y docker.io docker-compose

# Add current user to docker group
sudo usermod -aG docker $USER

# Install Git
sudo apt-get install -y git

# Install .NET SDK
wget https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt-get update
sudo apt-get install -y dotnet-sdk-8.0

# Install Java
sudo apt-get install -y default-jdk

# Clean up
rm packages-microsoft-prod.deb
sudo apt-get clean

# Output next steps
echo "Setup complete!"
echo "Please log out and log back in for Docker group changes to take effect."
echo "Then you can run the app with:"
echo "cd docker && docker-compose up --build"
