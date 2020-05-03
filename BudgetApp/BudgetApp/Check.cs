using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp
{
    public class Check
    {
        private List<string> inputLists = new List<string>();

        public string result { get; set; }

        public void Convert(string input)
        {
            inputLists.Add(input);
            result = string.Empty;

            for (int i = 0; i < inputLists.Count; i++)
            {
                result += i == 0 
                                ? inputLists[i].ToUpper() 
                                : $"-{inputLists[i].ToUpper()}";
                for (int j = 0; j < i; j++)
                {
                    result += $"{inputLists[i]}";
                }
            }
        }
    }
}
