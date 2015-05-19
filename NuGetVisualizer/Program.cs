using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuGet;
using static System.Console;

namespace NuGetVisualizer
{
    class Program
    {
        // http://stackoverflow.com/questions/6653715/view-nuget-package-dependency-hierarchy
        static void Main()
        {
            var repo = new LocalPackageRepository(@"c:\Projects\tower\packages\");
            var packages = repo.GetPackages();
            OutputGraph(repo, packages, 0);
        }

        static void OutputGraph(IPackageRepository repository, IEnumerable<IPackage> packages, int depth)
        {
            foreach (var package in packages)
            {
                WriteLine("{0}{1} v{2}", new string(' ', depth), package.Id, package.Version);

                var dependentPackages = from ds in package.DependencySets
                                        from dependency in ds.Dependencies
                                        let versionSpec = dependency.VersionSpec
                                        select repository.FindPackage(dependency.Id, versionSpec, false, true);

                OutputGraph(repository, dependentPackages, depth += 3);
            }
        }
    }
}
