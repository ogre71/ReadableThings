using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Ogresoft
{
    public delegate void ThingEventHandler(Thing sender, Thing thing);

    public partial class Thing : NameBase
    {
        public static object  DeSerialize(string input) 
        {
            return JsonConvert.DeserializeObject(input);
        }

        public static Thing Find(string name, List<Thing> container)
        {
            List<Thing> output = FindThings(name, container);
            if (output == null || output.Count == 0)
                return null;

            return output[0];
        }

        public static List<Thing> FindThings(List<string> words, List<Thing> container)
        {
            List<Thing> matches = new List<Thing>();

            foreach (Thing thing in container)
                if (thing.Matches(words))
                    matches.Add(thing);

            if (matches.Count == 0)
                return null;

            return matches;
        }

        public static List<Thing> FindThings(string name, List<Thing> container)
        {
            string[] words = name.Split(' ');

            return FindThings(words, container);
        }

        public static List<Thing> FindThings(string[] words, List<Thing> container)
        {
            List<Thing> matches = new List<Thing>();

            foreach (Thing thing in container)
                if (thing.Matches(words))
                    matches.Add(thing);

            if (matches.Count == 0)
                return null;

            return matches;
        }

        public static List<Thing> CloneList(List<Thing> input)
        {
            List<Thing> output = new List<Thing>();
            output.AddRange(input);
            return output;
        }

        /// <summary>
        /// Formats a string array into a readable list. 
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        public static string FormatList(string[] strings)
        {
            if (strings.Length == 0)
                return "";

            if (strings.Length == 1)
                return strings[0];

            if (strings.Length == 2)
                return strings[0] + " and " + strings[1];

            string output = "";

            if (strings.Length > 2)
            {
                for (int i = 0; i < strings.Length - 1; i++)
                    output += strings[i] + ", ";

                output += "and " + strings[strings.Length - 1];
            }

            return output;
        }

        /// <summary>
        /// the name of each memeber of the list. 
        /// </summary>
        public static string[] Strings(List<Thing> things)
        {
            string[] output = new string[things.Count];

            for (int i = 0; i < things.Count; i++)
                output[i] = things[i].Name;

            return output;
        }

        /// <summary>Converts List<Thing> to string[] by retrieving the DefiniteName from each Thing.</summary>
        public static string[] DefiniteStrings(List<Thing> things)
        {
            string[] output = new string[things.Count];
            for (int i = 0; i < things.Count; i++)
            {
                output[i] = things[i].DefiniteName;
            }
            return output;
        }

        /// <summary>Converts List<Thing> to string[] by retrieving the IndefiniteName from each Thing.</summary>
        public static string[] IndefiniteStrings(List<Thing> things)
        {
            string[] output = new string[things.Count];
            for (int i = 0; i < things.Count; i++)
            {
                output[i] = things[i].IndefiniteName;
            }
            return output;
        }

        public static string IndefiniteStringsPrintable(List<Thing> things)
        {
            string[] strings = IndefiniteStrings(things);
            return FormatList(strings);
        }

        public static implicit operator string(Thing thing)
        {
            if (thing == null)
                return "Null-Thing";

            return thing.Name;
        }

        public Thing()
        {
        }

        public Thing(string name)
            : this()
        {
            this.Name = name;
        }

        private void OnDeserialized()
        {

        }

        /// <summary>
        /// Event that is fired before this thing is moved from one container to another. 
        /// </summary>
        public event ThingEventHandler Moving;

        protected void OnMoving(Thing destination)
        {
            Moving?.Invoke(this, destination);
        }

        /// <summary>
        /// Event that is fired when this thing has moved from one container to another. 
        /// </summary>
        public event ThingEventHandler Moved;

        protected void OnMoved(Thing destination)
        {
            Moved?.Invoke(this, destination);
        }

        /// <summary>
        /// Event that is fired when this thing accepts another thing into its inventory.
        /// </summary>
        public event ThingEventHandler Accepted;

        protected void OnAccepted(Thing acceptedThing)
        {
            Accepted?.Invoke(this, null);
        }

        /// <summary>
        /// Event that is fired when this thing releases another thing from its inventory. 
        /// </summary>
        public event ThingEventHandler Released;

        protected void OnReleased(Thing releasedThing)
        {
            Released?.Invoke(this, releasedThing);
        }

        public event ThingEventHandler Disposing;

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            Disposing = null;

            Messages.Action("{It0} {v0:wink} out of existence.", this);

            if (this.Container != null)
                this.Container.Release(this);

            this.disposed = true;

            Disposing?.Invoke(this, this);

        }

        ~Thing()
        {
            Dispose(false);
        }

        private bool disposed = false;
        public bool Disposed
        {
            get { return disposed; }
        }

        public Thing Container { get; set; }

        /// <summary>
        /// This is locations where the Thing is contained, not locations that the thing can contain.
        /// </summary>
        public List<string> Locations { get; set; }

        public Thing GetRootContainer()
        {
            Thing currentContainer = this.Container;

            while (currentContainer.Container != null)
                currentContainer = currentContainer.Container;

            return currentContainer;
        }

        public Thing GetOutermostViewableContainer()
        {
            Thing currentContainer = this.Container;
            while (true)
            {
                if (currentContainer.Container == null)
                    break;

                if (currentContainer.Locations.Count == 0)
                    break;

                foreach (string location in this.Container.Locations)
                {
                    if ((this.Container.Inventory.GetOpacity(location) & Opacity.Out) == Opacity.None)
                        break;
                }

                currentContainer = currentContainer.Container;
            }

            return currentContainer;
        }

        public List<Thing> GetObservers()
        {
            if (this.Container == null)
                return null;

            return this.Container.GetObserversFor(this);
        }

        public virtual void Tell(string message)
        {
        }

        /// <summary>
        /// Indicates that the thing can be seen by normal means. 
        /// </summary>
        public virtual bool IsObservable { get; set; } = true;

        /// <summary>
        /// Indicates that this Thing is interested in receiving messages about the environment.
        /// </summary>
        public virtual bool IsObserver { get; set; } = false;

        public InventoryHash Inventory { get; set; } = new InventoryHash(); 

        protected string defaultKey = "in";
        /// <summary>
        /// The default Inventory key for this Thing. 
        /// </summary>
        public string DefaultKey
        {
            get { return defaultKey; }
        }

        public List<Thing> this[string index]
        {
            get { return this.Inventory[index]; }
        }

        //Allows the inventoried things to observe outside of this container. 
        protected Opacity opacity = Opacity.In | Opacity.Out;
        public virtual Opacity Opacity
        {
            get
            {
                if (this.Container == null)
                    return opacity & Opacity.In;

                return opacity;
            }
        }

        public bool MatchKey(string testKey)
        {
            for (int i = 0; i < this.Inventory.Keys.Length; i++)
                if (this.Inventory.Keys[i].StartsWith(testKey))
                {
                    testKey = this.Inventory.Keys[i];
                    return true;
                }

            return false;
        }

        /// <summary>
        /// Gets the observers for the specified Thing. 
        /// </summary>
        public virtual List<Thing> GetObserversFor(Thing thing)
        {
            List<Thing> output = new List<Thing>();

            foreach (Thing otherThing in ShallowInventory)
                if (otherThing.IsObserver)
                    if (otherThing != thing)
                        output.Add(otherThing);

            return output;
        }

        [JsonIgnore]
        /// <summary>
        /// The Inventory that is reachable with the 'my' keyword. 
        /// </summary>
        public virtual List<Thing> MyInventory
        {
            get
            {
                List<Thing> retVal = this.Inventory.Shallow;

                foreach (Thing thing in retVal)
                {
                    if (!thing.IsObservable)
                        retVal.Remove(thing);
                }

                return retVal;
            }
        }

        [JsonIgnore]
        /// <summary>
        /// The immediate inventory of this Thing. 
        /// </summary>
        public List<Thing> ShallowInventory
        {
            get { return this.Inventory.Shallow; }
        }

        [JsonIgnore]
        /// <summary>
        /// All the inventory and sub-inventory of this item. 
        /// </summary>
        public List<Thing> DeepInventory
        {
            get
            {
                List<Thing> things = new List<Thing>();

                things.AddRange(this.Inventory.Shallow);

                foreach (Thing thing in this.Inventory.Shallow)
                    things.AddRange(thing.DeepInventory);

                return things;
            }
        }

        private void Add(string location, Thing thing)
        {
            if (this.Inventory[location] == null)
                this.Inventory.Add(location);

            this.Inventory[location].Add(thing);
        }

        protected void Add(string location)
        {
            if (this.Inventory[location] == null)
                this.Inventory.Add(location);
        }

        protected void Remove(Thing thing)
        {
            this.Inventory.Remove(thing);
        }

        protected void Remove(Thing thing, string location)
        {
            if (this[location] == null)
                throw new Exception("Bad location");

            if (!(this[location].Contains(thing)))
                return;

            this[location].Remove(thing);
        }

        public virtual bool WillAccept(Thing thing, string location)
        {
            return true;
        }

        public virtual bool WillRelease(Thing thing)
        {
            return true;
        }

        public virtual bool WillRelease(Thing thing, string location)
        {
            return true;
        }

        public virtual bool Release(Thing thing)
        {
            this.Remove(thing);

            OnReleased(thing);
            return true;
        }

        public virtual void Accept(Thing thing, string location)
        {
            thing.Container = this;
            thing.Locations = new List<string>();
            thing.Locations.Add(location);

            this[location].Add(thing);

            OnAccepted(thing);
        }

        protected virtual bool WillAllowAccept(Thing destination, string location)
        {
            return true;
        }

        protected virtual bool WillAllowRelease(Thing destination, string location)
        {
            return true;
        }

        public virtual bool Move(Thing destination)
        {
            return Move(destination, destination.DefaultKey);
        }

        public virtual bool Move(Thing destination, string location)
        {
            if (!destination.Inventory.ContainsKey(location))
            {
                destination.Inventory.Add(location);
            }

            if (destination[location] == null)
                destination.Inventory.Add(location);

            //Chain of Responsibility...
            if (!WillAllowRelease(destination, location))
                return false;

            if (this.Container != null && !this.Container.WillRelease(this))
                return false;

            if (!destination.WillAccept(this, location))
                return false;

            if (!WillAllowAccept(destination, location))
                return false;

            //End of Chain

            OnMoving(destination);

            if (this.Container != null)
                this.Container.Release(this);

            destination.Accept(this, location);

            OnMoved(destination);

            return true;
        }

        public virtual string GetDescription(Thing observer)
        {
            return base.Description + "\r\n " + Inventory.GetDescription(observer);
        }

        public Thing Clone()
        {
            Thing thing = new Thing();
            thing.Name = this.Name;
            thing.Inventory = this.Inventory.Clone();
            return thing;
        }

        public string Serialize()
        {
            string serializedThing = JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.All
            });

            return serializedThing;
        }
    }
}