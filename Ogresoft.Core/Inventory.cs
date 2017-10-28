using System;
using System.Collections.Generic;
using System.Text;

namespace Ogresoft
{
    /// <summary>
    /// Used in the InventoryHash to describe List<Thing>s, like work, held, in the pocked. etc. 
    /// </summary>
    public class Inventory
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

        public Opacity Opacity { get; set; }

        public string Description { get; set; }

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
