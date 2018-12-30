namespace WAVFileReaderTest

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type TestLoadWAV () =

    [<TestMethod>]
    member this.TestLoadWAV () =
        let fmt,data = 
            IO.File.Open ("wav.wav",IO.FileMode.Open)
            |> WAVFileReader.ReadFile
        printfn "Fmt:"
        printfn "%A" fmt
        printfn "Data:"
        printfn "%A" data
