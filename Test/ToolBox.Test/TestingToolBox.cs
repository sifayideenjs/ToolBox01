using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToolBox.DataAccess.DataSet;

namespace ToolBox.Test
{
    [TestClass]
    public class TestingToolBox
    {
        [TestMethod]
        public void PopulateDataSet()
        {
            DataSet.PopulateDataSet();
        }
    }
}
