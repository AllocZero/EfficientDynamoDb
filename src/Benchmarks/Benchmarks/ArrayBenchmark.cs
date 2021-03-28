using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using EfficientDynamoDb.DocumentModel;

namespace Benchmarks
{
    public class ArrayBenchmark
    {
        [Params(100000)]
        public int Iterations;
        
        private AttributeValue[] _mapInput;
        private AttributeValue[] _listInput;
        private AttributeValue[] _nullListInput;

        [GlobalSetup]
        public void Setup()
        {
            var random = new Random();
            _mapInput = Enumerable.Range(0, 100000).Select(_ => new AttributeValue(new MapAttributeValue(new Document()))).ToArray();
            _listInput = Enumerable.Range(0, 100000)
                .Select(_ => new AttributeValue(new ListAttributeValue(new List<AttributeValue> {new AttributeValue(new MapAttributeValue(new Document()))}))).ToArray();
            _nullListInput = Enumerable.Range(0, 100000)
                .Select(_ => new AttributeValue(new ListAttributeValue(new List<AttributeValue> {new AttributeValue(new MapAttributeValue(null))}))).ToArray();
        }

        [Benchmark]
        public Document MapCopy()
        {
            var array = new AttributeValue[Iterations];
            
            for (var i = 0; i < Iterations; i++)
            {
                array[i] = _mapInput[i];
            }

            return CreateDocument(array);
        }
        
        [Benchmark]
        public Document ListCopy()
        {
            var array = new AttributeValue[Iterations];
            
            for (var i = 0; i < Iterations; i++)
            {
                array[i] = _listInput[i];
            }

            return CreateDocument(array);
        }
        
        [Benchmark]
        public Document NullListCopy()
        {
            var array = new AttributeValue[Iterations];
            
            for (var i = 0; i < Iterations; i++)
            {
                array[i] = _nullListInput[i];
            }

            return CreateDocument(array);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Document CreateDocument(AttributeValue[] array)
        {
            var document = new Document(Iterations);
            for (var i = 0; i < Iterations; i++)
                document.Add(i.ToString(), array[i]);

            return document;
        }
    }
}