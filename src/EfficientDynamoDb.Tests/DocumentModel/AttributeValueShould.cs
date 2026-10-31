using System;
using EfficientDynamoDb.DocumentModel;
using NUnit.Framework;
using Shouldly;

namespace EfficientDynamoDb.Tests.DocumentModel;

[TestFixture]
public class AttributeValueShould
{
    // A packing below the pointer size lowers the struct's alignment below that of its object reference. With Pack = 1, a
    // Nullable<AttributeValue> field read as type None in a self-contained linux-x64 ReadyToRun publish. This test project
    // is not ReadyToRun, so only the attribute can be checked here.
    [Test]
    public void NotAlignBelowItsObjectReference()
    {
        var pack = typeof(AttributeValue).StructLayoutAttribute!.Pack;

        (pack == 0 || pack >= IntPtr.Size).ShouldBeTrue($"Pack is {pack}");
    }
}
