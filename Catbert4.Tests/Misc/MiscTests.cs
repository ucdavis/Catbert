using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Catbert4.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Catbert4.Tests.Misc
{
    [TestClass]
    public class MiscTests
    {

        [TestMethod, Ignore]
        public void TestValidateAutoMaping()
        {
            #region Arrange
            Mapper.Initialize(cfg => cfg.AddProfile<ViewModelProfile>());
            #endregion Arrange

            #region Assert
            Mapper.AssertConfigurationIsValid();
            #endregion Assert		
        }
    }
}
