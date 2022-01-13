# Durable Functions Demo

Durable functions are really freaking cool. Currently this repo has 2 very simple durable orchestrations in it that follow the first two [application patterns](https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-overview?tabs=csharp#application-patterns) listed in Microsoft's documentation.

For a full overview of Azure Durable Functions, go to [Microsoft's documentation](https://docs.microsoft.com/en-us/azure/azure-functions/durable/) for more info and examples.

## Installation

This project is built on the .NET Core 3.1 Framework, so if you don't have that installed you can get it from the [download page](https://dotnet.microsoft.com/en-us/download/dotnet/3.1).

You will also need [Azure Functions Core Tools](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local?tabs=v4%2Cwindows%2Ccsharp%2Cportal%2Cbash%2Ckeda#install-the-azure-functions-core-tools) to test and develop Azure functions locally.

Durable Functions rely on an Azure storage account to store the state of an orchestration. For this demo I just used my local storage, and in order to do that on your machine you will need to install [azurite](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio#install-azurite).

## Usage

1. Open the project in Visual Studio Code and open a terminal in the root directory.
2. Run `dotnet build`.
3. In a separate terminal, simply run `azurite` to boot up your local storage.
4. Copy and paste the `local.settings.example.json` file in your root directory and name it `local.settings.json`
5. Finally, start debugging the project by either clicking `Run` then `Start Debugging` or pressing `F5`

It may take a couple seconds to start but eventually you should see something like this in your terminal indicating that the project is running:

```powershell
Function Runtime Version: 4.0.1.16815

[2022-01-13T15:39:15.385Z] Found C:\Users\3087099\Documents\durable-demo\durable-demo.csproj. Using for user secrets file configuration.

Functions:

        FanOutFanInOrchestration_HttpStart: [GET,POST] http://localhost:7071/api/FanOutFanInOrchestration_HttpStart

        VeryCoolOrchestration_HttpStart: [GET,POST] http://localhost:7071/api/VeryCoolOrchestration_HttpStart

        F1: activityTrigger

        F2: activityTrigger

        F3: activityTrigger

        FanOutFanInOrchestration: orchestrationTrigger

        VeryCoolOrchestration: orchestrationTrigger

        VeryCoolOrchestration_Hello: activityTrigger

For detailed output, run func with --verbose flag.
[2022-01-13T15:39:23.475Z] Host lock lease acquired by instance ID '000000000000000000000000C97C1807'.  
```

If you see an error message saying `Failed to verify "AzureWebJobsStorage" connection specified in "local.settings.json"`, make sure you run `azurite` in a separate terminal before starting the debugger.

Both orchestrations can be started by `ctrl + click`ing on the links listed in the terminal after starting up the project. Set some breakpoints and check out the terminal logs to see the true ✨magic✨ of durable functions!
