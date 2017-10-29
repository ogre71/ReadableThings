namespace Ogresoft
{
    /// <summary>
    /// Used in the InventoryHash to describe List<Thing>s, like work, held, in the pocked. etc. 
    /// </summary>
    public class Inventory
    {
        //public static bool operator ==(Inventory inventory, string name)
        //{
        //    if ((object)inventory == null && (object)name == null)
        //        return true;

        //    if ((object)inventory == null || (object)name == null)
        //        return false;

        //    return inventory.Name == name;
        //}

        //public static bool operator !=(Inventory inventory, string name)
        //{
        //    if ((object)inventory == null && (object)name == null)
        //        return false;

        //    if ((object)inventory == null || (object)name == null)
        //        return true;

        //    return inventory.Name != name;
        //}

        public Inventory(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }

        public Opacity Opacity { get; set; }

        public string Description { get; set; }

        //public override int GetHashCode()
        //{
        //    return Name.GetHashCode();
        //}

        //public override bool Equals(object obj)
        //{
        //    Inventory inventory = obj as Inventory;
        //    if (inventory == null)
        //    {
        //        string name = obj as string;

        //        if (name == null)
        //            return false;

        //        return name == this.name;
        //    }

        //    return inventory.name == this.name;
        //}

        public static explicit operator Inventory(string from)
        {
            return new Inventory(from); 
        }

        public static implicit operator string(Inventory from)
        {
            return from.Name; 
        }

        public override string ToString()
        {
            return this.Name; 
        }
    }
}
