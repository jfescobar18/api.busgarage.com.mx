using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utils;
using Xunit;

namespace UnitTests.Utils
{
    public class MailingUtilsTets
    {
        [Theory]
        [InlineData("<h1>Hello World</h1>", "jfescobar18@hotmail.com")]
        [InlineData("<p>Lorem Text</p>", "jfescobar18@hotmail.com")]
        public void Should_Send_Email(string htmlEmail, string to)
        {
            var SendEmail = typeof(MailingUtils).GetMethod("SendEmail", BindingFlags.Static | BindingFlags.NonPublic);
            SendEmail.Invoke(obj: null, parameters: new object[] { htmlEmail, to, "Email Unit Test" });
        }
    }
}
