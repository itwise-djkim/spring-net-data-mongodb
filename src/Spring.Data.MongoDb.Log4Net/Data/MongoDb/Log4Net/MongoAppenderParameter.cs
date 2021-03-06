﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MongoAppenderParameter.cs" company="The original author or authors.">
//   Copyright 2002-2012 the original author or authors.
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

using log4net.Layout;


namespace Spring.Data.MongoDb.Log4Net
{
    /// <summary>
    /// Parameter element within appender configuration for defining parameters to log into document
    /// </summary>
    /// <author>Thomas Trageser</author>
    public class MongoAppenderParameter
    {
        /// <summary>
        /// Defines the element key name in the mongoDB document
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// Defined the pattern to use for the specified element key
        /// </summary>
        public IRawLayout Layout { get; set; }
    }
}
