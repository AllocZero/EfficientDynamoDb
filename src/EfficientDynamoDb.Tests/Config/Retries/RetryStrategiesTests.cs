using System;
using EfficientDynamoDb.Configs.Retries;
using EfficientDynamoDb.Internal.Core.Utilities;
using NUnit.Framework;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace EfficientDynamoDb.Tests.Config.Retries
{
    [TestFixture]
    public class RetryStrategiesTests
    {
        [TestCase(0, 1, ExpectedResult = false)]
        [TestCase(2, 3, ExpectedResult = false)]
        public bool JitterRetryStrategy_RetriesCountExceeded(int maxAttemptsCount, int currentAttempt)
        {
            var strategy = RetryStrategyFactory.Jitter(2);

            return strategy.TryGetRetryDelay(3, out _);
        }

        [TestCaseSource(nameof(JitterTestCases))]
        public TimeSpan JitterRetryStrategy_DelayReturned(int maxRetryAttempts, int baseDelayMs, int maxDelayMs, int currentAttempt)
        {
            var randomMock = Mock.Create<IRandom>(Behavior.Strict);
            randomMock.Arrange(x => x.Next(Arg.AnyInt)).Returns(maxValue => maxValue).OccursOnce();

            var strategy = new JitterRetryStrategy(randomMock, maxRetryAttempts, baseDelayMs, maxDelayMs);

            var shouldRetry = strategy.TryGetRetryDelay(currentAttempt, out var delay);
            
            Assert.That(shouldRetry, Is.True);
            randomMock.Assert();

            return delay;
        }

        private static readonly TestCaseData[] JitterTestCases =
        {
            new TestCaseData(10, 50, 1_000, 0) {ExpectedResult = TimeSpan.FromMilliseconds(50)},
            new TestCaseData(10, 50, 1_000, 1) {ExpectedResult = TimeSpan.FromMilliseconds(100)},
            new TestCaseData(10, 50, 1_000, 4) {ExpectedResult = TimeSpan.FromMilliseconds(800)},
            new TestCaseData(10, 50, 1_000, 5) {ExpectedResult = TimeSpan.FromMilliseconds(1000)},
            new TestCaseData(10, 50, 1_000, 9) {ExpectedResult = TimeSpan.FromMilliseconds(1000)}
        };
    }
}