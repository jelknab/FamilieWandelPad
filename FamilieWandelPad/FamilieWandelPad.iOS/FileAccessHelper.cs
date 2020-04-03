using System;
using System.IO;
using FamilieWandelPad.iOS;
using Foundation;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileAccessHelper))]
namespace FamilieWandelPad.iOS
{
    public class FileAccessHelper : IFileAccessHelper
    {

        string IFileAccessHelper.MakeAssetAvailable(string fileName)
        {
            var docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var libFolder = Path.Combine(docFolder, "..", "Library", "Files");

            if (!Directory.Exists(libFolder)) Directory.CreateDirectory(libFolder);

            var outputPath = Path.Combine(libFolder, fileName);

            CopyFile(outputPath);

            return outputPath;
        }

        private static void CopyFile(string path)
        {
            var existingDb = NSBundle.MainBundle.PathForResource(Path.GetFileNameWithoutExtension(path), Path.GetExtension(path));
            File.Copy(existingDb, path, true);
        }
    }
}