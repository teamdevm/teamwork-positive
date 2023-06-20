using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Documently.Models;

class TemplatePreprocessor
{
    public static void MergeRuns (WordprocessingDocument doc)
    {
        string paraText;
        Body body = doc.MainDocumentPart.Document.Body;
        foreach (Paragraph para in body.ChildElements)
        {
            paraText = para.InnerText;
            if (para.FirstChild is Run paraRun)
            {
                if (paraRun.FirstChild is Text runText)
                {
                    runText.Text = paraText;
                }
            }
        }
    }
}