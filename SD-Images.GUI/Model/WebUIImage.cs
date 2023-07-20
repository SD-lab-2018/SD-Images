using CommunityToolkit.Mvvm.ComponentModel;
using MetadataExtractor;
using SD_Images.GUI.Model.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebUITable = System.Collections.Generic.Dictionary<string, string>;

namespace SD_Images.GUI.Model
{
    public class WebUIImage 
    {
        private WebUITable _properties = new();

        public WebUIImage(string filepath)
        {
            if (!File.Exists(filepath))
                throw new FileNotFoundException($"File \"{filepath}\" not found.");
            
            var fi = new FileInfo(filepath);

            Path = fi.FullName;
            Date = fi.LastWriteTime;

            var directories = ImageMetadataReader.ReadMetadata(filepath);
            var text = directories.FirstOrDefault(d => d.Name.ToLower().Contains("text"));

            var data = text?.Tags[0].Description ?? String.Empty;
            _properties = ParseData(data);

            Init(_properties);
        }

        private void Init(WebUITable parsedData)
        {
            var steps = parsedData.GetValueOrDefault("steps");
            var cfgScale = parsedData.GetValueOrDefault("cfg scale");
            var size = parsedData.GetValueOrDefault("size");
            var imageRewardScore = parsedData.GetValueOrDefault("imagerewardscore");

            PositivePrompt = parsedData.GetValueOrDefault("positive");
            NegativePrompt = parsedData.GetValueOrDefault("negative");
            Steps = steps is not null ? Convert.ToInt32(steps) : null;
            Sampler = parsedData.GetValueOrDefault("sampler");
            CFGScale = cfgScale is not null ? (float)Convert.ToDouble(steps) : null;
            Seed = parsedData.GetValueOrDefault("seed");
            ModelHash = parsedData.GetValueOrDefault("model hash");
            Size = size is not null ? new Size(size) : null;
            Model = parsedData.GetValueOrDefault("model");
            Version = parsedData.GetValueOrDefault("version");
            ImageRewardScore = imageRewardScore is not null ? (float)Convert.ToDouble(imageRewardScore) : null;
        }

        public string Path { get; private set; }
        public DateTime Date { get; private set; }

        public string? PositivePrompt { get; private set; }
        public string? NegativePrompt { get; private set; }
        public int? Steps { get; private set; }
        public string? Sampler { get; private set; }
        public float? CFGScale { get; private set; }
        public string? Seed { get; private set; }
        public Size? Size { get; private set; }
        public string? ModelHash { get; private set; }
        public string? Model { get; private set; }
        public string? Version { get; private set; }
        public float? ImageRewardScore { get; private set; }

        public IReadOnlyDictionary<string, string> Properties => _properties;

        private static WebUITable ParseData(string data)
        {
            var result = new WebUITable();
            var lines = data.Split("\n", StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length < 2)
            {
                return result;
            }

            result.Add("positive", lines[0][11..].Trim());
            result.Add("negative", lines[1][16..].Trim());

            for(int i = 2; i < lines.Length; i++)
                ParseLine(lines[i], result);

            return result;
        }

        private static WebUITable ParseLine(string line, WebUITable accumulator)
        {
            int current = 0;
            string key;
            for(int i = 0; i < line.Length; i++)            
            {
                if (!line[i].Equals(':'))
                    continue;

                key = line[current..i];
                current = i + 1;

                for (; i < line.Length && !line[i].Equals(','); i++);
                accumulator.Add(key.Trim().ToLower(), line[current..i].Trim());

                current = i + 1;
            }

            return accumulator;
        }
    }
}
