﻿#region License
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MongoTemplateTests.cs" company="The original author or authors.">
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

using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using NSubstitute;
using NUnit.Framework;
using Spring.Dao;
using Spring.Data.MongoDb.Core.HelperClasses;

namespace Spring.Data.MongoDb.Core
{
    /// <summary>
    /// Unit tests for <see cref="MongoTemplate"/>
    /// </summary>
    /// <author>Oliver Gierke</author>
    /// <author>Thomas Trageser</author>
    [TestFixture]
    [Category(TestCategory.Unit)]
    public class MongoTemplateTests
    {
        private MongoTemplate _mongoTemplate;
        private IMongoDatabaseFactory _dbFactory;
        private MongoServer _mongo;
        private MongoDatabase _mongoDatabase;
        private MongoCollection<object> _mongoCollection;
        private CommandResult _okComandResult;
        private CommandResult _failComandResult;

        [SetUp]
        public void SetUp()
        {
            _mongo = MongoTestHelper.GetCachedMockMongoServer();
            _mongoDatabase = MongoTestHelper.GetCachedMockMongoDatabase("test", WriteConcern.Acknowledged);
            _mongoCollection = MongoTestHelper.CreateMockCollection<object>("test", "tests");
            
            CreateOkCommandResult();
            CreateFailCommandResult();

            _dbFactory = Substitute.For<IMongoDatabaseFactory>();
            _dbFactory.GetDatabase().Returns(_mongoDatabase);
            _mongoDatabase.GetCollection<object>(Arg.Any<string>()).Returns(_mongoCollection);

            _mongoTemplate = new MongoTemplate(_dbFactory);
        }

        [Test]
        public void CollectionExistsGeneric()
        {
            _mongoDatabase.ClearReceivedCalls();
            _mongoDatabase.CollectionExists(Arg.Is("persons")).Returns(true);
            _mongoDatabase.CollectionExists(Arg.Is("notExists")).Returns(false);

            var exists = _mongoTemplate.CollectionExists<Person>();
            _mongoDatabase.Received(1).CollectionExists(Arg.Is("persons"));
            Assert.That(exists, Is.True);

            exists = _mongoTemplate.CollectionExists<NotExist>();
            _mongoDatabase.Received(1).CollectionExists(Arg.Is("notExists"));
            Assert.That(exists, Is.False);
        }

        [Test]
        public void CollectionExistsViaName()
        {
            _mongoDatabase.ClearReceivedCalls();
            _mongoDatabase.CollectionExists(Arg.Is("persons")).Returns(true);
            _mongoDatabase.CollectionExists(Arg.Is("notExists")).Returns(false);

            var exists = _mongoTemplate.CollectionExists("persons");

            _mongoDatabase.Received(1).CollectionExists(Arg.Is("persons"));
            Assert.That(exists, Is.True);

            exists = _mongoTemplate.CollectionExists("notExists");
            _mongoDatabase.Received(1).CollectionExists(Arg.Is("notExists"));
            Assert.That(exists, Is.False);
        }
        
        [Test]
        public void CollectionExistsMustHaveCollectionName()
        {
            Assert.That(delegate
            {
                _mongoTemplate.CollectionExists("");
            }, Throws.TypeOf<ArgumentNullException>());
        }
        
        [Test]
        public void CollectionExistsFailsWhenNoDatabase()
        {
            _dbFactory.GetDatabase().Returns((MongoDatabase)null);

            Assert.That(delegate
                {
                    _mongoTemplate.CollectionExists("funny");
                }, Throws.TypeOf<MongoException>());            
        }

        [Test]
        public void CreateCollectionViaGeneric()
        {
            var mongoCollection = MongoTestHelper.CreateMockCollection<Person>("unit", "persons");
            _mongoDatabase.ClearReceivedCalls();
            _mongoDatabase.CreateCollection("persons").Returns(_okComandResult);
            _mongoDatabase.GetCollection<Person>("persons").Returns((MongoCollection)mongoCollection);

            MongoCollection collection = _mongoTemplate.CreateCollection<Person>();

            _mongoDatabase.Received(1).CreateCollection("persons");
            _mongoDatabase.Received(1).GetCollection<Person>("persons");
            Assert.That(collection, Is.SameAs(mongoCollection));
        }

