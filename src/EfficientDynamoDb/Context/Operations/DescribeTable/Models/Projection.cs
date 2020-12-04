using System;
using System.Collections.Generic;
using EfficientDynamoDb.Context.Operations.DescribeTable.Models.Enums;

namespace EfficientDynamoDb.Context.Operations.DescribeTable.Models
{
    public class Projection
    {
        public IReadOnlyList<string> NonKeyAttributes { get; }    
        
        public ProjectionType ProjectionType { get; }

        public Projection(IReadOnlyList<string>? nonKeyAttributes, ProjectionType projectionType)
        {
            NonKeyAttributes = nonKeyAttributes ?? Array.Empty<string>();
            ProjectionType = projectionType;
        }
    }
}