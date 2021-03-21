using System;

namespace Services
{
    public class LuhnCheckService : ILuhnCheckService
    {
        public bool IsValidCardNumber(string cardNumber)
        {
            var isValid = false;
            var sum = 0;
            var isSecondDigit = false;

            cardNumber = cardNumber.Replace("-", string.Empty).Replace(" ", "");
            
            for (var i = cardNumber.Length - 1; i >= 0; i--)
            {
                var currChar = cardNumber[i];

                if (!Int32.TryParse(currChar.ToString(), out var currNum))
                {
                    break;
                }

                if (isSecondDigit)
                {
                    currNum = currNum * 2;
                    if (currNum > 9)
                    {
                        var n1 = currNum / 10;
                        var n2 = currNum % 10;
                        currNum = n1 + n2;
                    }
                }

                sum += currNum;

                isSecondDigit = !isSecondDigit;
            }

            isValid = sum != 0 && sum % 10 == 0;

            return isValid;
        }
    }

    public interface ILuhnCheckService
    {
        bool IsValidCardNumber(string cardNumber);
    }
}
