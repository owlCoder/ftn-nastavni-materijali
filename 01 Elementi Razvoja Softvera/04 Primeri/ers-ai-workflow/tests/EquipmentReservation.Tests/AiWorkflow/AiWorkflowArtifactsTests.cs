using System.Text.Json;
using EquipmentReservation.Mcp.Workspace;
using NUnit.Framework;

namespace EquipmentReservation.Tests.AiWorkflow;

public sealed class AiWorkflowArtifactsTests
{
    private static readonly string Root =
        ProjectRootLocator.Find(TestContext.CurrentContext.TestDirectory).FullPath;

    private static readonly string[] RequiredSkillSections =
        ["## Režim rada", "## Ulazi", "## Postupak", "## Izlaz", "## Ograničenja"];

    private static readonly string[] ScenarioKinds = ["positive", "negative"];

    private static IEnumerable<string> SkillNames() =>
        Directory
            .EnumerateDirectories(Path.Combine(Root, ".kova", "skills"))
            .Select(directory => Path.GetFileName(directory)!);

    private static IEnumerable<string> ScenarioFiles() =>
        Directory
            .EnumerateFiles(Path.Combine(Root, "evals"), "*.json")
            .Select(file => Path.GetFileName(file)!);

    [TestCaseSource(nameof(SkillNames))]
    public void Skill_DeclaresItsNameInputsOutputAndLimits(string skillName)
    {
        var skill = File.ReadAllText(
            Path.Combine(Root, ".kova", "skills", skillName, "SKILL.md"));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(skill, Does.Contain($"name: {skillName}"));
            foreach (var section in RequiredSkillSections)
                Assert.That(skill, Does.Contain(section));
        }
    }

    [TestCaseSource(nameof(ScenarioFiles))]
    public void EvalScenario_IsCompleteAndTargetsAnExistingSkill(string fileName)
    {
        using var scenario = ReadScenario(fileName);
        var root = scenario.RootElement;
        var expected = root.GetProperty("expected");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(root.GetProperty("id").GetString(), Is.Not.Empty);
            Assert.That(root.GetProperty("goal").GetString(), Is.Not.Empty);
            Assert.That(ScenarioKinds, Does.Contain(root.GetProperty("kind").GetString()));
            Assert.That(SkillNames(), Does.Contain(root.GetProperty("skill").GetString()));
            Assert.That(root.GetProperty("mode").GetString(), Is.Not.Empty);
            Assert.That(root.GetProperty("input").EnumerateObject().Count(), Is.Positive);
            Assert.That(expected.GetProperty("must").GetArrayLength(), Is.Positive);
            Assert.That(expected.GetProperty("mustNot").GetArrayLength(), Is.Positive);
        }
    }

    [Test]
    public void EvalScenarios_CoverAtLeastThreeCasesIncludingANegativeOne()
    {
        var kinds = ScenarioFiles()
            .Select(fileName =>
            {
                using var scenario = ReadScenario(fileName);
                return scenario.RootElement.GetProperty("kind").GetString();
            })
            .ToArray();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(kinds, Has.Length.GreaterThanOrEqualTo(3));
            Assert.That(kinds, Does.Contain("negative"));
        }
    }

    private static JsonDocument ReadScenario(string fileName) =>
        JsonDocument.Parse(File.ReadAllText(Path.Combine(Root, "evals", fileName)));
}
