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

            public Repl()
            {

            }

            public void Parse(string input, Thing doer)
            {
                string[] words = input.Split(' ');

                if (doer.AllowUseAlone(words[0]))
                {
                    return;
                }

                doer.Tell(Messages.Nonsense());
                // Assume first word is the verb
                //if (doer.)
            }//
        }
}
