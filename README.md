# Halo Infinite Settings Editor .NET
 Recreation of [Halo Infinite Settings Editor](https://github.com/aetopia/halo-infinite-settings-editor) but in the .NET Framework.

# Usage
![image](https://github.com/Aetopia/Halo-Infinite-Settings-Editor-NET/assets/41850963/a815aed7-7dec-4ab6-bfdf-dfcb1fcd0062)

|Action|Operation|
|-|-|
|Editing Values|Double click the cell, you want to edit.|
|Saving Settings|Click the `Save` button to save the edited settings.|
|Refreshing Settings|You can reset any edited values but to default by clicking the `Refresh` button.|
|Key Navigation|You may use the dropdown to navigate to a specific key.|

# Build
1. Download and install the .NET SDK and .NET Framework 4.8.1 Developer Pack from:<br>https://dotnet.microsoft.com/en-us/download/visual-studio-sdks
2. Open a Command Prompt or PowerShell window where the `.csproj` file is located.
3. Run the following command:

    ```cmd
    dotnet.exe build --configuration Release
    ```
4. The output should be generated in `"bin\Release\net481\Halo Infinite Settings Editor .NET.exe"`.
