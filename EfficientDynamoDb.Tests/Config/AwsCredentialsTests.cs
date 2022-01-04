using System;
using System.Collections.Generic;
using EfficientDynamoDb.Configs;
using EfficientDynamoDb.Configs.Retries;
using EfficientDynamoDb.Internal.Core.Utilities;
using NUnit.Framework;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace EfficientDynamoDb.Tests.Config.Retries
{
    [TestFixture]
    public class AwsCredentialsTests
    {
        [SetUp]
        public void ClearEnvironmentVariables()
        {
            var keys = new[]
             {
                    "AWS_ACCESS_KEY_ID",
                    "AWS_SECRET_ACCESS_KEY",
                    "AWS_SESSION_TOKEN",
                };
            foreach (var key in keys)
            {
                Environment.SetEnvironmentVariable(key, null, EnvironmentVariableTarget.Process);
            }

        }

        [Test]
        public void GetFromEnvironmentTest()
        {
            var keys = new[]
             {
                    "AWS_ACCESS_KEY_ID",
                    "AWS_SECRET_ACCESS_KEY"
                };
            foreach (var key in keys)
            {
                Environment.SetEnvironmentVariable(key, "TEST###" + key, EnvironmentVariableTarget.Process);
            }

            var result = AwsCredentials.GetAwsCredentials();

            Assert.NotNull(result);
            Assert.Equals("TEST###" + keys[0], result.AccessKey);
            Assert.Equals("TEST###" + keys[1], result.SecretKey);
            Assert.Null(result.Token);
        }


        [Test]
        public void GetFromEnvironmentTestWithToken()
        {
            var keys = new[]
             {
                    "AWS_ACCESS_KEY_ID",
                    "AWS_SECRET_ACCESS_KEY",
                    "AWS_SESSION_TOKEN",
                };
            foreach (var key in keys)
            {
                Environment.SetEnvironmentVariable(key, "TEST###" + key, EnvironmentVariableTarget.Process);
            }

            var result = AwsCredentials.GetAwsCredentials();

            Assert.NotNull(result);
            Assert.Equals("TEST###" + keys[0], result.AccessKey);
            Assert.Equals("TEST###" + keys[1], result.SecretKey);
            Assert.Equals("TEST###" + keys[2], result.Token);
        }

        [Test]
        public void GetNullFromEnvironment()
        {
            Assert.Null(AwsCredentials.GetAwsCredentials());
            var result = AwsCredentials.GetAwsCredentials();
        }

    }

}