using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace UpdatePackages
{
    public class UpdatePackageService
    {
        private const string CsprojExtensionStr = ".csproj";
        private const string Version = "Version=\"";

        public void UpdatePackage(
            string backEndSolutionPath,
            string solutionToUpdate,
            string packageName,
            string newPackageVersion)
        {
            var solutionFolderPath = Path.Combine(
                backEndSolutionPath,
                solutionToUpdate);

            if (!Directory.Exists(solutionFolderPath))
            {
                throw new ArgumentException(
                    $"There is no solution folder by following path: '${solutionFolderPath}'");
            }

            var projectPathList = new List<string>();
            FindProjectPathsInFolder(solutionFolderPath, projectPathList);

            foreach (var projectPath in projectPathList)
            {
                UpdatePackageForProject(projectPath, packageName, newPackageVersion);
            }
        }

        private static void FindProjectPathsInFolder(string folderPath, List<string> projectPathList)
        {
            var projectPaths =
                Directory.GetFiles(folderPath)
                    .Where(fileName => fileName.EndsWith(CsprojExtensionStr));
            projectPathList.AddRange(projectPaths);

            var internalFolderNames = Directory.GetDirectories(folderPath);
            foreach (var internalFolderName in internalFolderNames)
            {
                var internalFolderPath = Path.Combine(folderPath, internalFolderName);
                FindProjectPathsInFolder(internalFolderPath, projectPathList);
            }
        }

        private static void UpdatePackageForProject(
            string projectPath,
            string packageName,
            string newPackageVersion)
        {
            var projectContent = GetFileContent(projectPath);
            var packageNameIndex = projectContent
                .IndexOf(packageName, StringComparison.InvariantCultureIgnoreCase);

            if (packageNameIndex == -1)
            {
                return;
            }

            var packageVersionIndex =
                projectContent.IndexOf(
                    Version,
                    packageNameIndex,
                    StringComparison.InvariantCultureIgnoreCase);
            var packageVersionLastIndex = packageVersionIndex + Version.Length;
            var doubleQuotesClosingVersionIndex = projectContent.IndexOf(
                "\"",
                packageVersionLastIndex,
                StringComparison.InvariantCultureIgnoreCase);

            var sb = new StringBuilder();
            sb.Append(projectContent.Substring(0, packageVersionLastIndex));
            sb.Append(newPackageVersion);
            sb.Append(projectContent.Substring(
                doubleQuotesClosingVersionIndex,
                projectContent.Length - doubleQuotesClosingVersionIndex));

            WriteToFile(projectPath, sb.ToString());
        }

        private static string GetFileContent(string filePath)
        {
            using var reader = new StreamReader(filePath);
            return reader.ReadToEnd();
        }

        private static void WriteToFile(string filePath, string content)
        {
            using var writer = new StreamWriter(filePath, false, Encoding.UTF8);
            writer.Write(content);
            writer.Close();
        }
    }
}
