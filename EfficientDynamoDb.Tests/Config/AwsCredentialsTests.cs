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
            Assert.Equals(keys[0], result.AccessKey);
            Assert.Equals(keys[1], result.SecretKey);
            Assert.IsNull(result.Token);
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
            Assert.Equals(keys[0], result.AccessKey);
            Assert.Equals(keys[1], result.SecretKey);
            Assert.Equals(keys[2], result.Token);
        }



        [Test]
        public void GetNullFromEnvironment()
        {
            Assert.IsNull(AwsCredentials.GetAwsCredentials());
            var result = AwsCredentials.GetAwsCredentials();
        }

    }

    


}