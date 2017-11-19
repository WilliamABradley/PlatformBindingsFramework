using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Java.IO;
using Newtonsoft.Json;
using PlatformBindings.Common;
using PlatformBindings.Enums;

namespace PlatformBindings.Models.Settings
{
    public class AndroidSettingsContainer : ISettingsContainer
    {
        public AndroidSettingsContainer(string Name, AndroidSettingsContainer Parent)
        {
            this.Name = Name;
            this.Parent = Parent;

            if (Parent != null)
            {
                StoreName = $"{Parent.StoreName}.{Name}";
            }
            else
            {
                StoreName = Name;
                this.Name = Name == "Settings" ? "Local" : Name;
            }

            Prefs = Application.Context.GetSharedPreferences(StoreName, FileCreationMode.Private);
            using (var editor = Prefs.Edit())
            {
                editor.Commit();
            }
        }

        public void Clear()
        {
            var editor = Prefs.Edit();
            editor.Clear();
            editor.Apply();
        }

        public bool ContainsKey(string Key)
        {
            return Prefs.Contains(Key);
        }

        public ISettingsContainer GetContainer(string ContainerName)
        {
            return new AndroidSettingsContainer(ContainerName, this);
        }

        public IReadOnlyList<ISettingsContainer> GetContainers()
        {
            var containers = new List<ISettingsContainer>();

            var prefsdir = SharedPrefs;
            if (prefsdir.Exists() && prefsdir.IsDirectory)
            {
                foreach (var pref in prefsdir.List())
                {
                    string name = System.IO.Path.GetFileNameWithoutExtension(pref);

                    string path = StoreName + ".";
                    if (name != StoreName && name.StartsWith(path))
                    {
                        var subName = name.Replace(path, "");
                        if (!subName.Contains('.'))
                        {
                            containers.Add(new AndroidSettingsContainer(subName, this));
                        }
                    }
                }
            }
            return containers;
        }

        public T GetValue<T>(string Key)
        {
            var type = PlatformBindingHelpers.DetermineGeneric<T>();
            object result = null;
            try
            {
                switch (type)
                {
                    case ObjectType.Int:
                        result = Prefs.GetInt(Key, 0);
                        break;

                    case ObjectType.Long:
                        result = Prefs.GetLong(Key, 0);
                        break;

                    case ObjectType.Bool:
                        result = Prefs.GetBoolean(Key, false);
                        break;

                    case ObjectType.Float:
                        result = Prefs.GetFloat(Key, 0);
                        break;

                    default:
                        result = Prefs.GetString(Key, null);
                        if (type == ObjectType.ComplexObject)
                        {
                            result = JsonConvert.DeserializeObject<T>(result as string);
                        }
                        break;
                }
            }
            catch { result = default(T); }
            return (T)result;
        }

        public Dictionary<string, object> GetValues()
        {
            return Prefs.All.ToDictionary(item => item.Key, item => item.Value);
        }

        internal IReadOnlyList<string> GetValueHeaders()
        {
            return Prefs.All.Keys.ToList();
        }

        public void Remove()
        {
            Application.Context.DeleteSharedPreferences(StoreName);
        }

        public void RemoveContainer(string ContainerName)
        {
            var container = GetContainer(ContainerName);
            container.Remove();
        }

        public void RemoveKey(string Key)
        {
            var editor = Prefs.Edit();
            editor.Remove(Key);
            editor.Commit();
        }

        public void SetValue<T>(string Key, T Value)
        {
            var editor = Prefs.Edit();

            switch ((object)Value)
            {
                case int intg:
                    editor.PutInt(Key, intg);
                    break;

                case long lon:
                    editor.PutLong(Key, lon);
                    break;

                case bool Bool:
                    editor.PutBoolean(Key, Bool);
                    break;

                case float flot:
                    editor.PutFloat(Key, flot);
                    break;

                default:
                    var str = Value is string ? Value as string : JsonConvert.SerializeObject(Value);
                    editor.PutString(Key, str);
                    break;
            }

            editor.Commit();
        }

        public string Name { get; }
        public string StoreName { get; }

        public ISharedPreferences Prefs { get; }
        public ISettingsContainer Parent { get; }

        private static File SharedPrefs
        {
            get { return new File(Application.Context.DataDir, "shared_prefs"); }
        }
    }
}