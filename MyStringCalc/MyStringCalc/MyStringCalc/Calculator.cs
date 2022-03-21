using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyStringCalc
{
    public class Calculator
    {
        public int Add(string numbers)
        {
            var delimiters = new List<string> { "," , "\n" };

            if (numbers.StartsWith("//"))
            {
                var nArray = numbers.Split(new[] { '\n' }, 2);
                if(nArray.Length == 2)
                {
                    delimiters.AddRange(this.GetCustomDelimiters(nArray[0]));
                    numbers = nArray[1];
                }
            }
            
            var splitNumbers = numbers.Split(delimiters.ToArray(), StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            if(splitNumbers.Any(n=>n < 0))
            {
                var negatives = string.Join(",",splitNumbers.Where(n => n < 0));
                throw new Exception("Negatives not allowed: " + negatives);
            }

            splitNumbers.RemoveAll(n => n > 1000);
            return splitNumbers.Sum() ;
        }

        private List<string> GetCustomDelimiters(string delimitersString)
        {
            List<string> delimiters = new List<string>();
            if (delimitersString.Contains("[") && delimitersString.Contains("]"))
            {
                // (?<= l x ) => look after and ommit x
                // (?= x ) => look before and ommit x
                // .* => match any sign till the last ]
                // .*? => match any sign till the first ]
                Regex test = new Regex(@"(?<=\[).*?(?=\])");
                if (test.IsMatch(delimitersString))
                {
                    //there's at least 1 match
                    var res = test.Matches(delimitersString);
                    foreach(Match r in res)
                    {
                        delimiters.Add(r.Value);
                    }
                }
                return delimiters;
            }

            delimiters.Add(delimitersString.Replace("//", string.Empty));

            return delimiters;
        }
    }
}
