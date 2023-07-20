using System;
using System.Linq;

namespace SD_Images.GUI.Model.Entities
{
    public record Size
    {
        public Size() { }

        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public Size(int size) 
            => Width = Height = size;

        /// <param name="size">Format height x width. Example: 1024x512</param>
        public Size(string size)
        {
            var tokens = size.Split('x', 'X', StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim())
                .ToArray();

            if(tokens.Length != 2)
                throw new ArgumentException("Wrong size format.", nameof(size));

            Width = Convert.ToInt32(tokens.First());
            Height = Convert.ToInt32(tokens.Last());
        }


        public int Width { get; init; } 
        public int Height { get; init; }
    }
}
