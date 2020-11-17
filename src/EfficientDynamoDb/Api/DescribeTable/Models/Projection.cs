using System;
using System.Collections.Generic;
using EfficientDynamoDb.Api.DescribeTable.Models.Enums;

namespace EfficientDynamoDb.Api.DescribeTable.Models
{
    public class Projection
    {
        public IReadOnlyCollection<string> NonKeyAttributes { get; }    
        
        public ProjectionType ProjectionType { get; }

        public Projection(IReadOnlyCollection<string>? nonKeyAttributes, ProjectionType projectionType)
        {
            NonKeyAttributes = nonKeyAttributes ?? Array.Empty<string>();
            ProjectionType = projectionType;
        }
    }
}