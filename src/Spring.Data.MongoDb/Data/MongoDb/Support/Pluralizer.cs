﻿#region License
﻿/* ********************************************************************************** 
 * 
 * Copyright (c) Microsoft Corporation. All rights reserved. 
 * 
 * This source code is subject to terms and conditions of the Microsoft Permissive 
 * License (MS-PL). A copy of the license can be found in the license.htm file 
 * included in this distribution. 
 * 
 * You must not remove this notice, or any other, from this software. 
 * 
 * **********************************************************************************/

/*
 * Note: As provided by dmitryr (msft) on 11 Jan, 2007 on:
 * http://blogs.msdn.com/b/dmitryr/
 */
#endregion

using System;
using System.Collections.Generic;
using System.Text;

namespace Spring.Data.MongoDb.Support
{
    /// <summary>
    /// Helper class to convert a singluar word into plural
    /// </summary>
    public static class Pluralizer
    {
        #region public APIs

        public static string ToPlural(string noun)
        {
            return AdjustCase(ToPluralInternal(noun), noun);
        }

        public static string ToPluralWithUncapitalize(string noun)
        {
            return Uncapitalize(AdjustCase(ToPluralInternal(noun), noun));
        }

        public static string ToSingular(string noun)
        {
            return AdjustCase(ToSingularInternal(noun), noun);
        }

        public static bool IsNounPluralOfNoun(string plural, string singular)
        {
            return String.Compare(ToSingularInternal(plural), singular, StringComparison.OrdinalIgnoreCase) == 0;
        }

        /// <summary>
        /// Capitalize a <code>String</code>, changing the first letter to
        /// upper case as per <see cref="char.ToUpper(char)"/>.
        /// No other letters are changed.
        /// </summary>
        /// <param name="str">the String to capitalize, may be <code>null</code></param>
        /// <returns>
        /// the capitalized String, <code>null</code> if null
        /// </returns>
        public static string Capitalize(string str)
        {
            return ChangeFirstCharacterCase(str, true);
        }


        /// <summary>
        /// Uncapitalize a <code>String</code>, changing the first letter to
        /// lower case as per <see cref="char.ToLower(char)"/>.
        /// No other letters are changed.
        /// </summary>
        /// <param name="str">the String to uncapitalize, may be <code>null</code></param>
        /// <returns>
        /// the uncapitalized String, <code>null</code> if null
        /// </returns>
        public static string Uncapitalize(string str)
        {
            return ChangeFirstCharacterCase(str, false);
        }

        private static string ChangeFirstCharacterCase(string str, bool capitalize)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            var sb = new StringBuilder(str.Length);
            sb.Append(capitalize ? char.ToUpper(str[0]) : char.ToLower(str[0]));
            sb.Append(str.Substring(1));

            return sb.ToString();
        }

        #endregion

        #region Special Words Table

        private static string[] _specialWordsStringTable = new string[]
            {
                "agendum", "agenda", "",
                "albino", "albinos", "",
                "alga", "algae", "",
                "alumna", "alumnae", "",
                "apex", "apices", "apexes",
                "archipelago", "archipelagos", "",
                "bacterium", "bacteria", "",
                "beef", "beefs", "beeves",
                "bison", "", "",
                "brother", "brothers", "brethren",
                "candelabrum", "candelabra", "",
                "carp", "", "",
                "casino", "casinos", "",
                "child", "children", "",
                "chassis", "", "",
                "chinese", "", "",
                "clippers", "", "",
                "cod", "", "",
                "codex", "codices", "",
                "commando", "commandos", "",
                "corps", "", "",
                "cortex", "cortices", "cortexes",
                "cow", "cows", "kine",
                "criterion", "criteria", "",
                "datum", "data", "",
                "debris", "", "",
                "diabetes", "", "",
                "ditto", "dittos", "",
                "djinn", "", "",
                "dynamo", "", "",
                "elk", "", "",
                "embryo", "embryos", "",
                "ephemeris", "ephemeris", "ephemerides",
                "erratum", "errata", "",
                "extremum", "extrema", "",
                "fiasco", "fiascos", "",
                "fish", "fishes", "fish",
                "flounder", "", "",
                "focus", "focuses", "foci",
                "fungus", "fungi", "funguses",
                "gallows", "", "",
                "genie", "genies", "genii",
                "ghetto", "ghettos", "",
                "graffiti", "", "",
                "headquarters", "", "",
                "herpes", "", "",
                "homework", "", "",
                "index", "indices", "indexes",
                "inferno", "infernos", "",
                "japanese", "", "",
                "jumbo", "jumbos", "",
                "latex", "latices", "latexes",
                "lingo", "lingos", "",
                "mackerel", "", "",
                "macro", "macros", "",
                "manifesto", "manifestos", "",
                "measles", "", "",
                "money", "moneys", "monies",
                "mongoose", "mongooses", "mongoose",
                "mumps", "", "",
                "murex", "murecis", "",
                "mythos", "mythos", "mythoi",
                "news", "", "",
                "octopus", "octopuses", "octopodes",
                "ovum", "ova", "",
                "ox", "ox", "oxen",
                "photo", "photos", "",
                "pincers", "", "",
                "pliers", "", "",
                "pro", "pros", "",
                "rabies", "", "",
                "radius", "radiuses", "radii",
                "rhino", "rhinos", "",
                "salmon", "", "",
                "scissors", "", "",
                "series", "", "",
                "shears", "", "",
                "silex", "silices", "",
                "simplex", "simplices", "simplexes",
                "soliloquy", "soliloquies", "soliloquy",
                "species", "", "",
                "stratum", "strata", "",
                "swine", "", "",
                "trout", "", "",
                "tuna", "", "",
                "vertebra", "vertebrae", "",
                "vertex", "vertices", "vertexes",
                "vortex", "vortices", "vortexes",
            };

