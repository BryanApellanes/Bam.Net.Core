using System.IO;
using Bam.Net.Presentation.Drawing;

namespace Bam.Net.Yaml
{
    public static class PaletteExtensions
    {
        public static void AddYamlLoader()
        {
            ColorPalette.AddLoader(".yaml", path => File.ReadAllText(path).FromYaml<ColorPalette>());
        }

        public static void AddYamlSaver(this ColorPalette palette)
        {
            palette.AddSaver(".yaml", (pal, path, overwrite) => pal.ToYaml().SafeWriteToFile(path, overwrite));
        }
    }
}
