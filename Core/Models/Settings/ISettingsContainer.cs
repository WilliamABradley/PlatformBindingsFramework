using System.Collections.Generic;

namespace PlatformBindings.Models.Settings
{
    public interface ISettingsContainer
    {
        ISettingsContainer GetContainer(string ContainerName);

        void RemoveContainer(string ContainerName);

        T GetValue<T>(string Key);

        bool ContainsKey(string Key);

        void RemoveKey(string Key);

        void Remove();

        void Clear();

        void SetValue<T>(string Key, T Value);

        Dictionary<string, object> GetValues();

        IReadOnlyList<ISettingsContainer> GetContainers();

        string Name { get; }
        ISettingsContainer Parent { get; }
    }
}