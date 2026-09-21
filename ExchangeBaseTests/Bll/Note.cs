using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text;

namespace ExchangeBaseTests.Bll
{
    [TestClass]
    public class Note
    {
        [TestMethod]
        public void TestNoteUpdate()
        {
            StringBuilder sql = new StringBuilder();

            Exchangebase.Com.Bll.Note TestNote = new Exchangebase.Com.Bll.Note();

            TestNote.UpdateString(ref sql);

            Assert.IsFalse(sql.ToString().Contains(",WHERE") || sql.ToString().Contains(", WHERE"));

            Assert.IsFalse(sql.ToString().Contains("=,") || sql.ToString().Contains("= ,"));
        }
    }
}
