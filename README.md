# Aprimo.PublicLinks.Export

### Usage
- Navigate to the programs bin/(Debug or Release)/ folder
- command: Aprimo.PublicLinks.Exports.exe

### Considerations
- Currently this app does not support passing in a destination folder. It will use the value in the destinationFolder variable in Program.cs


### Configuration

##### app.config
- Set the values within <appsettings>
   - AprimoUsername
   - AprimoClientID
   - AprimoClientSecret
   - AprimoTenant
   
##### Program.cs
- destinationFolder - The folder destination the export should be saved to
- nameofCSV - the name of the exported CSV to be saved to destinationFolder
