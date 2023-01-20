// See https://aka.ms/new-console-template for more information
using CommandLine;
using MinecraftSkinpackGenerator;
using Newtonsoft.Json;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.Json.Nodes;

internal class Program
{
    static string SkinPackVersion = "1.0.0";
    static SkinPack SkinPack = new SkinPack("Marvel Characters", "Marvel Characters");
    static string? SkinPackName = "Marvel Characters";
    static string? ImagePath;
    private static void Main(string[] args)
    {

        var parser = new Parser(settings =>
        {
            settings.CaseSensitive = false;
            settings.HelpWriter = Console.Error;
            settings.IgnoreUnknownArguments = false;
        });

        var result = parser.ParseArguments<Options>(args);
        var exitCode = result.MapResult(
          options =>
          {

              if (options?.StringSequence?.Count() > 0)
              {
                  Console.WriteLine("unbound params: " +
                  String.Join(",", options.StringSequence)
                  );
                  return 1;
              }
              Console.WriteLine("Hi " + options.Name + ", your password is");
              Console.WriteLine(options.Directory);

              SkinPackName = options?.Name ?? "Skin_Assets";
              ImagePath = options?.Directory ?? "Marvel Characters";

              return 0;
          },
          errors =>
          {
              //Use defaults
              ImagePath = "Skin_Assets";
              return 1;
          });


        if(exitCode == 1)
        {
        }

        //Copy output into a directory.
        Directory.CreateDirectory(SkinPackName);


        if (File.Exists(ImagePath))
        {
            // This path is a file
            ProcessFile(ImagePath);
        }
        else if (Directory.Exists(ImagePath))
        {
            // This path is a directory
            ProcessDirectory(ImagePath);
        }
        else
        {
            Console.WriteLine("{0} is not a valid file or directory.", ImagePath);
        }

        var packJson = JsonConvert.SerializeObject(SkinPack, Formatting.Indented);

        Console.Write(packJson);

        File.WriteAllText($"{SkinPackName}/skins.json", packJson);

        

        //Generate lang file
        GenerateLangFile();

        //Generate Manifest file
        GenerateManifestFile();


        ZipFile.CreateFromDirectory(SkinPackName, $"{SkinPackName}.zip");
        File.Copy($"{SkinPackName}.zip", $"{SkinPackName}.mcpack", true);

    }

    private static void GenerateManifestFile()
    {
        var versionArray = SkinPackVersion.Split('.')?.Select(int.Parse).ToArray();
        var manifestInfo = new Manifest()
        {
            FormatVersion = 1,
            Header = new Header()
            {
                Name = SkinPackName,
                Uuid = Guid.NewGuid().ToString(),
                Version = versionArray
            },
            Modules = new Module[] { new Module() { Type = "skin_pack", Uuid = Guid.NewGuid().ToString(), Version = versionArray } }

        };

        File.WriteAllText($"{SkinPackName}/manifest.json", JsonConvert.SerializeObject(manifestInfo, Formatting.Indented));
    }

    private static void GenerateLangFile()
    {
        List<string> languageLists = new List<string>();
        //Pack Name
        languageLists.Add($"skinpack.{SkinPackName}={SkinPackName}");

        foreach (var skin in SkinPack.Skins)
        {
            languageLists.Add($"\t\t\t\tskin.{SkinPackName}.{skin.LocalizationName}={skin.LocalizationName}{Environment.NewLine}");
        }
        Directory.CreateDirectory($"{SkinPackName}/texts");
        File.AppendAllLines($"{SkinPackName}/texts/en_US.lang", languageLists);

    }


    // Process all files in the directory passed in, recurse on any directories
    // that are found, and process the files they contain.
    public static void ProcessDirectory(string targetDirectory)
    {
        // Process the list of files found in the directory.
        string[] fileEntries = Directory.GetFiles(targetDirectory);
        foreach (string fileName in fileEntries)
            ProcessFile(fileName);

        // Recurse into subdirectories of this directory.
        string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
        foreach (string subdirectory in subdirectoryEntries)
            ProcessDirectory(subdirectory);
    }

    // Insert logic for processing found files here.
    public static void ProcessFile(string path)
    {
        var skinName = Path.GetFileName(path).Split('.').FirstOrDefault();
        var skin = new Skin()
        {
            LocalizationName = skinName ?? string.Empty,
            Geometry = $"geometry.{SkinPackName}.{skinName}",
            Texture = Path.GetFileName(path),
            Type = "free"
        };
        File.Copy(path, $"{SkinPackName}/{Path.GetFileName(path)}");
        SkinPack.Skins.Add(skin);
        //Console.WriteLine(skin.ToString());
    }
}