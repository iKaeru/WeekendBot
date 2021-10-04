**How to launch project by built binaries with windows PowerShell** 
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