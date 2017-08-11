#region License
// Copyright (c) 2017 Doppler Relay Team
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if DNXCORE50
using Xunit;
using Test = Xunit.FactAttribute;
using Assert = MakingSense.SafeBrowsing.Tests.XUnitAssert;
#else
using NUnit.Framework;
#endif

namespace MakingSense.SafeBrowsing.Tests
{
    [TestFixture]
    public class SimpleRegexUrlCheckerTests : TestFixtureBase
    {
        [Test]
        public void Check_should_identify_dangerous_urls()
        {
            // Arrange
            var sut = new SimpleRegexUrlChecker(new[]
            {
                @"﻿﻿^.*jpe082ver\.info.*$",
                @"^.*ntfl-promo2017.info.*$"
            });
            var url = "http://www.jpe082ver.info/test";

            // Act
            var result = sut.Check(url);

            // Assert
            Assert.AreEqual(url, result.Url);
            Assert.AreEqual(ThreatType.Unknow, result.ThreatType);
            Assert.AreEqual(false, result.IsSafe);
        }

        [Test]
        public void Check_should_not_identify_safe_urls()
        {
            // Arrange
            var sut = new SimpleRegexUrlChecker(new[]
            {
                @"﻿^.*jpe082ver\.info.*$",
                @"^.*ntfl-promo2017\.info.*$"
            });
            var url = "http://www.safe.info/test";

            // Act
            var result = sut.Check(url);

            // Assert
            Assert.AreEqual(url, result.Url);
            Assert.AreEqual(true, result.IsSafe);
            Assert.AreEqual(ThreatType.NoThreat, result.ThreatType);
        }
    }
}
