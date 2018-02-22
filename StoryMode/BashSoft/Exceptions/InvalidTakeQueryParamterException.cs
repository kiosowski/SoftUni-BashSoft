using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Exceptions
{

    public class InvalidTakeQueryParamterException : Exception
    {
        private const string InvalidTakeQueryParamter =
            "The take command expected does not match the format wanted!";

        public InvalidTakeQueryParamterException()
            : base(InvalidTakeQueryParamter)
        {
        }

        public InvalidTakeQueryParamterException(string message)
            : base(message)
        {
        }
    }
}
