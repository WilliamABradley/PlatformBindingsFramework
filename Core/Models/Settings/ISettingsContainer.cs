// ******************************************************************
// Copyright (c) William Bradley
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

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