        [Test]
        public void CreateCollectionViaName()
        {
            var mongoCollection = MongoTestHelper.CreateMockCollection<Person>("unit", "persons");

            _mongoDatabase.ClearReceivedCalls();
            _mongoDatabase.CreateCollection("persons").Returns(_okComandResult);
            _mongoDatabase.GetCollection<Person>("persons").Returns(mongoCollection);

            var collection = _mongoTemplate.CreateCollection<Person>("persons");

            _mongoDatabase.Received(1).CreateCollection("persons");
            _mongoDatabase.Received(1).GetCollection<Person>("persons");
            Assert.That(collection, Is.SameAs(mongoCollection));            
        }

        [Test]
        public void CreateCollectionWithOptions()
        {
            var options = CollectionOptions.SetMaxDocuments(20);

            var mongoCollection = MongoTestHelper.CreateMockCollection<Person>("unit", "persons");
            _mongoDatabase.ClearReceivedCalls();
            _mongoDatabase.CreateCollection("persons", options).Returns(_okComandResult);
            _mongoDatabase.GetCollection<Person>("persons").Returns(mongoCollection);

            var collection = _mongoTemplate.CreateCollection<Person>("persons", options);

            _mongoDatabase.Received(1).CreateCollection("persons", options);
            _mongoDatabase.Received(1).GetCollection<Person>("persons");
            Assert.That(collection, Is.EqualTo(mongoCollection));                        
        }

        [Test]
        public void CreateCollectionWithNotAllowedCollectionName()
        {
            Assert.That(delegate { _mongoTemplate.CreateCollection<Person>("my$names"); }, Throws.TypeOf<MongoException>());
            Assert.That(delegate { _mongoTemplate.CreateCollection<Person>("system.fun"); }, Throws.TypeOf<MongoException>());
            Assert.That(delegate { _mongoTemplate.CreateCollection<Person>("funny\0character"); }, Throws.TypeOf<MongoException>());
            Assert.That(delegate { _mongoTemplate.CreateCollection<Person>("012345678901234567890123456789012345678901234567890123456789012345678901234567891"); }, Throws.TypeOf<MongoException>());
        }

        [Test]
        public void CreateCollectionCommandResultOkFalse()
        {
            var mongoCollection = MongoTestHelper.CreateMockCollection<Person>("unit", "persons");

            _mongoDatabase.CreateCollection("jokes").Returns(_failComandResult);
            _mongoDatabase.GetCollection<Person>("jokes").Returns(mongoCollection);

            Assert.That(delegate
            {
                _mongoTemplate.CreateCollection<Person>("jokes");
            }, Throws.TypeOf<MongoException>());
        }

