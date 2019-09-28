using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityModManagerNet;

namespace XLShredLib {

    public class XLShredDataRegistry {
        private static XLShredDataRegistry _instance;
        private Dictionary<string, Dictionary<string, object>> modData;

        static XLShredDataRegistry() {
            _instance = new XLShredDataRegistry {
                modData = new Dictionary<string, Dictionary<string, object>>()
            };
        }

        public static bool TryGetData(string modid, string key, out dynamic data, bool log = true) {

            if (!_instance.modData.TryGetValue(modid, out Dictionary<string, object> modDataEntries)) {
                if (log) XLShredLogger.Log($"The mod '{modid}' does not have any data registered.", _instance.GetType());
                data = null;
                return false;
            }
            if (!modDataEntries.TryGetValue(key, out object modData)) {
                if (log) XLShredLogger.Log($"The mod '{modid}' does not have the any data registered with key '{key}'.", _instance.GetType());
                data = null;
                return false;
            }
            data = modData;
            return true;
        }

        public static bool TryGetData<T>(string modid, string key, out T data, bool log = true) {

            if (!_instance.modData.TryGetValue(modid, out Dictionary<string, object> modDataEntries)) {
                if (log) XLShredLogger.Log($"The mod '{modid}' does not have any data registered.", _instance.GetType());
                data = default(T);
                return false;
            }
            if (!modDataEntries.TryGetValue(key, out object modData)) {
                if (log) XLShredLogger.Log($"The mod '{modid}' does not have the any data registered with key '{key}'.", _instance.GetType());
                data = default(T);
                return false;
            }

            data = (T)modData;
            return true;
        }

        public static dynamic GetDataOrDefault(string modid, string key, dynamic defaultValue, bool log = true) {

            if (!_instance.modData.TryGetValue(modid, out Dictionary<string, object> modDataEntries)) {
                if (log) XLShredLogger.Log($"The mod '{modid}' does not have any data registered.", _instance.GetType());
                return defaultValue;
            }
            if (!modDataEntries.TryGetValue(key, out object modData)) {
                if (log) XLShredLogger.Log($"The mod '{modid}' does not have the any data registered with key '{key}'.", _instance.GetType());
                return defaultValue;
            }
            return modData;
        }

        public static T GetDataOrDefault<T>(string modid, string key, T defaultValue, bool log = true) {

            if (!_instance.modData.TryGetValue(modid, out Dictionary<string, object> modDataEntries)) {
                if (log) XLShredLogger.Log($"The mod '{modid}' does not have any data registered.", _instance.GetType());
                return defaultValue;
            }
            if (!modDataEntries.TryGetValue(key, out object modData)) {
                if (log) XLShredLogger.Log($"The mod '{modid}' does not have the any data registered with key '{key}'.", _instance.GetType());
                return defaultValue;
            }
            return (T)modData;
        }

        public static void SetData(string modid, string key, object data) {

            if (!_instance.modData.TryGetValue(modid, out Dictionary<string, object> modDataEntries)) {
                _instance.modData[modid] = new Dictionary<string, object>();
            }

            _instance.modData[modid][key] = data;
        }
    }

    public class XLShredLogger {
        private string lastMessage;
        private int repeatMessageCount;
        private static XLShredLogger _instance;

        static XLShredLogger() {
            _instance = new XLShredLogger();
        }


        public static void Log(string message, Type type = null) {
            string newMessage = (type == null) ? $"MOD_LOG: {message}": $"MOD_LOG {type.Name}: {message}";

            if (_instance.lastMessage == newMessage) {
                _instance.repeatMessageCount++;
            } else {
                _instance.repeatMessageCount = 0;
                _instance.lastMessage = newMessage;
            }

            if (_instance.repeatMessageCount < 10) {
                Console.WriteLine(newMessage);
                return;
            }

            newMessage = $"{newMessage} ({_instance.repeatMessageCount})";

            if (_instance.repeatMessageCount <= 100 && _instance.repeatMessageCount % 10 == 0) {
                Console.WriteLine(newMessage);
            } else if (_instance.repeatMessageCount <= 1000 && _instance.repeatMessageCount % 100 == 0) {
                Console.WriteLine(newMessage);
            } else if (_instance.repeatMessageCount <= 10000 && _instance.repeatMessageCount % 1000 == 0) {
                Console.WriteLine(newMessage);
            }
        }
    }

    public static class ModUtils {

        public static Assembly GetModAssembly(string modID) {
            var mod = UnityModManager.FindMod(modID);

            return (mod != null) ? GetModAssembly(mod) : null;
        }

        public static Assembly GetModAssembly(this UnityModManager.ModEntry modEntry) {
            if (modEntry == null) return null;

            return AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "ReplayModLoader");
        }

        public static Type GetModType(string modID, string typeName) {
            var modEntry = UnityModManager.FindMod(modID);

            if (modEntry == null) return null;

            return modEntry.GetModType(typeName);
        }

        public static Type GetModType(this UnityModManager.ModEntry modEntry, string typeName) {
            if (modEntry == null) return null;

            Assembly assembly = GetModAssembly(modEntry);
            if (assembly == null) return null;

            return assembly.GetType(typeName, false);
        }

