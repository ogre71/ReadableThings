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
        private RoomThing roomThing; 

        public Repl()
        {
            this.adminThing = new AdminThing("some weirdo");
            this.roomThing = new RoomThing();
            this.adminThing.Move(this.roomThing); 
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

            if (words.Length == 0)
            {
                doer.Tell(Messages.Nonsense());
                return; 
            }

            string verb = doer.CompleteVerb(words[0]);
            if (words.Length == 1)
            {
                if (doer.AllowUseAlone(verb))
                {
                    return;
                }
            }

            List<string> remainingWords = words.Skip(1).ToList<string>();
            List<Thing> foundThings = new List<Thing>(); 

            foreach(Thing thing in doer.Container.ShallowInventory)
            {
                if (thing.Matches(remainingWords))
                {
                    foundThings.Add(thing);
                }
            }

            if (foundThings.Count == 0)
            {
                doer.Tell("I can't find that."); 
            }

            if (foundThings.Count > 1)
            {
                doer.Tell("I'm confused."); 
            }

            var foundThing = foundThings[0];

            if (!foundThing.AllowUsageAsDirObj(verb, doer))
            {

            }

            doer.Tell(Messages.Nonsense());
            // Assume first word is the verb
            //if (doer.)
        }//
    }
}
