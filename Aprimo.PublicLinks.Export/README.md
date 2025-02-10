# Aprimo.PublicLinks.Export

### Usage
- Open the Solution in an IDE, such as Visual Studio
- Navigate to app.config and fill in the settings in the <appSettings> section
- Build the solution in either Debug or Release
- Navigate to the programs bin/(Debug or Release)/ folder
- command: ./Aprimo.PublicLinks.Exports.exe

### Considerations
- Currently this app does not support passing in a destination folder. It will use the value in the destinationFolder variable in app.config


### Configuration

##### app.config
- Set the values within <appsettings>
   - AprimoClientID
   - AprimoClientSecret
   - AprimoTenant
   - DestinationFolder

# Open Source Policy

For more information about Aprimo's Open Source Policies, please refer to
https://community.aprimo.com/knowledgecenter/aprimo-connect/aprimo-connect-open-source
