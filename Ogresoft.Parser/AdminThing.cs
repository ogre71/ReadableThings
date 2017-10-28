using System;

namespace Ogresoft.Parser
{
    public class AdminThing : Thing
    {
        public AdminThing(string name) : base(name)
        {
            this.AddVerb("look", new Func<bool>(() => {
                Messages.Action("{O0} {v0look} around.", this);
                var message = Messages.PersonalAction(this, this.Container.GetDescription(this));
                this.Tell(message); 
                return true;
            }));

            this.AddVerb("inventory", new Func<bool>(() => {
                var message = Messages.PersonalAction(this, "{O0} {v0do} not appear to be holding anything except: " + this.Inventory.Description + ".", this);
                this.Tell(message); 
                return true;
            }));

            this.AddVerb("take", new Func<bool>(() => {
                Messages.PersonalAction(this, "What is it that you wish to take?");
                return true;
            }));

            this.AddVerbWithDirectObject("take", new Func<Thing, bool>((directObject) => {
                Messages.Action("{O0} {v0take} {dt1}.", this, directObject);
                return directObject.Move(this); 
            }));

            this.AddVerb("drop", new Func<bool>(() => {
                Messages.PersonalAction(this, "But you aren't holding anything.");
                return true;
            }));

            this.AddVerbWithDirectObject("drop", new Func<Thing, bool>((directObject) => {
               Messages.Action("{O0} {v0drop} {dt1}.", this, directObject);
               return directObject.Move(this.Container);
            }));

            this.AddVerbWithDirectObject("go", new Func<Thing, bool>((directObject) => {
                Exit exit = (Exit)directObject;
                Messages.Action("{O0} {v0go} " + exit.Preposition + " {dt1}.", this, directObject);
                bool success = this.Move(exit.GetDestination());
                if (success)
                {
                    var message = Messages.PersonalAction(this, this.Container.GetDescription(this));
                    this.Tell(message);
                }

                return success;
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
