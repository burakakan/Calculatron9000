using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculatron9000
{
    class InvalidExpressionException : Exception
    {
        private string _message;
        public InvalidExpressionException(string err)
        {
            _message = err;
        }
        public override string Message => _message;
    }
}
