using System;
using System.Collections.Generic;
using Benchmarks.Entities;

namespace Benchmarks.Mocks
{
    public static class EntitiesFactory
    {
        public static KeysOnlyEntity CreateKeysOnlyEntity(int index)
        {
            return new KeysOnlyEntity
            {
                Pk = $"pk_{index:0000}",
                Sk = $"sk_{index:0000}"
            };
        }
        
        public static MediumStringFieldsEntity CreateMediumStringEntity(int index)
        {
            return new MediumStringFieldsEntity
            {
                Pk = $"pk_{index:0000}",
                Sk = $"sk_{index:0000}",
                F1 = $"test_f1_{index:0000}",
                F2 = $"test_f2_{index:0000}",
                F3 = new DateTime(2020, 12, 01, 8, 15, 0).AddSeconds(index),
                F4 = new DateTime(2020, 12, 20, 20, 20, 0).AddSeconds(index),
                F5 = $"test_f5_{index:0000}",
                F6 = $"test_f6_{index:0000}",
                F7 = new DateTime(2020, 12, 5, 19, 15, 0).AddSeconds(index),
                F8 = new DateTime(2020, 12, 3, 15, 15, 0).AddSeconds(index),
                F9 = index,
                F10 = 1000 - index,
                F11 = 1000000,
                F12 = 1
            };
        }
        
        public static MediumComplexFieldsEntity CreateMediumComplexEntity(int index)
        {
            return new MediumComplexFieldsEntity
            {
                Pk = $"pk_{index:0000}",
                Sk = $"sk_{index:0000}",
                F1 = $"test_f1_{index:0000}",
                F2 = $"test_f2_{index:0000}",
                F3 = new DateTime(2020, 12, 01, 8, 15, 0).AddSeconds(index),
                F4 = new DateTime(2020, 12, 20, 20, 20, 0).AddSeconds(index),
                F5 = new TestNestedObject
                {
                    IntField1 = index,
                    IntField2 = 1000 - index,
                    IntField3 = 1000000,
                    IntField4 = 1,
                    StringField1 = $"test_sf1_{index:0000}",
                    StringField2 = $"test_sf2_{index:0000}",
                    StringField3= $"test_sf1_{index:0000}",
                    StringField4 = $"test_sf2_{index:0000}",
                }
            };
        }

        public static MixedEntity CreateMixedEntity(int index)
        {
            return new MixedEntity
            {
                Pk = $"pk_{index:0000}",
                Sk = $"sk_{index:0000}",
                B = index % 2 == 0,
                N = index,
                S = $"test_{index:0000}",
                Ns = new HashSet<int> {index},
                Ss = new HashSet<string> {$"test_set_{index:0000}"},
                M = new MapObject {P1 = $"test_p0_{index:0000}"},
                L1 = new List<MapObject> {new MapObject {P1 = $"test_p1_{index:0000}"}},
                L2 = new List<MapObject> {new MapObject {P1 = $"test_p2_{index:0000}"}},
                L3 = new List<MapObject> {new MapObject {P1 = $"test_p3_{index:0000}"}}
            };
        }
        
        public static MixedEntityFromInterface CreateMixedEntityFromInterface(int index)
        {
            return new MixedEntityFromInterface
            {
                Pk = $"pk_{index:0000}",
                Sk = $"sk_{index:0000}",
                B = index % 2 == 0,
                N = index,
                S = $"test_{index:0000}",
                Ns = new HashSet<int> {index},
                Ss = new HashSet<string> {$"test_set_{index:0000}"},
                M = new MapObject {P1 = $"test_p0_{index:0000}"},
                L1 = new List<MapObject> {new MapObject {P1 = $"test_p1_{index:0000}"}},
                L2 = new List<MapObject> {new MapObject {P1 = $"test_p2_{index:0000}"}},
                L3 = new List<MapObject> {new MapObject {P1 = $"test_p3_{index:0000}"}}
            };
        }
        
        public static LargeStringFieldsEntity CreateLargeStringEntity(int index)
        {
            return new LargeStringFieldsEntity
            {
                Pk = $"pk_{index:0000}",
                Sk = $"sk_{index:0000}",
                F1 = $"test_f1_{index:0000}",
                F2 = $"test_f2_{index:0000}",
                F3 = $"test_f3_{index:0000}",
                F4 = $"test_f4_{index:0000}",
                F5 = $"test_f5_{index:0000}",
                F6 = $"test_f6_{index:0000}",
                F7 = $"test_f7_{index:0000}",
                F8 = $"test_f8_{index:0000}",
                F9 = $"test_f9_{index:0000}",
                F10 = $"test_10_{index:0000}",
                F11 = $"test_f11_{index:0000}",
                F12 = $"test_f12_{index:0000}",
                F13 = $"test_f13_{index:0000}",
                F14 = $"test_f14_{index:0000}",
                F15 = $"test_f15_{index:0000}",
                F16 = $"test_f16_{index:0000}",
                F17 = $"test_f17_{index:0000}",
                F18 = $"test_f18_{index:0000}",
                F19 = $"test_f19_{index:0000}",
                F20 = $"test_f20_{index:0000}",
                F21 = $"test_f21_{index:0000}",
                F22 = $"test_f22_{index:0000}",
                F23 = $"test_f23_{index:0000}",
                F24 = $"test_f24_{index:0000}",
                F25 = $"test_f25_{index:0000}",
                F26 = $"test_f26_{index:0000}",
                F27 = $"test_f27_{index:0000}",
                F28 = $"test_f28_{index:0000}",
                F29 = $"test_f29_{index:0000}",
                F30 = $"test_f30_{index:0000}",
                F31 = $"test_f31_{index:0000}",
                F32 = $"test_f32_{index:0000}",
                F33 = $"test_f33_{index:0000}",
                F34 = $"test_f34_{index:0000}",
                F35 = $"test_f35_{index:0000}",
                F36 = $"test_f36_{index:0000}",
                F37 = $"test_f37_{index:0000}",
                F38 = $"test_f38_{index:0000}",
                F39 = $"test_f39_{index:0000}",
                F40 = $"test_f40_{index:0000}",
            };
        }
    }
}