/*

Copyright dotNetRDF Project 2009-12
dotnetrdf-develop@lists.sf.net

------------------------------------------------------------------------

This file is part of dotNetRDF.

dotNetRDF is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

dotNetRDF is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with dotNetRDF.  If not, see <http://www.gnu.org/licenses/>.

------------------------------------------------------------------------

dotNetRDF may alternatively be used under the LGPL or MIT License

http://www.gnu.org/licenses/lgpl.html
http://www.opensource.org/licenses/mit-license.php

If these licenses are not suitable for your intended use please contact
us at the above stated email address to discuss alternative
terms.

*/

using System;
using VDS.RDF.Configuration;
using VDS.RDF.Parsing;

namespace VDS.RDF.Query.FullText.Schema
{
    /// <summary>
    /// Default Index Schema
    /// </summary>
    public class DefaultIndexSchema
        : BaseIndexSchema, IConfigurationSerializable
    {
        /// <summary>
        /// Constants for the Field Names used by the Default Index Schema
        /// </summary>
        public const String DefaultIndexField = "nodeIndex",
                            DefaultGraphField = "nodeGraph",
                            DefaultHashField = "nodeIndexHash",
                            DefaultNodeTypeField = "nodeType",
                            DefaultNodeValueField = "nodeValue",
                            DefaultNodeMetaField = "nodeMeta";

        /// <summary>
        /// Creates a new Default Index Schema
        /// </summary>
        public DefaultIndexSchema()
        {
            this.IndexField = DefaultIndexField;
            this.GraphField = DefaultGraphField;
            this.HashField = DefaultHashField;
            this.NodeMetaField = DefaultNodeMetaField;
            this.NodeTypeField = DefaultNodeTypeField;
            this.NodeValueField = DefaultNodeValueField;
        }

        /// <summary>
        /// Serializes the Schemas Configuration
        /// </summary>
        /// <param name="context">Serialization Context</param>
        public void SerializeConfiguration(ConfigurationSerializationContext context)
        {
            context.EnsureObjectFactory(typeof(FullTextObjectFactory));

            INode schemaObj = context.NextSubject;
            context.Graph.Assert(schemaObj, context.Graph.CreateUriNode(UriFactory.Create(RdfSpecsHelper.RdfType)), context.Graph.CreateUriNode(UriFactory.Create(FullTextHelper.ClassSchema)));
            context.Graph.Assert(schemaObj, context.Graph.CreateUriNode(UriFactory.Create(ConfigurationLoader.PropertyType)), context.Graph.CreateLiteralNode(this.GetType().FullName + ", dotNetRDF.Query.FullText"));
        }
    }
}
