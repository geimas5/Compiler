namespace Compiler.Assembly
{
    public abstract class DataEntry : AssemblyObject
    {
        protected DataEntry(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}
