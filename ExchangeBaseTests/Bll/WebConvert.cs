using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ExchangeBaseTests.Bll
{
    [TestClass]
    public class WebConvert
    {
        [TestMethod]
        public void ToInt32NullableTest()
        {
            //Assert.AreEqual(0, Exchangebase.Com.Bll.WebConvert.ToInt32Nullable(0));
            Assert.AreEqual(null, Exchangebase.Com.Bll.WebConvert.ToInt32Nullable("asdfsadf"));
        }

        [TestMethod]
        public void ToInt32()
        {
            Assert.AreEqual(0, Exchangebase.Com.Bll.WebConvert.ToInt32("asdfsadf", 0));
            Assert.AreEqual(32, Exchangebase.Com.Bll.WebConvert.ToInt32("32", 0));
            Assert.AreEqual(0, Exchangebase.Com.Bll.WebConvert.ToInt32(true, 0));       
        }
    }
}
