﻿using System.IO;
using System.Text;

namespace UpdatePackages.Services
{
    public interface IFileService
    {
        string GetFileContent(string filePath);
        void WriteToFile(string filePath, string content, Encoding encoding);
        Encoding GetFileEncoding(string filename);
    }

    public class FileService : IFileService
    {
        public string GetFileContent(string filePath)
        {
            using var reader = new StreamReader(filePath);

            return reader.ReadToEnd();
        }

        public void WriteToFile(string filePath, string content, Encoding encoding)
        {
            using var writer = new StreamWriter(filePath, false, encoding);
            writer.Write(content);
            writer.Close();
        }

        /// <summary>
        /// Determines a text file's encoding by analyzing its byte order mark (BOM).
        /// Defaults to ASCII when detection of the text file's endianness fails.
        /// </summary>
        /// <param name="filename">The text file to analyze.</param>
        /// <returns>The detected encoding.</returns>
        public Encoding GetFileEncoding(string filename)
        {
            // Read the BOM
            var bom = new byte[4];
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                file.Read(bom, 0, 4);
            }

            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
            if (bom[0] == 0xff && bom[1] == 0xfe && bom[2] == 0 && bom[3] == 0) return Encoding.UTF32; //UTF-32LE
            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return new UTF32Encoding(true, true);  //UTF-32BE

            return Encoding.ASCII;
        }
    }
}
