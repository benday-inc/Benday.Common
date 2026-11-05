namespace Benday.Common.UnitTests.MockingUtilities;

public class ConcreteDependency
{
    public virtual string GetValue() => "real value";
}

public class ClassWithConcreteDependency
{
    public ClassWithConcreteDependency(ConcreteDependency dependency)
    {
        Dependency = dependency;
    }

    public ConcreteDependency Dependency { get; private set; }
}
