using Newtonsoft.Json;
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
        }

        public Dictionary<Inventory, List<Thing>> InnerHash { get; set; } = new Dictionary<Inventory, List<Thing>>(); 


        public void Add(string key)
        {
            Inventory inventory = new Inventory(key);

            this.InnerHash.Add(inventory, new List<Thing>());
        }

        public void Remove(Thing thing)
        {
            foreach (List<Thing> things in this.InnerHash.Values)
                things.Remove(thing);
        }

        public bool ContainsKey(string key)
        {
            return this.InnerHash.Keys.Any(k => k.Name == key); 
        }

        [JsonIgnore]
        public string[] Keys
        {
            get
            {
                string[] output = new string[this.InnerHash.Count];
                int i = 0;

                foreach (Inventory inventory in this.InnerHash.Keys)
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
                List<Inventory> foundKeys = this.InnerHash.Keys.Where(k => k.Name == key).ToList<Inventory>(); 

                if (foundKeys.Count == 0)
                {
                    return null; 
                }

                return this.InnerHash[foundKeys[0]]; 
            }
        }

        [JsonIgnore]
        public List<Thing> Shallow
        {
            get
            {
                List<Thing> output = new List<Thing>();

                foreach (List<Thing> things in this.InnerHash.Values)
                    output.AddRange(things);

                return output;
            }
        }

        public InventoryHash Clone()
        {
            InventoryHash output = new InventoryHash();

            foreach (Inventory inventory in this.InnerHash.Keys)
            {
                output.Add(inventory.Name);
                List<Thing> things = (List<Thing>)this.InnerHash[inventory];

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
            foreach (Inventory inventory in this.InnerHash.Keys)
                if (inventory == name)
                    return inventory.Opacity;

            return Opacity.None;
        }

        [JsonIgnore]
        public string Description
        {
            get
            {
                string output = "";
                foreach (Inventory inventory in this.InnerHash.Keys)
                {
                    if ((inventory.Opacity & Opacity.In) == Opacity.None)
                        continue;

                    if (inventory.Name != "in")
                        continue;

                    List<Thing> things = (List<Thing>)this.InnerHash[inventory];

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
            foreach (Inventory inventory in this.InnerHash.Keys)
            {
                if ((inventory.Opacity & Opacity.In) == Opacity.None)
                    continue;

                if (inventory.Name != "in")
                    continue;

                List<Thing> things = (List<Thing>)this.InnerHash[inventory];

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

        [JsonIgnore]
        public string FullDescription
        {
            get
            {
                string output = "";

                foreach (Inventory inventory in this.InnerHash.Keys)
                {
                    output += inventory.Name + ":\n";

                    List<Thing> things = (List<Thing>)this.InnerHash[inventory];
                    string[] indefiniteStrings = Thing.IndefiniteStrings(things);

                    for (int j = 0; j < indefiniteStrings.Length; j++)
                        output += "     " + indefiniteStrings[j] + "\n";
                }

                return output;
            }
        }
    }
}