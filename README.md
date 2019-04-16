# Spider.Print
Console application based on .net core 2.1, the Spider.Print can get data using api url and combined the data with html template,
and then generate pdf file from rendered html string, finally it send the binarry data to remote printer.

## Using appsettings.json
Spider.Print uses `appsettings.json` to get the application configurations as `ApiUrl` which the application get the data, `PrinterIP` remote printer ip, `PrinterPort` printer port, and `TestingMode` to use specific order `Id` for testig purpose only.
```json=
"MySettings": {
  "ApiUrl": "http://localhost:7588",
  "PrinterIP": "10.10.10.10",
  "PrinterPort": 9100,
  "TestingMode": true
}
```
