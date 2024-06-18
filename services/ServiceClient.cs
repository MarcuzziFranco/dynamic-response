
using dynamic_response.models;
namespace dynamic_response.services;


public class ServiceClient
{
    private List<ClientData> listClient = [];
    public ServiceClient()
    {
        LoadClientData();
    }

    private void LoadClientData(){
        listClient = 
        [
            new () { Id = "0", Name = "Juan", LastName = "Perez", DocumentNumber = "12345678", Direction = new Direction { Street = "mermelada", StreetNumber = "1234" } },
            new () { Id = "1", Name = "Mirian", LastName = "Bregman", DocumentNumber = "8795456", Direction = new Direction { Street = "Afuera", StreetNumber = "78452" } },
            new () { Id = "2", Name = "Carlos", LastName = "Lopez", DocumentNumber = "45678912", Direction = new Direction { Street = "Santa Fe", StreetNumber = "5678" } },
            new () { Id = "3", Name = "Ana", LastName = "Garcia", DocumentNumber = "98765432", Direction = new Direction { Street = "Corrientes", StreetNumber = "3456" } },
            new () { Id = "4", Name = "Luis", LastName = "Martinez", DocumentNumber = "32165498", Direction = new Direction { Street = "San Martin", StreetNumber = "7890" } },
            new () { Id = "5", Name = "Elena", LastName = "Rodriguez", DocumentNumber = "65498721", Direction = new Direction { Street = "Belgrano", StreetNumber = "123" } },
            new () { Id = "6", Name = "Marcos", LastName = "Gonzalez", DocumentNumber = "78912345", Direction = new Direction { Street = "Rivadavia", StreetNumber = "4567" } },
            new () { Id = "7", Name = "Laura", LastName = "Sanchez", DocumentNumber = "32178945", Direction = new Direction { Street = "Mitre", StreetNumber = "8910" } },
            new () { Id = "8", Name = "Pedro", LastName = "Diaz", DocumentNumber = "14785236", Direction = new Direction { Street = "Alsina", StreetNumber = "1112" } },
            new () { Id = "9", Name = "Carmen", LastName = "Fernandez", DocumentNumber = "96385274", Direction = new Direction { Street = "Sarmiento", StreetNumber = "1314" } }
        ];
    }

    public ClientData GetClientById(string id){
        return listClient.FirstOrDefault(client => client.Id == id);
    }

    public List<ClientData> GetClientAll(){
        return listClient;
    }
}
