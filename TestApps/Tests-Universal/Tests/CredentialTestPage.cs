using PlatformBindings;
using System.Linq;
using System.Threading.Tasks;
using Tests.TestGenerator;

namespace Tests.Tests
{
    public class CredentialTestPage : TestPage
    {
        public CredentialTestPage(ITestPageGenerator PageGenerator) : base(PageGenerator)
        {
            AddTest(new TestTask
            {
                Name = "Get Credentials",
                Test = ui => Task.Run(() =>
                {
                    var creds = AppServices.Credentials.AllCredentials;
                    return string.Join("\n", creds.Select(item => $"Resource: {item.ResourceName} Username: {item.Username} Password: {item.Password}"));
                })
            });

            AddTest(new TestTask
            {
                Name = "Create Credential",
                Test = ui => Task.Run(async () =>
                {
                    var creator = "Create Credential";
                    var resource = await AppServices.UI.RequestTextFromUserAsync(creator, "Resource", "OK", "Cancel");
                    if (resource == null) return Cancelled;
                    var username = await AppServices.UI.RequestTextFromUserAsync(creator, "Username", "OK", "Cancel");
                    if (username == null) return Cancelled;
                    var password = await AppServices.UI.RequestTextFromUserAsync(creator, "Password", "OK", "Cancel");
                    if (password == null) return Cancelled;

                    AppServices.Credentials.Store(new PlatformBindings.Models.CredentialContainer(resource, username, password));
                    var cred = AppServices.Credentials.Retrieve(resource, username);
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

            AddTest(new TestTask
            {
                Name = "Get all Credentials for Resource",
                Test = ui => Task.Run(async () =>
                {
                    var resource = await AppServices.UI.RequestTextFromUserAsync("Get Credentials for Resource", "Resource Name", "OK", "Cancel");
                    if (resource == null) return Cancelled;

                    var creds = AppServices.Credentials.FetchByResource(resource);
                    if (creds.Any())
                    {
                        return string.Join("\n", creds.Select(item => $"Resource: {item.ResourceName} Username: {item.Username} Password: {item.Password}"));
                    }
                    else return "No Credentials exist for the provided Resource";
                })
            });

            AddTest(new TestTask
            {
                Name = "Get Credential",
                Test = ui => Task.Run(async () =>
                {
                    var resource = await AppServices.UI.RequestTextFromUserAsync("Credential Resource", "Resource", "OK", "Cancel");
                    if (resource == null) return Cancelled;

                    var username = await AppServices.UI.RequestTextFromUserAsync("Credential Username", "Username", "OK", "Cancel");
                    if (username == null) return Cancelled;

                    var cred = AppServices.Credentials.Retrieve(resource, username);
                    if (cred != null)
                    {
                        return $"Resource: {cred.ResourceName} Username: {cred.Username} Password: {cred.Password}";
                    }
                    return "Credential not found";
                })
            });

            AddTest(new TestTask
            {
                Name = "Remove Credential",
                Test = ui => Task.Run(async () =>
                {
                    var resource = await AppServices.UI.RequestTextFromUserAsync("Credential Resource", "Resource", "OK", "Cancel");
                    if (resource == null) return Cancelled;

                    var username = await AppServices.UI.RequestTextFromUserAsync("Credential Username", "Username", "OK", "Cancel");
                    if (username == null) return Cancelled;

                    var cred = AppServices.Credentials.Retrieve(resource, username);
                    if (cred != null)
                    {
                        AppServices.Credentials.Remove(cred);
                        if (AppServices.Credentials.AllCredentials.FirstOrDefault(item => item.ResourceName == resource && item.Username == username) != null)
                        {
                            return "Failed: Credential still exists";
                        }
                        else return "Success";
                    }
                    return "Credential not found";
                })
            });

            AddTest(new TestTask
            {
                Name = "Clear Credentials",
                Test = ui => Task.Run(() =>
                {
                    AppServices.Credentials.Clear();
                    if (AppServices.Credentials.AllCredentials.Any())
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