namespace dynamic_response.services;

public class ServicePropertyPath
{
    public List<string> GetPropertyPaths(Type type, string prefix = "")
    {
        List<string> paths = [];
        foreach (var property in type.GetProperties())
        {
            string propertyPath = string.IsNullOrEmpty(prefix) ? property.Name : $"{prefix}/{property.Name}";
            paths.Add(propertyPath);

            if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
            {
                paths.AddRange(GetPropertyPaths(property.PropertyType, propertyPath));
            }
        }
        return paths;
    }
}
