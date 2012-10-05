using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ogresoft
{
    public delegate void ThingEventHandler(Thing sender, Thing thing); 

    [Serializable]
    public partial class Thing : NameBase
    {


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
                        
                        for(int i = 0; i < things.Count; i++)
                                output[i] = things[i].Name;

                        return output; 
                }

                /// <summary>Converts List<Thing> to string[] by retrieving the DefiniteName from each Thing.</summary>
                public static string[] DefiniteStrings(List<Thing> things)
                {
                        string[] output = new string[things.Count];
                        for(int i =0 ; i < things.Count; i++)
                        {
                                output[i] = things[i].DefiniteName;  
                        }
                        return output; 
                }
                
                /// <summary>Converts List<Thing> to string[] by retrieving the IndefiniteName from each Thing.</summary>
                public static string[] IndefiniteStrings(List<Thing> things)
                {
                                string[] output = new string[things.Count];
                                for(int i =0 ; i < things.Count; i++)
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
            //MethodInfo[] methods = this.GetType().GetMethods(System.Reflection.BindingFlags.Public | BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            //foreach (MethodInfo method in methods)
            //{
            //    object[] attributes = method.GetCustomAttributes(typeof(VerbAttribute), true);
            //    foreach (VerbAttribute attribute in attributes)
            //    {
            //        if (!_verbHash.ContainsKey(attribute.Name))
            //            _verbHash.Add(attribute.Name, new Dictionary<Type, Delegate>());

            //        _verbHash[attribute.Name].Add(attribute.DelegateType, Delegate.CreateDelegate(attribute.DelegateType, this, method));
            //    }

            //    object[] initializerAttributes = method.GetCustomAttributes(typeof(VerbInitializerAttribute), true);
            //    foreach (VerbInitializerAttribute attribute in initializerAttributes)
            //    {
            //        method.Invoke(this, new object[] { });
            //    }
            //}
        }

        public Thing(string name)
            : this()
        {
            this.Name = name;
        }

        public Thing(string name, Guid guid)
        {
            this.guid = guid;
            this.Name = name;

        }

        /// <summary>
        /// Event that is fired before this thing is moved from one container to another. 
        /// </summary>
        public event ThingEventHandler Moving;

        protected void OnMoving(Thing destination)
        {
            if (Moving != null)
                Moving(this, destination);
        }

        /// <summary>
        /// Event that is fired when this thing has moved from one container to another. 
        /// </summary>
        public event ThingEventHandler Moved;

        protected void OnMoved(Thing destination)
        {
            if (Moved != null)
                Moved(this, destination);
        }

        
        /// <summary>
        /// Event that is fired when this thing accepts another thing into its inventory.
        /// </summary>
        public event ThingEventHandler Accepted;
        
        protected void OnAccepted(Thing acceptedThing)
        {
            if (Accepted != null)
                Accepted(this, null);
        }

        /// <summary>
        /// Event that is fired when this thing releases another thing from its inventory. 
        /// </summary>
        public event ThingEventHandler Released;

        protected void OnReleased(Thing releasedThing)
        {
            if (Released != null)
                Released(this, releasedThing);
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

            if (Disposing != null)
                Disposing(this, this);

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

        private string identity;
        public string Identity
        {
            get { return identity; }
            set { identity = value; }
        }

        private Guid guid = new Guid();
        /// <summary>
        /// A guid uniquely identifying this particular instance of Thing. 
        /// </summary>
        public Guid Guid
        {
            get { return guid; }
        }

        [NonSerialized]
        private Thing container;
        [System.Xml.Serialization.XmlIgnore]
        public Thing Container
        {
            get { return container; }
            set
            {
                container = value;

                if (value != null)
                    containerGuid = value.Guid;
            }
        }

        private Guid containerGuid;
        /// <summary>
        /// The Guid of the Thing's Container, so that this thing can be placed in the container after deserialization. 
        /// </summary>
        public Guid ContainerGuid
        {
            get { return containerGuid; }
        }

        private List<string> locations;
        /// <summary>
        /// This is locations where the Thing is contained, not locations that the thing can contain.
        /// </summary>
        public List<string> Locations
        {
            get { return locations; }
            set { locations = value; }
        }

        public Thing GetRootContainer()
        {
            Thing currentContainer = container;

            while (currentContainer.Container != null)
                currentContainer = currentContainer.Container;

            return currentContainer;
        }

        public Thing GetOutermostViewableContainer()
        {
            Thing currentContainer = container;
            while (true)
            {
                if (currentContainer.container == null)
                    break;

                if (currentContainer.locations.Count == 0)
                    break;

                foreach (string location in container.locations)
                {
                    if ((container.Inventory.GetOpacity(location) & Opacity.Out) == Opacity.None)
                        break;
                }

                currentContainer = currentContainer.Container;
            }

            return currentContainer;
        }

        public List<Thing> GetObservers()
        {
            if (container == null)
                return null;

            return container.GetObserversFor(this);
        }

        public virtual void Tell(string message)
        {
        }

        private bool isObservable = true;
        /// <summary>
        /// Indicates that the thing can be seen by normal means. 
        /// </summary>
        public virtual bool IsObservable
        {
            get { return isObservable; }
            set { isObservable = value; }
        }

        private bool isObserver;
        /// <summary>
        /// Indicates that this Thing is interested in receiving messages about the environment.
        /// </summary>
        public virtual bool IsObserver
        {
            get
            {
                if (isObserver == true)
                    return true;

                return false;
            }
            set { isObserver = value; }
        }

        private InventoryHash locationMap = new InventoryHash();
        public InventoryHash Inventory
        {
            get { return locationMap; }
            set { locationMap = value; } 
        }

        protected string defaultKey = "in";
        /// <summary>
        /// The default Inventory key for this Thing. 
        /// </summary>
        public string DefaultKey
        {
            get { return defaultKey; }
        }

        public string[] InventoryLocations
        {
            get { return locationMap.Keys; }
        }

        public List<Thing> this[string index]
        {
            get { return locationMap[index]; }
        }

        //Allows the inventoried things to observe outside of this container. 
        protected Opacity opacity = Opacity.In | Opacity.Out;
        public virtual Opacity Opacity
        {
            get
            {
                if (this.container == null)
                    return opacity & Opacity.In;

                return opacity;
            }
        }

        public bool MatchKey(string testKey)
        {
            for (int i = 0; i < locationMap.Keys.Length; i++)
                if (locationMap.Keys[i].StartsWith(testKey))
                {
                    testKey = locationMap.Keys[i];
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

        /// <summary>
        /// The Inventory that is reachable with the 'my' keyword. 
        /// </summary>
        public virtual List<Thing> MyInventory
        {
            get
            {
                List<Thing> retVal = locationMap.Shallow;

                foreach (Thing thing in retVal)
                {
                    if (!thing.IsObservable)
                        retVal.Remove(thing);
                }

                return retVal;
            }
        }

        /// <summary>
        /// The immediate inventory of this Thing. 
        /// </summary>
        public List<Thing> ShallowInventory
        {
            get { return locationMap.Shallow; }
        }

        /// <summary>
        /// All the inventory and sub-inventory of this item. 
        /// </summary>
        public List<Thing> DeepInventory
        {
            get
            {
                List<Thing> things = new List<Thing>();

                things.AddRange(locationMap.Shallow);

                foreach (Thing thing in locationMap.Shallow)
                    things.AddRange(thing.DeepInventory);

                return things;
            }
        }

        private void Add(string location, Thing thing)
        {
            if (locationMap[location] == null)
                locationMap.Add(location);

            locationMap[location].Add(thing);
        }

        protected void Add(string location)
        {
            if (locationMap[location] == null)
                locationMap.Add(location);
        }

        protected void Remove(Thing thing)
        {
            locationMap.Remove(thing);
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
            if (destination[location] == null)
                destination.Add(location); 

            //Chain of Responsibility...
            if (!WillAllowRelease(destination, location))
                return false;

            if (container != null && !container.WillRelease(this))
                return false;

            if (!destination.WillAccept(this, location))
                return false;

            if (!WillAllowAccept(destination, location))
                return false;

            //End of Chain

            OnMoving(destination);

            if (container != null)
                container.Release(this);

            destination.Accept(this, location);

            OnMoved(destination);

            return true;
        }

        public override string Description
        {
            get { return base.Description; }// + locationMap.Description; }
        }

        public virtual string GetDescription(Thing observer)
        {
            return base.Description + "\n" + Inventory.GetDescription(observer);
        }

        public Thing Clone()
        {
            Thing thing = new Thing();
            thing.Name = this.Name;
            thing.Inventory = this.Inventory.Clone();
            return thing; 
        }
    }   
}


