using System.Collections.Generic;
using System.Linq;
using Benchmarks.AwsDdbSdk.Entities;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace Benchmarks.Mocks
{
    public static class EntityExtensions
    {
        public static Document ToDocument(this KeysOnlyEntity entity)
        {
            return new Document
            {
                {"pk", entity.Pk},
                {"sk", entity.Sk},
            };
        }

        public static Document ToDocument(this MediumStringFieldsEntity entity)
        {
            return new Document
            {
                {"pk", entity.Pk},
                {"sk", entity.Sk},
                {"f1", entity.F1},
                {"f2", entity.F2},
                {"f3", entity.F3.ToString("O")},
                {"f4", entity.F4.ToString("O")},
                {"f5", entity.F5},
                {"f6", entity.F6},
                {"f7", entity.F7.ToString("O")},
                {"f8", entity.F8.ToString("O")},
                {"f9", entity.F9},
                {"f10", entity.F10},
                {"f11", entity.F11},
                {"f12", entity.F12}
            };
        }
        
        public static Document ToDocument(this MediumComplexFieldsEntity entity)
        {
            return new Document
            {
                {"pk", entity.Pk},
                {"sk", entity.Sk},
                {"f1", entity.F1},
                {"f2", entity.F2},
                {"f3", entity.F3.ToString("O")},
                {"f4", entity.F4.ToString("O")},
                {"f5", new Document
                {
                    {"f1", entity.F5.StringField1},
                    {"f2", entity.F5.StringField2},
                    {"f3", entity.F5.StringField3},
                    {"f4", entity.F5.StringField4},
                    {"f5", entity.F5.IntField1},
                    {"f6", entity.F5.IntField2},
                    {"f7", entity.F5.IntField3},
                    {"f8", entity.F5.IntField4},
                }}
            };
        }

        public static Document ToDocument(this MixedEntity entity)
        {
            return new Document
            {
                {"pk", entity.Pk},
                {"sk", entity.Sk},
                {"b", entity.B},
                {"n", entity.N},
                {"s", entity.S},
                {"ns", new NumberSetAttributeValue(new HashSet<string>(entity.Ns.Select(x => x.ToString())))},
                {"ss", new StringSetAttributeValue(entity.Ss)},
                {
                    "m", new AttributeValue(new MapAttributeValue(new Document
                    {
                        {"p1", entity.M.P1}
                    }))
                },
                {
                    "l1", new ListAttributeValue(entity.L1.Select(x => new AttributeValue(new MapAttributeValue(new Document
                    {
                        {"p1", x.P1}
                    }))).ToList())
                },
                {
                    "l2", new ListAttributeValue(entity.L2.Select(x => new AttributeValue(new MapAttributeValue(new Document
                    {
                        {"p1", x.P1}
                    }))).ToList())
                },
                {
                    "l3", new ListAttributeValue(entity.L3.Select(x => new AttributeValue(new MapAttributeValue(new Document
                    {
                        {"p1", x.P1}
                    }))).ToList())
                },
            };
        }
        
        public static Document ToDocument(this LargeStringFieldsEntity entity)
        {
            return new Document
            {
                {"pk", entity.Pk},
                {"sk", entity.Sk},
                {"f1", entity.F1},
                {"f2", entity.F2},
                {"f3", entity.F3},
                {"f4", entity.F4},
                {"f5", entity.F5},
                {"f6", entity.F6},
                {"f7", entity.F7},
                {"f8", entity.F8},
                {"f9", entity.F9},
                {"f10", entity.F10},
                {"f11", entity.F11},
                {"f12", entity.F12},
                {"f13", entity.F13},
                {"f14", entity.F14},
                {"f15", entity.F15},
                {"f16", entity.F16},
                {"f17", entity.F17},
                {"f18", entity.F18},
                {"f19", entity.F19},
                {"f20", entity.F20},
                {"f21", entity.F21},
                {"f22", entity.F22},
                {"f23", entity.F23},
                {"f24", entity.F24},
                {"f25", entity.F25},
                {"f26", entity.F26},
                {"f27", entity.F27},
                {"f28", entity.F28},
                {"f29", entity.F29},
                {"f30", entity.F30},
                {"f31", entity.F31},
                {"f32", entity.F32},
                {"f33", entity.F33},
                {"f34", entity.F34},
                {"f35", entity.F35},
                {"f36", entity.F36},
                {"f37", entity.F37},
                {"f38", entity.F38},
                {"f39", entity.F39},
                {"f40", entity.F40},
            };
        }
    }
}