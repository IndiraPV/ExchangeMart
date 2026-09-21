using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text;

namespace ExchangeBaseTests.Bll
{
    [TestClass]
    public class Deal
    {
        [TestMethod]
        public void TestDealUpdate()
        {
            StringBuilder sql = new StringBuilder();

            Exchangebase.Com.Bll.Deal TestDeal = new Exchangebase.Com.Bll.Deal();

            TestDeal.UpdateString(ref sql);

            Assert.IsFalse(sql.ToString().Contains(",WHERE") || sql.ToString().Contains(", WHERE"));

            Assert.IsFalse(sql.ToString().Contains("=,") || sql.ToString().Contains("= ,"));
        }
    }
}
