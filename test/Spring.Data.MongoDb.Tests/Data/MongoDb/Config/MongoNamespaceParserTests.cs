﻿#region License
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MongoNamespaceParserTests.cs" company="The original author or authors.">
//   Copyright 2002-2013 the original author or authors.
//   
//   Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with
//   the License. You may obtain a copy of the License at
//   
//   http://www.apache.org/licenses/LICENSE-2.0
//   
//   Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on
//   an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the
//   specific language governing permissions and limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

using NUnit.Framework;
using Spring.Objects.Factory.Xml;

namespace Spring.Data.MongoDb.Config
{
    /// <summary>
    /// Unit test for <see cref="MongoNamespaceParser"/>
    /// </summary>
    /// <author></author>
    /// <author></author>
    [TestFixture]
    [Category(TestCategory.Unit)]
    public class MongoNamespaceParserTests
    {
        [SetUp]
        public void Setup()
        {
            NamespaceParserRegistry.RegisterParser(typeof(MongoNamespaceParser));
        }

        [Test]
        public void Registered()
        {
            INamespaceParser parser = NamespaceParserRegistry.GetParser("http://www.springframework.net/mongo");

            Assert.That(parser , Is.Not.Null);
        }
    }
}
