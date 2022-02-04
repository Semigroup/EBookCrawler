using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;

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
        public Length Width { get; set; } = new Length() { Value = 1, IsProportional = true };
        public Alignment MyAlignment { get; set; } = Alignment.Unspecified;

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
            if (Border > 0)
            {
                writer.WriteLine(@"\setlength{\fboxsep}{0pt}");
                writer.WriteLine(@"\setlength{\fboxrule}{1pt}");
                writer.WriteLine(@"\fbox{\includegraphics[width=" + Width + "]{" + path + "}}");
            }
            else
                writer.WriteLine(@"\includegraphics[width=" + Width + "]{" + path + "}");
            writer.WriteLine(@"}");
        }
        protected void WriteCaption(LatexWriter writer)
        {
            string capt = GetCaption();
            if (capt != null)
            {
                writer.Write(@"\caption{");
                writer.WriteText(capt);
                writer.WriteLine(@"}");
            }
        }
        protected void WriteWrapFigue(LatexWriter writer, string path)
        {
            writer.BeginEnvironment("wrapfigure", MyAlignment == Alignment.Right ? "R" : "L", Width.ToString());   
            writer.WriteAlignment(Alignment.Center);
            WriteInLineGraphic(writer, path);
            WriteCaption(writer);

            writer.EndEnvironment("wrapfigure");
            writer.WriteLine();
        }
        public override void ToLatex(LatexWriter writer)
        {
            string path = DownloadImage(
                writer.BuildRoot,
                writer.BuildDirectory,
                writer.CurrentChapter.Chapter);

            switch (MyKind)
            {
                case Kind.Lettrine:
                    writer.WriteLine(@"\lettrine[lines=3,image=true]{" + path + "}{}");
                    break;
                case Kind.Figure:
                    writer.BeginEnvironment("figure");
                    writer.WriteLine();
                    writer.WriteAlignment(MyAlignment);
                    writer.WriteLine();
                    WriteInLineGraphic(writer, path);
                    WriteCaption(writer);
                    writer.EndEnvironment("figure");
                    writer.WriteLine();
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
        public string DownloadImage(string buildRoot, string buildDirectory, Chapter chapter)
        {
            string targetDirectory = Path.Combine(buildDirectory, "bilder\\");
            if (!Directory.Exists(targetDirectory))
                Directory.CreateDirectory(targetDirectory);
            int part = chapter.Part.Reference.Number + 1;
            string imageName = Path.GetFileNameWithoutExtension(RelativePath);
            string absolutePath = Path.Combine(targetDirectory, imageName + ".part" + part + ".png");
            string relativePath = absolutePath.Substring(buildDirectory.Length).Replace('\\', '/');
            if (File.Exists(absolutePath))
                return relativePath;

            string absoluteGifPath = Path.Combine(targetDirectory, Path.GetFileName(RelativePath));
            using (WebClient wClient = new WebClient())
                wClient.DownloadFile(Uri, absoluteGifPath);
            using (Bitmap bmp = new Bitmap(absoluteGifPath))
                bmp.Save(absolutePath, ImageFormat.Png);
            return relativePath;

            //string targetFile = RelativePath.Replace('/', '\\');
            //string absolutePath = Path.Combine(buildRoot, targetFile);
            //string targetDirectory = Path.GetDirectoryName(absolutePath);
            //targetFile = absolutePath.Substring(buildDirectory.Length);
            //targetFile = targetFile.Replace('\\', '/');
            //if (!Directory.Exists(targetDirectory))
            //    Directory.CreateDirectory(targetDirectory);

            //var pngAbsPath = absolutePath.Substring(0, absolutePath.Length - 3) + "png";
            //targetFile = targetFile.Substring(0, targetFile.Length - 3) + "png";
            //if (File.Exists(pngAbsPath))
            //    return targetFile;

            //using (WebClient wClient = new WebClient())
            //    wClient.DownloadFile(Uri, absolutePath);
            //using (Bitmap bmp = new Bitmap(absolutePath))
            //    bmp.Save(pngAbsPath, ImageFormat.Png);

            //return targetFile;
        }
    }
}
