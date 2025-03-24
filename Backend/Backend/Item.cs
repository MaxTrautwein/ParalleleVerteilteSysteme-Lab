using System.ComponentModel.DataAnnotations;

namespace Backend;

public class Item
{
    public Item(string name, int quantity)
    {
        Id = _idCounter++;
        Name = name;
        Quantity = quantity;
    }

    public void Update(Item item)
    {
        Name = item.Name;
        Quantity = item.Quantity;
    }

    private static int _idCounter = 1;
    
    public int Id { get;}

    [Required]
    public string Name { get; set; }

    [Required]
    public int Quantity { get; set; }
}