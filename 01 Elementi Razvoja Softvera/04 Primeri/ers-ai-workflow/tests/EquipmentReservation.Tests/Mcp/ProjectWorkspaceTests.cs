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
        Assert.That(_reader.ReadText("README.md"), Is.EqualTo("# Teaching example"));
    }

    [TestCase(".env")]
    [TestCase(".git/config.json")]
    [TestCase("src/bin/Program.cs")]
    [TestCase("notes.txt")]
    [TestCase("../outside.md")]
    public void ReadText_WhenPathIsNotExposed_DeniesAccess(string relativePath)
    {
        Action read = () => _reader.ReadText(relativePath);

        Assert.That(read, Throws.TypeOf<ProjectAccessDeniedException>());
    }

    [Test]
    public void ReadText_WhenPathIsAbsolute_DeniesAccess()
    {
        var absolutePath = Path.Combine(_rootPath, "README.md");

        Action read = () => _reader.ReadText(absolutePath);

        Assert.That(read, Throws.TypeOf<ProjectAccessDeniedException>());
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
