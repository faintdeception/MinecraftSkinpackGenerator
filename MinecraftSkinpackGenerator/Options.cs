using CommandLine.Text;
using CommandLine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftSkinpackGenerator
{
    class Options
    {
        [Option('n', "name", Required = true, HelpText = "skinpack name")]
        public string? Name { get; set; }

        [Option('d', "directory", Required = true, HelpText = "skinn directory")]
        public string? Directory { get; set; }

        [Value(0)]
        public IEnumerable<string>? StringSequence { get; set; }
    }
}
