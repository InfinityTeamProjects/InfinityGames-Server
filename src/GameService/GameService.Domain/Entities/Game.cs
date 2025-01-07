using System.ComponentModel.DataAnnotations;

namespace GameService.Domain.Entities;

public class Game
{
    public Guid Id { get; set; }
    [MaxLength(100, ErrorMessage = "Название игры превышает порог максимально допустимых символов!")]
    public string Name { get; set; }
    public string Poster { get; set; }
    public float Price { get; set; }
    public Guid? DiscountId { get; set; }
    public string Trailer { get; set; }
    public IEnumerable<string> Photos { get; set; }
    [MaxLength(800, ErrorMessage = "Описание игры не должно быть таким большим!")]
    public string Description { get; set; }
    [MaxLength(100, ErrorMessage = "Название разработчика превышает порог максимально допустимых символов!")]
    public string Developer { get; set; }
    public DateTime ReleaseDate { get; set; }
    public IEnumerable<string> Genres { get; set; }
    public IEnumerable<string> Tags { get; set; }
    public ICollection<Guid>? SystemRequirementsIds { get; set; }
}
