using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text;

namespace ExchangeBaseTests.Bll
{
    [TestClass]
    public class ItemsWanted
    {
        [TestMethod]
        public void TestUpdate()
        {
            StringBuilder sql = new StringBuilder();

            Exchangebase.Com.Bll.ItemsWanted TestRequest = new Exchangebase.Com.Bll.ItemsWanted();
            TestRequest.IwaIclID = 50; //Putting this in to prevent hitting DB at GenerateItemsWantedName().  This test is to just make sure the the string builder is working correctly.  

            TestRequest.GetUpdateString(ref sql);

            Assert.IsFalse(sql.ToString().Contains(",WHERE") || sql.ToString().Contains(", WHERE"));

            Assert.IsFalse(sql.ToString().Contains("=,") || sql.ToString().Contains("= ,"));
        }
    }
}
