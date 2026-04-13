using ProjectMaui.Domain.Common;
using SQLite;

namespace ProjectMaui.Models
{
    public class Category
    {
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; private set; } = default!;
        public string Description { get; private set; } = default!;

        public Category() { }

        public Category(string name, string description)
        {
            Name = Guard.NotNullOrWhiteSpace(name, nameof(name));
            Description = description;
        }
    }
}