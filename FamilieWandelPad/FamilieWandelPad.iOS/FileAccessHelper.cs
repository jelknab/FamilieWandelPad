using System;
using System.IO;
using Foundation;

namespace FamilieWandelPad.iOS
{
    public class FileAccessHelper
    {
        public static string MakeAssetAvailable(string filename)
        {
            var docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder)) Directory.CreateDirectory(libFolder);

            var dbPath = Path.Combine(libFolder, filename);

            CopyFile(dbPath);

            return dbPath;
        }

        private static void CopyFile(string dbPath)
        {
            var existingDb = NSBundle.MainBundle.PathForResource(Path.GetFileNameWithoutExtension(dbPath), Path.GetExtension(dbPath));
            File.Copy(existingDb, dbPath, true);
        }
    }
}