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
        private const string SolutionToUpdate = SolutionNameConstants.UserManagementService;

        /// <summary>
        /// Change package name, which you want to update 
        /// </summary>
        private const string PackageName = PackageNameConstants.CascadeCommonDataServiceConsumer;

        /// <summary>
        /// Change package version
        /// </summary>
        private const string NewPackageVersion = "1.0.20220427.131241";

        static void Main()
        {
            var updatePackageService = new UpdatePackageService();
            updatePackageService.UpdatePackage(
                BackEndSolutionPath,
                SolutionToUpdate,
                PackageName,
                NewPackageVersion);
        }
    }
}
