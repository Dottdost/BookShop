namespace BookShop.Data.Models;

public class BookAttribute
{
    public Guid Id { get; set; } = Guid.NewGuid(); // Идентификатор атрибута
    public string Name { get; set; } // Название атрибута (например, "Язык", "Формат")

    // Навигационное свойство для связи с BookAttributeValue
    public ICollection<BookAttributeValue> BookAttributeValues { get; set; } = new List<BookAttributeValue>();
}