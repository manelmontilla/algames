using System;
using CommandLine;

namespace _4inr
{
    public class Options
    {
        [Option('r', "resume", Required = false, HelpText = "Resumes an existing game stored in the specified file")]
        public string Resume { get; set; }

        [Option('f', "from", Required = false, HelpText = "Specifies the movement to start from a resumed game.\nCan only be used in conjunction with resume")]
        public int? BackTo { get; set; }

    }
}