using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;

namespace Ogresoft.Parser
{
    public class Parser
    {
        public const string Garbage = "I don't know how to '{0}'";

        private AdminThing adminThing;
        private FileSystemThing roomThing; 

        public Parser()
        {
            this.adminThing = new AdminThing("some weirdo");
            this.adminThing.Unique = true; 
            this.roomThing = new FileSystemThing();
            this.adminThing.Move(this.roomThing); 
        }

        public AdminThing AdminThing { get { return this.adminThing; } }

        public string Serialize()
        {
            string serializedAdmin = this.adminThing.Serialize();
            return serializedAdmin;
        }

        public Exception Shell()
        {

            while (true)
            {
                Console.Write("Zork>");
                string input = Console.ReadLine();
                if (input == "exit" || input == "quit")
                {
                    var serialized = Serialize();

                    var tempFileName = System.IO.Path.GetTempFileName();
                    System.IO.File.WriteAllLines(tempFileName, new string[] { serialized });
                    Process.Start("notepad", tempFileName);

                    return null;
                }

                try
                {
                    Execute(input);
                }
                catch(Exception ex)
                {
                    return ex;
                }
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

            if (verb == null)
            {
                doer.Tell(string.Format(Garbage, words[0]));
                return; 
            }

            if (words.Length == 1)
            {
                if (doer.AllowUseAlone(verb))
                {
                    return;
                }

                Debug.Assert(false); 
                return; 
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

            foreach(Thing thing in doer.ShallowInventory)
            {
                if (thing.Matches(remainingWords))
                {
                    foundThings.Add(thing);
                }
            }

            if (foundThings.Count == 0)
            {
                doer.Tell(Messages.PersonalAction(doer, "I can't find {dt1}.", doer, new Thing(string.Join(" ", remainingWords))));
                return; 
            }

            if (foundThings.Count > 1)
            {
                doer.Tell("I'm confused, which one do you mean?");
                return; 
            }

            var foundThing = foundThings[0];

            if (!foundThing.AllowUsageAsDirObj(verb, doer))
            {
                var message = Messages.PersonalAction(doer, "{O0} can't {v0" + verb + "} that.", doer);
                doer.Tell(message);
                return; 
            }

            if (doer.AllowUsageWithDirObj(verb, foundThing))
            {
                return;
            }

            doer.Tell(Messages.Nonsense());
            // Assume first word is the verb
            //if (doer.)
        }//
    }
}
