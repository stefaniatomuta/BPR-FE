namespace BPRBE.Tests;

[TestFixture]
internal class Temporary
{
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
    }

    [SetUp]
    public void SetUp()
    {
    }

    [TearDown]
    public void TearDown()
    {
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
    }

    [Test]
    public void MethodName_WhenSomething_ThenSomething()
    {
        Assert.Pass();
    }
}
