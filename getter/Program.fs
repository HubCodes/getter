open System
open System.Net
open System.IO

let get (url: string) =
    async {
        try
            let request = WebRequest.Create url
            use! response = request.AsyncGetResponse()
            use responseStream = response.GetResponseStream()
            use streamReader = new StreamReader(responseStream)
            let result = streamReader.ReadToEnd()

            return Ok result
        with
        | :? System.UriFormatException -> return Error "Malformed URL"
        | :? System.Net.WebException as e -> return Error e.Message
    }

[<EntryPoint>]
let main argv =
    Async.Parallel
        [ for url in argv ->
            async {
                let! result = get url
                match result with
                | Ok data -> printfn "%s" data
                | Error e -> Console.Error.WriteLine("{0}", e)

                return result
            } ]
    |> Async.RunSynchronously
    |> ignore
    0
