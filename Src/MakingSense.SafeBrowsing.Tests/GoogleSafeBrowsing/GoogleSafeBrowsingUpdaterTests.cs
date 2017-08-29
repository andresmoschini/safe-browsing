﻿#region License
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
using MakingSense.SafeBrowsing.Tests;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
#if DNXCORE50
using Xunit;
using Test = Xunit.FactAttribute;
using Assert = MakingSense.SafeBrowsing.Tests.XUnitAssert;
#else
using NUnit.Framework;
#endif

namespace MakingSense.SafeBrowsing.GoogleSafeBrowsing
{
    [TestFixture]
    public class GoogleSafeBrowsingUpdaterTests : TestFixtureBase
    {
        [Test]
        public void SimpleRegexRulesHttpUpdater_should_update_rules_reading_remote_file_real_request()
        {
            // Arrange
            var cfg = new GoogleSafeBrowsingApiConfiguration()
            {
                Apikey = "apikey"
            };

            var httpClient = new HttpClientDouble();
            httpClient.Setup_PostString(GoogleSafeBrowsingTestResponses.Response1);

            var dateTimeProvider = new DateTimeProviderDouble();

            var db = new GoogleSafeBrowsingDatabase();
            var sut = new GoogleSafeBrowsingUpdater(cfg, db, httpClient, dateTimeProvider);
            //Assert.IsFalse(rules.Blacklist.Any());

            // Act
            sut.UpdateAsync().Wait();

            // Assert
            //Assert.IsTrue(rules.Blacklist.Any());
        }
    }
}
