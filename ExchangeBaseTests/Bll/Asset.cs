using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text;
namespace ExchangeBaseTests.Bll
{
    [TestClass]
    public class Asset
    {
        [TestMethod]
        public void TestUpdateString()
        {
            StringBuilder sql = new StringBuilder();

            Exchangebase.Com.Bll.Asset TestAsset = new Exchangebase.Com.Bll.Asset();

            TestAsset.GetUpdateString(ref sql);

            Assert.IsFalse(sql.ToString().Contains(",WHERE"));
   
        }
    }
}
