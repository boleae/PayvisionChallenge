// <copyright file="FraudRadarTests.cs" company="Payvision">
// Copyright (c) Payvision. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO.Abstractions.TestingHelpers;
using Refactoring.FraudDetection.Models;
using Refactoring.FraudDetection.Interfaces;
using Refactoring.FraudDetection.Services;

namespace Refactoring.FraudDetection.Tests
{
    [TestClass]
    public class FraudRadarTests
    {
        [TestMethod]
        [DeploymentItem("./Files/OneLineFile.txt", "Files")]
        public void CheckFraud_OneLineFile_NoFraudExpected()
        {
            var result = ExecuteTest(Path.Combine(Environment.CurrentDirectory, "Files", "OneLineFile.txt"));

            result.Should().NotBeNull("The result should not be null.");
            result.Should().HaveCount(0, "The result should not contains fraudulent lines");
        }

        [TestMethod]
        [DeploymentItem("./Files/TwoLines_FraudulentSecond.txt", "Files")]
        public void CheckFraud_TwoLines_SecondLineFraudulent()
        {
            var result = ExecuteTest(Path.Combine(Environment.CurrentDirectory, "Files", "TwoLines_FraudulentSecond.txt"));

            result.Should().NotBeNull("The result should not be null.");
            result.Should().HaveCount(1, "The result should contains the number of lines of the file");
            result.First().IsFraudulent.Should().BeTrue("The first line is not fraudulent");
            result.First().OrderId.Should().Be(2, "The first line is not fraudulent");
        }

        [TestMethod]
        [DeploymentItem("./Files/ThreeLines_FraudulentSecond.txt", "Files")]
        public void CheckFraud_ThreeLines_SecondLineFraudulent()
        {
            var result = ExecuteTest(Path.Combine(Environment.CurrentDirectory, "Files", "ThreeLines_FraudulentSecond.txt"));

            result.Should().NotBeNull("The result should not be null.");
            result.Should().HaveCount(1, "The result should contains the number of lines of the file");
            result.First().IsFraudulent.Should().BeTrue("The first line is not fraudulent");
            result.First().OrderId.Should().Be(2, "The first line is not fraudulent");
        }

        [TestMethod]
        [DeploymentItem("./Files/FourLines_MoreThanOneFraudulent.txt", "Files")]
        public void CheckFraud_FourLines_MoreThanOneFraudulent()
        {
            var result = ExecuteTest(Path.Combine(Environment.CurrentDirectory, "Files", "FourLines_MoreThanOneFraudulent.txt"));

            result.Should().NotBeNull("The result should not be null.");
            result.Should().HaveCount(2, "The result should contains the number of lines of the file");
          
        }

        [TestMethod]
        [DeploymentItem("./Files/EmptyFile.txt", "Files")]
        public void CheckFraud_EmptyFile()
        {
            var result = ExecuteTest(Path.Combine(Environment.CurrentDirectory, "Files", "EmptyFile.txt"));

            result.Should().NotBeNull("The result should not be null.");
            result.Should().HaveCount(0, "The result should contains the number of lines of the file");

        }

        [TestMethod]
        [DeploymentItem("./Files/ThreeLines_SecondBadFormat.txt", "Files")]
        public void CheckFraud_ThreeLines_SecondBadFormat()
        {
            var result = ExecuteTest(Path.Combine(Environment.CurrentDirectory, "Files", "ThreeLines_SecondBadFormat.txt"));

            result.Should().NotBeNull("The result should not be null.");
            result.Should().HaveCount(0, "The result should contains the number of lines of the file");

        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void CheckFraud_FileNoexists_FileNotFoundExceptionExpected()
        {
            ExecuteTest(Path.Combine(Environment.CurrentDirectory, "Files", "NotExistsFile.txt"));
        }

        private static List<FraudResult> ExecuteTest(string filePath)
        {

            MockFileSystem mockFileSystem = InitializeMockFileSystem(filePath);
            INormalizationService normalizationService = new NormalizationService();
            IDetectFraudService detectFraudService = new DetectFraudService();
            IFraudRadar fraudRadar = new FraudRadar(mockFileSystem, normalizationService, detectFraudService, filePath);
            return fraudRadar.Check().ToList();
        }

        private static MockFileSystem InitializeMockFileSystem(string filePath)
        {
            var mockFileSystem = new MockFileSystem();
            var mockInputFile = new MockFileData(File.ReadAllText(filePath));
            mockFileSystem.AddFile(filePath, mockInputFile);
            return mockFileSystem;
        }

       

       
    }
}