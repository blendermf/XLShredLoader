using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

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
    }
}