        #endregion

        #region Suffix Rules Table

        private static string[] _suffixRulesStringTable = new string[]
            {
                "ch", "ches",
                "sh", "shes",
                "ss", "sses",

                "ay", "ays",
                "ey", "eys",
                "iy", "iys",
                "oy", "oys",
                "uy", "uys",
                "y", "ies",

                "ao", "aos",
                "eo", "eos",
                "io", "ios",
                "oo", "oos",
                "uo", "uos",
                "o", "oes",

                "cis", "ces",
                "sis", "ses",
                "xis", "xes",

                "louse", "lice",
                "mouse", "mice",

                "zoon", "zoa",

                "man", "men",

                "deer", "deer",
                "fish", "fish",
                "sheep", "sheep",
                "itis", "itis",
                "ois", "ois",
                "pox", "pox",
                "ox", "oxes",

                "foot", "feet",
                "goose", "geese",
                "tooth", "teeth",

                "alf", "alves",
                "elf", "elves",
                "olf", "olves",
                "arf", "arves",
                "leaf", "leaves",
                "nife", "nives",
                "life", "lives",
                "wife", "wives",
            };

        #endregion

        #region Implementation Details

        private class Word
        {
            public readonly string Singular;
            public readonly string Plural;
            public readonly string Plural2;

            public Word(string singular, string plural, string plural2)
            {
                Singular = singular;
                Plural = plural;
                Plural2 = plural2;
            }
        }

        private class SuffixRule
        {
            private string _singularSuffix;
            private string _pluralSuffix;

            public SuffixRule(string singular, string plural)
            {
                _singularSuffix = singular;
                _pluralSuffix = plural;
            }

            public bool TryToPlural(string word, out string plural)
            {
                if (word.EndsWith(_singularSuffix, StringComparison.OrdinalIgnoreCase))
                {
                    plural = word.Substring(0, word.Length - _singularSuffix.Length) + _pluralSuffix;
                    return true;
                }
                else
                {
                    plural = null;
                    return false;
                }
            }

            public bool TryToSingular(string word, out string singular)
            {
                if (word.EndsWith(_pluralSuffix, StringComparison.OrdinalIgnoreCase))
                {
                    singular = word.Substring(0, word.Length - _pluralSuffix.Length) + _singularSuffix;
                    return true;
                }
                else
                {
                    singular = null;
                    return false;
                }
            }
        }

        private static Dictionary<string, Word> _specialSingulars;
        private static Dictionary<string, Word> _specialPlurals;
        private static List<SuffixRule> _suffixRules;

        static Pluralizer()
        {
            // populate lookup tables for special words 
            _specialSingulars = new Dictionary<string, Word>(StringComparer.OrdinalIgnoreCase);
            _specialPlurals = new Dictionary<string, Word>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < _specialWordsStringTable.Length; i += 3)
            {
                string s = _specialWordsStringTable[i];
                string p = _specialWordsStringTable[i + 1];
                string p2 = _specialWordsStringTable[i + 2];

                if (string.IsNullOrEmpty(p))
                {
                    p = s;
                }

                Word w = new Word(s, p, p2);

                _specialSingulars.Add(s, w);
                _specialPlurals.Add(p, w);

                if (!string.IsNullOrEmpty(p2))
                {
                    _specialPlurals.Add(p2, w);
                }
            }

            // populate suffix rules list 
            _suffixRules = new List<SuffixRule>();

            for (int i = 0; i < _suffixRulesStringTable.Length; i += 2)
            {
                string singular = _suffixRulesStringTable[i];
                string plural = _suffixRulesStringTable[i + 1];
                _suffixRules.Add(new SuffixRule(singular, plural));
            }
        }

        private static string ToPluralInternal(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            // lookup special words 
            Word word;

            if (_specialSingulars.TryGetValue(s, out word))
            {
                return word.Plural;
            }

            // apply suffix rules 
            string plural;

            foreach (SuffixRule rule in _suffixRules)
            {
                if (rule.TryToPlural(s, out plural))
                {
                    return plural;
                }
            }

            // apply the default rule 
            return s + "s";
        }

        private static string ToSingularInternal(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            // lookup special words 
            Word word;

            if (_specialPlurals.TryGetValue(s, out word))
            {
                return word.Singular;
            }

            // apply suffix rules 
            string singular;

            foreach (SuffixRule rule in _suffixRules)
            {
                if (rule.TryToSingular(s, out singular))
                {
                    return singular;
                }
            }

            // apply the default rule 
            if (s.EndsWith("s", StringComparison.OrdinalIgnoreCase))
            {
                return s.Substring(0, s.Length - 1);
            }

            return s;
        }

        private static string AdjustCase(string s, string template)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            // determine the type of casing of the template string 
            bool foundUpperOrLower = false;
            bool allLower = true;
            bool allUpper = true;
            bool firstUpper = false;

            for (int i = 0; i < template.Length; i++)
            {
                if (Char.IsUpper(template[i]))
                {
                    if (i == 0) firstUpper = true;
                    allLower = false;
                    foundUpperOrLower = true;
                }
                else if (Char.IsLower(template[i]))
                {
                    allUpper = false;
                    foundUpperOrLower = true;
                }
            }

            // change the case according to template 
            if (foundUpperOrLower)
            {
                if (allLower)
                {
                    s = s.ToLowerInvariant();
                }
                else if (allUpper)
                {
                    s = s.ToUpperInvariant();
                }
                else if (firstUpper)
                {
                    if (!Char.IsUpper(s[0]))
                    {
                        s = s.Substring(0, 1).ToUpperInvariant() + s.Substring(1);
                    }
                }
            }

            return s;
        }

        #endregion
    }
}