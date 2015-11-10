using System.ComponentModel;
using System.Reflection;

namespace CPU_OS_Simulator.CPU
{
    public static class Extentions
    {
        public static string DescriptionAttr<T>(this T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return source.ToString();
        }

        public static int NumberOfOperandsAttr<T>(this T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());
            NumberOfOperandsAttribute[] attributes = (NumberOfOperandsAttribute[])fi.GetCustomAttributes(typeof(NumberOfOperandsAttribute), false);
            if (attributes != null && attributes.Length > 0)
                return attributes[0].Value;
            else
                return 2;
        } 
    }
}