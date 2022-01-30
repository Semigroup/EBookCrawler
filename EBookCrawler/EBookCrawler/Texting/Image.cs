using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace EBookCrawler.Texting
{
    public class Image : TextElement
    {
        public enum Kind
        {
            Lettrine,
            Figure,
            WrapFigure,
            InLine
        }

        public string Title { get; set; }
        public string AlternativeTitle { get; set; }
        public string RelativePath { get; set; }
        public string Uri { get; set; }

        public string Caption { get; set; }
        public Length Width { get; set; }
        public int Alignment { get; set; }

        public Kind MyKind { get; set; } = Kind.InLine;

        //ToDo
        public double VSpace { get; set; }
        public double HSpace { get; set; }
        public double Border { get; set; }
        public Length Height { get; set; }

        protected string GetCaption()
        {
            if (Caption != null)
                return Caption;
            else if (Title != null)
                return Title;
            else if (AlternativeTitle != null)
                return AlternativeTitle;
            return null;
        }
        protected void WriteInLineGraphic(LatexWriter writer, string path)
        {
            writer.WriteLine(@"{");
            writer.WriteLine(@"\setlength{\fboxsep}{0pt}");
            writer.WriteLine(@"\setlength{\fboxrule}{1pt}");
            writer.WriteLine(@"\fbox{\includegraphics[width=" + Width + "]{" + path + "}}");
            WriteCaption(writer);
            writer.WriteLine(@"}");
        }
        protected void WriteCaption(LatexWriter writer)
        {
            string capt = GetCaption();
            if (capt != null)
            {
                writer.Write(@"\caption{");
                writer.WriteText(Caption);
                writer.WriteLine(@"}");
            }
        }
        protected void WriteWrapFigue(LatexWriter writer, string path)
        {
            writer.Write(@"\begin{wrapfigure}{");
            if (Alignment == 2)
                writer.Write("R");
            else
                writer.Write("L");
            writer.Write("}{" + Width + "}");

            writer.WriteAlignment(1);
            WriteInLineGraphic(writer, path);

            writer.WriteLine(@"\end{wrapfigure}");
        }
        public override void ToLatex(LatexWriter writer)
        {
            string path = DownloadImage(writer.BuildDirectory);

            switch (MyKind)
            {
                case Kind.Lettrine:
                    writer.WriteLine(@"\lettrine[lines=3,image=true]{" + path + "}{}");
                    break;
                case Kind.Figure:
                    writer.WriteLine(@"\begin{figure}");
                    writer.WriteAlignment(Alignment);
                    WriteInLineGraphic(writer, path);
                    writer.WriteLine(@"\end{figure}");
                    break;
                case Kind.WrapFigure:
                    WriteWrapFigue(writer, path);
                    break;
                case Kind.InLine:
                    WriteInLineGraphic(writer, path);
                    break;
                default:
                    throw new NotImplementedException();
            }

        }
        public string DownloadImage(string directory)
        {
            string targetFile = RelativePath.Replace('/', '\\');
            string absolutePath = Path.Combine(directory, targetFile);
            string targetDirectory = Path.GetDirectoryName(absolutePath);
            if (!Directory.Exists(targetDirectory))
                Directory.CreateDirectory(targetDirectory);
            if (File.Exists(absolutePath))
                return targetFile;

            WebClient wClient = new WebClient();
            wClient.DownloadFile(Uri, absolutePath);
            return targetFile;
        }
    }
}
