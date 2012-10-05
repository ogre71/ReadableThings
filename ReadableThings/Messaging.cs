using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Ogresoft
{
    public class Messages
    {

        private class Verb
        {
            public static string ThirdPerson(string verb)
            {
                if (verb.StartsWith("^v"))
                    verb = verb.Substring(2);

                if (verb == "are" || verb == "be")
                    return "is";

                if (verb == "have" || verb == "has")
                    return "has";

                if (NeedsES(verb)) // EndsWithX(verb) || EndsWithO(verb) || EndsWithCH(verb) || EndsWithS(verb) )
                    return verb + "es";

                return verb + "s";
            }

            public static string SecondPerson(string verb)
            {
                if (verb.StartsWith("^v"))
                    verb = verb.Substring(2);

                if (verb == "are" || verb == "be")
                    return "are";

                if (verb == "have" || verb == "has")
                    return "have";

                return verb;
            }

            private static Regex _EndsWithX = new Regex("x$");
            private static Regex _EndsWithO = new Regex("o$");
            private static Regex _EndsWithCH = new Regex("ch$");
            private static Regex _EndsWithS = new Regex("s$");

            private static Regex _NeedsES = new Regex("(x|o|ch|s|sh)$");

            private static bool EndsWithX(string verb)
            {
                if (_EndsWithX.IsMatch(verb))
                    return true;

                return false;
            }

            private static bool EndsWithO(string verb)
            {
                if (_EndsWithO.IsMatch(verb))
                    return true;

                return false;
            }

            private static bool EndsWithCH(string verb)
            {
                if (_EndsWithCH.IsMatch(verb))
                    return true;

                return false;
            }

            private static bool EndsWithS(string verb)
            {
                if (_EndsWithS.IsMatch(verb))
                    return true;

                return false;
            }

            private static bool NeedsES(string verb)
            {
                if (_NeedsES.IsMatch(verb))
                    return true;

                return false;
            }
            private Verb() { }
        }

        public static string Capitalize(string input)
        {
            Debug.Assert(input != null && input != string.Empty);

            string firstChar = input[0].ToString();
            return firstChar.ToUpper() + input.Substring(1);
        }

        public static string Punctuate(string input)
        {
            if (input.EndsWith("!") ||
                input.EndsWith("?") ||
                input.EndsWith("."))

                return input;

            input = input + ".";

            return Capitalize(input);
        }

        public static string Nonsense()
        {
            return "I don't know what you're talking about.";
        }

        private class RegExps
        {
            //^v[0-9]
            public static Regex verbs = new Regex(Regex.Escape("{") + @"v[0-9]\S+\b" + Regex.Escape("}"));
            //^V[0-9]
            public static Regex Verbs = new Regex(Regex.Escape("{") + @"V[0-9]\S+\b" + Regex.Escape("}"));
            //^o[0-9]
            public static Regex objects = new Regex(Regex.Escape("{") + "o[0-9]+" + Regex.Escape("}"));
            //^O[0-9]
            public static Regex Objects = new Regex(Regex.Escape("{") + "O[0-9]+" + Regex.Escape("}"));
            //^it[0-9]
            public static Regex indefiniteObjects = new Regex(Regex.Escape("{") + "it[0-9]+" + Regex.Escape("}"));
            //^It[0-9]
            public static Regex IndefiniteObjects = new Regex(Regex.Escape("{") + "It[0-9]+" + Regex.Escape("}"));
            //^od[0-9]
            public static Regex definiteObjects = new Regex(Regex.Escape("{") + "dt[0-9]+" + Regex.Escape("}"));
            //^Od[0-9]
            public static Regex DefiniteObjects = new Regex(Regex.Escape("{") + "Dt[0-9]+" + Regex.Escape("}"));
            //^h[0-9]
            public static Regex nominativePronouns = new Regex(Regex.Escape("{") + "h[0-9]+" + Regex.Escape("}"));
            //^H[0-9]
            public static Regex NominativePronouns = new Regex(Regex.Escape("{") + "H[0-9]+" + Regex.Escape("}"));
            //^d[0-9]
            public static Regex dativePronouns = new Regex(Regex.Escape("{") + "d[0-9]+" + Regex.Escape("}"));
            //^D[0-9]
            public static Regex DativePronouns = new Regex(Regex.Escape("{") + "D[0-9]+" + Regex.Escape("}"));
            //^r[0-9]
            public static Regex reflexivePronouns = new Regex(Regex.Escape("{") + "r[0-9]+" + Regex.Escape("}"));
            //^R[0-9]
            public static Regex ReflexivePronouns = new Regex(Regex.Escape("{") + "R[0-9]+" + Regex.Escape("}"));
            //^p[0-9]
            public static Regex possessivePronouns = new Regex(Regex.Escape("{") + "p[0-9]+" + Regex.Escape("}"));
            //^P[0-9]
            public static Regex PossessivePronouns = new Regex(Regex.Escape("{") + "P[0-9]+" + Regex.Escape("}"));
            //^s[0-9]
            public static Regex possessiveNouns = new Regex(Regex.Escape("{") + "s[0-9]+" + Regex.Escape("}"));
            //^S[0-9]
            public static Regex PossessiveNouns = new Regex(Regex.Escape("{") + "S[0-9]+" + Regex.Escape("}"));
            //^si[0-9] a pointy stick's 
            public static Regex possessiveIndefiniteNouns = new Regex(Regex.Escape("{") + "si[0-9]+" + Regex.Escape("}"));
            //^Si[0-9] A pointy stick's 
            public static Regex PossessiveIndefiniteNouns = new Regex(Regex.Escape("{") + "Si[0-9]+" + Regex.Escape("}"));
            //^sd[0-9] the pointy stick's 
            public static Regex possessiveDefiniteNouns = new Regex(Regex.Escape("{") + "sd[0-9]+" + Regex.Escape("}"));
            //^Sd[0-9] The pointy stick's 
            public static Regex PossessiveDefiniteNouns = new Regex(Regex.Escape("{") + "Sd[0-9]+" + Regex.Escape("}"));
        }

        public static List<string> Actions(string actionString, List<Thing> things, List<Thing> observers)
        {
            List<string> output = new List<string>();

            foreach (Thing thing in observers)
            {
                string str = PersonalAction(thing, actionString, things);
                output.Add(str);
            }

            return output;
        }

        public static string PersonalAction(Thing thing, string actionString, params Thing[] things)
        {
            return PersonalAction(thing, actionString, things.ToList<Thing>());
        }

        public static string PersonalAction(Thing thing, string actionString, List<Thing> things)
        {
            //Debug.WriteLineIf(MessagesSwitch.Verbose, "PersonalAction(Thing thing = '" + thing + "', string actionString = '" + actionString + "', List<Thing> things = '" + things + "')", "Messages");

            //^vn
            MatchCollection matches = RegExps.verbs.Matches(actionString);
            foreach (Match match in matches)
            {
                string targetnum = match.Value.Substring(2);
                string verb = targetnum.Substring(2, match.Value.Length - 5);

                targetnum = targetnum.Substring(0, 1);

                int i = System.Int32.Parse(targetnum);

                if (things[i] == thing)
                    verb = Verb.SecondPerson(verb);
                else
                    verb = Verb.ThirdPerson(verb);

                actionString = actionString.Replace(match.Value, verb);
            }

            //^Vn
            matches = RegExps.Verbs.Matches(actionString);
            foreach (Match match in matches)
            {
                string targetnum = match.Value.Substring(2);
                string verb = targetnum.Substring(2, match.Value.Length - 5);

                targetnum = targetnum.Substring(0, 1);

                int i = System.Int32.Parse(targetnum);

                if (things[i] == thing)
                    verb = Verb.SecondPerson(verb);
                else
                    verb = Verb.ThirdPerson(verb);

                actionString = actionString.Replace(match.Value, Capitalize(verb));
            }

            //^hn
            matches = RegExps.nominativePronouns.Matches(actionString);
            foreach (Match match in matches)
            {
                int i = System.Int32.Parse(match.Value.Substring(2));

                if (things[i] == thing)
                {
                    actionString = actionString.Replace(match.Value, "your");
                    continue;
                }

                actionString = actionString.Replace(match.Value, things[i].NominativePronoun);
            }

            //^Hn
            matches = RegExps.NominativePronouns.Matches(actionString);
            foreach (Match match in matches)
            {
                int i = System.Int32.Parse(match.Value.Substring(2));

                if (things[i] == thing)
                {
                    actionString = actionString.Replace(match.Value, "Your");
                    continue;
                }

                actionString = actionString.Replace(match.Value, Capitalize(things[i].NominativePronoun));
            }

            //^pn
            matches = RegExps.possessivePronouns.Matches(actionString);
            foreach (Match match in matches)
            {
                int i = System.Int32.Parse(match.Value.Substring(2, match.Value.Length - 3));

                if (things[i] == thing)
                {
                    actionString = actionString.Replace(match.Value, "your");
                    continue;
                }

                actionString = actionString.Replace(match.Value, things[i].PossessivePronoun);
            }

            //^Pn
            matches = RegExps.PossessivePronouns.Matches(actionString);
            foreach (Match match in matches)
            {
                int i = System.Int32.Parse(match.Value.Substring(2, match.Value.Length - 3));

                if (things[i] == thing)
                {
                    actionString = actionString.Replace(match.Value, "Your");
                    continue;
                }

                actionString = actionString.Replace(match.Value, Capitalize(things[i].PossessivePronoun));
            }

            //{o<n>}
            matches = RegExps.objects.Matches(actionString);
            foreach (Match match in matches)
            {
                int i = System.Int32.Parse(match.Value.Substring(2, match.Value.Length - 3));

                if (things[i] == thing)
                {
                    actionString = actionString.Replace(match.Value, "you");
                    continue;
                }

                actionString = actionString.Replace(match.Value, things[i].Name);
            }

            //^O
            matches = RegExps.Objects.Matches(actionString);
            foreach (Match match in matches)
            {
                int i = System.Int32.Parse(match.Value.Substring(2, match.Value.Length - 3));

                if (things[i] == thing)
                {
                    actionString = actionString.Replace(match.Value, "You");
                    continue;
                }

                actionString = actionString.Replace(match.Value, Capitalize(things[i].Name));
            }

            //^dt
            matches = RegExps.definiteObjects.Matches(actionString);
            foreach (Match match in matches)
            {
                int i = System.Int32.Parse(match.Value.Substring(3, match.Value.Length - 4));

                if (things[i] == thing)
                {
                    actionString = actionString.Replace(match.Value, "you");
                    continue;
                }

                actionString = actionString.Replace(match.Value, things[i].DefiniteName);
            }

            //^Dt
            matches = RegExps.DefiniteObjects.Matches(actionString);
            foreach (Match match in matches)
            {
                int i = System.Int32.Parse(match.Value.Substring(3, match.Value.Length - 4));

                if (things[i] == thing)
                {
                    actionString = actionString.Replace(match.Value, "You");
                    continue;
                }

                actionString = actionString.Replace(match.Value, Capitalize(things[i].DefiniteName));
            }

            //^it
            matches = RegExps.indefiniteObjects.Matches(actionString);
            foreach (Match match in matches)
            {
                int i = System.Int32.Parse(match.Value.Substring(3, match.Value.Length - 4));

                if (things[i] == thing)
                {
                    actionString = actionString.Replace(match.Value, "you");
                    continue;
                }

                actionString = actionString.Replace(match.Value, things[i].IndefiniteName);
            }

            //^It
            matches = RegExps.IndefiniteObjects.Matches(actionString);
            foreach (Match match in matches)
            {
                int i = System.Int32.Parse(match.Value.Substring(3, match.Value.Length - 4));

                if (things[i] == thing)
                {
                    actionString = actionString.Replace(match.Value, "You");
                    continue;
                }

                actionString = actionString.Replace(match.Value, Capitalize(things[i].IndefiniteName));
            }

            //^dn
            matches = RegExps.dativePronouns.Matches(actionString);
            foreach (Match match in matches)
            {
                int i = System.Int32.Parse(match.Value.Substring(2));

                if (things[i] == thing)
                {
                    actionString = actionString.Replace(match.Value, "you");
                    continue;
                }

                actionString = actionString.Replace(match.Value, things[i].DativePronoun);
            }

            //^Dn
            matches = RegExps.DativePronouns.Matches(actionString);
            foreach (Match match in matches)
            {
                int i = System.Int32.Parse(match.Value.Substring(2));

                if (things[i] == thing)
                {
                    actionString = actionString.Replace(match.Value, "You");
                    continue;
                }

                actionString = actionString.Replace(match.Value, Capitalize(things[i].DativePronoun));
            }

            //^rn
            matches = RegExps.reflexivePronouns.Matches(actionString);
            foreach (Match match in matches)
            {
                int i = System.Int32.Parse(match.Value.Substring(2));

                if (things[i] == thing)
                {
                    actionString = actionString.Replace(match.Value, "yourself");
                    continue;
                }

                actionString = actionString.Replace(match.Value, things[i].ReflexivePronoun);
            }

            //^Rn
            matches = RegExps.ReflexivePronouns.Matches(actionString);
            foreach (Match match in matches)
            {
                int i = System.Int32.Parse(match.Value.Substring(2));

                if (things[i] == thing)
                {
                    actionString = actionString.Replace(match.Value, "Yourself");
                    continue;
                }

                actionString = actionString.Replace(match.Value, Capitalize(things[i].ReflexivePronoun));
            }

            //^sn
            matches = RegExps.possessiveNouns.Matches(actionString);
            foreach (Match match in matches)
            {
                int i = System.Int32.Parse(match.Value.Substring(2));

                if (things[i] == thing)
                {
                    actionString = actionString.Replace(match.Value, "your");
                    continue;
                }

                actionString = actionString.Replace(match.Value, things[i].PossessiveName);
            }

            //^Sn
            matches = RegExps.PossessiveNouns.Matches(actionString);
            foreach (Match match in matches)
            {
                int i = System.Int32.Parse(match.Value.Substring(2));

                if (things[i] == thing)
                {
                    actionString = actionString.Replace(match.Value, "Your");
                    continue;
                }

                actionString = actionString.Replace(match.Value, Capitalize(things[i].PossessiveName));
            }

            //^sin
            matches = RegExps.possessiveIndefiniteNouns.Matches(actionString);
            foreach (Match match in matches)
            {
                int i = System.Int32.Parse(match.Value.Substring(3));

                if (things[i] == thing)
                {
                    actionString = actionString.Replace(match.Value, "your");
                    continue;
                }

                actionString = actionString.Replace(match.Value, things[i].PossessiveIndefiniteName);
            }

            //^Sin
            matches = RegExps.PossessiveIndefiniteNouns.Matches(actionString);
            foreach (Match match in matches)
            {
                int i = System.Int32.Parse(match.Value.Substring(3));

                if (things[i] == thing)
                {
                    actionString = actionString.Replace(match.Value, "Your");
                    continue;
                }

                actionString = actionString.Replace(match.Value, Capitalize(things[i].PossessiveIndefiniteName));
            }

            //^sdn
            matches = RegExps.possessiveDefiniteNouns.Matches(actionString);
            foreach (Match match in matches)
            {
                int i = System.Int32.Parse(match.Value.Substring(3));

                if (things[i] == thing)
                    actionString = actionString.Replace(match.Value, "your");
                else
                    actionString = actionString.Replace(match.Value, things[i].PossessiveDefiniteName);
            }

            //^Sdn
            matches = RegExps.PossessiveDefiniteNouns.Matches(actionString);
            foreach (Match match in matches)
            {
                int i = System.Int32.Parse(match.Value.Substring(3));

                if (things[i] == thing)
                    actionString = actionString.Replace(match.Value, "Your");
                else
                    actionString = actionString.Replace(match.Value, Capitalize(things[i].PossessiveDefiniteName));
            }

            actionString = Capitalize(actionString);
            return actionString;
        }

        public static void MyAction(Thing doer, string actionString)
        {
            //Debug.WriteLineIf(MessagesSwitch.Verbose, "MyAction(Thing doer = '" + doer + "', string actionString = '" + actionString + "')", "Messages"); 
            List<Thing> things = new List<Thing>();
            things.Add(doer);

            doer.Tell(PersonalAction(doer, actionString, things));
        }

        public static void MyAction(Thing doer, string actionString, params Thing[] things)
        {
            doer.Tell(PersonalAction(doer, actionString, things.ToList<Thing>()));
        }

        public static void MyAction(Thing doer, string actionString, List<Thing> targets)
        {
            //Debug.WriteLineIf(MessagesSwitch.Verbose, "MyAction(Thing doer = '" + doer + "', string actionString = '" + actionString + "', List<Thing> targets = '" + targets + "')", "Messages"); 

            doer.Tell(PersonalAction(doer, actionString, targets));
        }

        /*
        public static void ExecuteMyVerbalAction(Thing doer, string actionString, string garbleString, params Thing[] Things)
        {
            Action action = new Action(doer, actionString, garbleString, Things); 
            action.ExecuteFirst(); 
        }*/

        /// <summary>
        /// Broadcasts an action to all observers except the doer of the action. 
        /// </summary>
        public static void OtherAction(Thing doer, string actionString)
        {
            List<Thing> things = new List<Thing>();
            things.Add(doer);

            List<Thing> listeners = doer.Container.GetObserversFor(doer);
            string message = PersonalAction(null, actionString, things);

            foreach (Thing thing in listeners)
                thing.Tell(message);
        }

        public static void OtherAction(Thing doer, string actionString, List<Thing> targets)
        {
            //Debug.WriteLineIf(MessagesSwitch.Verbose, "OtherAction(Thing doer = '" + doer + "', string actionString = '" + actionString + "', List<Thing> targets = '" + targets + "')", "Messages"); 

            List<Thing> listeners = doer.GetObservers();
            string message = PersonalAction(null, actionString, targets);

            foreach (Thing thing in listeners)
                if (targets.Contains(thing))
                    thing.Tell(PersonalAction(thing, actionString, targets));
                else
                    thing.Tell(message);
        }

        /*
        public static void ExecuteOtherVerbalAction(Thing doer, string actionString, string garbleString, params Thing[] Things)
        {
            Action action = new Action(doer, actionString, garbleString, Things);
            action.ExecuteOthers();
        }
        */

        /*
        public static void ExecuteVerbalAction(Thing doer, string actionString, string garbleString, params Thing[] Things)
        {
            Action action = new Action(doer, actionString, garbleString, Things);
            action.Execute(); 
        }
        */

        public static void Action(string actionString, params Thing[] targets)
        {
            List<Thing> things = new List<Thing>();
            things.AddRange(targets);

            Action(actionString, things);
        }

        public static void Action(string actionString, List<Thing> targets)
        {
            //Debug.WriteLineIf(MessagesSwitch.Verbose, "Action(string actionString = '" + actionString + "', List<Thing> targets = '" + targets + "')", "Messages");  

            List<Thing> listeners = targets[0].GetObservers();
            string message = PersonalAction(null, actionString, targets);

            if (listeners != null)
                foreach (Thing thing in listeners)
                    if (targets.Contains(thing))
                        thing.Tell(PersonalAction(thing, actionString, targets));
                    else
                        thing.Tell(message);

            targets[0].Tell(PersonalAction(targets[0], actionString, targets));
        }
    }
}