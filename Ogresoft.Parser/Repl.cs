using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ogresoft;

namespace Ogresoft.Parser
{
    public class Repl
    {
        private AdminThing adminThing;

        public Repl()
        {
            this.adminThing = new AdminThing("some weirdo");
        }

        public AdminThing AdminThing { get { return this.adminThing; } }

        public void Shell()
        {
            while (true)
            {
                Console.Write("Zork>");
                string input = Console.ReadLine();
                if (input == "exit" || input == "quit")
                {
                    return;
                }

                Execute(input);
            }
        }

        public void Execute(string input)
        {
            Parse(input, this.adminThing); 
        }

        public void Parse(string input, Thing doer)
        {
            string[] words = input.Split(' ');

            string verb = doer.CompleteVerb(words[0]);
            if (doer.AllowUseAlone(verb))
            {
                return;
            }

            doer.Tell(Messages.Nonsense());
            // Assume first word is the verb
            //if (doer.)
        }//
    }
}
