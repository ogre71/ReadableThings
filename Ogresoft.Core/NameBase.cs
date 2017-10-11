
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ogresoft
{
    public class NameBase
    {
        public static bool GetIDAndAdjs(List<string> words, out string id, out string[] adjs)
        {
            id = null;
            adjs = null;

            if (words.Count < 1)
                return false;

            adjs = new String[words.Count - 1];

            for (int i = 0; i < words.Count - 1; i++)
                adjs[i] = (string)words[i];


            //get id
            id = (string)words[words.Count - 1];

            return true;
        }

        /// <summary>Loads id and adjs with the id and adjs derived from the words.</summary>
        public static bool GetIDAndAdjs(string words, out string id, out string[] adjs)
        {
            string[] words_array = words.Split(' ');

            id = null;
            adjs = null;

            if (words_array.Length < 1)
                return false;

            adjs = new String[words_array.Length - 1];

            for (int i = 0; i < words_array.Length - 1; i++)
                adjs[i] = (string)words_array[i];


            //get id
            id = (string)words_array[words_array.Length - 1];

            return true;
        }

        public static bool CompareStringArrays(string[] test, string[] compareto)
        {
            if (compareto == null)
                return false;

            if (test.Length > compareto.Length)
                return false;

            int lastj = 0;

            for (int i = 0; i < test.Length; i++)
            {
                for (int j = lastj; j < compareto.Length; j++)
                {
                    if (test[i].ToLower() == "" || compareto[j].ToLower().StartsWith(test[i]))
                    {
                        //We have a match.
                        lastj = j;
                        goto match_found;
                    }
                }

                //no match was found.
                return false;
            match_found:
                continue;
            }

            return true;
        }

        /// <summary>Prepends the indefinite article to a string. </summary>
        /// <param name="input">string</param>
        /// <returns>string</returns>
        public static string Indefinite(string input)
        {
            if (input == "")
                return "";
            if ("AaEeIiOoUuHh".IndexOf(input[0]) == -1)
                return "a " + input;
            else
                return "an " + input;
        }

        /// <summary>Attaches the definite article to the front of a string.</summary>
        /// <param name="input">string</param>
        /// <returns>string</returns>
        public static string Definite(string input)
        {
            return "the " + input;
        }

        public NameBase() { }

        /// <summary>Used to determine if "some" should be used instead of the indefinite article "a/an"</summary>
        /// <remarks>Fred eats some food as opposed to fred eats a food.</remarks>
        private bool aggregate = false;
        public bool Aggregate
        {
            get { return aggregate; }
            set { aggregate = value; }
        }

        private bool hasIrregularPlural = false;
        /// <summary>
        /// Indicates that the Thing has an irregular plural. 
        /// </summary>
        public bool HasIrregularPlural
        {
            get { return hasIrregularPlural; }
            set { hasIrregularPlural = value; }
        }

        /// <summary>Used to determine if the indefinite article is appropriate.</summary>
        /// <remarks>Fred and a dead orc. Fred is unique, the dead orc is not.</remarks>
        private bool unique = false;
        public virtual bool Unique
        {
            get { return unique; }
            set { unique = value; }
        }

        private string[] adjs;
        public string[] Adjs
        {
            get { return adjs; }
            set { adjs = value; }
        }

        private string[] postAdjs;
        /// <summary>
        /// Modifiers that appear after the base noun. 
        /// </summary>
        public string[] PostAdjs
        {
            get { return postAdjs; }
            set { postAdjs = value; }
        }

        private string pluralBaseName = "";
        /// <summary>
        /// The noun that will be used to describe the Thing in the plural. 
        /// </summary>
        public string PluralBaseName
        {
            get { return pluralBaseName; }
            set { pluralBaseName = value; }
        }

        private string baseName = "";
        /// <summary>
        /// The base noun that matches this Thing. 
        /// </summary>
        public string BaseName
        {
            get { return baseName; }
            set { baseName = value; }
        }

        private string description;
        /// <summary>
        /// A description of this Thing. 
        /// </summary>
        public virtual string Description
        {
            get
            {
                if (description != null)
                    return description;

                return "It looks just like every other " + this.Name + " that you've ever seen.";
            }
            set { description = value; }
        }

        /// <summary>Enumerated value representing gender.</summary>
        private Gender gender = Gender.neuter;
        public Gender Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        /// <summary>Gets or sets the full name of the object, including adjectives and id.</summary>
        public virtual string Name
        {
            get
            {
                string output = "";

                if (Adjs != null)
                    output += String.Join(" ", Adjs);

                output += " " + BaseName;

                if (PostAdjs != null)
                    output += " " + String.Join(" ", PostAdjs);

                output = output.Trim();
                return output;
            }

            set
            {
                if (value == null)
                    return;

                //red hot pants
                string[] input = value.Split(' ');
                int length = input.Length;

                BaseName = input[length - 1];

                if (input.Length < 2)
                    return;
                Adjs = new string[input.Length - 1];

                for (int i = 0; i < length - 1; i++)
                    Adjs[i] = input[i];

            }
        }

        public bool Matches(List<string> words)
        {
            string[] strings = new string[words.Count];
            words.CopyTo(strings, 0);

            return Matches(strings);
        }

        public bool Matches(string[] words)
        {
            for (int i = 0; i < words.Length; i++)
            {
                if (!baseName.ToLower().StartsWith(words[i].ToLower()))
                    continue;

                if (i != 0)
                {
                    string[] adjs = new string[i];

                    for (int j = 0; j < i; j++)
                        adjs[j] = words[j];

                    if (!CompareStringArrays(adjs, this.adjs))
                        continue;
                }

                if (i != words.Length - 1)
                {
                    string[] post_adjs = new string[words.Length - i - 1];

                    for (int j = i + 1; j < words.Length; j++)
                        post_adjs[j - 1 - i] = words[j];

                    if (!CompareStringArrays(post_adjs, postAdjs))
                        continue;
                }

                return true;
            }

            return false;
        }

        public bool Matches(string name)
        {
            string[] words = name.Split(' ');

            return Matches(words);
        }

        /// <summary>Retrieves a string representation of the gender.</summary>
        /// <returns>"male", "female", or "neuter"</returns>
        public string GenderToString()
        {
            return System.Enum.GetName(typeof(Gender), Gender);
        }

        /// <summary>
        /// Retrieves the name without the PostAdj modifiers. 
        /// </summary>
        public virtual string NameNoPost
        {
            get
            {
                string output = "";
                if (Adjs != null)
                    output += String.Join(" ", Adjs);
                output += " " + BaseName;
                output = output.Trim();
                return output;
            }
        }

        /// <summary>
        /// The possessive name of the Thing. 
        /// </summary>
        public string PossessiveName
        {
            get
            {
                string name = Name;
                if (name.EndsWith("s"))
                    return name + "'";

                return Name + "'s";
            }
        }

        /// <summary>
        /// The possessive name associated with the definite article. 
        /// </summary>
        public string PossessiveDefiniteName
        {
            get
            {
                string name = this.DefiniteName;
                if (name.EndsWith("s"))
                    return name + "'";

                return name + "'s";
            }
        }

        /// <summary>
        /// The possessive name associated with the indefinite article 
        /// </summary>
        public string PossessiveIndefiniteName
        {
            get
            {
                string name = this.IndefiniteName;
                if (name.EndsWith("s"))
                    return name + "'";

                return name + "'s";
            }
        }

        /// <summary>Prepends the indefinite article to the name. </summary>
        public virtual string IndefiniteName
        {
            get
            {
                if (Unique)
                    return Name;
                if (Aggregate)
                    return "some " + Name;

                return Indefinite(Name);
            }
        }

        /// <summary>Prepends the definite article to the name.</summary>
        public virtual string DefiniteName
        {
            get
            {
                return Definite(Name);
            }
        }

        /// <summary>
        /// The possessive pronoun for this Thing. 
        /// </summary>
        public string PossessivePronoun
        {
            get
            {
                if (this.Gender == Gender.neuter)
                    return "its";

                if (this.Gender == Gender.female)
                    return "her";

                if (this.Gender == Gender.male)
                    return "his";

                return "her";
            }
        }

        /// <summary>
        /// The pronoun in the dative case(indirect object). 
        /// </summary>
        public string DativePronoun
        {
            get
            {
                if (this.Gender == Gender.neuter)
                    return "it";

                if (this.Gender == Gender.female)
                    return "her";

                if (this.Gender == Gender.male)
                    return "him";

                return "her";
            }
        }

        /// <summary>
        /// The pronoun in the nominative case (subject). 
        /// </summary>
        public string NominativePronoun
        {
            get
            {
                if (this.Gender == Gender.neuter)
                    return "it";

                if (this.Gender == Gender.female)
                    return "she";

                if (this.Gender == Gender.male)
                    return "he";

                return "she";
            }
        }

        /// <summary>
        /// The reflexive pronoun appropriate for use with this thing. 
        /// </summary>
        public string ReflexivePronoun
        {
            get
            {
                if (this.Gender == Gender.neuter)
                    return "itself";

                if (this.Gender == Gender.female)
                    return "herself";

                if (this.Gender == Gender.male)
                    return "himself";

                return "herself";
            }
        }
    }
}


