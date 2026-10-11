using EquipmentReservation.Mcp.Workspace;
using NUnit.Framework;

namespace EquipmentReservation.Tests.Mcp;

public sealed class ProjectWorkspaceTests
{
    private string _rootPath = null!;
    private ProjectFileReader _reader = null!;
    private ProjectStructureProvider _structure = null!;

    [SetUp]
    public void SetUp()
    {
        _rootPath = Directory.CreateTempSubdirectory("ers-mcp-").FullName;
        WriteFile("README.md", "# Teaching example");
        WriteFile(".env", "SECRET=1");
        WriteFile("notes.txt", "plain text");
        WriteFile(Path.Combine(".git", "config.json"), "{}");
        WriteFile(Path.Combine("src", "Program.cs"), "// code");
        WriteFile(Path.Combine("src", "bin", "Program.cs"), "// build output");

        var root = new ProjectRoot(_rootPath);
        var pathPolicy = new ProjectPathPolicy(root);
        _reader = new ProjectFileReader(pathPolicy);
        _structure = new ProjectStructureProvider(root, pathPolicy);
    }

    [TearDown]
    public void TearDown() => Directory.Delete(_rootPath, recursive: true);

    [Test]
    public void ReadText_WhenFileIsExposed_ReturnsContent()
    {
        Assert.That(_reader.ReadText("README.md").Content, Is.EqualTo("# Teaching example"));
    }

    [TestCase(".env")]
    [TestCase(".git/config.json")]
    [TestCase("src/bin/Program.cs")]
    [TestCase("notes.txt")]
    [TestCase("../outside.md")]
    public void ReadText_WhenPathIsNotExposed_DeniesAccess(string relativePath)
    {
        var result = _reader.ReadText(relativePath);

        Assert.That(result.Error, Is.EqualTo("ProjectAccessDenied"));
    }

    [Test]
    public void ReadText_WhenPathIsAbsolute_DeniesAccess()
    {
        var absolutePath = Path.Combine(_rootPath, "README.md");

        var result = _reader.ReadText(absolutePath);

        Assert.That(result.Error, Is.EqualTo("ProjectAccessDenied"));
    }

    [TestCase(".env.production")]
    [TestCase("folder/secrets.json")]
    [TestCase("folder/appsettings.secrets.json")]
    public void ReadText_WhenFileIsSecret_DeniesAccess(string fileName)
    {
        WriteFile(fileName, "not for MCP");

        Assert.That(_reader.ReadText(fileName).Success, Is.False);
    }

    [Test]
    public void ListFiles_ReturnsOnlyExposedFiles()
    {
        var files = _structure.ListFiles();

        Assert.That(
            files,
            Is.EquivalentTo(new[] { "notes.txt", "README.md", Path.Combine("src", "Program.cs") }));
    }

    private void WriteFile(string relativePath, string content)
    {
        var fullPath = Path.Combine(_rootPath, relativePath);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
        File.WriteAllText(fullPath, content);
    }
}
