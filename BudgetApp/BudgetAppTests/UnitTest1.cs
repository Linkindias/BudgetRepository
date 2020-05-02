﻿using System;
using System.Collections.Generic;
using BudgetApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BudgetAppTests
{
    [TestClass]
    public class UnitTest1
    {
        private Budget budget = new Budget();


        [TestMethod]
        public void OneMonth()
        {
            var repo = NSubstitute.Substitute.For<IBudgetRepo>();
            repo.GetALL().Returns(new List<Budget>() {
                new Budget() { YearMonth = "202004", Amount = 300 },
                new Budget() { YearMonth = "202005", Amount = 3100 },
                new Budget() { YearMonth = "202006", Amount = 30000 }
            });
            var budgetService = new BudgetService(repo);
            var dateTime = DateTime.Now;
            var queryResult = budgetService.Query(dateTime, dateTime);

            Assert.AreEqual(queryResult, 3100);


        }

    }
}
