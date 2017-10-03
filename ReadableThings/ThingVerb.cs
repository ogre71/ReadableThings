using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ogresoft
{
    public partial class Thing
    {
        public delegate bool AllowUseAloneDelegate();
        public delegate bool AllowUsageAsDirObjDelegate(Thing doer, Thing dirObj);
        public delegate bool AllowUsageWithDirObjDelegate(Thing dirObj);
        public delegate bool AllowUsageAsIndObjInAdverbialPhraseDelegate(Thing doer, string preposition);
        public delegate bool AllowUsageWithAdverbialPhraseDelegate(Thing indObj, string preposition);
        public delegate bool AllowUsageWithStrDelegate(string str);
        public delegate bool UseAsIndObjInAdverbialPhraseDelegate(Thing doer, string preposition);
        public delegate bool UseAloneDelegate();
        public delegate bool UseWithDirObjDelegate(Thing dirObj);
        public delegate bool UseAsDirObjDelegate(Thing doer);
        public delegate bool UseWithAdverbialPhraseDelegate(Thing indObj, string preposition);
        public delegate bool UseWithStrDelegate(string str);

        public Dictionary<string, Dictionary<Type, Delegate>> _verbHash = new Dictionary<string, Dictionary<Type, Delegate>>();
        public Dictionary<string, string> _aliases = new Dictionary<string, string>();
        public Dictionary<string, List<string>> _adverbs = new Dictionary<string, List<string>>();

        public void AddVerb(string verbName, Delegate thisDelegate)
        {
            if (!_verbHash.ContainsKey(verbName))
                _verbHash.Add(verbName, new Dictionary<Type, Delegate>());

            _verbHash[verbName].Add(thisDelegate.GetType(), thisDelegate);
        }

        public void AddVerb(string verbName, Func<bool> func)
        {
            if (!_verbHash.ContainsKey(verbName))
                _verbHash.Add(verbName, new Dictionary<Type, Delegate>());

            _verbHash[verbName].Add(typeof(AllowUseAloneDelegate), new AllowUseAloneDelegate(func));
        }

        protected void AddAlias(string alias, string verbName)
        {
            _aliases.Add(alias, verbName);
        }

        protected void AddAdverb(string verbName, string adverb)
        {
            if (!_adverbs.ContainsKey(verbName))
                _adverbs.Add(verbName, new List<string>());

            _adverbs[verbName].Add(adverb);
        }

        public string GetAlias(string alias)
        {
            if (_verbHash.ContainsKey(alias))
                return alias;

            if (_aliases.ContainsKey(alias))
                return _aliases[alias];

            return null;
        }

        public bool AllowUseAlone(string verbName)
        {
            if (!_verbHash.ContainsKey(verbName))
                return false;

            if (!_verbHash[verbName].ContainsKey(typeof(AllowUseAloneDelegate)))
                return false;

            return ((AllowUseAloneDelegate)_verbHash[verbName][typeof(AllowUseAloneDelegate)]).Invoke();
        }

        public bool AllowUsageAsDirObj(string verbName, Thing doer)
        {
            if (!_verbHash.ContainsKey(verbName))
                return false;

            if (!_verbHash[verbName].ContainsKey(typeof(AllowUsageAsDirObjDelegate)))
                return false;

            AllowUsageAsDirObjDelegate thisDelegate = (AllowUsageAsDirObjDelegate)_verbHash[verbName][typeof(AllowUsageAsDirObjDelegate)];
            return thisDelegate.Invoke(doer, this);
        }

        public bool AllowUsageWithAdverbialPhrase(string verbName, Thing indObj, string preposition)
        {
            if (!_verbHash.ContainsKey(verbName))
                return false;

            if (!_verbHash[verbName].ContainsKey(typeof(AllowUsageWithAdverbialPhraseDelegate)))
                return false;

            AllowUsageWithAdverbialPhraseDelegate thisDelegate = (AllowUsageWithAdverbialPhraseDelegate)_verbHash[verbName][typeof(AllowUsageWithAdverbialPhraseDelegate)];
            return thisDelegate.Invoke(indObj, preposition);
        }

        public bool AllowUsageWithDirObj(string verbName, Thing dirObj)
        {
            if (!_verbHash.ContainsKey(verbName))
                return false;

            if (!_verbHash[verbName].ContainsKey(typeof(AllowUsageWithDirObjDelegate)))
                return false;

            return ((AllowUsageWithDirObjDelegate)_verbHash[verbName][typeof(AllowUsageWithDirObjDelegate)]).Invoke(dirObj);
        }

        public bool AllowUsageAsIndObjInAdverbialPhrase(string verbName, Thing doer, string preposition)
        {
            if (!_verbHash.ContainsKey(verbName))
                return false;

            if (!_verbHash[verbName].ContainsKey(typeof(AllowUsageAsIndObjInAdverbialPhraseDelegate)))
                return false;

            return ((AllowUsageAsIndObjInAdverbialPhraseDelegate)_verbHash[verbName][typeof(AllowUsageAsIndObjInAdverbialPhraseDelegate)]).Invoke(doer, preposition);
        }

        public bool AllowUsageWithStr(string verbName, string str)
        {
            if (!_verbHash.ContainsKey(verbName))
                return false;

            if (!_verbHash[verbName].ContainsKey(typeof(AllowUsageWithStrDelegate)))
                return false;

            return ((AllowUsageWithStrDelegate)_verbHash[verbName][typeof(AllowUsageWithStrDelegate)]).Invoke(str);
        }

        public bool UseAsIndObjInAdverbialPhrase(string verbName, Thing doer, string preposition)
        {
            if (!_verbHash.ContainsKey(verbName))
                return false;

            if (!_verbHash[verbName].ContainsKey(typeof(UseAsIndObjInAdverbialPhraseDelegate)))
                return false;

            return ((UseAsIndObjInAdverbialPhraseDelegate)_verbHash[verbName][typeof(UseAsIndObjInAdverbialPhraseDelegate)]).Invoke(doer, preposition);
        }

        public bool UseAlone(string verbName)
        {
            if (!_verbHash.ContainsKey(verbName))
                return false;

            if (!_verbHash[verbName].ContainsKey(typeof(UseAloneDelegate)))
                return false;

            return ((UseAloneDelegate)_verbHash[verbName][typeof(UseAloneDelegate)]).Invoke();
        }

        public bool UseWithDirObj(string verbName, Thing dirObj)
        {
            if (!_verbHash.ContainsKey(verbName))
                return false;

            if (!_verbHash[verbName].ContainsKey(typeof(UseWithDirObjDelegate)))
                return false;

            return ((UseWithDirObjDelegate)_verbHash[verbName][typeof(UseWithDirObjDelegate)]).Invoke(dirObj);
        }

        public bool UseAsDirObj(string verbName, Thing doer)
        {
            if (!_verbHash.ContainsKey(verbName))
                return false;

            if (!_verbHash[verbName].ContainsKey(typeof(UseAsDirObjDelegate)))
                return false;

            return ((UseAsDirObjDelegate)_verbHash[verbName][typeof(UseAsDirObjDelegate)]).Invoke(doer);
        }

        public bool UseWithAdverbialPhrase(string verbName, Thing indObj, string preposition)
        {
            if (!_verbHash.ContainsKey(verbName))
                return false;

            if (!_verbHash[verbName].ContainsKey(typeof(UseWithAdverbialPhraseDelegate)))
                return false;

            return ((UseWithAdverbialPhraseDelegate)_verbHash[verbName][typeof(UseWithAdverbialPhraseDelegate)]).Invoke(indObj, preposition);
        }

        public bool UseWithStr(string verbName, string str)
        {
            if (!_verbHash.ContainsKey(verbName))
                return false;

            if (!_verbHash[verbName].ContainsKey(typeof(UseWithStrDelegate)))
                return false;

            return ((UseWithStrDelegate)_verbHash[verbName][typeof(UseWithStrDelegate)]).Invoke(str);
        }

        public string CompleteVerb(string verbName)
        {
            if (_verbHash.ContainsKey(verbName))
            {
                return verbName; 
            }

            string verb = _verbHash.Keys.Where(k => k.StartsWith(verbName)).FirstOrDefault();
            return verb;
        }
    }
}