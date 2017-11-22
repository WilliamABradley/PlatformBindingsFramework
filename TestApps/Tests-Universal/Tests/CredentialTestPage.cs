using PlatformBindings;
using System.Linq;
using System.Threading.Tasks;
using Tests.TestGenerator;

namespace Tests.Tests
{
    public class CredentialTestPage : TestPage
    {
        public CredentialTestPage(ITestPageGenerator PageGenerator) : base("Credential Tests", PageGenerator)
        {
            AddTestItem(new TestTask
            {
                Name = "Get Credentials",
                Test = ui => Task.Run(() =>
                {
                    var creds = AppServices.Current.Credentials.AllCredentials;
                    return string.Join("\n", creds.Select(item => $"Resource: {item.ResourceName} Username: {item.Username} Password: {item.Password}"));
                })
            });

            AddTestItem(new TestTask
            {
                Name = "Create Credential",
                Test = ui => Task.Run(async () =>
                {
                    var creator = "Create Credential";
                    var resource = await AppServices.Current.UI.RequestTextFromUserAsync(creator, "Resource", "OK", "Cancel");
                    if (resource == null) return Cancelled;
                    var username = await AppServices.Current.UI.RequestTextFromUserAsync(creator, "Username", "OK", "Cancel");
                    if (username == null) return Cancelled;
                    var password = await AppServices.Current.UI.RequestTextFromUserAsync(creator, "Password", "OK", "Cancel");
                    if (password == null) return Cancelled;

                    AppServices.Current.Credentials.Store(new PlatformBindings.Models.CredentialContainer(resource, username, password));
                    var cred = AppServices.Current.Credentials.Retrieve(resource, username);
                    if (cred != null)
                    {
                        if (cred.ResourceName == resource && cred.Username == username && cred.Password == password)
                        {
                            return "Success";
                        }
                        else return "Credential Storage Failed: Retrieved Credential Malformed";
                    }
                    return "Credential Storage Failed";
                })
            });

            AddTestItem(new TestTask
            {
                Name = "Get all Credentials for Resource",
                Test = ui => Task.Run(async () =>
                {
                    var resource = await AppServices.Current.UI.RequestTextFromUserAsync("Get Credentials for Resource", "Resource Name", "OK", "Cancel");
                    if (resource == null) return Cancelled;

                    var creds = AppServices.Current.Credentials.FetchByResource(resource);
                    if (creds.Any())
                    {
                        return string.Join("\n", creds.Select(item => $"Resource: {item.ResourceName} Username: {item.Username} Password: {item.Password}"));
                    }
                    else return "No Credentials exist for the provided Resource";
                })
            });

            AddTestItem(new TestTask
            {
                Name = "Get Credential",
                Test = ui => Task.Run(async () =>
                {
                    var resource = await AppServices.Current.UI.RequestTextFromUserAsync("Credential Resource", "Resource", "OK", "Cancel");
                    if (resource == null) return Cancelled;

                    var username = await AppServices.Current.UI.RequestTextFromUserAsync("Credential Username", "Username", "OK", "Cancel");
                    if (username == null) return Cancelled;

                    var cred = AppServices.Current.Credentials.Retrieve(resource, username);
                    if (cred != null)
                    {
                        return $"Resource: {cred.ResourceName} Username: {cred.Username} Password: {cred.Password}";
                    }
                    return "Credential not found";
                })
            });

            AddTestItem(new TestTask
            {
                Name = "Remove Credential",
                Test = ui => Task.Run(async () =>
                {
                    var resource = await AppServices.Current.UI.RequestTextFromUserAsync("Credential Resource", "Resource", "OK", "Cancel");
                    if (resource == null) return Cancelled;

                    var username = await AppServices.Current.UI.RequestTextFromUserAsync("Credential Username", "Username", "OK", "Cancel");
                    if (username == null) return Cancelled;

                    var cred = AppServices.Current.Credentials.Retrieve(resource, username);
                    if (cred != null)
                    {
                        AppServices.Current.Credentials.Remove(cred);
                        if (AppServices.Current.Credentials.AllCredentials.FirstOrDefault(item => item.ResourceName == resource && item.Username == username) != null)
                        {
                            return "Failed: Credential still exists";
                        }
                        else return "Success";
                    }
                    return "Credential not found";
                })
            });

            AddTestItem(new TestTask
            {
                Name = "Clear Credentials",
                Test = ui => Task.Run(() =>
                {
                    AppServices.Current.Credentials.Clear();
                    if (AppServices.Current.Credentials.AllCredentials.Any())
                    {
                        return "Failed: Credentials still exist";
                    }
                    return "Success";
                })
            });
        }

        private string Cancelled => "Cancelled";
    }
}