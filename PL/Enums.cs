using System.Collections;
namespace PL;

internal class Enums
{

}
internal class EngineerExperienceCollection : IEnumerable //enum without the option None
{
    static readonly IEnumerable<BO.EngineerExperience> s_enums =
      (Enum.GetValues(typeof(BO.EngineerExperience)) as IEnumerable<BO.EngineerExperience>)!;

    public IEnumerator GetEnumerator()
    {
        foreach (var item in s_enums)
        {
            if (item == BO.EngineerExperience.None)
                continue;
            yield return item;
        }
    }
}
internal class FilterEngineerExperienceCollection : IEnumerable
{
    static readonly IEnumerable<BO.EngineerExperience> s_enums =
      (Enum.GetValues(typeof(BO.EngineerExperience)) as IEnumerable<BO.EngineerExperience>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}
