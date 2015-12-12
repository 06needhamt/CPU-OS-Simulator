using System.ComponentModel;
using System.Reflection;

namespace CPU_OS_Simulator.Console
{
    /// <summary>
    /// This class contains extension methods for getting attributes from enum items
    /// </summary>
    public static class Extentions
    {
        /// <summary>
        /// This function gets the description attribute from an enum item
        /// </summary>
        /// <typeparam name="T"> The enum type of the item</typeparam>
        /// <param name="source"> The enum item to get the description from</param>
        /// <returns> The description attribute of the enum item </returns>
        public static int NumberOfParametersAttr<T>(this T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            NumberOfParametersAttribute[] attributes = (NumberOfParametersAttribute[])fi.GetCustomAttributes(typeof(NumberOfParametersAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Value;
            else
                return 0;
        }

    }
}