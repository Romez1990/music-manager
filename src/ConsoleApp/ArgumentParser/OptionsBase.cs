using CommandLine;

namespace ConsoleApp.ArgumentParser {
    public class OptionsBase {
        [Value(0, HelpText = "Directory path")]
        public string Path { get; set; }

        [Option('a', "album", SetName = "album", HelpText = "Album directory mode")]
        public bool Album { get; set; }

        [Option('b', "band", SetName = "band", HelpText = "Band directory mode")]
        public bool Band { get; set; }

        [Option('c', "compilation", SetName = "compilation", HelpText = "Compilation directory mode")]
        public bool Compilation { get; set; }
    }
}
