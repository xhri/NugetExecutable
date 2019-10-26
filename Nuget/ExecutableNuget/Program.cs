namespace ExecutableNuget
{
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    class Program
    {
        static void Main(string[] args)
        {
            var config = GetConfig(args);
            var action = GetAction(args);
            if (action != null)
            {
                action.Act(config);
            }
        }

        static Configuration GetConfig(string[] args)
        {
            var path = GetCommandLineValue(args, "-c");
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                return new Configuration();
            }
            else
            {
                return JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(args[0]));
            }
        }

        static ICustomAction GetAction(string[] args)
        {
            var name = GetCommandLineValue(args, "-a");
            if(name == null)
            {
                Console.WriteLine("You have to give an action name!");
                ShowHelp();
                return null;
            }

            var action = FindAction(name);
            if (action == null)
            {
                Console.WriteLine("Couldn't find given action!");
            }

            return action;
        }

        static string GetCommandLineValue(string[] args, string paramName)
        {
            var position = Array.IndexOf(args, paramName);
            if(position<0 || args.Length < position + 1)
            {
                return null;
            }
            else
            {
                return args[position + 1];
            }
        }

        static void ShowHelp()
        {
            Console.WriteLine("Usage");
            Console.WriteLine("     dotnet ExecutableNuget.dll -a <action_name> [-c <configuration_path>]");
        }

        static ICustomAction FindAction(string name)
        {
            var directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            var libraries = directory.GetFiles("*.dll");
            foreach(var dll in libraries)
            {
                try
                {
                    var assembly = Assembly.LoadFrom(dll.FullName);
                    var type = assembly.GetExportedTypes().Single(t => typeof(ICustomAction).IsAssignableFrom(t) && t.Name.Equals(name));
                    return (ICustomAction)Activator.CreateInstance(type);
                }
                catch (Exception){}
            }

            return null;
        }
    }
}
