using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace UpdatePackages.Services
{
    public class UpdatePackageService
    {
        private const string CsprojExtensionStr = ".csproj";
        private const string Version = "Version=\"";
        private const string PackageReference = "PackageReference Include=\"";
        private readonly IFileService _fileService;

        public UpdatePackageService(IFileService fileService)
        {
            _fileService = fileService;
        }

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

        private void UpdatePackageForProject(
            string projectPath,
            string packageName,
            string newPackageVersion)
        {
            var projectContent = _fileService.GetFileContent(projectPath);
            var packageNameIndex = projectContent
                .IndexOf($"{PackageReference}{packageName}\"", StringComparison.InvariantCultureIgnoreCase);

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

            var encoding = _fileService.GetFileEncoding(projectPath);
            _fileService.WriteToFile(projectPath, sb.ToString(), encoding);

            Console.WriteLine($"{projectPath.Split("\\").Last()}: package {packageName} " +
                $"was updated with version '{newPackageVersion}'.");
        }
    }
}
