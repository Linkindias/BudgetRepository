using System;
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
        public void OneDay()
        {
            var repo = NSubstitute.Substitute.For<IBudgetRepo>();
            repo.GetALL().Returns(new List<Budget>() {
                new Budget() { YearMonth = "202004", Amount = 3000 },
                new Budget() { YearMonth = "202005", Amount = 3100 },
                new Budget() { YearMonth = "202006", Amount = 3000 }
            });
            var budgetService = new BudgetService(repo);
            var dateTime = DateTime.Now;
            var queryResult = budgetService.Query(dateTime, dateTime);

            Assert.AreEqual(queryResult, 100);
        }

        [TestMethod]
        public void OneMonth()
        {
            var repo = NSubstitute.Substitute.For<IBudgetRepo>();
            repo.GetALL().Returns(new List<Budget>() {
                new Budget() { YearMonth = "202004", Amount = 3000 },
                new Budget() { YearMonth = "202005", Amount = 3100 },
                new Budget() { YearMonth = "202006", Amount = 3000 }
            });
            var budgetService = new BudgetService(repo);
            DateTime date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var queryResult = budgetService.Query(firstDayOfMonth, lastDayOfMonth);

            Assert.AreEqual(queryResult, 3100);
        }

        [TestMethod]
        public void MultipleDay()
        {
            var repo = NSubstitute.Substitute.For<IBudgetRepo>();
            repo.GetALL().Returns(new List<Budget>() {
                new Budget() { YearMonth = "202004", Amount = 300 },
                new Budget() { YearMonth = "202005", Amount = 3100 },
                new Budget() { YearMonth = "202006", Amount = 30000 },
                new Budget() { YearMonth = "202007", Amount = 31000 }
            });
            var budgetService = new BudgetService(repo);

            DateTime date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 10);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(2);

            var queryResult = budgetService.Query(firstDayOfMonth, lastDayOfMonth);

            Assert.AreEqual(queryResult, 42200);
        }

    }
}
