// <copyright file="PositiveBitCounter.cs" company="Payvision">
// Copyright (c) Payvision. All rights reserved.
// </copyright>

namespace Algorithms.CountingBits
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PositiveBitCounter
    {
        private readonly int CONVERT_BASE = 2;

        public IEnumerable<int> Count(int input)
        {
            
            if (input < 0)
                throw new ArgumentException("Input must be greater than or equal to 0");


            string bitString = ConvertIntToStringBits(input);
            bitString = ReverseBitString(bitString);
            List<int> positionsOfSetBits = GetPositionsOfSetBits(bitString).ToList();
            return InsertLength(positionsOfSetBits);
            
        }

        private string ConvertIntToStringBits(int number)
        {
            return Convert.ToString(number, CONVERT_BASE);
        }

        private string ReverseBitString(string sourceBitString)
        {
             char[] stringToCharArray = sourceBitString.ToCharArray();
             Array.Reverse(stringToCharArray);
             return new string(stringToCharArray);
           
        }

        private IEnumerable<int> GetPositionsOfSetBits(string sourceBitString)
        {
            List<int> positionsSetBits = new List<int>();
            for(int i=0;i < sourceBitString.Length; i++)
            {
                if (sourceBitString[i].Equals('1'))
                    positionsSetBits.Add(i);
            }

            return positionsSetBits;
        }

        private IEnumerable<int> InsertLength(IEnumerable<int> source)
        {
            List<int> sourceList = source.ToList();
            sourceList.Insert(0, sourceList.Count);
            return sourceList;
        }
       

    
    }
}
