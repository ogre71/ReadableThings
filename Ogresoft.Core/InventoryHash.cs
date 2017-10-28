using System.Collections.Generic;
using System.Linq;

namespace Ogresoft
{
    /// <summary>
    /// Summary description for InventoryHash.
    /// </summary>
    public class InventoryHash
    {
        public InventoryHash()
        {
            innerHash = new Dictionary<Inventory, List<Thing>>();
        }

        private Dictionary<Inventory, List<Thing>> innerHash;


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
            return this.innerHash.Keys.Any(k => k.Name == key); 
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

            foreach (Inventory inventory in innerHash.Keys)
            {
                output.Add(inventory.Name);
                List<Thing> things = (List<Thing>)innerHash[inventory];

                foreach (Thing thing in things)
                {
                    Thing newThing = thing.Clone();
                    output[inventory.Name].Add(newThing);
                }
            }

            return output;
        }

        public Opacity GetOpacity(string name)
        {
            foreach (Inventory inventory in innerHash.Keys)
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


    }
}