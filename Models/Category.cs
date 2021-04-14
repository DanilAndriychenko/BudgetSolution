using System;

namespace Models
{
    public class Category
    {
        private string Name { get; }
        private Color Color { get; }
        private Icon Icon { get; }
        private string Description { get; }

        public Category(string name, Color color = Color.Gray, Icon icon = Icon.Uncategorized, string description = "")
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Color = color;
            Icon = icon;
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        public override string ToString()
        {
            return Name;
        }
    }
}