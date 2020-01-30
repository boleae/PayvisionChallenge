// <copyright file="FraudRadar.cs" company="Payvision">
// Copyright (c) Payvision. All rights reserved.
// </copyright>

namespace Refactoring.FraudDetection
{
    using Refactoring.FraudDetection.Interfaces;
    using Refactoring.FraudDetection.Models;
    using Refactoring.FraudDetection.Services;
    using System;
    using System.Collections.Generic;
    using System.IO.Abstractions;
    using System.Text.RegularExpressions;
    using System.Linq;

    public class FraudRadar : IFraudRadar
    {
        private readonly IFileSystem _fileSystem;
        private readonly INormalizationService _normalizationService;
        private readonly IDetectFraudService _detectFraudService;
        private readonly string _filePath;
        private readonly int NUMBER_FIELDS = 8;
        private readonly int ZIP_CODE_LENGTH = 5;

        private readonly string MatchEmailPattern =
           @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
    + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
    + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
    + @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$";

        public FraudRadar() : this(new FileSystem(), new NormalizationService(), new DetectFraudService(), string.Empty) { }

        public FraudRadar(IFileSystem fileSystem,
                          INormalizationService normalizationService,
                          IDetectFraudService detectFraudService,
                          string filePath)
        {
            _fileSystem = fileSystem;
            _normalizationService = normalizationService;
            _detectFraudService = detectFraudService;
            _filePath = filePath;
        }



        public IEnumerable<FraudResult> Check()
        {
            // READ FRAUD LINES
            var orders = new List<Order>();
            var fraudResults = new List<FraudResult>();


            var lines = _fileSystem.File.ReadAllLines(_filePath);

            foreach (var line in lines)
            {
                var items = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (ValidateInput(items))
                {
                    var order = new Order
                    {
                        OrderId = int.Parse(items[0]),
                        DealId = int.Parse(items[1]),
                        Email = _normalizationService.NormalizeEmail(items[2].ToLower()),
                        Street = _normalizationService.NormalizeStreet(items[3].ToLower()),
                        City = items[4].ToLower(),
                        State = _normalizationService.NormalizeState(items[5].ToLower()),
                        ZipCode = items[6],
                        CreditCard = items[7]
                    };

                    if (_detectFraudService.IsFraudOrder(orders, order))
                        fraudResults.Add(new FraudResult { IsFraudulent = true, OrderId = order.OrderId });

                    orders.Add(order);
                }
            }

            return fraudResults;
        }

        private bool ValidateInput(string[] dataInput)
        {
            if (dataInput.Length != NUMBER_FIELDS)
                return false;
            if (dataInput.Any(s => string.IsNullOrEmpty(s)))
                return false;
            if (!int.TryParse(dataInput[0], out int resultFirst))
                return false;
            if (!int.TryParse(dataInput[1], out int resultSecond))
                return false;
            if (!Regex.IsMatch(_normalizationService.NormalizeEmail(dataInput[2].ToLower()), MatchEmailPattern))
                return false;
            if (dataInput[6].Length != ZIP_CODE_LENGTH)
                return false;
            return true;

        }
    }
}