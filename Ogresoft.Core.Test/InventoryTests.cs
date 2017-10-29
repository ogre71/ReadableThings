using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Ogresoft.Core.Test
{
    [TestClass]
    public class InventoryTests
    {
        [TestMethod]
        public void ShouldSerialize()
        {
            Inventory inventory = new Inventory("in"); 

            var serializedThing = JsonConvert.SerializeObject(inventory, Formatting.Indented, new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.All,
            });

            Assert.IsTrue(!string.IsNullOrEmpty(serializedThing));

            var newThing = JsonConvert.DeserializeObject<Inventory>(serializedThing);

            Assert.IsTrue(newThing != null); 
        }
    }
}
