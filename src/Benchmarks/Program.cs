using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using BenchmarkDotNet.Running;
using Benchmarks.AwsDdbSdk.Benchmarks;
using Benchmarks.AwsDdbSdk.Benchmarks.Deserialization;
using Benchmarks.AwsDdbSdk.Benchmarks.Deserialization.Models;
using Benchmarks.AwsDdbSdk.Entities;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.DocumentModel.Extensions;

namespace Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(Assembly.GetExecutingAssembly()).Run();
        }
    }
}