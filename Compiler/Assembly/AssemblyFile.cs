﻿namespace Compiler.Assembly
{
    using System.Collections.Generic;
    using System.IO;

    using cfg= ControlFlowGraph;

    public class AssemblyFile : AssemblyObject
    {
        public AssemblyFile()
        {
            this.DataSection = new DataSection();
            this.CodeSection = new CodeSection();
            this.IncludedLibraries = new List<string>(new[] { "msvcrt.lib" });
        }

        public IList<string> IncludedLibraries { get; set; }

        public DataSection DataSection { get; private set; }

        public CodeSection CodeSection { get; private set; }

        public override void Write(TextWriter writer)
        {
            writer.WriteLine("option casemap:none");

            foreach (var library in this.IncludedLibraries)
            {
                writer.WriteLine("includelib " + library);
            }

            writer.WriteLine("externdef printf : near");
            writer.WriteLine("externdef exit : near");
            writer.WriteLine("externdef pow : near");
            writer.WriteLine("externdef malloc : near");

            DataSection.Write(writer);
            CodeSection.Write(writer);

            writer.WriteLine(GetLibaryFunctions());

            writer.WriteLine("END");
        }

        private string GetLibaryFunctions()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var resourceName = "Compiler.Assembly.LibraryFunctions.asm";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
