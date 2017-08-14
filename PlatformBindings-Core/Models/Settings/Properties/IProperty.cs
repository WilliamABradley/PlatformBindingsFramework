namespace PlatformBindings.Models.Settings.Properties
{
    public interface IProperty
    {
        ISettingsContainer Parent { get; }

        void Attach(ISettingsContainer Parent);

        void Remove();

        string PropertyName { get; set; }
    }
}