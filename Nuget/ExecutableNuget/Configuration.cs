namespace ExecutableNuget
{
    public class Configuration
    {
        public Configuration()
        {
            this.Size = 5;
            this.Name = "Example name";
        }

        public int Size { get; set; }
        public string Name { get; set; }
    }
}
