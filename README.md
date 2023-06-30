# Halo Infinite Settings Editor .NET
 Recreation of [Halo Infinite Settings Editor](https://github.com/aetopia/halo-infinite-settings-editor) but in the **`.NET Framework`**.

# Usage
|Action|Operation|
|-|-|
|Editing Values|Double click the cell, you want to edit.|
|Saving Settings|Click the `Save` button to save the edited settings.|
|Refreshing Settings|You can reset any edited values but to default by clicking the `Refresh` button.|
|Common/Multiplayer Settings|Hitting the `Common/Multiplayer` button will load common/multiplayer settings. The button text displays the currently loaded settings.|
|Key Navigation|You may use the dropdown to navigate to a specific key.|

# Build
1. Download and install the .NET SDK and .NET Framework 4.8.1 Developer Pack from:<br>https://dotnet.microsoft.com/en-us/download/visual-studio-sdks
2. Run the following command:

    ```cmd
    dotnet build Halo-Infinite-Settings-Editor-NET\Halo-Infinite-Settings-Editor-NET.csproj --configuration Release
    ```
