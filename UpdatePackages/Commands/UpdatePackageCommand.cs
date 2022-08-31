using System.Collections.Generic;
using CommandLine;
using UpdatePackages.Services;

namespace UpdatePackages.Commands
{
    [Verb("update", isDefault: true, HelpText = "Updates package for solution with version")]
    public class UpdatePackageCommand : ICommand
    {
        /// <summary>
        /// Change folder path, which contains all back-end folders
        /// </summary>
        private const string BackEndSolutionPathConst = "C:\\git";

        /// <summary>
        /// Change solution, which you want to update
        /// </summary>
        private const string SolutionToUpdateConst = Constants.SolutionNames.SharedServices;

        /// <summary>
        /// Change package name, which you want to update 
        /// </summary>
        private const string PackageNameConst = Constants.PackageNames.ShipManagerConnectCore;

        /// <summary>
        /// Select project names you want to ignore
        /// </summary>
        private static readonly List<string> ProjectsToIgnore = new List<string>
        {
            "ShipManager.Vessel"
        };

        /// <summary>
        /// Change package version
        /// </summary>
        private const string NewPackageVersionConst = "2.33.0";

        [Option('p', "solutionPath", Required = false, HelpText = "Enter Solution path to update", Default = BackEndSolutionPathConst)]
        public string BackEndSolutionPath { get; set; }

        [Option('s', "solutionName", Required = false, HelpText = "Enter Solution where update package", Default = SolutionToUpdateConst)]
        public string SolutionToUpdate { get; set; }

        [Option('n', "packageName", Required = false, HelpText = "Enter package name", Default = PackageNameConst)]
        public string PackageName { get; set; }

        [Option('v', "packageVersion", Required = false, HelpText = "Enter package version", Default = NewPackageVersionConst)]
        public string NewPackageVersion { get; set; }
        
        public void Execute()
        {
            var updatePackageService = new UpdatePackageService(new FileService());
            updatePackageService.UpdatePackage(
                BackEndSolutionPath,
                SolutionToUpdate,
                PackageName,
                NewPackageVersion,
                ProjectsToIgnore);
        }
    }
}