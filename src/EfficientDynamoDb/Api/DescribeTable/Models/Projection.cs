using System;
using System.Collections.Generic;
using EfficientDynamoDb.Api.DescribeTable.Models.Enums;

namespace EfficientDynamoDb.Api.DescribeTable.Models
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