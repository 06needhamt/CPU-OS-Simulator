using System.ComponentModel;
using System.Reflection;

namespace CPU_OS_Simulator.CPU
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
        public static string DescriptionAttr<T>(this T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            return source.ToString();
        }

        /// <summary>
        /// This function gets the NumberOfOperands attribute from an enum item
        /// </summary>
        /// <typeparam name="T"> The enum type of the item</typeparam>
        /// <param name="source"> The enum item to get the number of operands from</param>
        /// <returns> The NumberOfOperands attribute of the enum item </returns>
        public static int NumberOfOperandsAttr<T>(this T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());
            NumberOfOperandsAttribute[] attributes = (NumberOfOperandsAttribute[])fi.GetCustomAttributes(typeof(NumberOfOperandsAttribute), false);
            if (attributes != null && attributes.Length > 0)
                return attributes[0].Value;
            return 2;
        }
    }
}