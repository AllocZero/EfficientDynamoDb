using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Operations.GetItem;
using NUnit.Framework;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace EfficientDynamoDb.Tests.Examples
{
    [TestFixture(TestOf = typeof(DynamoDbContext))]
    public class DynamoDbContextExampleTests
    {
        [Test]
        public async Task GetItemFluentApiMockingTest()
        {
            var expectedResult = new object();
            var dbContext = Mock.Create<IDynamoDbContext>(Behavior.Strict);
            var builderMock = Mock.Create<IGetItemEntityRequestBuilder<object>>();

            dbContext.Arrange(x => x.GetItem<object>()).Returns(builderMock);

            builderMock.Arrange(x => x.WithPrimaryKey(Arg.AnyString)
                .ToItemAsync(Arg.IsAny<CancellationToken>()))
            #if NET6_0
                .TaskResult(expectedResult);
            #elif NET7_0_OR_GREATER
                .ReturnsAsync(expectedResult);
            #endif
            
            var result = await dbContext.GetItem<object>().WithPrimaryKey("pk").ToItemAsync();

            Assert.That(result, Is.EqualTo(expectedResult));
            builderMock.Assert();
        }
    }
}