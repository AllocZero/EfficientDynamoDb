using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel.Constants;
using EfficientDynamoDb.Internal.Constants;

namespace EfficientDynamoDb.DocumentModel.AttributeValues
{
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct NumberSetAttributeValue
    {
        [FieldOffset(0)]
        private readonly HashSet<string> _items;

        public HashSet<string> Items => _items;
        
        public NumberSetAttributeValue(HashSet<string> items)
        {
            _items = items;
        }

        public float[] ToFloatArray()
        {
            var result = new float[_items.Count];

            var i = 0;
            foreach (var item in _items)
                result[i++] = float.Parse(item, CultureInfo.InvariantCulture);

            return result;
        }
        
        public HashSet<float> ToFloatSet()
        {
            var result = new HashSet<float>(_items.Count);
                
            foreach (var item in _items)
                result.Add(float.Parse(item, CultureInfo.InvariantCulture));

            return result;
        }
        
        public int[] ToIntArray()
        {
            var result = new int[_items.Count];

            var i = 0;
            foreach (var item in _items)
                result[i++] = int.Parse(item, CultureInfo.InvariantCulture);

            return result;
        }
        
        public HashSet<int> ToIntSet()
        {
            var result = new HashSet<int>(_items.Count);
                
            foreach (var item in _items)
                result.Add(int.Parse(item, CultureInfo.InvariantCulture));

            return result;
        }
        
        public long[] ToLongArray()
        {
            var result = new long[_items.Count];

            var i = 0;
            foreach (var item in _items)
                result[i++] = long.Parse(item, CultureInfo.InvariantCulture);

            return result;
        }
        
        public HashSet<long> ToLongSet()
        {
            var result = new HashSet<long>(_items.Count);
                
            foreach (var item in _items)
                result.Add(long.Parse(item, CultureInfo.InvariantCulture));

            return result;
        }
        
        public double[] ToDoubleArray()
        {
            var result = new double[_items.Count];

            var i = 0;
            foreach (var item in _items)
                result[i++] = double.Parse(item, CultureInfo.InvariantCulture);

            return result;
        }
        
        public HashSet<double> ToDoubleSet()
        {
            var result = new HashSet<double>(_items.Count);
                
            foreach (var item in _items)
                result.Add(double.Parse(item, CultureInfo.InvariantCulture));

            return result;
        }
        
        public short[] ToShortArray()
        {
            var result = new short[_items.Count];

            var i = 0;
            foreach (var item in _items)
                result[i++] = short.Parse(item, CultureInfo.InvariantCulture);

            return result;
        }
        
        public HashSet<short> ToShortSet()
        {
            var result = new HashSet<short>(_items.Count);
                
            foreach (var item in _items)
                result.Add(short.Parse(item, CultureInfo.InvariantCulture));

            return result;
        }
        
        public byte[] ToByteArray()
        {
            var result = new byte[_items.Count];

            var i = 0;
            foreach (var item in _items)
                result[i++] = byte.Parse(item, CultureInfo.InvariantCulture);

            return result;
        }
        
        public HashSet<byte> ToByteSet()
        {
            var result = new HashSet<byte>(_items.Count);
                
            foreach (var item in _items)
                result.Add(byte.Parse(item, CultureInfo.InvariantCulture));

            return result;
        }

        public void Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName(DdbTypeNames.NumberSet);
            
            writer.WriteStartArray();

            foreach (var item in _items)
                writer.WriteStringValue(item);
            
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
        
        public override string ToString() => $"[{string.Join(", ", _items)}]";
    }
}