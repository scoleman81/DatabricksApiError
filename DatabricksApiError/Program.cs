// See https://aka.ms/new-console-template for more information

using DatabricksApiError;

// ToDo: Enter Values here:
var databricksWorkspaceUrl = "https://adb-XXXXXXXXXXXXXXX.XX.azuredatabricks.net";
var databricksPatToken = "XXXXXXXXXXXXXXXXXXXXXXXXXXX";

var testClass = new ConnectionTesting(databricksWorkspaceUrl, databricksPatToken);

await testClass.TestWithPat();
await testClass.TestWithAzureCredentials();

