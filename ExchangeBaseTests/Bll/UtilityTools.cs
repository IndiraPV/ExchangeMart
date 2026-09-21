using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ExchangeBaseTests
{
    [TestClass]
    public class UtilityTools
    {
        [TestMethod]
        public void TestFormattedAddress()
        {       
            string FormattedAddress = Exchangebase.Com.Bll.UtilityTools.FormatAddress("", "", "", "", "");
            Assert.AreEqual(" <br/>", FormattedAddress);
        }

        [TestMethod]
        public void TestAmmendChangeLog()
        {
            List<string> ChangeLog = new List<string>();

            Exchangebase.Com.Bll.UtilityTools.AmmendChangeLog(ref ChangeLog, "Test1");
            Exchangebase.Com.Bll.UtilityTools.AmmendChangeLog(ref ChangeLog, "Test2","0","2");

            Assert.IsTrue(ChangeLog.Contains("Changed Test1"));
            Assert.IsFalse(ChangeLog.Contains("ChangedTest1"));

            Assert.IsTrue(ChangeLog.Contains("Changed Test2 from 0 to 2"));
            Assert.IsFalse(ChangeLog.Contains("Changed Test2"));           
            Assert.IsFalse(ChangeLog.Contains("Added Test2"));
            Assert.IsFalse(ChangeLog.Contains("Removed Test2"));

        }
    }
}
