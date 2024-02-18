namespace BO;

using System.Collections;
using System.Reflection;
public static class Tools
{
    
    //public static string ToStringProperty<T>(this T t)
    //{
    //    string str = "";
    //    if (t is IEnumerable)
    //    {
    //        foreach (var item in (IEnumerable)t)
    //        {
    //            str += item.ToString() + "\n";
    //        }
    //    }
    //    else
    //    {
    //        foreach (PropertyInfo item in t!.GetType().GetProperties())
    //        {
    //            str += "\n" + item.Name + ": " + item.GetValue(t, null);
    //        }
    //    }
    //    return str;
    //}
    public static string ToStringProperty<T>(this T t, string suffix = "")
    {
        string str = "";
        foreach (PropertyInfo prop in t!.GetType().GetProperties())
        {

            var value = prop.GetValue(t, null);

            if (value is IEnumerable)
            {
                str += "\n" + suffix + prop.Name + ": ";
                foreach (var item in (IEnumerable)value)
                {
                    str += item.ToStringProperty(" ");

                }
                if (value is string)
                    str += value;
            }
            else
                str += "\n" + suffix + prop.Name + ": " + value;
        }
        return str;
    }

}    
