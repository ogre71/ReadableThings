using System;
using System.Collections.Generic;
using System.Linq;

namespace Ogresoft.Parser
{
    public class FileSystemThing : Thing
    {
        public FileSystemThing() : this("FileSystem", System.IO.Directory.GetCurrentDirectory()) { }

        public FileSystemThing(string name, string path) : base(name)
        {
            this.Path = path;

            IEnumerable<string> files = System.IO.Directory.EnumerateFiles(this.Path);
            IEnumerable<string> fileNames = files.Select(s => System.IO.Path.GetFileName(s));
            IEnumerable<FileThing> fileThings = fileNames.Select(s => new FileThing(s, this.Path));
            fileThings.ToList().ForEach(fileThing => fileThing.Move(this));

            IEnumerable<string> directories = System.IO.Directory.EnumerateDirectories(this.Path);
            directories.ToList().ForEach(d =>
            {
                string directoryName = System.IO.Path.GetFileName(d);
                var exit = new Exit(directoryName, () => new FileSystemThing(directoryName, d));
                exit.Move(this);
            });

            var parent = System.IO.Directory.GetParent(this.Path); 
            if (parent == null)
            {
                return; 
            }

            var up = new Exit("up", () => new FileSystemThing(System.IO.Path.GetFileName(parent.Name), parent.FullName));
            up.Move(this); 
        }

        public string Path { get; set; }

        private IEnumerable<string> AddCommasOrAnd(IEnumerable<string> input)
        {
            var enumerator = input.GetEnumerator();
            yield return enumerator.Current;

            enumerator.MoveNext(); 
            string previous = enumerator.Current;
            
            while(enumerator.MoveNext())
            {
                yield return ", " + previous;
                previous = enumerator.Current; 
            }

            yield return ", and " + previous;
        }

        private string GetFiles()
        {
            IEnumerable<string> fileNames = System.IO.Directory.GetFiles(System.IO.Directory.GetCurrentDirectory())
                .Select(s => System.IO.Path.GetFileName(s));

            return string.Join("", AddCommasOrAnd(fileNames));
        }
    }
}
