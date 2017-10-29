using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Ogresoft.Core.Test
{
    [TestClass]
    public class InventoryHashTests
    {
        [TestMethod]
        public void ShouldSerialize()
        {
            InventoryHash inventoryHash = new InventoryHash();

            var serializedThing = JsonConvert.SerializeObject(inventoryHash, Formatting.Indented, new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.All,
            });

            Assert.IsTrue(!string.IsNullOrEmpty(serializedThing));

            var newInventoryHash = JsonConvert.DeserializeObject<InventoryHash>(serializedThing);

            Assert.IsTrue(newInventoryHash != null);
        }

        [TestMethod]
        public void ShouldSerializeThings()
        {
            InventoryHash inventoryHash = new InventoryHash();
            Thing thing = new Thing("thing");

            inventoryHash.Add("in");
            inventoryHash["in"].Add(thing); 

            var serializedThing = JsonConvert.SerializeObject(inventoryHash, Formatting.Indented, new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.All,
            });

            Assert.IsTrue(!string.IsNullOrEmpty(serializedThing));

            var newInventoryHash = JsonConvert.DeserializeObject<InventoryHash>(serializedThing);

            Assert.IsTrue(newInventoryHash != null);

            Assert.IsTrue(newInventoryHash.ContainsKey("in"));

            Assert.IsTrue(newInventoryHash["in"].Count == 1);

            Assert.IsTrue(newInventoryHash["in"][0].Name == "thing"); 
        }
    }
}
