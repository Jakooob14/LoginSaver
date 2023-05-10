using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace LoginSaver
{
    public class LauncherCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public static class LauncherHelper
    {
        public static Dictionary<string, LauncherCredentials> CredentialsMap = JsonConvert.DeserializeObject<Dictionary<string, LauncherCredentials>>(ReadFile());
        
        public static string GetUsername(string platform)
        {
            return CredentialsMap[platform].Username;
        }
        
        public static string GetPassword(string platform)
        {
            return CredentialsMap[platform].Password;
        }


        public static void SetUsername(string platform, string username)
        {
            try
            {
                CredentialsMap[platform].Username = username;
            }
            catch (KeyNotFoundException)
            {
                CredentialsMap[platform] = new LauncherCredentials { Username = username };
            }
            WriteFile();
        }
        
        public static void SetPassword(string platform, string password)
        {
            try
            {
                CredentialsMap[platform].Password = password;
            }
            catch (KeyNotFoundException)
            {
                CredentialsMap[platform] = new LauncherCredentials { Password = password };
            }
            WriteFile();
        }

        public static void Remove(string platform)
        {
            CredentialsMap.Remove(platform);
            WriteFile();
        }

        private static void WriteFile()
        {
            string json = JsonConvert.SerializeObject(CredentialsMap, Formatting.Indented);
            File.WriteAllText("launchers.json", json);
        }

        private static string ReadFile()
        {
            try
            {
                string file = File.ReadAllText("launchers.json");
                JsonTextReader reader = new JsonTextReader(new StringReader(file));
                return file;
            }
            catch (FileNotFoundException)
            {
                return "{}";
            }
        }
    }
}