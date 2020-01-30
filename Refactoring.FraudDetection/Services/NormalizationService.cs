using Refactoring.FraudDetection.Interfaces;
using System;
using System.Collections.Generic;

namespace Refactoring.FraudDetection.Services
{
    public class NormalizationService : INormalizationService
    {
        private readonly char AT_SYMBOL = '@';
        private readonly Dictionary<string, string> _stateDictionary = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _streetDictionary = new Dictionary<string, string>();

        public NormalizationService()
        {
            _stateDictionary.Add("il", "illinois");
            _stateDictionary.Add("ca", "california");
            _stateDictionary.Add("ny", "new york");
            _streetDictionary.Add("st.", "street");
            _streetDictionary.Add("rd.", "road");
        }

        public string NormalizeEmail(string email)
        {
            var emailSplitted = email.Split(new char[] { AT_SYMBOL }, StringSplitOptions.RemoveEmptyEntries);

            var atIndex = emailSplitted[0].IndexOf("+", StringComparison.Ordinal);

            emailSplitted[0] = atIndex < 0 ? emailSplitted[0].Replace(".", "") : emailSplitted[0].Replace(".", "").Remove(atIndex);

            return string.Join(AT_SYMBOL, new string[] { emailSplitted[0], emailSplitted[1] });
           
        }

        public string NormalizeState(string state)
        {
            if (_stateDictionary.ContainsKey(state))
                return _stateDictionary[state];
            return state;
        }

        public string NormalizeStreet(string street)
        {
            string[] splittedStreet = street.Split(new char[] { ' ' });
            for(int i = 0; i < splittedStreet.Length; i++)
            {
                if (_streetDictionary.ContainsKey(splittedStreet[i].ToLower()))
                    splittedStreet[i] = _streetDictionary[splittedStreet[i]];
               
            }

            return string.Join(' ', splittedStreet);
        }
    }
}
