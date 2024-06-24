using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.AddressableAssets;

namespace WorldVis
{
    static class Util
    {
        public static R GetField<T, R>(this T obj, string name)
        {
            var bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
            var field = typeof(T).GetField(name, bindingFlags);
            return (R)field?.GetValue(obj);
        }

        public static void SetField<T, V>(this T obj, string name, V value)
        {
            var bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
            var field = typeof(T).GetField(name, bindingFlags);
            field.SetValue(obj, value);
        }

        public static void SetProperty<T, V>(this T obj, string name, V value)
        {
            typeof(T).InvokeMember(name,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty,
                Type.DefaultBinder, obj, new object[] { value });
        }

        public static List<string> GetAllAddressableKeys()
        {
            var keys = new List<string>();

            foreach (var loc in Addressables.ResourceLocators)
            {
                foreach (var key in loc.Keys)
                {
                    if (!loc.Locate(key, typeof(object), out var resourceLocations))
                        continue;

                    keys.Add(key.ToString());
                }
            }

            return keys;
        }
    }
}
