using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EBookCrawler
{
    [Serializable]
    public class BookReference
    {
        public string Name { get; set; }
        public string SubTitle { get; set; }
        public bool PartsHaveDifferentSubTitles { get; set; }
        public List<string> Genres { get; set; } = new List<string>();
        public string Identifier { get; set; }

        public PartReference[] Parts { get; set; }

        public void SetIdentifer()
        {
            int hash = 0;
            foreach (var part in Parts)
                hash ^= part.Link.GetHashCode();
            this.Identifier = Name + " | " + SubTitle + " | " + Parts.Length + " @" + hash;
        }

        public void WriteLatex(string root, string outputDirectory)
        {
            var doc = new Texting.Document(this, Parts.Select(p => p.Part.ParseText(root)));
            string path = Parts[0].Link;
            path = path.Substring("https://www.projekt-gutenberg.org/".Length);
            path = path.Replace('/', '\\');
            path = Path.Combine(outputDirectory, path);
            var dir = Path.GetDirectoryName(path);
            path = Path.Combine(dir, Name.ToFileName() + ".tex");

            Console.WriteLine("Writing " + path);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using (var writer = new Texting.LatexWriter(outputDirectory, path))
                doc.ToLatex(writer);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
