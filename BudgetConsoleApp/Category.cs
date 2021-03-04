using System;

namespace Budget
{
    public class Category
    {
        private string Name { get; }
        private string Description { get; }
        private Color Color { get; }
        private Icon Icon { get; }

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

        public override bool Equals(object? obj)
        {
            Category category = obj as Category;
            if (category == null)
                return false;
            else
                return Name.Equals(category.Name);
        }
            
    }
}