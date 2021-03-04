---
id: design
title: Design Principles
sidebar_label: Design Principles
slug: ../design
---

**EfficientDynamoDb** is built with performance and scalability in mind. Our main goals are:

* Reducing CPU cycles required to parse and serialize DynamoDB JSON.
* Reducing memory consumed by typical operations.

## Raw Performance

Major design decisions that significantly improve performance:

1. Direct DynamoDB JSON conversion to C# objects without intermediate entities.
1. Custom low-level `System.Text.Json` serializer and deserializer thoroughly tuned for DynamoDB syntax.
1. Using high-performance low-level C# features like spans, ref structs, stackallocs even for non-critical code paths.

## RAM Usage and GC Pressure

Allocations are slow. GC collections are slow.
We mitigate both issues by keeping allocations count as low as possible.

Preeminent solutions that help with memory pressure:

1. Excessive use of array pools for large arrays.
1. `stackalloc` for small arrays when applicable.
1. Immutable fluent API is specifically designed to allocate only set properties instead of having a single "god object."
1. Not reading redundant info from DDB responses, e.g., repeated data types and attribute names.

## Benchmarks

Data must back every performance decision.
Benchmarking is the only way to tell which solution is faster in a given context.

We use the superb [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet) to ensure that performance improves with every new version.
