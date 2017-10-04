using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogresoft.Parser
{
    public class FileSystemThing : Thing
    {
        public FileSystemThing() : this("FileSystem") { }

        public FileSystemThing(string name) : base(name)
        {
            this.Path = Environment.CurrentDirectory; 
        }

        public string Path { get; set; }

        private IEnumerable<string> GetFileNameWithoutPath(string[] filePaths)
        {
            foreach (string filePath in filePaths)
            {
                yield return System.IO.Path.GetFileName(filePath);
            }
        }

        private string IEnumerableToString(IEnumerable<string> enumerableStrings)
        {
            string outputString = string.Empty;
            string lastElement = null;

            foreach (string element in enumerableStrings)
            {
                if (outputString != string.Empty)
                {
                    outputString += ", ";
                }

                outputString += lastElement;
                lastElement = element;
            }

            outputString += ", and " + lastElement;
            return outputString;
        }

        private string GetFiles()
        {
            string[] files = System.IO.Directory.GetFiles(Environment.CurrentDirectory);

            IEnumerable<string> fileNames = GetFileNameWithoutPath(files);

            return IEnumerableToString(fileNames);
        }

        public override string Description => base.Description + "\n " + Environment.CurrentDirectory + "\n" + this.GetFiles();

    }
}
