using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ExchangeBaseTests
{
    [TestClass]
    public class SQLInjection
    {
        Exchangebase.Com.Bll.SQLInjection si = new Exchangebase.Com.Bll.SQLInjection();
        [TestMethod]
        public void Test_FormatStringForDb()
        {
            string InjectedSQL = si.FormatStringForDb("'");
            //string CommentInjected = si.FormatStringForDb("select * from test where string = 'teststring'--' and id > 0");

            Assert.AreEqual("''", InjectedSQL);
            //Assert.AreEqual("", CommentInjected);

        }

    }
}
