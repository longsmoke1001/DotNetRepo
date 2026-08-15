namespace GameApi.Models;
public class Player
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Score { get; set; }
    public int Level { get; set; }
}
