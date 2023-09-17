# ChatGPT.Client

## Overview

This repository contains a .NET 7 console application that integrates with OpenAI's GPT-3 API, allowing users to interact with the ChatGPT model directly from their console. You can ask ChatGPT anything, and it will respond in real-time, providing detailed answers to a wide range of queries.

## Features

- **Real-time Interaction**: Ask questions and get immediate responses from ChatGPT.
- **Easy Configuration**: Set up the service easily with appsettings.json and user secrets.
- **Clean and Organized Code**: The codebase is structured following the best practices to ensure maintainability and extensibility.

## Prerequisites

- [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- An active OpenAI API key. You can obtain it from [OpenAI] (https://www.openai.com/).

## Setup and Installation

1. Clone this repository to your local machine: https://github.com/D1No-git/ChatGPT.Client

2. Navigate to the project directory: cd path/to/your/repo

3. Setup your OpenAI API key using .NET secret manager. Replace `YOUR_OPENAI_API_KEY` with your actual API key:

- Navigate to your project directory (where your .csproj file is located) in a terminal or command prompt,
- Run the following command to initialize the Secret Manager for your project: dotnet user-secrets init 
- Now, add your secret API key to the Secret Manager with a command like the following (replace "MyApiKey" and "12345" with your actual key name and value): dotnet user-secrets set ""OpenAI:MyApiKey" "12345"


4. Build the application: dotnet build

5. Run the application: dotnet run 

## Usage

Upon running the application, you will be greeted with a message welcoming you to the ChatGPT console app. You will be prompted to enter a question. Simply type your question and press enter to receive a response from ChatGPT. To exit the application, type `exit` and press enter.

## Contributing

If you'd like to contribute to this project, feel free to open a pull request or report any issues you encounter.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.

## Contact

If you have any questions, feel free to reach out to the maintainer of this repository:
- [Dino](dino.alija@outlook.com)

## Acknowledgements

- Thanks to OpenAI for providing the GPT-3 API.

