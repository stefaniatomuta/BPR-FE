using BPR.Model.Requests;

namespace BPR.Tests;

[TestFixture]
internal class MLAnalysisRequestModelTests
{
    [Test]
    public void Constructor_WhenPathContainsBackSlashes_ThenTheyAreReplacedWithForwardSlash()
    {
        // Arrange
        var path = "C:\\This\\Is\\My\\Path";

        // Act
        var model = new MLAnalysisRequestModel(path, new List<string>());

        // Assert
        var expectedPath = path.Replace("\\", "/");

        Assert.That(model.Path, Is.Not.EqualTo(path));
        Assert.That(model.Path, Is.EqualTo(expectedPath));
    }
}