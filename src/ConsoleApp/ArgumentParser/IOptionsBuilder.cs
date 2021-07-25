using System;

namespace ConsoleApp.ArgumentParser {
    public interface IOptionsBuilder {
        Type CreateOptionsType();
    }
}
