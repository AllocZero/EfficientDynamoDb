/*
 * Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License").
 * You may not use this file except in compliance with the License.
 * A copy of the License is located at
 * 
 *  http://aws.amazon.com/apache2.0
 * 
 * or in the "license" file accompanying this file. This file is distributed
 * on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
 * express or implied. See the License for the specific language governing
 * permissions and limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using Amazon.Runtime.Internal.Util;
using Amazon.Util;
using ThirdParty.Json.LitJson;

namespace Amazon.Runtime.Internal.Transform
{
    static class SimpleTypeUnmarshaller<T>
    {
        public static T Unmarshall(XmlUnmarshallerContext context)
        {
            string text = context.ReadText();
            return (T)Convert.ChangeType(text, typeof(T), CultureInfo.InvariantCulture);
        }

        public static T Unmarshall(JsonUnmarshallerContext context)
        {
            context.Read();
            string text = context.ReadText();
            if (text == null)
                return default(T);

            return (T)Convert.ChangeType(text, typeof(T), CultureInfo.InvariantCulture);
        }
    }

    /// <summary>
    /// Unmarshaller for int fields
    /// </summary>
    public class IntUnmarshaller : IUnmarshaller<int, XmlUnmarshallerContext>, IUnmarshaller<int, JsonUnmarshallerContext>
    {        
        private IntUnmarshaller() { }

        private static IntUnmarshaller _instance = new IntUnmarshaller();

        public static IntUnmarshaller Instance
        {
            get
            {
                return _instance;
            }
        }

        public static IntUnmarshaller GetInstance()
        {
            return IntUnmarshaller.Instance;
        }

        public int Unmarshall(XmlUnmarshallerContext context)
        {
            return SimpleTypeUnmarshaller<int>.Unmarshall(context);
        }
        public int Unmarshall(JsonUnmarshallerContext context)
        {
            return SimpleTypeUnmarshaller<int>.Unmarshall(context);
        }
    }

    /// <summary>
    /// Unmarshaller for nullable int fields. Implemented only for JSON context
    /// to handle cases where value can be null e.g. {'Priority': null}.
    /// This unmarshaller is not implemented for XML context, as XML responses
    /// will null elements (xsi:nil='true') will be skipped by the XML parser.
    /// </summary>
    public class NullableIntUnmarshaller : IUnmarshaller<int?, JsonUnmarshallerContext>
    {   
        private NullableIntUnmarshaller() { }

        private static NullableIntUnmarshaller _instance = new NullableIntUnmarshaller();

        public static NullableIntUnmarshaller Instance
        {
            get
            {
                return _instance;
            }
        }

        public static NullableIntUnmarshaller GetInstance()
        {
            return NullableIntUnmarshaller.Instance;
        }

        public int? Unmarshall(JsonUnmarshallerContext context)
        {
            context.Read();
            string text = context.ReadText();

            if (text == null)
            {
                return null;
            }
            return int.Parse(text, CultureInfo.InvariantCulture);
        }
    }

    /// <summary>
    /// Unmarshaller for long fields
    /// </summary>
    public class LongUnmarshaller : IUnmarshaller<long, XmlUnmarshallerContext>, IUnmarshaller<long, JsonUnmarshallerContext>
    {        
        private LongUnmarshaller() { }

        private static LongUnmarshaller _instance = new LongUnmarshaller();

        public static LongUnmarshaller Instance
        {
            get
            {
                return _instance;
            }
        }

        public static LongUnmarshaller GetInstance()
        {
            return LongUnmarshaller.Instance;
        }

        public long Unmarshall(XmlUnmarshallerContext context)
        {
            return SimpleTypeUnmarshaller<long>.Unmarshall(context);
        }
        public long Unmarshall(JsonUnmarshallerContext context)
        {
            return SimpleTypeUnmarshaller<long>.Unmarshall(context);
        }
    }

    /// <summary>
    /// Unmarshaller for float fields
    /// </summary>
    public class FloatUnmarshaller : IUnmarshaller<float, XmlUnmarshallerContext>, IUnmarshaller<float, JsonUnmarshallerContext>
    {        
        private FloatUnmarshaller() { }

        private static FloatUnmarshaller _instance = new FloatUnmarshaller();

        public static FloatUnmarshaller Instance
        {
            get
            {
                return _instance;
            }
        }

        public static FloatUnmarshaller GetInstance()
        {
            return FloatUnmarshaller.Instance;
        }

        public float Unmarshall(XmlUnmarshallerContext context)
        {
            return SimpleTypeUnmarshaller<float>.Unmarshall(context);
        }
        public float Unmarshall(JsonUnmarshallerContext context)
        {
            return SimpleTypeUnmarshaller<float>.Unmarshall(context);
        }
    }

    /// <summary>
    /// Unmarshaller for double fields
    /// </summary>
    public class DoubleUnmarshaller : IUnmarshaller<double, XmlUnmarshallerContext>, IUnmarshaller<double, JsonUnmarshallerContext>
    {        
        private DoubleUnmarshaller() { }

        private static DoubleUnmarshaller _instance = new DoubleUnmarshaller();

        public static DoubleUnmarshaller Instance
        {
            get
            {
                return _instance;
            }
        }


        public static DoubleUnmarshaller GetInstance()
        {
            return DoubleUnmarshaller.Instance;
        }

        public double Unmarshall(XmlUnmarshallerContext context)
        {
            return SimpleTypeUnmarshaller<double>.Unmarshall(context);
        }
        public double Unmarshall(JsonUnmarshallerContext context)
        {
            return SimpleTypeUnmarshaller<double>.Unmarshall(context);
        }
    }

    /// <summary>
    /// Unmarshaller for decimal fields
    /// </summary>
    public class DecimalUnmarshaller : IUnmarshaller<decimal, XmlUnmarshallerContext>, IUnmarshaller<decimal, JsonUnmarshallerContext>
    {
        private DecimalUnmarshaller() { }

        private static DecimalUnmarshaller _instance = new DecimalUnmarshaller();

        public static DecimalUnmarshaller Instance
        {
            get
            {
                return _instance;
            }
        }

        public static DecimalUnmarshaller GetInstance()
        {
            return DecimalUnmarshaller.Instance;
        }

        public decimal Unmarshall(XmlUnmarshallerContext context)
        {
            return SimpleTypeUnmarshaller<decimal>.Unmarshall(context);
        }
        public decimal Unmarshall(JsonUnmarshallerContext context)
        {
            return SimpleTypeUnmarshaller<decimal>.Unmarshall(context);
        }
    }

    /// <summary>
    /// Unmarshaller for bool fields
    /// </summary>
    public class BoolUnmarshaller : IUnmarshaller<bool, XmlUnmarshallerContext>, IUnmarshaller<bool, JsonUnmarshallerContext>
    {        
        private BoolUnmarshaller() { }

        private static BoolUnmarshaller _instance = new BoolUnmarshaller();

        public static BoolUnmarshaller Instance
        {
            get
            {
                return _instance;
            }
        }

        public static BoolUnmarshaller GetInstance()
        {
            return BoolUnmarshaller.Instance;
        }

        public bool Unmarshall(XmlUnmarshallerContext context)
        {
            return SimpleTypeUnmarshaller<bool>.Unmarshall(context);
        }
        public bool Unmarshall(JsonUnmarshallerContext context)
        {
            return SimpleTypeUnmarshaller<bool>.Unmarshall(context);
        }
    }

    /// <summary>
    /// Unmarshaller for string fields
    /// </summary>
    public class StringUnmarshaller : IUnmarshaller<string, XmlUnmarshallerContext>, IUnmarshaller<string, JsonUnmarshallerContext>
    {        
        private StringUnmarshaller() { }

        private static StringUnmarshaller _instance = new StringUnmarshaller();

        public static StringUnmarshaller Instance
        {
            get
            {
                return _instance;
            }
        }

        public static StringUnmarshaller GetInstance()
        {
            return StringUnmarshaller.Instance;
        }

        public string Unmarshall(XmlUnmarshallerContext context)
        {
            return SimpleTypeUnmarshaller<string>.Unmarshall(context);
        }
        public string Unmarshall(JsonUnmarshallerContext context)
        {
            return SimpleTypeUnmarshaller<string>.Unmarshall(context);
        }
    }

    /// <summary>
    /// Unmarshaller for byte fields
    /// </summary>
    public class ByteUnmarshaller : IUnmarshaller<byte, XmlUnmarshallerContext>, IUnmarshaller<byte, JsonUnmarshallerContext>
    {        
        private ByteUnmarshaller() { }

        private static ByteUnmarshaller _instance = new ByteUnmarshaller();

        public static ByteUnmarshaller Instance
        {
            get
            {
                return _instance;
            }
        }

        public static ByteUnmarshaller GetInstance()
        {
            return ByteUnmarshaller.Instance;
        }

        public byte Unmarshall(XmlUnmarshallerContext context)
        {
            return SimpleTypeUnmarshaller<byte>.Unmarshall(context);
        }
        public byte Unmarshall(JsonUnmarshallerContext context)
        {
            return SimpleTypeUnmarshaller<byte>.Unmarshall(context);
        }
    }

    /// <summary>
    /// Unmarshaller for DateTime fields
    /// </summary>
    public class DateTimeUnmarshaller : IUnmarshaller<DateTime, XmlUnmarshallerContext>, IUnmarshaller<DateTime, JsonUnmarshallerContext>
    {        
        private DateTimeUnmarshaller() { }

        private static DateTimeUnmarshaller _instance = new DateTimeUnmarshaller();

        public static DateTimeUnmarshaller Instance
        {
            get
            {
                return _instance;
            }
        }

        public static DateTimeUnmarshaller GetInstance()
        {
            return DateTimeUnmarshaller.Instance;
        }

        public DateTime Unmarshall(XmlUnmarshallerContext context)
        {
            string text = context.ReadText();
            return UnmarshallInternal(text, treatAsNullable: false).Value;
        }

        public DateTime Unmarshall(JsonUnmarshallerContext context)
        {
            context.Read();
            string text = context.ReadText();
            return UnmarshallInternal(text, treatAsNullable: false).Value;
        }

        /// <summary>
        ///  Unmarshalls given string as a DateTime. Handles cases where we want to unmarshall 
        ///  as just a DateTime or a nullable Datetime.
        /// </summary>
        /// <param name="text">Value to be parsed</param>
        /// <param name="treatAsNullable">If true, the method will return null if text is null. 
        /// If false, the method will return default(DateTime), if text is null.</param>
        /// <returns></returns>
        internal static DateTime? UnmarshallInternal(string text, bool treatAsNullable)
        {
            Double seconds;
            if (Double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out seconds))
            {
                return AWSSDKUtils.EPOCH_START.AddSeconds(seconds);
            }
            else
            {
                if (text == null)
                {
                    if (treatAsNullable) { return null; }
                    else { return default(DateTime); }
                }

                return DateTime.Parse(text, CultureInfo.InvariantCulture);
            }
        }
    }

    /// <summary>
    /// Unmarshaller for nullable DateTime fields. Implemented only for JSON context
    /// to handle cases where value can be null e.g. {'Priority': null}.
    /// This unmarshaller is not implemented for XML context, as XML responses
    /// will null elements (xsi:nil='true') will be skipped by the XML parser.
    /// </summary>
    public class NullableDateTimeUnmarshaller : IUnmarshaller<DateTime?, JsonUnmarshallerContext>
    {
        private NullableDateTimeUnmarshaller() { }

        private static NullableDateTimeUnmarshaller _instance = new NullableDateTimeUnmarshaller();

        public static NullableDateTimeUnmarshaller Instance
        {
            get
            {
                return _instance;
            }
        }

        public static NullableDateTimeUnmarshaller GetInstance()
        {
            return NullableDateTimeUnmarshaller.Instance;
        }

        public DateTime? Unmarshall(JsonUnmarshallerContext context)
        {
            context.Read();
            string text = context.ReadText();
            return DateTimeUnmarshaller.UnmarshallInternal(text, treatAsNullable: true);
        }
    }

    public class DateTimeEpochLongMillisecondsUnmarshaller : IUnmarshaller<DateTime, XmlUnmarshallerContext>, IUnmarshaller<DateTime, JsonUnmarshallerContext>
    {
        private DateTimeEpochLongMillisecondsUnmarshaller() { }

        private static DateTimeEpochLongMillisecondsUnmarshaller _instance = new DateTimeEpochLongMillisecondsUnmarshaller();

        public static DateTimeEpochLongMillisecondsUnmarshaller Instance
        {
            get
            {
                return _instance;
            }
        }

        public static DateTimeEpochLongMillisecondsUnmarshaller GetInstance()
        {
            return DateTimeEpochLongMillisecondsUnmarshaller.Instance;
        }

        public DateTime Unmarshall(XmlUnmarshallerContext context)
        {
            return SimpleTypeUnmarshaller<DateTime>.Unmarshall(context);
        }
        public DateTime Unmarshall(JsonUnmarshallerContext context)
        {
            long millseconds = LongUnmarshaller.Instance.Unmarshall(context);
            var ret = Amazon.Util.AWSSDKUtils.EPOCH_START.AddMilliseconds(millseconds);
            return ret;
        }
    }

    /// <summary>
    /// Unmarshaller for MemoryStream fields
    /// </summary>
    public class MemoryStreamUnmarshaller : IUnmarshaller<MemoryStream, XmlUnmarshallerContext>, IUnmarshaller<MemoryStream, JsonUnmarshallerContext>
    {        
        private MemoryStreamUnmarshaller() { }

        private static MemoryStreamUnmarshaller _instance = new MemoryStreamUnmarshaller();

        public static MemoryStreamUnmarshaller Instance
        {
            get
            {
                return _instance;
            }
        }

        public static MemoryStreamUnmarshaller GetInstance()
        {
            return MemoryStreamUnmarshaller.Instance;
        }

        public MemoryStream Unmarshall(XmlUnmarshallerContext context)
        {
            byte[] bytes = Convert.FromBase64String(context.ReadText());
            MemoryStream stream = new MemoryStream(bytes);
            return stream;
        }

        public MemoryStream Unmarshall(JsonUnmarshallerContext context)
        {
            context.Read();
            if (context.CurrentTokenType == JsonToken.Null)
                return null;

            byte[] bytes = Convert.FromBase64String(context.ReadText());
            MemoryStream stream = new MemoryStream(bytes);
            return stream;
        }
    }

    /// <summary>
    /// Unmarshaller for ResponseMetadata
    /// </summary>
    public class ResponseMetadataUnmarshaller : IUnmarshaller<ResponseMetadata, XmlUnmarshallerContext>, IUnmarshaller<ResponseMetadata, JsonUnmarshallerContext>
    {        
        private ResponseMetadataUnmarshaller() { }

        private static ResponseMetadataUnmarshaller _instance = new ResponseMetadataUnmarshaller();

        public static ResponseMetadataUnmarshaller Instance
        {
            get
            {
                return _instance;
            }
        }

        public static ResponseMetadataUnmarshaller GetInstance()
        {
            return ResponseMetadataUnmarshaller.Instance;
        }

        public ResponseMetadata Unmarshall(XmlUnmarshallerContext context)
        {
            ResponseMetadata metadata = new ResponseMetadata();

            int depth = context.CurrentDepth;

            while (depth <= context.CurrentDepth)
            {
                context.Read();
                if (context.IsStartElement)
                {
                    if (context.TestExpression("ResponseMetadata/RequestId"))
                        metadata.RequestId = StringUnmarshaller.GetInstance().Unmarshall(context);
                    else
                        metadata.Metadata.Add(context.CurrentPath.Substring(context.CurrentPath.LastIndexOf('/') + 1), StringUnmarshaller.GetInstance().Unmarshall(context));
                }
            }

            return metadata;
        }
        public ResponseMetadata Unmarshall(JsonUnmarshallerContext context)
        {
            ResponseMetadata metadata = new ResponseMetadata();
            int depth = context.CurrentDepth;

            while (context.CurrentDepth >= depth)
            {
                context.Read();
                if (context.TestExpression("ResponseMetadata/RequestId"))
                {
                    metadata.RequestId = StringUnmarshaller.GetInstance().Unmarshall(context);
                }
            }

            return metadata;
        }
    }

    public class KeyValueUnmarshaller<K, V, KUnmarshaller, VUnmarshaller> :
    IUnmarshaller<KeyValuePair<K, V>, XmlUnmarshallerContext>, 
    IUnmarshaller<KeyValuePair<K, V>, JsonUnmarshallerContext>
        where KUnmarshaller : IUnmarshaller<K, XmlUnmarshallerContext>, IUnmarshaller<K, JsonUnmarshallerContext>
        where VUnmarshaller : IUnmarshaller<V, XmlUnmarshallerContext>, IUnmarshaller<V, JsonUnmarshallerContext>
    {
        private KUnmarshaller keyUnmarshaller;
        private VUnmarshaller valueUnmarshaller;

        public KeyValueUnmarshaller(KUnmarshaller keyUnmarshaller, VUnmarshaller valueUnmarshaller)
        {
            this.keyUnmarshaller = keyUnmarshaller;
            this.valueUnmarshaller = valueUnmarshaller;
        }

        public KeyValuePair<K, V> Unmarshall(XmlUnmarshallerContext context)
        {
            K key = default(K);
            V value = default(V);

            int originalDepth = context.CurrentDepth;
            int targetDepth = originalDepth + 1;

            while (context.Read())
            {
                if (context.TestExpression("key", targetDepth))
                {
                    key = this.keyUnmarshaller.Unmarshall(context);
                }
                else if (context.TestExpression("name", targetDepth))
                {
                    key = this.keyUnmarshaller.Unmarshall(context);
                }
                else if (context.TestExpression("value", targetDepth))
                {
                    value = this.valueUnmarshaller.Unmarshall(context);
                }
                else if (context.IsEndElement && context.CurrentDepth < originalDepth)
                {
                    break;
                }
            }

            return new KeyValuePair<K, V>(key, value);
        }

        public KeyValuePair<K, V> Unmarshall(JsonUnmarshallerContext context)
        {
            K key = this.keyUnmarshaller.Unmarshall(context);
            V value = this.valueUnmarshaller.Unmarshall(context);

            return new KeyValuePair<K, V>(key, value);
        }
    }

    public class ListUnmarshaller<I, IUnmarshaller> : IUnmarshaller<List<I>, XmlUnmarshallerContext>, IUnmarshaller<List<I>, JsonUnmarshallerContext>
        where IUnmarshaller : IUnmarshaller<I, XmlUnmarshallerContext>, IUnmarshaller<I, JsonUnmarshallerContext>
    {
        private IUnmarshaller iUnmarshaller;

        public ListUnmarshaller(IUnmarshaller iUnmarshaller)
        {
            this.iUnmarshaller = iUnmarshaller;
        }

        public List<I> Unmarshall(XmlUnmarshallerContext context)
        {
            int originalDepth = context.CurrentDepth;
            int targetDepth = originalDepth + 1;

            var list = new List<I>();
            while (context.Read())
            {
                if (context.IsStartElement)
                {
                    if (context.TestExpression("member", targetDepth))
                    {
                        var item = iUnmarshaller.Unmarshall(context);
                        list.Add(item);
                    }
                }
            }
            return list;
        }
        public List<I> Unmarshall(JsonUnmarshallerContext context)
        {
            context.Read(); // Read [ or null
            if (context.CurrentTokenType == JsonToken.Null)
                return new List<I>();

            // If a list is present in the response, use AlwaysSendList,
            // so if the response was empty, reusing the object in the request we will
            // end up sending the same empty collection back.
            List<I> list = new AlwaysSendList<I>();
            while (!context.Peek(JsonToken.ArrayEnd)) // Peek for ]
            {
                list.Add(iUnmarshaller.Unmarshall(context));
            }
            context.Read(); // Read ]
            return list;
        }
    }

    public class DictionaryUnmarshaller<TKey, TValue, TKeyUnmarshaller, TValueUnmarshaller> : IUnmarshaller<Dictionary<TKey, TValue>, XmlUnmarshallerContext>, IUnmarshaller<Dictionary<TKey, TValue>, JsonUnmarshallerContext>
        where TKeyUnmarshaller : IUnmarshaller<TKey, XmlUnmarshallerContext>, IUnmarshaller<TKey, JsonUnmarshallerContext>
        where TValueUnmarshaller : IUnmarshaller<TValue, XmlUnmarshallerContext>, IUnmarshaller<TValue, JsonUnmarshallerContext>
    {
        private KeyValueUnmarshaller<TKey, TValue, TKeyUnmarshaller, TValueUnmarshaller> KVUnmarshaller;

        public DictionaryUnmarshaller(TKeyUnmarshaller kUnmarshaller, TValueUnmarshaller vUnmarshaller)
        {
            KVUnmarshaller = new KeyValueUnmarshaller<TKey, TValue, TKeyUnmarshaller, TValueUnmarshaller>(kUnmarshaller, vUnmarshaller);
        }                

        public Dictionary<TKey, TValue> Unmarshall(XmlUnmarshallerContext context)
        {
            var originalDepth = context.CurrentDepth;
            var targetDepth = originalDepth + 1;

            // If a dictionary is present in the response, use AlwaysSendDictionary,
            // so if the response was empty, reusing the object in the request we will
            // end up sending the same empty collection back.
            var dictionary = new AlwaysSendDictionary<TKey, TValue>();
            
            while (context.Read())
            {
                if (context.IsEndElement && context.CurrentDepth < originalDepth)
                {
                    break;
                }

                var item = KVUnmarshaller.Unmarshall(context);
                dictionary.Add(item.Key, item.Value);
            }                        
            
            return dictionary;
        }

        public Dictionary<TKey, TValue> Unmarshall(JsonUnmarshallerContext context)
        {
            context.Read(); // Read { or null
            if (context.CurrentTokenType == JsonToken.Null)
                return new Dictionary<TKey,TValue>();

            // If a dictionary is present in the response, use AlwaysSendDictionary,
            // so if the response was empty, reusing the object in the request we will
            // end up sending the same empty collection back.
            Dictionary<TKey, TValue> dictionary = new AlwaysSendDictionary<TKey, TValue>();
            while (!context.Peek(JsonToken.ObjectEnd)) // Peek }
            {
                KeyValuePair<TKey, TValue> item = KVUnmarshaller.Unmarshall(context);
                dictionary.Add(item.Key, item.Value);
            }
            context.Read(); // Read }
            return dictionary;

        }
    }

    public static class UnmarshallerExtensions
    {
        public static void Add<TKey, TValue>(this Dictionary<TKey, TValue> dict, KeyValuePair<TKey, TValue> item)
        {
            dict.Add(item.Key, item.Value);
        }
        //public static void Add<T>(this List<T> list, List<T> items)
        //{
        //    list.AddRange(items);
        //}
    }
}
