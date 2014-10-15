namespace Compiler.Assembly
{
    public abstract class Section : AssemblyObject
    {
        public Section(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}
