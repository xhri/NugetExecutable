namespace MyActions
{
    using ExecutableNuget;
    using System;

    public class FirstAction : ICustomAction
    {
        public void Act(Configuration config)
        {
            Console.WriteLine("My first action started");
            Console.WriteLine($"Name used in configuration: {config.Name}");
        }
    }
}
