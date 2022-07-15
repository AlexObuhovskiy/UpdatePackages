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
        private const string SolutionToUpdate = SolutionNameConstants.SharedServices;

        /// <summary>
        /// Change package name, which you want to update 
        /// </summary>
        private const string PackageName = PackageNameConstants.SmcServiceCore;

        /// <summary>
        /// Change package version
        /// </summary>
        private const string NewPackageVersion = "1.20.0";

        static void Main()
        {
            var updatePackageService = new UpdatePackageService(new FileService());
            updatePackageService.UpdatePackage(
                BackEndSolutionPath,
                SolutionToUpdate,
                PackageName,
                NewPackageVersion);
        }
    }
}
