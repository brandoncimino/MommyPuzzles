namespace MommyPuzzles;

/// <summary>
/// Replace the code inside of the "Add" method so that it returns the sum of "a" and "b".
/// </summary>
public class Challenge_1
{
    public int Add(int a, int b)
    {
        throw new NotImplementedException();
    }

    #region Tests

    /// <summary>
    /// This code checks to see if "Add" was written properly.
    ///
    /// You can execute it by clicking on the green arrow arrow next to its name or the green arrow next to
    /// "public class Challenge_1" at the top of the file, and then selecting "Run All".
    /// </summary>
    [Test]
    [TestCase(0, 0)]
    [TestCase(1, 2)]
    [TestCase(-1, -5)]
    public void Challenge_1_Test(int a, int b)
    {
        Backstage.Check(Add, (a, b), Is.EqualTo(a + b));
    }

    #endregion
}