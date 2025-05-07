# Aprimo.PublicLinks.Export
### Aprimo's Open Source Policy 
This code is provided by Aprimo _as-is_ as an example of how you might solve a specific business problem. It is not intended for direct use in Production without modification.

You are welcome to submit issues or feedback to help us improve visibility into potential bugs or enhancements. Aprimo may, at its discretion, address minor bugs, but does not guarantee fixes or ongoing support.

It is expected that developers who clone or use this code take full responsibility for supporting, maintaining, and securing any deployments derived from it.

If you are interested in a production-ready and supported version of this solution, please contact your Aprimo account representative. They can connect you with our technical services team or a partner who may be able to build and support a packaged implementation for you.

Please note: This code may include references to non-Aprimo services or APIs. You are responsible for acquiring any required credentials or API keys to use those services—Aprimo does not provide them.

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
