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
        private static void CopyFile(string dbPath)
        {
            var existingDb = NSBundle.MainBundle.PathForResource("people", "db3");
            File.Copy(existingDb, dbPath);
        }

        string IFileAccessHelper.MakeAssetAvailable(string fileName)
        {
            var docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder)) Directory.CreateDirectory(libFolder);

            var outputPath = Path.Combine(libFolder, fileName);

            CopyFile(outputPath);

            return outputPath;
        }
    }
}