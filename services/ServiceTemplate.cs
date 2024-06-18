using System.Reflection;
using System.Text.Json;
using dynamic_response.models;

namespace dynamic_response.services;

public class ServiceTemplate
{
    private List<Template>? listTemplates = [];
    public ServiceTemplate()
    {
        LoadTemplateList();
    }

    public void LoadTemplateList()
    {
        listTemplates =
        [
            new() { Id = "TEMPLATE_1", TemplateProperties = new Dictionary<string, object> { { "additionalProp1", "Name" }, { "additionalProp2", "LastName" } } },
            new() { Id = "TEMPLATE_2", TemplateProperties = new Dictionary<string, object> { { "additionalProp1", "DocumentNumber" }, { "additionalProp2", "Direction/Street" } } },
            new() { Id = "TEMPLATE_3", TemplateProperties = new Dictionary<string, object> { { "additionalProp1", "Direction/StreetNumber" }, { "additionalProp2", "Direction" } } },
            new() { Id = "TEMPLATE_4", TemplateProperties = new Dictionary<string, object> { { "additionalProp1", "Name" }, { "additionalProp2", "Direction/Street" }, { "additionalProp3", "DocumentNumber" } } },
            new() { Id = "TEMPLATE_5", TemplateProperties = new Dictionary<string, object> { { "additionalProp1", "LastName" }, { "additionalProp2", "DocumentNumber" }, { "additionalProp3", "Direction/StreetNumber" } } },
            new() { Id = "TEMPLATE_6", TemplateProperties = new Dictionary<string, object> { { "additionalProp1", "Name" }, { "additionalProp2", "Direction" }, { "additionalProp3", "Direction/Street" } } },
            new() { Id = "TEMPLATE_7", TemplateProperties = new Dictionary<string, object> { { "additionalProp1", "Direction/Street" }, { "additionalProp2", "Direction/StreetNumber" }, { "additionalProp3", "DocumentNumber" }, { "additionalProp4", "LastName" } } },
            new() { Id = "TEMPLATE_8", TemplateProperties = new Dictionary<string, object> { { "additionalProp1", "Name" }, { "additionalProp2", "LastName" }, { "additionalProp3", "DocumentNumber" }, { "additionalProp4", "Direction/Street" }, { "additionalProp5", "Direction/StreetNumber" } } },
        ];
    }

    public void AddTemplate(Template template)
    {
        if (template != null)
        {
            if (!CheckExistIdTemplate(template.Id))
                listTemplates.Add(template);
        }
    }

    private bool CheckExistIdTemplate(string id)
    {
        return listTemplates.FirstOrDefault((template) => template.Id == id) != null;
    }

    public Template? GetTemplateById(string? id)
    {
        return listTemplates.FirstOrDefault((template) => template.Id == id);
    }

    public List<Template> GetTemplates()
    {
        return listTemplates;
    }

    public Dictionary<string, object> ApplyTemplate(dynamic obj, string? templateId)
    {
        Template? template = GetTemplateById(templateId);

        if (template == null)
        {
            return null;
        }

        var result = new Dictionary<string, object>();
        var objType = obj.GetType();

        foreach (var prop in template.TemplateProperties)
        {
            if (prop.Value.GetType() == typeof(JsonElement))
            {
                HandleJsonElementProperty(result, obj, prop.Key, (JsonElement)prop.Value);
            }
            else
            {
                HandleTemplateExpression(result, obj, prop.Key, (string)prop.Value);
            }
        }

        return result;
    }
    
    //Not implemented for get method rest.
    public Dictionary<string, object> ApplyTemplate(dynamic obj, string? templateId, Template? rawTemplate = null)
    {
        Template? template = null;

        if (templateId != null)
        {
            template = GetTemplateById(templateId);
        }

        if (template == null && rawTemplate != null)
        {
            template = rawTemplate;
        }

        if (template == null)
        {
            return null;
        }

        var result = new Dictionary<string, object>();
        var objType = obj.GetType();

        foreach (var prop in template.TemplateProperties)
        {
            if (prop.Value.GetType() == typeof(JsonElement))
            {
                HandleJsonElementProperty(result, obj, prop.Key, (JsonElement)prop.Value);
            }
            else
            {
                HandleTemplateExpression(result, obj, prop.Key, (string)prop.Value);
            }
        }

        return result;
    }

    private void HandleJsonElementProperty(Dictionary<string, object> result, dynamic obj, string propKey, JsonElement jsonElement)
    {
        if (jsonElement.ValueKind == JsonValueKind.Object)
        {
            var nestedResult = new Dictionary<string, object>();

            foreach (var nestedProp in jsonElement.EnumerateObject())
            {
                var nestedExpression = nestedProp.Value.GetString();
                if (nestedExpression != null)
                {
                    HandleTemplateExpression(nestedResult, obj, nestedProp.Name, nestedExpression);
                }
            }

            result[propKey] = nestedResult;
        }
        else
        {
            var expression = jsonElement.GetString();
            if (expression != null)
            {
                HandleTemplateExpression(result, obj, propKey, expression);
            }
        }
    }

    private void HandleTemplateExpression(Dictionary<string, object> result, dynamic obj, string propKey, string expression)
    {
        if (expression.Contains("+"))
        {
            var parts = expression.Split('+');
            var concatenatedValue = "";

            foreach (var part in parts)
            {
                var trimmedPart = part.Trim();
                var propertyNames = trimmedPart.Split('/');
                var value = GetNestedPropertyValue(obj, propertyNames);

                if (value != null)
                {
                    if (!string.IsNullOrEmpty(concatenatedValue))
                    {
                        concatenatedValue += " ";  // Agregar espacio entre las partes
                    }
                    concatenatedValue += value.ToString();
                }
            }

            result[propKey] = concatenatedValue;
        }
        else
        {
            var propertyNames = expression.Split('/');
            var value = GetNestedPropertyValue(obj, propertyNames);

            if (value != null)
            {
                result[propKey] = value;
            }
        }
    }

    private object? GetNestedPropertyValue(object obj, string[] propertyNames)
    {
        var currentObj = obj;

        foreach (var name in propertyNames)
        {
            if (currentObj == null) return null;

            var propertyInfo = currentObj.GetType().GetProperty(name);
            if (propertyInfo == null) return null;

            currentObj = propertyInfo.GetValue(currentObj);
        }

        return currentObj;
    }

}
