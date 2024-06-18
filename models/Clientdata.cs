namespace dynamic_response.models;

public class ClientData
{

    public ClientData()
    {
        new Direction();
    }

    public string? Id { get; set;}
    public string? Name { get; set;}
    public string? LastName { get; set; }
    public string? DocumentNumber { get; set; }	
    public Direction? Direction { get; set; }

}


public class Direction{
    public string? Street { get; set; }  
    public string? StreetNumber { get; set; }      
}
