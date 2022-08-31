using CommandLine;
using UpdatePackages.Commands;

namespace UpdatePackages
{
    class Program
    {
        static void Main(params string[] args)
        {
            Parser.Default.ParseArguments<UpdatePackageCommand>(args)
                .WithParsed(t => t.Execute());
        }
    }
}
