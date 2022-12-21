using System;
using System.Threading.Tasks;
using _InitTest;
using Xunit;

namespace Mastonet.Tests;

public class InstanceTest : MastodonClientTests
{
    [Fact]
    [Obsolete]
    public async Task GetInstanceV1()
    {
        var client = GetTestClient();

        var instance = await client.GetInstance();

        Assert.Equal(InitTest.Instance, instance.Uri);
    }

    [Fact]
    public async Task GetInstanceV2()
    {
        var client = GetTestClient();
        
        var instance = await client.GetInstanceV2();

        Assert.Equal(InitTest.Instance, instance.Domain);
    }
}