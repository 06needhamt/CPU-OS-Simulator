
namespace CPU_OS_Simulator.Console
{
    /// <summary>
    /// This interface represents a attribute with a single value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAttribute<T> where T : struct 
    {
        /// <summary>
        /// The value of the attribute
        /// </summary>
         T Value { get; set; }
    }
}