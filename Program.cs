using System.Net;
using dynamic_response.models;
using dynamic_response.services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

ServiceTemplate serviceTemplate = new();
ServiceClient serviceClient = new();
ServicePropertyPath servicePropertyPath= new();


app.UseSwagger();
app.MapGet("/", () => "Hello World!");

app.MapPost("/template", async(Template template) =>
{
    serviceTemplate.AddTemplate(template);
    return HttpStatusCode.Created;
});

app.MapGet("/template/{id}", async(string id) => 
{
    return serviceTemplate.GetTemplateById(id);
});

app.MapGet("/template-all", async() => 
{
    return serviceTemplate.GetTemplates();
});

app.MapGet("/client-all", async () => 
{   
    return serviceClient.GetClientAll();
});

app.MapGet("/client-path", async () => 
{   
    
    return servicePropertyPath.GetPropertyPaths(typeof(ClientData), "");
});

app.MapGet("/client/{id}", async (string id) => 
{
    return serviceClient.GetClientById(id);
});

app.MapGet("/client-by-template/{id}", async (string id,string? templateId) => 
{
    ClientData  clientData = serviceClient.GetClientById(id);
    if(clientData != null){
        //Create clientData by template id.
        var result = serviceTemplate.ApplyTemplate(clientData, templateId);
        return result;
    }else{
        return null;
    }
});



app.UseSwaggerUI();
app.Run();
