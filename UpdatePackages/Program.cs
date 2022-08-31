using System.Collections.Generic;
using UpdatePackages.Services;

namespace UpdatePackages
{
    class Program
    {
        /// <summary>
        /// Change folder path, which contains all back-end folders
        /// </summary>
        private const string BackEndSolutionPath = "C:\\git";

        /// <summary>
        /// Change solution, which you want to update
        /// </summary>
        private const string SolutionToUpdate = Constants.SolutionNames.SharedServices;

        /// <summary>
        /// Change package name, which you want to update 
        /// </summary>
        private const string PackageName = Constants.PackageNames.ShipManagerConnectCore;

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
        private const string NewPackageVersion = "2.33.0";


        // `dotnet pack`
        static void Main()
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
