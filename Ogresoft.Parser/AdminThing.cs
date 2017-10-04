﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogresoft.Parser
{
    public class AdminThing : Thing
    {
        public AdminThing(string name) : base(name)
        {
            this.AddVerb("look", new Func<bool>(() => {
                Messages.Action("{O0} {v0look} around.", this);
                var message = Messages.PersonalAction(this, this.Container.Description);
                this.Tell(message); 
                return true;
            }));

            this.AddVerb("inventory", new Func<bool>(() => {
                var message = Messages.PersonalAction(this, "{O0} {v0do} not appear to be holding anything.", this);
                this.Tell(message); 
                return true;
            }));

            this.AddVerb("take", new Func<bool>(() => {
                Messages.PersonalAction(this, "What is it that you wish to take?");
                return true;
            }));

            this.AddVerbWithDirectObject("take", new Func<Thing, Thing, bool>((doer, directObject) => {
                Messages.PersonalAction(this, "What is it that you wish to take?");
                return true;
            }));

            this.AddVerb("drop", new Func<bool>(() => {
                Messages.PersonalAction(this, "But you aren't holding anything.");
                return true;
            }));
        }

        public override void Tell(string message)
        {
            base.Tell(message);

            Console.WriteLine(message);
            this.LastMessage = message; 
        }

        public string LastMessage { get; private set; }
    }
}
