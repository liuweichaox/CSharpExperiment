%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\installutil.exe C:\Users\Administrator\Desktop\NetCoe\NetCoreDemo\WindowsService\bin\Debug\WindowsService.exe
Net Start MyService
sc config MyService start = auto
pause