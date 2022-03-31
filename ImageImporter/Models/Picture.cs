using ImageImporter.Application;
using SixLabors.ImageSharp;
using System.ComponentModel.DataAnnotations;

namespace ImageImporter.Models
{
    public class Picture
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(128)]
        public byte[]? HashA { get; set; }
        [MaxLength(128)]
        public byte[]? HashD { get; set; }
        public string? RelativePath { get; set; }


        public override string ToString()
        {
            return $"HashA = '{Convert.ToHexString(HashA, 0, 32)}' Filename = '{RelativePath}'";
        }

        public static Picture? Generate(AppSettings settings, string file)
        {
            IImageInfo imageInfo = Image.Identify(file);

            if (imageInfo != null)
            {
                IImageHashGenerator aHashGenerator = new ImageAHashGenerator();
                IImageHashGenerator dHashGenerator = new ImageDHashGenerator();

                string relativeFile = Path.GetRelativePath(settings.ImportFolder, file);
                var hashA = aHashGenerator.Generate(file);
                var hashD = dHashGenerator.Generate(file);

                Picture pic = new Picture();
                pic.HashA = hashA;
                pic.HashD = hashD;
                pic.RelativePath = relativeFile;
                return pic;
            }

            return null;
        }
    }
}
