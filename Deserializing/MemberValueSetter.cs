using System.Reflection;

namespace CsvParserCli.Deserializing;

internal class MemberValueSetter
{
    protected internal void Set<T>(T obj, MemberInfo member, string value)
    {
        try
        {
            object convertedValue;
            Type memberType;

            if (member is PropertyInfo property)
            {
                memberType = property.PropertyType;
            }
            else if (member is FieldInfo field)
            {
                memberType = field.FieldType;
            }
            else
            {
                throw new InvalidOperationException("Member is neither a property nor a field.");
            }

            if (memberType == typeof(string))
            {
                convertedValue = value;
            }
            else if (memberType.IsEnum)
            {
                convertedValue = Enum.Parse(memberType, value, true);
            }
            else
            {
                convertedValue = Convert.ChangeType(value, memberType);
            }

            if (member is PropertyInfo prop)
            {
                prop.SetValue(obj, convertedValue);
            }
            else if (member is FieldInfo field)
            {
                field.SetValue(obj, convertedValue);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error setting member {member.Name}: {ex.Message}");
        }
    }
}
