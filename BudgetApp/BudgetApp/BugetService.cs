using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp
{
    public class BudgetService
    {
        private IBudgetRepo _repo;

        public BudgetService(IBudgetRepo repo)
        {
            this._repo = repo;
        }

        public decimal Query(DateTime startDateTime, DateTime endDateTime)
        {
            if (startDateTime > endDateTime)
            {
                return 0;
            }

            var all = this._repo.GetALL()
                                                .Select(x => new BudgetEntity
                                                {
                                                    YeatMonthDateTime = x.YearMonth,
                                                    Amount = x.Amount
                                                });
            BudgetEntity NowMonth = null;

            if (IsOneMonth(startDateTime, endDateTime))
            {
                var now = startDateTime.ToString("yyyyMM");
                NowMonth = all.Where(o => startDateTime.Month == endDateTime.Month && o.YeatMonthDateTime == now).FirstOrDefault();
            }
            else if (IsOneDay(startDateTime, endDateTime))
            {
                var now = startDateTime.ToString("yyyyMM");
                NowMonth = all.Where(o => startDateTime.Month == endDateTime.Month && o.YeatMonthDateTime == now).FirstOrDefault();

                return NowMonth.Amount / DateTime.DaysInMonth(startDateTime.Year, startDateTime.Month);
            }
            else if (IsCrossMonth(startDateTime, endDateTime))
            {
                int firstDay = DateTime.DaysInMonth(startDateTime.Year, startDateTime.Month) - startDateTime.Day + 1;
                var now = startDateTime.ToString("yyyyMM");
                NowMonth = all.Where(o => o.YeatMonthDateTime == now).FirstOrDefault();
                decimal firstAmount = NowMonth.Amount * (firstDay / DateTime.DaysInMonth(startDateTime.Year, startDateTime.Month));

                decimal middleAmount = 0;
                if (endDateTime.Month - startDateTime.Month > 1)
                {
                    var moveMonth = startDateTime;
                    for (int i = startDateTime.Month; i < endDateTime.Month; i++)
                    {
                        moveMonth.AddMonths(+1);
                        var middleMonth = moveMonth.ToString("yyyyMM");
                        var middleBudget = all.Where(o => o.YeatMonthDateTime == middleMonth).FirstOrDefault();
                        middleAmount += middleBudget.Amount;
                    }
                }

                if (endDateTime.Month - startDateTime.Month == 1)
                {
                    var nowNext = endDateTime.ToString("yyyyMM");
                    var NextMonth = all.Where(o => o.YeatMonthDateTime == nowNext).FirstOrDefault();

                    decimal lastAmount = (NextMonth.Amount / DateTime.DaysInMonth(endDateTime.Year, endDateTime.Month)) * endDateTime.Day;
                    return firstAmount + middleAmount + lastAmount;
                }
            }

            return NowMonth.Amount;
        }

        private static bool IsCrossMonth(DateTime startDateTime, DateTime endDateTime)
        {
            return startDateTime.Month < endDateTime.Month || startDateTime.Year != endDateTime.Year;
        }

        private static bool IsOneDay(DateTime startDateTime, DateTime endDateTime)
        {
            return startDateTime.Date == endDateTime.Date;
        }

        private static bool IsOneMonth(DateTime startDateTime, DateTime endDateTime)
        {
            return startDateTime.Month == endDateTime.Month && startDateTime.Day == 1 && endDateTime.Day == DateTime.DaysInMonth(endDateTime.Year, endDateTime.Month);
        }
    }
}
