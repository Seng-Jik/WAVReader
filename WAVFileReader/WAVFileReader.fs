module WAVFileReader

open System.IO

type AudioDataFormat =
| PCM
| Unknown

[<Struct>]
type FormatChunk = {
    DataFormat : AudioDataFormat
    Channels : uint16
    SampleRate : uint32
    ByteRate : uint32 }

[<Struct>]
type DataChunk = {
    Begin : int64
    Size : uint32 }

let ReadFile (wavData : Stream) =
    use reader = new BinaryReader(wavData)

    let checkByteArray a b =
        b
        |> Array.map byte
        |> Array.forall2 (fun a b -> a = b) a
        |> function
        | false -> failwith "Not wav file."
        | true -> ()
    
    checkByteArray (reader.ReadBytes 4) [|'R';'I';'F';'F'|]
    let _mainChunkSize = reader.ReadUInt32 ()
    checkByteArray (reader.ReadBytes 4) [|'W';'A';'V';'E'|]
    checkByteArray (reader.ReadBytes 4) [|'f';'m';'t';' '|]
    let fmtChunckSize = reader.ReadUInt32 () |> int64
    let dataChunkPosition = fmtChunckSize + reader.BaseStream.Position
    
    let fmtChunk = {
        DataFormat = 
            reader.ReadUInt16 ()
            |> function
            | 1us -> PCM
            | _ -> Unknown
        Channels = reader.ReadUInt16 ()
        SampleRate = reader.ReadUInt32 ()
        ByteRate = reader.ReadUInt32 () }
    
    reader.BaseStream.Position <- dataChunkPosition
    checkByteArray (reader.ReadBytes 4) [|'d';'a';'t';'a'|]
    let dataSize = reader.ReadUInt32 ()
    let dataChunck = {
        Begin = reader.BaseStream.Position
        Size = dataSize }
    fmtChunk,dataChunck






