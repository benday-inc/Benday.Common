namespace Benday.Common.UnitTests.MockingUtilities;

public class ClassWithAbstractDependency
{
    public ClassWithAbstractDependency(TimeProvider clock)
    {
        Clock = clock;
    }

    public TimeProvider Clock { get; private set; }
}
