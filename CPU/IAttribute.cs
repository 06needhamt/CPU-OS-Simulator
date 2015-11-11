namespace CPU_OS_Simulator.CPU
{
    /// <summary>
    /// This interface defines a basic attribute
    /// </summary>
    /// <typeparam name="T"> The type of the attribute </typeparam>
    internal interface IAttribute<T>
    {
        T Value { get; }
    }
}