        public static object GetModObject(this UnityModManager.ModEntry modEntry, string typeName, Func<Type, object> getValue) {
            Type type = modEntry.GetModType(typeName);
            
            return (type == null) ? null : getValue.Invoke(type);
        }

        public static object GetModObject(string modID, string typeName, Func<Type, object> getValue) {
            var modEntry = UnityModManager.FindMod(modID);

            return modEntry?.GetModObject(typeName, getValue);
        }

        public static object GetModObject(string modID, string typeName, out UnityModManager.ModEntry modEntry, Func<Type, object> getValue) {
            modEntry = UnityModManager.FindMod(modID);

            return modEntry?.GetModObject(typeName, getValue);
        }

        public static void DumpGameObject(GameObject gameObject, string indent, bool fields = false) {
            Console.WriteLine("{0}+{1}", indent, gameObject.name);

            foreach (Component component in gameObject.GetComponents<Component>()) {
                DumpComponent(component, indent + "  ", fields);
            }

            foreach (Transform child in gameObject.transform) {
                DumpGameObject(child.gameObject, indent + "  ", fields);
            }
        }

        private static string FormatDumpValue(object value, string valName, string indent) {
            return value == null ? "(null)" : value.ToString().Replace("\n", "\n" + indent + new string(' ', valName.Length + 4));
        }

        public static void DumpComponent(Component component, string indent, bool fields = false) {
            Console.WriteLine("{0}{1}", indent, (component == null ? "(null)" : component.GetType().Name));

            if (fields) {
                foreach (var thisVar in component.GetType().GetFields()) {
                    Console.WriteLine("{0}.{1} = {2}", indent + "  ", thisVar.Name, FormatDumpValue(thisVar.GetValue(component), thisVar.Name, indent + "  "));
                }
                foreach (var thisVar in component.GetType().GetProperties()) {
                    Console.WriteLine("{0}.{1} = {2}", indent + "  ", thisVar.Name, FormatDumpValue(thisVar.GetValue(component, null), thisVar.Name, indent + "  "));
                }
            }
        }

        public static void DumpGameObjectCloneCompare(GameObject o1, GameObject o2, string indent) {
            Console.WriteLine("{0}+{1}", indent, o1.name);

            var o1c = o1.GetComponents<Component>();
            var o2c = o2.GetComponents<Component>();
            
            for (int i = 0; i < o1c.Length; i++) {
                DumpComponentCloneCompare(o1c[i], o2c[i], indent + "  ");
            }

            for (int i = 0; i < o1.transform.childCount; i++) {
                DumpGameObjectCloneCompare(o1.transform.GetChild(i).gameObject, o2.transform.GetChild(i).gameObject, indent + "  ");
            }
        }

        private static string FormatCompareDumpValues(object v1, object v2, string valName, string indent) {
            string output = "";
            var val1 = (v1 == null ? "(null)" : v1.ToString()).Split('\n');
            var val2 = (v2 == null ? "(null)" : v2.ToString()).Split('\n');

            int maxWidthVal1 = val1.Max(v => v.Length);
            int maxLines = Math.Max(val1.Length, val2.Length);

            for (int i = 0; i < maxLines; i++) {
                if (i < val1.Length) {
                    if (i == 0) {
                        output += val1[0];
                    } else {
                        output += '\n' + indent + new string(' ', valName.Length + 4) + val1[i];
                    }

                    if (i < val2.Length) {
                        output += $"{new string(' ', maxWidthVal1 - val1[i].Length)} | {val2[i]}";
                    }
                } else if (i < val2.Length) {
                    output += $"{indent}{new string(' ', valName.Length + 4 + maxWidthVal1)} | {val2[i]}";
                }
            }
            return output;
        }

        public static void DumpComponentCloneCompare(Component c1 , Component c2, string indent) {
            Console.WriteLine("{0}{1}", indent, (c1 == null ? "(null)" : c1.GetType().Name));
            var c1p = c1.GetType().GetProperties();
            var c2p = c2.GetType().GetProperties();
            for (var i = 0; i < c1p.Count(); i++) {
                var c1pv = c1p[i].GetValue(c1, null);
                var c2pv = c2p[i].GetValue(c2, null);
                var name = c1p[i].Name;
                if ((c1pv == null ? "(null)" : c1pv.ToString()) != (c2pv == null ? "(null)" : c2pv.ToString())) {
                    Console.WriteLine("{0}.{1} = {2}", indent + "  ", name, FormatCompareDumpValues(c1pv, c2pv, name, indent + "  "));
                }
            }
            var c1f = c1.GetType().GetFields();
            var c2f = c2.GetType().GetFields();
            for (var i = 0; i < c1f.Count(); i++) {
                var c1fv = c1f[i].GetValue(c1);
                var c2fv = c2f[i].GetValue(c2);
                var name = c1f[i].Name;
                if ((c1fv == null ? "(null)" : c1fv.ToString()) != (c2fv == null ? "(null)" : c2fv.ToString())) {
                    Console.WriteLine("{0}.{1} = {2}", indent + "  ", name, FormatCompareDumpValues(c1fv, c2fv, name, indent + "  "));
                }
            }
        }
    }
}
