using System.Text.RegularExpressions;
using DialogueSystem.Input;

namespace DialogueSystem.UI {
    public class GlyphProcessor {
        private readonly GlyphMapping mapping;

        private static readonly Regex TokenRegex = new(@"\[(.*?)\]", RegexOptions.Compiled);

        public GlyphProcessor(GlyphMapping mapping) {
            this.mapping = mapping;
        }

        public string Process(string rawText, InputDeviceType device) {
            if (string.IsNullOrEmpty(rawText) || mapping == null) { return rawText; }

            return TokenRegex.Replace(rawText, match => {
                string token = match.Groups[1].Value;
                string spriteName = mapping.GetSpriteName(token, device);
                return string.IsNullOrEmpty(spriteName) 
                    ? match.Value 
                    : $"<sprite name=\"{spriteName}\">";
            });
        }
    }
}