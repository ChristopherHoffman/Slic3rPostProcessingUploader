using System.Globalization;
using System.Text;

namespace Slic3rPostProcessingUploader.Services
{
    internal class TitleService
    {
        public string GetTitle(string? filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return "";
            }

            string snakeCaseFilename = ToSnakeCase(filename);
            string title = string.Join(" ", snakeCaseFilename.Split('_')
                .Where(segment => !segment.Equals("gcode", StringComparison.OrdinalIgnoreCase))
                .Select(CultureInfo.CurrentCulture.TextInfo.ToTitleCase))
                .Trim();

            return title.Length > 100 ? title[..100] : title;
        }

        public string ToSnakeCase(string? text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }

            if (text.Length < 2)
            {
                return text.ToLowerInvariant();
            }

            StringBuilder sb = new();
            sb.Append(char.ToLowerInvariant(text[0]));
            for (int i = 1; i < text.Length; ++i)
            {
                char c = text[i];
                if (char.IsUpper(c))
                {
                    sb.Append('_');
                    sb.Append(char.ToLowerInvariant(c));
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}
