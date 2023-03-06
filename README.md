# Aprimo.PublicLinks.Export

### Considerations
- Currently the app does not support passing in a destination folder. It uses the value found in Program.cs's destinationFolder variable.


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
