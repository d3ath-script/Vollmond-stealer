using System.Reflection;

namespace Vollmond.Browsers
{
    public class ExtractResources
    {
        public static string ExtractResource(string Namespace, string filename, string outputPath)
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();

                using (Stream stream = assembly.GetManifestResourceStream($"{Namespace}.{filename}"))
                using (FileStream fileStream = new FileStream(Path.Combine(outputPath, filename), FileMode.Create))
                {
                    try
                    {
                        stream.CopyTo(fileStream);
                    }
                    catch
                    {
                        throw new Exception();
                    }
                }
                return "";
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}
