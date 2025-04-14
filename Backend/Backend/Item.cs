using System.ComponentModel.DataAnnotations;

namespace Backend;

public class Item(string name, int quantity)
{
    public void Update(Item item)
    {
        Name = item.Name;
        Quantity = item.Quantity;
    }
    
    public int Id { get; init; }

    [Required]
    public string Name { get; set; } = name;

    [Required]
    public int Quantity { get; set; } = quantity;
}