# ServiceApplication


The ServiceApplication is a Windows Presentation Foundation (WPF) application that allows you to control and monitor IoT devices hosted on Azure. The application provides features to interact with your devices. It also views weather data, and display date and time information.

## Features

- Control and monitor IoT devices on Azure.
- Retrieve and display weather data from an external API.
- Display date and time information.
- Toggle the power state of IoT devices (on/off).
- View device details and status.

## Application Components

The project consists of the following components:

- **ServiceApplication:** The main application for controlling IoT devices and viewing weather and time information.
- **Azure Function (SaveDataToCosmosDb):** An Azure Function responsible for saving IoT device messages to a Cosmos database.
- **Shared Library:** Contains common models and services used across the projects.
- **IoT Devices (Azure):** IoT devices hosted on Azure IoT Hub.
