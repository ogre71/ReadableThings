using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ogresoft;
using Ogresoft.Parser;
using Newtonsoft.Json;

namespace Ogresoft.Parser.Test
{
    [TestClass]
    public class AdminThingTests
    {
        [TestMethod]
        public void TakeShouldWork()
        {
            var adminThing = new AdminThing("some weirdo");
            var roomThing = new RoomThing();
            adminThing.Move(roomThing);
            var uselessThing = new Thing("useless thing");
            //uselessThing.AddVerb("take", new Thing.AllowUsageAsDirObjDelegate((taker, myself) => { return true; }));
            uselessThing.AddDirectObjectHandler("take", new Func<Thing, Thing, bool>((taker, myself) => { return true; }));
            uselessThing.Move(roomThing);

            Parser repl = new Parser();
            repl.Parse("look", adminThing);

            Assert.IsFalse(adminThing.ShallowInventory.Contains(uselessThing));
            repl.Parse("take thing", adminThing);
            Assert.IsTrue(adminThing.ShallowInventory.Contains(uselessThing));
        }

        [TestMethod]
        public void DropShouldWork()
        {
            var adminThing = new AdminThing("some weirdo");
            var roomThing = new RoomThing();
            adminThing.Move(roomThing);
            var uselessThing = new Thing("useless thing");
            //uselessThing.AddVerb("take", new Thing.AllowUsageAsDirObjDelegate((taker, myself) => { return true; }));
            uselessThing.AddDirectObjectHandler("take", new Func<Thing, Thing, bool>((taker, myself) => { return true; }));
            uselessThing.AddDirectObjectHandler("drop", new Func<Thing, Thing, bool>((dropper, myself) =>
            {
                if(!dropper.ShallowInventory.Contains(myself))
                {
                    dropper.Tell(Messages.PersonalAction(dropper, "You aren't holding {d1}", dropper, myself));
                    return false; 
                }

                return true; 
            }));

            uselessThing.Move(roomThing);

            Parser repl = new Parser();
            repl.Parse("look", adminThing);

            Assert.IsFalse(adminThing.ShallowInventory.Contains(uselessThing));
            repl.Parse("take thing", adminThing);
            Assert.IsTrue(adminThing.ShallowInventory.Contains(uselessThing));

            repl.Parse("drop thing", adminThing);
            Assert.IsFalse(adminThing.ShallowInventory.Contains(uselessThing));

            Assert.IsTrue(roomThing.ShallowInventory.Contains(uselessThing)); 
        }

        [TestMethod]
        public void GoShouldWork()
        {
            var adminThing = new AdminThing("some weirdo");
            adminThing.Unique = true; 

            var room1 = new RoomThing("room one");
            var room2 = new RoomThing("room two");
            adminThing.Move(room1);

            var exit = new Exit("strange portal", room2);
            exit.Unique = true;

            exit.Move(room1);

            Assert.IsTrue(adminThing.Container == room1);

            Parser repl = new Parser();
            repl.Parse("l", adminThing);
            repl.Parse("go portal", adminThing);

            Assert.IsTrue(adminThing.Container == room2); 
        }

        [TestMethod]
        public void GoShouldWork2()
        {
            //var adminThing = new AdminThing("some weirdo");
            //adminThing.Unique = true;

            //var room1 = new RoomThing("room one");
            //var room2 = new RoomThing("room two");
            //adminThing.Move(room1);

            //var exit = new Exit("strange portal", room2);
            //exit.Unique = true;

            //exit.Move(room1);

            //Assert.IsTrue(adminThing.Container == room1);

            Parser repl = new Parser();
            repl.Parse("l", repl.AdminThing);
        }


        [TestMethod]
        public void ShouldPutRockOnTable()
        {
            var adminThing = new AdminThing("some weirdo");
            var roomThing = new RoomThing();
            adminThing.Move(roomThing); 
            var tableThing = new AdminThing("ordinary table");
            tableThing.Unique = false;
            var rockThing = new AdminThing("rock");

            Parser repl = new Parser();
            repl.Parse("take rock", repl.AdminThing);
            repl.Parse("put rock on table", repl.AdminThing);
        }

        [TestMethod]
        public void TakeShouldCommunicate()
        {
            var adminThing = new AdminThing("some weirdo");
            var roomThing = new RoomThing();
            adminThing.Move(roomThing);

            Parser repl = new Parser();

            var lastMessage = adminThing.LastMessage; 
            repl.Parse("take ugly dead fish", adminThing);

            Assert.IsTrue(adminThing.LastMessage != lastMessage); 
        }

        [TestMethod]
        public void ShouldSerialize()
        {
            var adminThing = new AdminThing("some weirdo");
            var serializedString = adminThing.Serialize();
            var newThing = JsonConvert.DeserializeObject<AdminThing>(serializedString); 
        }
    }
}
