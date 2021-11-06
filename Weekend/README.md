Used versions:
.NET Core SDK 3.1.409 (x64)
.NET Framework 4.8.04084

**How to launch project with windows PowerShell** 

- With prebuilt binaries

1. Set environment variable with bot token:
`$Env:token = "test"`
2. Launch binary file:
`.\Weekend\bin\Debug\netcoreapp3.1\Weekend.exe`
3. Display variable value:
`Get-Item Env:\token`
4. To remove environment variable execute:
`Remove-Item Env:\token`
5. Add variable and launch exe in **one command**:
`$env:token='FOO'; .\Weekend\bin\Debug\netcoreapp3.1\Weekend.exe`

- With dotnet compilation

1. `dotnet build`
2. setup env "token" variable
3. `dotnet run`

**How to launch project in WSL**

- With dotnet compilation
1. Check subsystem version via Windows PowerShell
`wsl --list --verbose`
or
`wsl -l -v`
2. `dotnet build --runtime <subsytem.version>-x<bitness>`
i.e. `dotnet build --runtime ubuntu.20.04-x64`
3. `token="<value>" dotnet run`