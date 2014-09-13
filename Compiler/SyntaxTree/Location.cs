namespace Compiler.SyntaxTree
{
    public class Location
    {
        public int Column { get; set; }

        public string File { get; set; }

        public int Line { get; set; }

        public Location(int column, int line)
        {
            this.Column = column;
            this.Line = line;
        }

        public override string ToString()
        {
            return string.Format("({0},{1})", Line, Column);
        }
    }
}
