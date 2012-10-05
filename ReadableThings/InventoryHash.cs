using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Ogresoft
{
    /// <summary>
    /// Summary description for InventoryHash.
    /// </summary>
    [Serializable]
    public class InventoryHash
    {
        public InventoryHash()
        {
            innerHash = new Hashtable();
        }

        private Hashtable innerHash;


        public void Add(string key)
        {
            Inventory inventory = new Inventory(key);

            innerHash.Add(inventory, new List<Thing>());
        }

        public void Remove(Thing thing)
        {
            foreach (List<Thing> things in innerHash.Values)
                things.Remove(thing);
        }

        public bool ContainsKey(string key)
        {
            return innerHash.ContainsKey(key);
        }

        public string[] Keys
        {
            get
            {
                string[] output = new string[innerHash.Count];
                int i = 0;

                foreach (Inventory inventory in innerHash.Keys)
                {
                    output[i] = inventory.Name;
                    i++;
                }

                return output;
            }
        }

        public List<Thing> this[string key]
        {
            get
            {
                Inventory inventory = new Inventory(key);

                return innerHash[inventory] as List<Thing>;
            }
        }

        public List<Thing> Shallow
        {
            get
            {
                List<Thing> output = new List<Thing>();

                foreach (List<Thing> things in innerHash.Values)
                    output.AddRange(things);

                return output;
            }
        }

        public InventoryHash Clone()
        {
            InventoryHash output = new InventoryHash();

            foreach (string key in innerHash.Keys)
            {
                output.Add(key);
                List<Thing> things = (List<Thing>)innerHash[key];

                foreach (Thing thing in things)
                {
                    Thing newThing = thing.Clone();
                    output[key].Add(newThing);
                }
            }

            return output;
        }

        public Opacity GetOpacity(string name)
        {
            foreach (Inventory inventory in innerHash.Values)
                if (inventory == name)
                    return inventory.Opacity;

            return Opacity.None;
        }

        public string Description
        {
            get
            {
                string output = "";
                foreach (Inventory inventory in innerHash.Keys)
                {
                    if ((inventory.Opacity & Opacity.In) == Opacity.None)
                        continue;

                    if (inventory.Name != "in")
                        continue;

                    List<Thing> things = (List<Thing>)innerHash[inventory];

                    if (things.Count == 0)
                        continue;

                    output += "You see " + Thing.IndefiniteStringsPrintable(things) + " here.";
                }

                return output;
            }
        }

        public string GetDescription(Thing observer)
        {
            string output = "";
            foreach (Inventory inventory in innerHash.Keys)
            {
                if ((inventory.Opacity & Opacity.In) == Opacity.None)
                    continue;

                if (inventory.Name != "in")
                    continue;

                List<Thing> things = (List<Thing>)innerHash[inventory];

                if (things.Contains(observer))
                {
                    things = Thing.CloneList(things);
                    things.Remove(observer);
                }

                if (things.Count == 0)
                    continue;

                output += "You see " + Thing.IndefiniteStringsPrintable(things) + " here.";
            }

            return output;
        }

        public string FullDescription
        {
            get
            {
                string output = "";

                foreach (Inventory inventory in innerHash.Keys)
                {
                    output += inventory.Name + ":\n";

                    List<Thing> things = (List<Thing>)innerHash[inventory];
                    string[] indefiniteStrings = Thing.IndefiniteStrings(things);

                    for (int j = 0; j < indefiniteStrings.Length; j++)
                        output += "     " + indefiniteStrings[j] + "\n";
                }

                return output;
            }
        }

        /// <summary>
        /// Used in the InventoryHash to describe List<Thing>s, like work, held, in the pocked. etc. 
        /// </summary>
        [Serializable]
        private class Inventory
        {
            public static bool operator ==(Inventory inventory, string name)
            {
                if ((object)inventory == null && (object)name == null)
                    return true;

                if ((object)inventory == null || (object)name == null)
                    return false;

                return inventory.name == name;
            }

            public static bool operator !=(Inventory inventory, string name)
            {
                if ((object)inventory == null && (object)name == null)
                    return false;

                if ((object)inventory == null || (object)name == null)
                    return true;

                return inventory.name != name;
            }

            public Inventory(string name)
            {
                this.name = name;
            }

            private string name;
            public string Name
            {
                get { return name; }
            }

            private Opacity opacity = Opacity.In | Opacity.Out;
            public Opacity Opacity
            {
                get { return opacity; }
                set { opacity = value; }
            }

            private string description;
            public string Description
            {
                get { return description; }
                set { description = value; }
            }

            public override int GetHashCode()
            {
                return name.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                Inventory inventory = obj as Inventory;
                if (inventory == null)
                {
                    string name = obj as string;

                    if (name == null)
                        return false;

                    return name == this.name;
                }

                return inventory.name == this.name;
            }

        }
    }
}