        [Test]
        public void CreateCollectionFailsIfNoCollectionName()
        {
            Assert.That(delegate
                {
                    _mongoTemplate.CreateCollection<Person>("");
                }, Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void CreateCollectionFailsWhenNoDatabase()
        {
            _dbFactory.GetDatabase().Returns((MongoDatabase)null);

            Assert.That(delegate
            {
                _mongoTemplate.CreateCollection<Person>("funny");
            }, Throws.TypeOf<MongoException>());
        }

        [Test]
        public void GetCollectioNames()
        {
            _mongoDatabase.ClearReceivedCalls();
            _mongoDatabase.GetCollectionNames().Returns(new List<string>() { "persons", "nerds", "geeks" });

            var names = _mongoTemplate.GetCollectionNames();

            _mongoDatabase.Received(1).GetCollectionNames();
            Assert.That(names, Is.Not.Null);
            Assert.That(names, Has.Count.EqualTo(3));
        }

        [Test]
        public void GetCollectioNamesFailsWhenNoDatabase()
        {
            _dbFactory.GetDatabase().Returns((MongoDatabase)null);

            Assert.That(delegate
                {
                    _mongoTemplate.GetCollectionNames();
                }, Throws.TypeOf<MongoException>());
        }

        [Test]
        public void RejectsNullDatabaseName()
        {
            Assert.That(delegate
                {
                    var template = new MongoTemplate(_mongo, null);
                },
                        Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void RejectsNullMongo()
        {
            Assert.That(delegate
                {
                    var template = new MongoTemplate(null, "database");
                },
                        Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void GetCollectionNameViaTypeName()
        {
            var collectionName = _mongoTemplate.GetCollectionName<Person>();

            Assert.That(collectionName, Is.EqualTo("persons"));
        }

        [Test]
        public void GetCollectionNameViaAttribute()
        {
            var collectionName = _mongoTemplate.GetCollectionName<PersonWithAttribute>();

            Assert.That(collectionName, Is.EqualTo("persons"));
        }

        [Test]
        public void ShouldGetIdFieldForQuery()
        {
            IMongoQuery query = _mongoTemplate.GetIdQueryFor(new Person("TT"));

            Assert.That(query, Is.Not.Null);
            Assert.That(query.ToString(), Is.EqualTo("{ \"_id\" : ObjectId(\"000000000000000000000000\") }"));
        }

        [Test]
        public void RemoveHandlesMongoExceptionProperly()
        {
            _mongoDatabase.GetCollection("collection").Returns(x => { throw new Exception("Exception"); });

            Assert.That(delegate { _mongoTemplate.Remove((object) null); }, Throws.InstanceOf<DataAccessException>());
            Assert.That(delegate { _mongoTemplate.Remove("collection", (object) null); },
                        Throws.InstanceOf<DataAccessException>());
            Assert.That(delegate { _mongoTemplate.Remove((string) null, (object) null); },
                        Throws.InstanceOf<DataAccessException>());
            Assert.That(delegate { _mongoTemplate.Remove<object>(null); }, Throws.InstanceOf<DataAccessException>());
            Assert.That(delegate { _mongoTemplate.Remove("collection", (IMongoQuery) null); },
                        Throws.InstanceOf<DataAccessException>());
            Assert.That(delegate { _mongoTemplate.Remove((string) null, (IMongoQuery) null); },
                        Throws.InstanceOf<DataAccessException>());
        }

        [Test]
        public void ExecuteRejectsNullForCollectionCallback()
        {
            Assert.That(delegate
                {
                    _mongoTemplate.Execute<object>("collection", null);
                }, Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ExecuteRejectsNullForCollectionNameMissing()
        {
            Assert.That(delegate
                {
                    _mongoTemplate.Execute<object>((string) null, null);
                }, Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void RemoveExecutesExecuteWithCollectionCallback()
        {
            var template = new TestMongoTemplate(_dbFactory);

            template.Remove(new Person(new ObjectId("000000000000000000000001"), "Thomas"));

            Assert.That(template.WasExecuted, Is.True);
            Assert.That(template.CollectionName, Is.EqualTo("persons"));
            Assert.That(template.ReturnType, Is.EqualTo(typeof (WriteConcernResult)));
            Assert.That(template.Func, Is.Not.Null);
        }

        private void CreateOkCommandResult()
        {
            BsonDocument response = new BsonDocument();
            response.Add("ok", BsonValue.Create(true));
            _okComandResult = new CommandResult(null, response);
        }

        private void CreateFailCommandResult()
        {
            BsonDocument response = new BsonDocument();
            response.Add("ok", BsonValue.Create(false));
            response.Add("errmsg", BsonValue.Create("Error happen from time to time"));
            _failComandResult = new CommandResult(null, response);
        }

        public class NotExist
        {
            public string Id { get; set; }
        }

    }

    public class TestMongoTemplate : MongoTemplate
    {
        private string _collectionName;
        private Type _returnType;
        private object _func;
        private bool _execute;

        public TestMongoTemplate(IMongoDatabaseFactory factory)
            : base(factory)
        {
        }

        public override TReturn Execute<TReturn>(string collectionName,
                                                 Func<MongoCollection, TReturn> collectionCallback)
        {
            _collectionName = collectionName;
            _func = collectionCallback;
            _returnType = typeof (TReturn);
            _execute = true;
            return default(TReturn);
        }

        public bool WasExecuted
        {
            get { return _execute; }
        }

        public string CollectionName
        {
            get { return _collectionName; }
        }

        public Type ReturnType
        {
            get { return _returnType; }
        }

        public object Func
        {
            get { return _func; }
        }
    }
}