open System.Net
open System.IO

let get (url: string) =
    async {
        let request = WebRequest.Create url
        let! response = request.AsyncGetResponse()
        let responseStream = response.GetResponseStream()
        use streamReader = new StreamReader(responseStream)
        let result = streamReader.ReadToEnd()

        return result
    }

[<EntryPoint>]
let main argv =
    Async.Parallel
        [ for url in argv ->
            async {
                let! result = get url
                printfn "%s" result

                return result
            } ]
    |> Async.RunSynchronously
    |> ignore
    0
